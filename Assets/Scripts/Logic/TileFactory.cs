using System;
using UnityEngine;
using Utils;

namespace MagicTower.Logic
{
    public class TileFactory : Singleton<TileFactory>
    {
        public TileFactory()
        {
            
        }

        public Tile_Player CreatePlayer()
        {
            return new Tile_Player();
        }

        public Tile CreateMonster(uint id)
        {
            //return new Tile_Monster(id);
            return null;
        }

        public Tile CreateTile(char tile_type)
        {
            if (tile_type == '1')
            {
                return new Tile(Tile.EType.Floor);
            }
            else if (tile_type == '2')
            {
                return new Tile(Tile.EType.Wall);
            }
            else if (tile_type == '3')
            {
                return new Tile(Tile.EType.Water);
            }
            else if (tile_type == '4')
            {
                return new Tile(Tile.EType.Sky);
            }
            else
            {
                return null;
            }
        }
    }
}

