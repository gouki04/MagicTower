using SafeCoroutine;
using System;
using System.Collections;
using UnityEngine;

namespace MagicTower
{
    namespace Logic
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

            /// <summary>
            /// 地图宽度
            /// </summary>
            private uint mWidth;
            public uint Width
            {
                get { return mWidth; }
            }

            /// <summary>
            /// 地图高度
            /// </summary>
            private uint mHeight;
            public uint Height
            {
                get { return mHeight; }
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

            public void Init(uint width, uint height)
            {
                mWidth = width;
                mHeight = height;

                mLayerFloor = new TileLayer();
                mLayerFloor.Init(this, width, height);

                mLayerCollide = new TileLayer();
                mLayerCollide.Init(this, width, height);
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
}
