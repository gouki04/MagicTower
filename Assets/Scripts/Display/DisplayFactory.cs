using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicTower
{
    namespace Display
    {
        public interface DisplayFactory
        {
            GameDisplay GetGameDisplay(Logic.Game game);

            TileMapDisplay GetTileMapDisplay(Logic.TileMap tile_map);

            TileDisplay GetTileDisplay(Logic.Tile tile);
        } 
    }
}
