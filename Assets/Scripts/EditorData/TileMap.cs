using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
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
        public int Level;

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

            var sprite_renderer = tile.GetComponent<SpriteRenderer>();
            if (sprite_renderer != null)
            {
                sprite_renderer.sortingLayerName = layer_type == ETileMapLayer.Floor ? "Floor" : "Collide";
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

        #region serialization

        private int _parseTerrainTile(GameObject gameobject)
        {
            return (int)gameobject.GetComponent<Tile>().TileType;
        }

        private int[,] _parseFloorLayer()
        {
            var floor_layer = new int[Height, Width];

            for (int r = 0; r < Height; ++r)
            {
                for (int c = 0; c < Width; ++c)
                {
                    var go = mLayerFloor[r, c];
                    if (go != null)
                        floor_layer[r, c] = _parseTerrainTile(go);
                    else
                        floor_layer[r, c] = 0;
                }
            }

            return floor_layer;
        }

        private Data.MonsterData[] _parseMonsters()
        {
            var monsters = new List<Data.MonsterData>();

            for (uint r = 0; r < Height; ++r)
            {
                for (uint c = 0; c < Width; ++c)
                {
                    var go = mLayerCollide[r, c];
                    if (go == null)
                        continue;

                    var tile = go.GetComponent<Tile>();
                    if (tile.TileType == Logic.Tile.EType.Monster)
                    {
                        var data = new Data.MonsterData();
                        data.Row = r;
                        data.Col = c;
                        data.Id = (uint)tile.Properties["MonsterId"];

                        monsters.Add(data);
                    }
                }
            }

            return monsters.ToArray();
        }

        public void Save(string file_path)
        {
            var tile_map_data = new Data.TileMapData();
            tile_map_data.Level = Level;
            tile_map_data.Width = Width;
            tile_map_data.Height = Height;

            tile_map_data.FloorLayer = _parseFloorLayer();
            tile_map_data.MonsterDatas = _parseMonsters();

            using (var file = new FileStream(file_path, FileMode.Create))
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(file, tile_map_data);
            }
        }

        #endregion
    }
}