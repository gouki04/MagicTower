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

        private bool PosField(Dictionary<string, object> properties, string key, out Logic.TilePosition pos)
        {
			EditorGUILayout.BeginVertical ();
            EditorGUILayout.LabelField(key);

            var origin = (Logic.TilePosition)properties[key];

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Row");
            pos.Row = (uint)EditorGUILayout.IntField((int)origin.Row);
			EditorGUILayout.EndHorizontal ();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Col");
            pos.Col = (uint)EditorGUILayout.IntField((int)origin.Col);
            EditorGUILayout.EndHorizontal();

			EditorGUILayout.EndVertical ();

            return pos.Row != origin.Row || pos.Col != origin.Col;
        }

        private uint IntField(Dictionary<string, object> properties, string key)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(key);
			uint origin = (uint)properties [key];
            uint result = (uint)EditorGUILayout.IntField((int)origin);
            EditorGUILayout.EndHorizontal();

            return result;
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
            else if (mTile.TileType == Logic.Tile.EType.Portal)
            {
				mTile.Properties["DestLevel"] = (uint)IntField(mTile.Properties, "DestLevel");

                Logic.TilePosition pos;
                if (PosField(mTile.Properties, "DestPos", out pos))
                {
                    mTile.Properties["DestPos"] = pos;
                }
            }
            else if (mTile.TileType == Logic.Tile.EType.Door)
            {
                
            }
        }
    }
}
