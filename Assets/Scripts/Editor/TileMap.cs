using UnityEditor;
using UnityEngine;

namespace MagicTower.EditorData
{
    public class Tile : MonoBehaviour
    {
        public Logic.Tile.EType TileType;
        public object[] Properties = null;
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

        public void SetTile(int r, int c, GameObject tile)
        {
            var layer = tile.layer == 1 ? mLayerCollide : mLayerFloor;

            var org_tile = layer[r, c];
            if (org_tile != null)
            {
                Destroy(org_tile);
            }

            tile.transform.parent = transform;
            tile.transform.localPosition = new Vector3(c, r, 0);

            layer[r, c] = tile;
        }

        void OnDrawGizmos()
        {
            Vector3 pos = gameObject.transform.position;

            for (float y = 0; y <= Height; ++y)
            {
                Gizmos.DrawLine(new Vector3(pos.x - 0.5f, pos.y + y - 0.5f, 0.0f),
                                new Vector3(pos.x - 0.5f + Width, pos.y + y - 0.5f, 0.0f));
            }

            for (float x = 0; x <= Width; ++x)
            {
                Gizmos.DrawLine(new Vector3(pos.x + x - 0.5f, pos.y - 0.5f, 0.0f),
                                new Vector3(pos.x + x - 0.5f, pos.y + Height - 0.5f, 0.0f));
            }
        }
    }  
}