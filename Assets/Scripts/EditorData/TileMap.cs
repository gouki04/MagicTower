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

    /// <summary>
    /// 地图编辑器里使用的地图
    /// </summary>
    public class TileMap : MonoBehaviour
    {
        public uint Level;

        private uint mWidth = 11;
        public uint Width
        {
            get { return mWidth; }
        }

        private uint mHeight = 11;
        public uint Height
        {
            get { return mHeight; }
        }

        private GameObject[,] mLayerFloor;
        private GameObject[,] mLayerCollide;

        public void Init(uint width, uint height)
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

        private Logic.Tile.EType _parseTerrainTile(GameObject gameobject)
        {
            return gameobject.GetComponent<Tile>().TileType;
        }

        private Logic.Tile.EType[,] _parseFloorLayer()
        {
            var floor_layer = new Logic.Tile.EType[Height, Width];

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

        private List<Data.MonsterData> _parseMonsters()
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
                        data.Pos = new Logic.TilePosition(r, c);
                        data.Id = (uint)tile.Properties["MonsterId"];

                        monsters.Add(data);
                    }
                }
            }

            return monsters;
        }

        public void Save(string file_path)
        {
            var tile_map_data = new Data.TileMapData();
            tile_map_data.Level = Level;
            tile_map_data.Width = Width;
            tile_map_data.Height = Height;

            tile_map_data.FloorLayer = _parseFloorLayer();
            tile_map_data.MonsterDatas = new List<Data.MonsterData>();
            tile_map_data.PortalDatas = new List<Data.PortalData>();
            tile_map_data.ItemDatas = new List<Data.ItemData>();
			tile_map_data.DoorDatas = new List<Data.DoorData> ();

            for (uint r = 0; r < Height; ++r)
            {
                for (uint c = 0; c < Width; ++c)
                {
                    var go = mLayerCollide[r, c];
                    if (go == null)
                        continue;

                    var tile = go.GetComponent<Tile>();
                    switch (tile.TileType)
                    {
                        case Logic.Tile.EType.Monster:
                            {
                                var data = new Data.MonsterData();
                                data.Pos = new Logic.TilePosition(r, c);
                                data.Id = (uint)tile.Properties["MonsterId"];

                                tile_map_data.MonsterDatas.Add(data);
                                break;
                            }
                        case Logic.Tile.EType.Portal:
                            {
                                var data = new Data.PortalData();
                                data.Pos = new Logic.TilePosition(r, c);
                                data.DestinationLevel = (uint)tile.Properties["DestLevel"];
                                data.DestinationPosition = (Logic.TilePosition)tile.Properties["DestPos"];

                                tile_map_data.PortalDatas.Add(data);

                                break;
                            }
                        case Logic.Tile.EType.Item:
                            {
                                var data = new Data.ItemData();
                                data.Pos = new Logic.TilePosition(r, c);
                                data.Id = (uint)tile.Properties["ItemId"];

                                tile_map_data.ItemDatas.Add(data);
                                break;
                            }
                        case Logic.Tile.EType.Door:
                            {
                                var data = new Data.DoorData();
                                data.Pos = new Logic.TilePosition(r, c);
                                data.DoorType = (Logic.EDoorType)tile.Properties["DoorType"];

                                tile_map_data.DoorDatas.Add(data);
                                break;
                            }
                    }
                }
            }

            //tile_map_data.MonsterDatas = new List<Data.MonsterData>();
            //tile_map_data.PortalDatas = new List<Data.PortalData>();
            //tile_map_data.ItemDatas = new List<Data.ItemData>();
            //tile_map_data.DoorDatas = new List<Data.DoorData>();

            var content = LitJson.JsonMapper.ToJson(tile_map_data);
            //var content = Newtonsoft.JsonConvert.SerializeObject(tile_map_data);
            using (var file = new StreamWriter(file_path, false))
            {
                file.Write(content);
            }
            //File.WriteAllText(file_path, content);
            //using (var file = File.Create(file_path))
            //{
            //    var content = JsonConvert.SerializeObject(tile_map_data);
            //    file.
            //    //formatter.Serialize(file, tile_map_data);
            //}
        }

        #endregion
    }
}