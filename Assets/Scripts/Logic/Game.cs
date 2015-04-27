using System.Collections;
using UnityEngine;
using SafeCoroutine;
using Utils;

namespace MagicTower.Logic
{
    /// <summary>
    /// 游戏类
    /// 负责与EventQueue对接
    /// </summary>
    public class Game : Singleton<Game>
    {
        /// <summary>
        /// 当前使用的Display工厂
        /// </summary>
        private Display.IDisplayFactory mDisplayFactory;
        public Display.IDisplayFactory DisplayFactory
        {
            get { return mDisplayFactory; }
        }

        /// <summary>
        /// 游戏的Display
        /// </summary>
        private Display.IGameDisplay mDisplay;
        public Display.IGameDisplay Display
        {
            get { return mDisplay; }
        }

        /// <summary>
        /// 当前的TileMap
        /// </summary>
        private TileMap mCurTileMap;

        /// <summary>
        /// 当前的玩家
        /// </summary>
        private Tile_Player mPlayer;
        public Tile_Player Player
        {
            get { return mPlayer; }
        }

        /// <summary>
        /// 是否在战斗中
        /// </summary>
        private bool mIsInBattle = false;
        public bool IsInBattle
        {
            get { return mIsInBattle; }
            set { mIsInBattle = value; }
        }

        public Game()
        {

        }

        /// <summary>
        /// 初始化游戏
        /// </summary>
        /// <param name="factory">display工厂</param>
        /// <returns></returns>
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

        /// <summary>
        /// 切换场景
        /// </summary>
        /// <param name="level">目标层</param>
        /// <param name="origin_position">玩家在目标层的出生位置</param>
        /// <returns></returns>
        public IEnumerator ChangeLevel(uint level, TilePosition origin_position)
        {
            yield return mCurTileMap.Exit();

            mPlayer.Parent = null;
            mPlayer.Position = origin_position;

            yield return _enterTileMap(level);
        }

        /// <summary>
        /// tile移动逻辑
        /// </summary>
        /// <param name="tile">要移动的tile</param>
        /// <param name="destination">目标坐标</param>
        /// <returns></returns>
        public IEnumerator MoveTileTo(Tile tile, TilePosition destination)
        {
            do
            {
                // 移动中不能移动
                if (tile == null || tile.IsMoving || tile.Position == destination)
                    break;

                // 判断碰撞层是否允许移动
                var dst_collide_tile = mCurTileMap.LayerCollide[destination];
                if (dst_collide_tile != null && !dst_collide_tile.ValidateMove(tile))
                    break;

                // 判断地板层是否允许移动
                var dst_floor_tile = mCurTileMap.LayerFloor[destination];
                if (dst_floor_tile != null && !dst_floor_tile.ValidateMove(tile))
                    break;

                // 开始移动
                tile.IsMoving = true;

                if (dst_collide_tile != null)
                {
                    // 先trigger碰撞层
                    yield return dst_collide_tile.BeginTrigger(tile);

                    if ((bool)SafeCoroutine.Coroutine.GlobalResult == false)
                        break;
                }

                if (dst_floor_tile != null)
                {
                    // trigger地板层
                    yield return dst_floor_tile.BeginTrigger(tile);

                    if ((bool)SafeCoroutine.Coroutine.GlobalResult == false)
                        break;
                }

                // 等待移动
                yield return tile.MoveTo(destination);

                // 碰撞层没有trigger结束
                // 因为现在layer里同一个位置不允许有多个tile，所以moveto后，原tile就被干掉了，所以不需要trigger结束

                if (dst_floor_tile != null)
                {
                    // 地板层trigger结束
                    yield return dst_floor_tile.EndTrigger(tile);
                }
            } while (false);

            tile.IsMoving = false;
        }

        /// <summary>
        /// 加载游戏数据
        /// </summary>
        /// <returns></returns>
        private IEnumerator _loadGameData()
        {
            PlayerData.Instance.LoadDataFromFile();

            // 第一次创建玩家
            mPlayer = TileFactory.Instance.CreatePlayer();
            mPlayer.Position = PlayerData.Instance.LastPlayerPosition;

            yield return null;
        }

        /// <summary>
        /// 进入场景
        /// </summary>
        /// <param name="lv">层数</param>
        /// <returns></returns>
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