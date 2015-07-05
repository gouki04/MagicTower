using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Utils;

namespace MagicTower.Editor
{
    public class PortalList : SpriteList
    {
		public static Sprite GetSprite(Logic.EProtalDirection direction)
		{
			return SpriteSheetManager.Instance[direction == Logic.EProtalDirection.Down ? "stair_down" : "stair_up"];
		}

        public override Sprite GetSprite(int index)
        {
            if (index == 0)
				return GetSprite (Logic.EProtalDirection.Down);
            else if (index == 1)
				return GetSprite (Logic.EProtalDirection.Up);
            else
                return null;
        }

		public static GameObject CreateTile(Logic.EProtalDirection direction, uint dst_lv, Logic.TilePosition dst_pos)
		{
			var obj = new GameObject(direction == Logic.EProtalDirection.Down ? "stair_down" : "stair_up");
			obj.layer = 1;
			
			var renderer = obj.AddComponent<SpriteRenderer>();
			renderer.sprite = GetSprite(direction);
			
			var tile = obj.AddComponent<EditorData.Tile>();
			tile.TileType = Logic.Tile.EType.Portal;
			tile.Properties["Direction"] = direction;
			tile.Properties["DestLevel"] = dst_lv;
			tile.Properties["DestPos"] = dst_pos;
			
			return obj;
		}
		
		public override GameObject CreateTile(int index)
		{
			if (index == 0)
            {
				return CreateTile(Logic.EProtalDirection.Down, 1, new Logic.TilePosition(0, 0));
            }
            else
            {
				return CreateTile(Logic.EProtalDirection.Up, 1, new Logic.TilePosition(0, 0));
            }
        }
    }
}
