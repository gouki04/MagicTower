using UnityEditor;
using UnityEngine;
using MagicTower;
using Utils;
using System.Collections.Generic;

namespace MagicTower.Editor
{
    public abstract class SpriteList<T> : List<T>
    {
        public int Margin = 1;
        public int Border = 1;
        public int TileWidth = 32;
        public int TileHeight = 32;
        public int TilePerRow = 12;

        public int SelectedIndex = -1;

        protected abstract Sprite GetSprite(int index);

        public bool Draw()
        {
            var sheet = SpriteSheetManager.Instance;

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

                    if (SelectedIndex == index)
                        GUI.color = Color.yellow;
                    else
                        GUI.color = Color.white;

                    GUI.DrawTextureWithTexCoords(rect, sprite.texture, texRect);

                    Event e = Event.current;
                    if (e.type == EventType.MouseDown)
                    {
                        if (rect.Contains(e.mousePosition))
                            SelectedIndex = index;

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
        protected override Sprite GetSprite(int index)
        {
            var monster_id = this[index];
            var sprite_name = CSVManager.Instance["monster"][monster_id]["sprite"];
            return SpriteSheetManager.Instance[sprite_name];
        }
    }

    public class ItemList : SpriteList<uint>
    {
        protected override Sprite GetSprite(int index)
        {
            var item_id = this[index];
            var sprite_name = CSVManager.Instance["item"][item_id]["sprite"];
            return SpriteSheetManager.Instance[sprite_name];
        }
    }

    [CustomEditor(typeof(EditorData.TileMap))]
    public class TileMapEditor : UnityEditor.Editor
    {
        EditorData.TileMap tilemap;
        MonsterList mMonsterList;
        ItemList mItemList;

        public void OnEnable()
        {
            tilemap = (EditorData.TileMap)target;

            SceneView.onSceneGUIDelegate += TileMapUpdate;

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

        void TileMapUpdate(SceneView scene_view)
        {
            Event e = Event.current;
            if (e.type == EventType.MouseDown)
            {
                Ray r = Camera.current.ScreenPointToRay(
                                new Vector3(e.mousePosition.x, -e.mousePosition.y + Camera.current.pixelHeight));

                var mousePos = r.origin;

                var local_pos = tilemap.gameObject.transform.InverseTransformPoint(mousePos);
                local_pos.x = Mathf.Floor(local_pos.x + 0.5f);
                local_pos.y = Mathf.Floor(local_pos.y + 0.5f);
                local_pos.z = 0.0f;

                if (local_pos.x < 0 || local_pos.x >= tilemap.Width || local_pos.y < 0 || local_pos.y >= tilemap.Height)
                    return;

                var obj = new GameObject("Tile");

                var renderer = obj.AddComponent<SpriteRenderer>();
                renderer.sprite = SpriteSheetManager.Instance.GetSprite("wall_0");

                obj.transform.parent = tilemap.transform;
                obj.transform.localPosition = local_pos;
            }
        }

        public override void OnInspectorGUI()
        {
            var sheet = SpriteSheetManager.Instance;

            EditorGUILayout.LabelField("Monster List", EditorStyles.boldLabel);
            if (mMonsterList.Draw())
                Repaint();

            EditorGUILayout.LabelField("Item List", EditorStyles.boldLabel);
            if (mItemList.Draw())
                Repaint();

            SceneView.RepaintAll();
        }
    } 
}