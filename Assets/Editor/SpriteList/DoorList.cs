using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Utils;

namespace MagicTower.Editor
{
    public class DoorList : SpriteList
    {
        public static string GetSpriteName(Logic.EDoorType door_type)
        {
            switch (door_type)
            {
                case Logic.EDoorType.Yellow:
                    return "door_yellow";
                case Logic.EDoorType.Blue:
                    return "door_blue";
                case Logic.EDoorType.Red:
                    return "door_red";
                case Logic.EDoorType.Trigger:
                    return "door_special";
                case Logic.EDoorType.Fences:
                    return "door_fences";
            }

            return string.Empty;
        }

        public static Sprite GetSprite(Logic.EDoorType door_type)
        {
            return SpriteSheetManager.Instance[GetSpriteName(door_type)];
        }

        public override Sprite GetSprite(int index)
        {
            if (index == 0)
                return GetSprite(Logic.EDoorType.Yellow);
            else if (index == 1)
                return GetSprite(Logic.EDoorType.Blue);
            else if (index == 2)
                return GetSprite(Logic.EDoorType.Red);
            else if (index == 3)
                return GetSprite(Logic.EDoorType.Fences);
            else if (index == 4)
                return GetSprite(Logic.EDoorType.Trigger);
            else
                return null;
        }

        public static GameObject CreateTile(Logic.EDoorType door_type)
        {
            var obj = new GameObject(GetSpriteName(door_type));
            obj.layer = 1;

            var renderer = obj.AddComponent<SpriteRenderer>();
            renderer.sprite = GetSprite(door_type);

            var tile = obj.AddComponent<EditorData.Tile>();
            tile.TileType = Logic.Tile.EType.Door;
            tile.Properties["DoorType"] = door_type;

            return obj;
        }

        public override GameObject CreateTile(int index)
        {
            if (index == 0)
                return CreateTile(Logic.EDoorType.Yellow);
            else if (index == 1)
                return CreateTile(Logic.EDoorType.Blue);
            else if (index == 2)
                return CreateTile(Logic.EDoorType.Red);
            else if (index == 3)
                return CreateTile(Logic.EDoorType.Fences);
            else if (index == 4)
                return CreateTile(Logic.EDoorType.Trigger);
            else
                return null;
        }
    }
}
