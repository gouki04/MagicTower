using UnityEngine;
using Utils;

namespace MagicTower.Editor
{
    public class ItemList : SpriteList
    {
        public static Sprite GetSpriteByItemId(uint item_id)
        {
            var sprite_name = CSVManager.Instance["item"][item_id]["sprite"];
            return SpriteSheetManager.Instance[sprite_name];
        }

        public override Sprite GetSprite(int index)
        {
            var item_id = this[index];
            return GetSpriteByItemId(item_id);
        }

        public override GameObject CreateTile(int index)
        {
            var item_id = this[index];

            var obj = new GameObject("Item_" + item_id);
            obj.layer = 1;

            var renderer = obj.AddComponent<SpriteRenderer>();
            renderer.sprite = GetSprite(index);

            var tile = obj.AddComponent<EditorData.Tile>();
            tile.TileType = Logic.Tile.EType.Item;
            tile.Properties["ItemId"] = item_id;

            return obj;
        }
    }
}
