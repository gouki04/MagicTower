using UnityEditor;
using UnityEngine;

namespace MagicTower.EditorData
{
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

        public void Init(int width, int height)
        {
            mWidth = width;
            mHeight = height;
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