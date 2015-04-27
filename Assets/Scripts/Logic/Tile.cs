using UnityEngine;
using System.Collections;
using SafeCoroutine;

namespace MagicTower.Logic
{
    /// <summary>
    /// tile基类
    /// </summary>
    public class Tile
    {
        /// <summary>
        /// display接口
        /// </summary>
        protected Display.ITileDisplay mDisplay;
        public Display.ITileDisplay Display
        {
            get { return mDisplay; }
        }

        /// <summary>
        /// tile的类型
        /// </summary>
        public enum EType
        {
            None = 0,
            Floor = 1,
            Wall = 2,
            Water = 3,
            Sky = 4,
            Monster = 5,
            Npc = 6,
            Item = 7,
            Trigger = 8,
            Door = 9,
            Portal = 10,
            Player = 11,
        }

        protected EType mType;
        public EType Type
        {
            get { return mType; }
        }

        /// <summary>
        /// 所在的tile map
        /// </summary>
        protected TileMap mParent = null;
        public TileMap Parent
        {
            get { return mParent; }
            set { mParent = value; }
        }

        /// <summary>
        /// 所在的tile layer
        /// </summary>
        protected TileLayer mLayer;
        public TileLayer Layer
        {
            get { return mLayer; }
            set { mLayer = value; }
        }

        public Tile(EType type)
        {
            mType = type;
        }

        /// <summary>
        /// 当前的逻辑位置
        /// </summary>
        protected TilePosition mPosition = new TilePosition(0, 0);
        public TilePosition Position
        {
            get { return mPosition; }
            set { mPosition = value; }
        }

        /// <summary>
        /// 是否会阻挡移动
        /// </summary>
        protected bool mIsBlock = true;
        public bool IsBlock
        {
            get { return mIsBlock; }
            set { mIsBlock = value; }
        }

        /// <summary>
        /// 判断是否允许target移动过来
        /// </summary>
        /// <param name="target"></param>
        /// <returns>允许就返回true</returns>
        public virtual bool ValidateMove(Tile target)
        {
            return !mIsBlock;
        }

        /// <summary>
        /// 开始和target碰撞
        /// </summary>
        /// <param name="target"></param>
        /// <returns>碰撞成功返回true</returns>
        public virtual IEnumerator BeginTrigger(Tile target)
        {
            yield return true;
        }

        /// <summary>
        /// 碰撞结束
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public virtual IEnumerator EndTrigger(Tile target)
        {
            yield return null;
        }

        /// <summary>
        /// 移动tile到指定位置
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public virtual IEnumerator MoveTo(uint row, uint col)
        {
            yield return MoveTo(new TilePosition(row, col));
        }

        public virtual IEnumerator MoveTo(TilePosition destination)
        {
            yield return mDisplay.MoveTo(destination);

            mLayer[Position] = null;
            mLayer[destination] = this;

            yield return null;
        }

        /// <summary>
        /// 首次进入场景
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerator Enter()
        {
            if (mDisplay == null)
            {
                mDisplay = Game.Instance.DisplayFactory.GetTileDisplay(this);
            }

            yield return mDisplay.Enter();
        }

        /// <summary>
        /// 离开场景
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerator Exit()
        {
            mLayer[mPosition] = null;

            yield return mDisplay.Exit();

            mDisplay = null;
        }

        /// <summary>
        /// 当前是否在移动中
        /// </summary>
        private bool mIsMoving = false;
        public bool IsMoving
        {
            get { return mIsMoving; }
            set { mIsMoving = value; }
        }

        public float Speed
        {
            get { return 1.0f; }
        }
    }
}

