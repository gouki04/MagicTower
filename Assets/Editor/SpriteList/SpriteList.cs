using System.Collections.Generic;
using UnityEngine;

namespace MagicTower.Editor
{
    public class Selecting
    {
        public static SpriteList SelectedList;
        public static int SelectedIndex;
    }

    public abstract class SpriteList : List<uint>
    {
        public int Margin = 1;
        public int Border = 1;
        public int TileWidth = 32;
        public int TileHeight = 32;
        public int TilePerRow = 12;

        public abstract Sprite GetSprite(int index);

        public abstract GameObject CreateTile(int index);

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
}
