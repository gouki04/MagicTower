using System.Collections;
using UnityEngine;
using SafeCoroutine;
using Utils;

namespace MagicTower
{
    namespace Logic
    {
        public class Game : Singleton<Game>
        {
            private Display.IDisplayFactory mDisplayFactory;
            public Display.IDisplayFactory DisplayFactory
            {
                get { return mDisplayFactory; }
            }

            private Display.IGameDisplay mDisplay;
            public Display.IGameDisplay Display
            {
                get { return mDisplay; }
            }

            private TileMap mCurTileMap;
            private Tile_Player mPlayer;
            public Tile_Player Player
            {
                get { return mPlayer; }
            }

            private bool mIsInBattle = false;
            public bool IsInBattle
            {
                get { return mIsInBattle; }
                set { mIsInBattle = value; }
            }

            public Game()
            {

            }

            public IEnumerator Init(Display.IDisplayFactory factory)
            {
                mDisplayFactory = factory;

                // set the display
                mDisplay = mDisplayFactory.GetGameDisplay(this);

                // load the save data
                yield return _loadGameData();

                // enter the map
                yield return _enterTileMap(PlayerData.Instance.LastTileMapLevel);
            }

            public IEnumerator ChangeLevel(uint level, TilePosition origin_position)
            {
                yield return mCurTileMap.Exit();

                mPlayer.Parent = null;
                mPlayer.Position = origin_position;

                yield return _enterTileMap(level);
            }

            public IEnumerator MoveTileTo(Tile tile, TilePosition destination)
            {
                do
                {
                    if (tile == null || tile.IsMoving || tile.Position == destination)
                        break;

                    var dst_collide_tile = mCurTileMap.LayerCollide[destination];
                    if (dst_collide_tile != null && !dst_collide_tile.ValidateMove(tile))
                        break;

                    var dst_floor_tile = mCurTileMap.LayerFloor[destination];
                    if (dst_floor_tile != null && !dst_floor_tile.ValidateMove(tile))
                        break;

                    tile.IsMoving = true;

                    if (dst_collide_tile != null)
                    {
                        yield return dst_collide_tile.BeginTrigger(tile);

                        if ((bool)SafeCoroutine.Coroutine.GlobalResult == false)
                            break;
                    }

                    if (dst_floor_tile != null)
                    {
                        yield return dst_floor_tile.BeginTrigger(tile);

                        if ((bool)SafeCoroutine.Coroutine.GlobalResult == false)
                            break;
                    }

                    yield return tile.MoveTo(destination);

                    if (dst_floor_tile != null)
                    {
                        yield return dst_floor_tile.EndTrigger(tile);
                    }
                } while (false);

                tile.IsMoving = false;
            }

            private IEnumerator _loadGameData()
            {
                PlayerData.Instance.LoadDataFromFile();

                mPlayer = TileFactory.Instance.CreatePlayer();
                mPlayer.Position = PlayerData.Instance.LastPlayerPosition;

                yield return null;
            }

            private IEnumerator _enterTileMap(uint lv)
            {
                yield return TileMapManager.Instance.LoadMap(lv);

                if (SafeCoroutine.Coroutine.GlobalResult == null)
                {
                    Logger.LogFatal("Load Tile Map ({0}) Failed!", lv);
                    yield return null;
                }

                mCurTileMap = SafeCoroutine.Coroutine.GlobalResult as TileMap;
                mCurTileMap.LayerCollide[mPlayer.Position] = mPlayer;

                yield return mCurTileMap.Enter();
            }
        } 
    }
}
