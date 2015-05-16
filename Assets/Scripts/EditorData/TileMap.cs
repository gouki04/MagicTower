using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MagicTower.EditorData
{
    public enum ETileMapLayer
    {
        Floor = 0,
        Collide = 1
    }

    public class TileMap : MonoBehaviour
    {
        private int mWidth = 11;
        public int Width
        {
            get { return mWidth; }
        }

        private int mHeight = 11;
        public int Height
        {
            get { return mHeight; }
        }

        private GameObject[,] mLayerFloor;
        private GameObject[,] mLayerCollide;

        public void Init(int width, int height)
        {
            mWidth = width;
            mHeight = height;

            mLayerFloor = new GameObject[height, width];
            mLayerCollide = new GameObject[height, width];
        }

        public void SetTile(int r, int c, GameObject tile, ETileMapLayer layer_type)
        {
            var layer = layer_type == ETileMapLayer.Floor ? mLayerFloor : mLayerCollide;

            var org_tile = layer[r, c];
            if (org_tile != null)
            {
                DestroyImmediate(org_tile);
            }

            tile.transform.parent = transform;
            tile.transform.localPosition = new Vector3(c, r, 0);

            layer[r, c] = tile;
        }

        public void RemoveTile(int r, int c, ETileMapLayer layer_type)
        {
            var layer = layer_type == ETileMapLayer.Floor ? mLayerFloor : mLayerCollide;

            var org_tile = layer[r, c];
            if (org_tile != null)
                DestroyImmediate(org_tile);

            layer[r, c] = null;
        }

        void OnDrawGizmos()
        {
            Vector3 pos = gameObject.transform.position;

            for (float y = 0; y <= Height; ++y)
            {
                Gizmos.DrawLine(new Vector3(pos.x, pos.y + y, 0.0f),
                                new Vector3(pos.x + Width, pos.y + y, 0.0f));
            }

            for (float x = 0; x <= Width; ++x)
            {
                Gizmos.DrawLine(new Vector3(pos.x + x, pos.y, 0.0f),
                                new Vector3(pos.x + x, pos.y + Height, 0.0f));
            }
        }
    }  
}