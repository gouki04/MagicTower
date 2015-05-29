using UnityEngine;
using Utils;
using MagicTower.Logic;

namespace MagicTower.Editor
{
    public class TerrainList : SpriteList
    {
        private string[] mTileNames = new string[] { "floor_0", "wall_0", "water_0", "sky_0" };
        private Tile.EType[] mTileTypes = new Tile.EType[] { Tile.EType.Floor, Tile.EType.Wall, Tile.EType.Water, Tile.EType.Sky };

        public override Sprite GetSprite(int index)
        {
            return SpriteSheetManager.Instance[mTileNames[index]];
        }

        public static GameObject CreateTile(Tile.EType tile_type)
        {
            string tile_name = "";
            switch (tile_type)
            {
                case Tile.EType.Floor:
                    tile_name = "floor_0";
                    break;
                case Tile.EType.Wall:
                    tile_name = "wall_0";
                    break;
                case Tile.EType.Water:
                    tile_name = "water_0";
                    break;
                case Tile.EType.Sky:
                    tile_name = "sky_0";
                    break;
                default:
                    return null;
            }

            var obj = new GameObject(tile_name);

            var renderer = obj.AddComponent<SpriteRenderer>();
            renderer.sprite = SpriteSheetManager.Instance[tile_name];

            var tile = obj.AddComponent<EditorData.Tile>();
            tile.TileType = tile_type;

            return obj;
        }

        public override GameObject CreateTile(int index)
        {
            var tile_name = mTileNames[index];

            var obj = new GameObject(tile_name);

            var renderer = obj.AddComponent<SpriteRenderer>();
            renderer.sprite = GetSprite(index);

            var tile = obj.AddComponent<EditorData.Tile>();
            tile.TileType = mTileTypes[index];

            return obj;
        }
    }
}

