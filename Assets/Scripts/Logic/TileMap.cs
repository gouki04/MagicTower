using SafeCoroutine;
using System;
using System.Collections;
using UnityEngine;

namespace MagicTower.Logic
{
    /// <summary>
    /// Tile map.
    /// </summary>
    public class TileMap
    {
        private Display.ITileMapDisplay mDisplay;
        public Display.ITileMapDisplay Display
        {
            get { return mDisplay; }
        }

        private Data.TileMapData mData;
        public Data.TileMapData Data
        {
            get { return mData; }
        }

        /// <summary>
        /// 地图宽度
        /// </summary>
        public uint Width
        {
            get { return Data.Width; }
        }

        /// <summary>
        /// 地图高度
        /// </summary>
        public uint Height
        {
            get { return Data.Height; }
        }

        /// <summary>
        /// 地板层
        /// </summary>
        private TileLayer mLayerFloor;
        public TileLayer LayerFloor
        {
            get { return mLayerFloor; }
        }

        /// <summary>
        /// 碰撞层
        /// </summary>
        private TileLayer mLayerCollide;
        public TileLayer LayerCollide
        {
            get { return mLayerCollide; }
        }

        public void Init(Data.TileMapData data)
        {
            mData = data;

            mLayerFloor = new TileLayer();
            mLayerFloor.Init(this, Width, Height);

            mLayerCollide = new TileLayer();
            mLayerCollide.Init(this, Width, Height);
        }

        public IEnumerator Enter()
        {
            if (mDisplay == null)
            {
                mDisplay = Game.Instance.DisplayFactory.GetTileMapDisplay(this);
            }

            yield return mDisplay.BeginEnter();

            foreach (Tile tile in mLayerFloor)
            {
                if (tile != null)
                {
                    tile.Parent = this;
                    tile.Layer = mLayerFloor;
                    yield return tile.Enter();
                }
            }

            foreach (Tile tile in mLayerCollide)
            {
                if (tile != null)
                {
                    tile.Parent = this;
                    tile.Layer = mLayerCollide;
                    yield return tile.Enter();
                }
            }

            yield return mDisplay.EndEnter();
        }

        public IEnumerator Exit()
        {
            yield return mDisplay.BeginExit();

            foreach (Tile tile in mLayerFloor)
            {
                if (tile != null)
                {
                    yield return tile.Exit();
                }
            }

            foreach (Tile tile in mLayerCollide)
            {
                if (tile != null)
                {
                    yield return tile.Exit();
                }
            }

            yield return mDisplay.EndExit();
        }
    }
}