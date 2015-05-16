using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace MagicTower.Editor
{
    [CustomEditor(typeof(EditorData.Tile))]
    public class TileEditor : UnityEditor.Editor
    {
        private EditorData.Tile mTile;

        private void OnEnable()
        {
            mTile = (EditorData.Tile)target;
        }

        private bool IdField(Dictionary<string, object> properties, string key, int[] options, out uint value)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(key);

            var origin = (uint)properties[key];

            string[] option_strs = new string[options.Length];
            for (int i = 0; i < options.Length; ++i )
            {
                option_strs[i] = options[i].ToString();
            }

            value = (uint)EditorGUILayout.IntPopup((int)origin, option_strs, options);

            EditorGUILayout.EndHorizontal();

            return value != origin;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (mTile.TileType == Logic.Tile.EType.Monster)
            {
                uint monster_id;
                var options = new int[31];
                for (int i = 1; i <= 31; ++i)
                    options[i - 1] = i;

                if (IdField(mTile.Properties, "MonsterId", options, out monster_id))
                {
                    mTile.Properties["MonsterId"] = monster_id;
                    mTile.GetComponent<SpriteRenderer>().sprite = MonsterList.GetSpriteByMonserId(monster_id);
                }
            }
            else if (mTile.TileType == Logic.Tile.EType.Item)
            {
                uint item_id;
                var options = new int[7];
                for (int i = 1; i <= 7; ++i)
                    options[i - 1] = i;

                if (IdField(mTile.Properties, "ItemId", options, out item_id))
                {
                    mTile.Properties["ItemId"] = item_id;
                    mTile.GetComponent<SpriteRenderer>().sprite = ItemList.GetSpriteByItemId(item_id);
                }
            }
        }
    }
}
