using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using Utils;

namespace MagicTower.Logic
{
	public class TileMapManager : Singleton<TileMapManager>
	{
		private Dictionary<uint, Data.TileMapData> mMaps;
	
		public bool HasLoaded(uint lv)
		{
            Data.TileMapData map = null;
			if (mMaps.TryGetValue (lv, out map))
				return true;
			
			return false;
		}

        public Data.TileMapData GetLoadedMapData(uint lv)
        {
            Data.TileMapData map = null;
            if (mMaps.TryGetValue(lv, out map))
                return map;

            return null;
        }
	
		public TileMapManager ()
		{
            mMaps = new Dictionary<uint, Data.TileMapData>();
		}

        public IEnumerator LoadMap(uint lv)
        {
            Data.TileMapData tile_map_data = GetLoadedMapData(lv);
            if (tile_map_data == null)
            {
                var async = Resources.LoadAsync("level" + lv);
                yield return async;

                var asset = async.asset as TextAsset;

                using (var stream = new MemoryStream(asset.bytes))
                {
                    var formatter = new BinaryFormatter();
                    tile_map_data = formatter.Deserialize(stream) as Data.TileMapData;

                    mMaps[lv] = tile_map_data;
                }
            }

            var map = new TileMap();
            map.Init(tile_map_data);

            yield return map.LoadFromData();
                
            yield return map;
        }
	}
}


