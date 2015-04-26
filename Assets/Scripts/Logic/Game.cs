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

                yield return null;
            }

            public IEnumerator MoveTileTo(Tile tile, TilePosition destination)
            {
                do
                {
                    if (tile == null || tile.IsMoving)
                        break;

                    var dst_tile = mCurTileMap.LayerCollide[destination];
                    if (tile == dst_tile)
                        break;

                    tile.IsMoving = true;
                    
                    if (dst_tile != null)
                    {
                        if (!dst_tile.ValidateMove(tile))
                            break;

                        yield return dst_tile.BeginTrigger(tile);

                        if ((bool)SafeCoroutine.Coroutine.GlobalResult == false)
                            break;
                    }

                    yield return tile.MoveTo(destination);
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
