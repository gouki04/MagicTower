using UnityEngine;
using Utils;

namespace MagicTower.Editor
{
    public class MonsterList : SpriteList
    {
        public static Sprite GetSpriteByMonserId(uint monster_id)
        {
            var sprite_name = CSVManager.Instance["monster"][monster_id]["sprite"];
            return SpriteSheetManager.Instance[sprite_name];
        }

        public override Sprite GetSprite(int index)
        {
            var monster_id = this[index];
            return GetSpriteByMonserId(monster_id);
        }

        public override GameObject CreateTile(int index)
        {
            var monster_id = this[index];

            var obj = new GameObject("Monster_" + monster_id);
            obj.layer = 1;

            var renderer = obj.AddComponent<SpriteRenderer>();
            renderer.sprite = GetSprite(index);

            var tile = obj.AddComponent<EditorData.Tile>();
            tile.TileType = Logic.Tile.EType.Monster;
            tile.Properties["MonsterId"] = monster_id;

            return obj;
        }
    }
}
