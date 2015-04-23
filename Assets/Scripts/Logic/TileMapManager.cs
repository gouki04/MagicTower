using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace MagicTower.Logic
{
	public class TileMapManager : Singleton<TileMapManager>
	{
		private Dictionary<uint, TileMap> mMaps;
	
        //public TileMap GetMap(uint lv)
        //{
        //    TileMap map = null;
        //    if (mMaps.TryGetValue(lv, out map))
        //        return map;
        //    else
        //        return LoadMap(lv);
        //}
	
		public bool HasLoaded(uint lv)
		{
			TileMap map = null;
			if (mMaps.TryGetValue (lv, out map))
				return true;
			
			return false;
		}
	
		public TileMapManager ()
		{
			mMaps = new Dictionary<uint, TileMap> ();
		}
	
		private int parseMapSize(string[] content, int cur, out uint width, out uint height)
		{
			++cur;
	
			var tmp = content[cur++].Split (new char[] {','});
			width = Convert.ToUInt32 (tmp [0]);
			height = Convert.ToUInt32 (tmp [1]);
	
			return cur;
		}
	
		private int parseLayer(string[] content, int cur, TileLayer layer)
		{
			++cur;
	
			var width = layer.Width;
			var height = layer.Height;
	
			for (int r = 0; r < height; ++r, ++cur) {
				for (int c = 0; c < width; ++c) {
					var tile_char = content[cur][c];
					var tile = TileFactory.Instance.CreateTile(tile_char);
					if (tile != null) {
						layer[(uint)(height - r - 1), (uint)c] = tile;
					}
				}
			}
	
			return cur;
		}
	
		private int parseMonter(string[] content, int cur, TileLayer layer)
		{
			++cur;
	
			int count = Convert.ToInt32 (content [cur++]);
			for (int i = 0; i < count; ++i, ++cur) {
                var monster_info = content[cur].Split(new char[] { ',' });
                var row = Convert.ToUInt32(monster_info[0]);
                var col = Convert.ToUInt32(monster_info[1]);
                var monster_id = Convert.ToUInt32(monster_info[2]);

                var monster_tile = TileFactory.Instance.CreateMonster(monster_id);
                layer[row, col] = monster_tile;
			}
	
			return cur;
		}
	
		private int parseNpc(string[] content, int cur, TileLayer layer)
		{
			++cur;
			
			int count = Convert.ToInt32 (content [cur++]);
			for (int i = 0; i < count; ++i, ++cur) {
				// TODO add npc
			}
			
			return cur;
		}
	
		private int parseItem(string[] content, int cur, TileLayer layer)
		{
			++cur;
			
			int count = Convert.ToInt32 (content [cur++]);
			for (int i = 0; i < count; ++i, ++cur) {
				// TODO add item
			}
			
			return cur;
		}
	
		private int parsePortal(string[] content, int cur, TileLayer layer)
		{
			++cur;
			
			int count = Convert.ToInt32 (content [cur++]);
			for (int i = 0; i < count; ++i, ++cur) {
	
			}
			
			return cur;
		}
	
		public IEnumerator LoadMap(uint lv)
		{
			var asset = Resources.Load ("level" + lv) as TextAsset;
			var content = asset.text.Split (new char[] {'\n'}, StringSplitOptions.RemoveEmptyEntries);
	
			int cur_idx = 0;
	
			// map size
			uint width, height;
			cur_idx = parseMapSize (content, cur_idx, out width, out height);

            var map = new TileMap();
			map.Init (width, height);
	
			// layer 0
			cur_idx = parseLayer (content, cur_idx, map.LayerFloor);
	
			// layer 1
			cur_idx = parseLayer (content, cur_idx, map.LayerCollide);
	
			// monster
			cur_idx = parseMonter (content, cur_idx, map.LayerCollide);
	
			// npc
			cur_idx = parseNpc (content, cur_idx, map.LayerCollide);
	
			// item
			cur_idx = parseItem (content, cur_idx, map.LayerCollide);
	
			// portal
			cur_idx = parsePortal (content, cur_idx, map.LayerCollide);
	
			yield return map;
		}
	}
}


