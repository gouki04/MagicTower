using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace mt
{
    public class Game : Singleton<Game>
    {
        private TileMapComponent mTileMap;
        public TileMapComponent TileMapObj
        {
            get { return mTileMap; }
        }

        private TileComponent mPlayer;
        public TileComponent PlayerObj
        {
            get { return mPlayer; }
        }

        public Game()
        {

        }

        public void Init(GameObject tile_map_obj, GameObject player_obj_)
        {
            mTileMap = tile_map_obj.AddComponent<TileMapComponent>();

            var player_obj = TileObjFactory.Instance.CreatePlayer(TileFactory.Instance.CreatePlayer());
            mPlayer = player_obj.GetComponent<TileComponent>();

            //mPlayer = player_obj;
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
            TileMapBuilder.Instance.BuildTileMap(mTileMap.gameObject, map);

            // setup the player
            var tile_player = mPlayer.GetTileData<Tile_Player>();
            mTileMap.TileMapData.LayerCollide[row, col] = tile_player;

            return mTileMap.gameObject;
        }
    }
}
