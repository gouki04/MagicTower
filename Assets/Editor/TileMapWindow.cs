using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using Utils;

namespace MagicTower.Editor
{
    public class Selecting
    {
        public static object SelectedList;
        public static int SelectedIndex;
    }

    public abstract class SpriteList<T> : List<T>
    {
        public int Margin = 1;
        public int Border = 1;
        public int TileWidth = 32;
        public int TileHeight = 32;
        public int TilePerRow = 12;

        public abstract Sprite GetSprite(int index);

        public bool Draw()
        {
            int row = (int)Mathf.Ceil(Count / (float)TilePerRow);

            var rect = GUILayoutUtility.GetRect(Border * 2 + TileWidth * TilePerRow + Margin * (TilePerRow - 1),
                Border * 2 + TileHeight * row + Margin * (row - 1));

            rect.width = TileWidth;
            rect.height = TileHeight;

            var origin_x = rect.x;
            var origin_y = rect.y;

            bool selected_tile = false;
            for (int r = 0; r < row; ++r)
            {
                rect.x = origin_x + Border;
                rect.y = origin_y + Border + r * TileHeight;

                if (r > 0)
                    rect.y += (r - 1) * Margin;

                for (int c = 0; c < TilePerRow; ++c)
                {
                    var index = TilePerRow * r + c;
                    if (index >= Count)
                        break;

                    var sprite = GetSprite(index);

                    var texRect = new Rect(
                        sprite.textureRect.xMin / sprite.texture.width,
                        sprite.textureRect.yMin / sprite.texture.height,
                        sprite.textureRect.width / sprite.texture.width,
                        sprite.textureRect.height / sprite.texture.height);

                    if (Selecting.SelectedList == this && Selecting.SelectedIndex == index)
                        GUI.color = Color.yellow;
                    else
                        GUI.color = Color.white;

                    GUI.DrawTextureWithTexCoords(rect, sprite.texture, texRect);

                    Event e = Event.current;
                    if (e.type == EventType.MouseDown)
                    {
                        if (rect.Contains(e.mousePosition))
                        {
                            Selecting.SelectedList = this;
                            Selecting.SelectedIndex = index;
                        }

                        selected_tile = true;
                    }

                    rect.x += TileWidth + Margin;
                }
            }

            return selected_tile;
        }
    }

    public class MonsterList : SpriteList<uint>
    {
        public override Sprite GetSprite(int index)
        {
            var monster_id = this[index];
            var sprite_name = CSVManager.Instance["monster"][monster_id]["sprite"];
            return SpriteSheetManager.Instance[sprite_name];
        }
    }

    public class ItemList : SpriteList<uint>
    {
        public override Sprite GetSprite(int index)
        {
            var item_id = this[index];
            var sprite_name = CSVManager.Instance["item"][item_id]["sprite"];
            return SpriteSheetManager.Instance[sprite_name];
        }
    }

    public class TileMapWindow : EditorWindow
    {
        MonsterList mMonsterList;
        ItemList mItemList;

        [MenuItem("Window/TileMapWindow")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(TileMapWindow));
        }

        void OnEnable()
        {
            SpriteSheetManager.Instance.Load("tile");

            mMonsterList = new MonsterList();
            for (uint i = 1; i <= 30; ++i)
            {
                mMonsterList.Add(i);
            }

            mItemList = new ItemList();
            for (uint i = 1; i <= 7; ++i)
            {
                mItemList.Add(i);
            }
        }

        void OnGUI()
        {
            EditorGUILayout.LabelField("Monster List", EditorStyles.boldLabel);
            if (mMonsterList.Draw())
                Repaint();

            EditorGUILayout.LabelField("Item List", EditorStyles.boldLabel);
            if (mItemList.Draw())
                Repaint();
        }
    } 
}