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

        public override GameObject CreateTile(int index)
        {
            var tile_name = mTileNames[index];

            var obj = new GameObject(tile_name);
            obj.layer = 0;

            var renderer = obj.AddComponent<SpriteRenderer>();
            renderer.sprite = GetSprite(index);

            var tile = obj.AddComponent<EditorData.Tile>();
            tile.TileType = mTileTypes[index];

            return obj;
        }
    }
}

