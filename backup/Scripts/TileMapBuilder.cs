using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace mt
{
    public class TileMapBuilder : Singleton<TileMapBuilder>
    {
        public TileMapBuilder()
        {

        }

        private void _buildTileMapLayer(GameObject tile_map_obj, TileLayer layer)
        {
            for (uint r = 0; r < layer.Height; ++r)
            {
                for (uint c = 0; c < layer.Width; ++c)
                {
                    var tile = layer[r, c];
                    if (tile != null)
                    {
                        var tile_obj = TileObjFactory.Instance.CreateTile(tile);
                        if (tile_obj != null)
                        {
                            tile_obj.transform.parent = tile_map_obj.transform;
                        }
                    }
                }
            }
        }

        public GameObject BuildTileMap(GameObject tile_map_obj, TileMap tile_map_data)
        {
            if (tile_map_obj == null)
            {
                tile_map_obj = new GameObject();
            }

            // set the tile map data
            var tile_map_component = tile_map_obj.GetComponent<TileMapComponent>();
            tile_map_component.TileMapData = tile_map_data;

            // build floor layer
            _buildTileMapLayer(tile_map_obj, tile_map_data.LayerFloor);
            
            // build collide layer
            _buildTileMapLayer(tile_map_obj, tile_map_data.LayerCollide);

            // TODO build monster, items, npcs, events, etc

            return tile_map_obj;
        }
    }
}
