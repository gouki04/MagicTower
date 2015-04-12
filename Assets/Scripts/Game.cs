using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace mt
{
    public class Game : Singleton<Game>
    {
        private GameObject mTileMap;
        public GameObject TileMapObj
        {
            get { return mTileMap; }
        }

        private GameObject mPlayer;
        public GameObject PlayerObj
        {
            get { return mPlayer; }
        }

        public Game()
        {

        }

        public void Init(GameObject tile_map_obj, GameObject player_obj)
        {
            mTileMap = tile_map_obj;
            mPlayer = player_obj;
        }

        public GameObject EnterMap(uint map_id, uint row, uint col)
        {
            // find the map data
            var map = TileMapManager.Instance.GetMap(map_id);
            if (map == null)
            {
                throw new Exception(string.Format("Load Map ({0}) Failed!", map_id));
            }

            // build the tile map obj
            TileMapBuilder.Instance.BuildTileMap(mTileMap, map);

            // setup the player
            // TODO

            return mTileMap;
        }
    }
}
