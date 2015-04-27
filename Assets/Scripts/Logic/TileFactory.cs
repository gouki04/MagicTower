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
            var player = new Tile_Player();
            player.IsBlock = false;

            return player;
        }

        public Tile CreateMonster(uint id)
        {
            var monster = new Tile_Monster(id);
            monster.IsBlock = false;

            return monster;
        }

        public Tile_Portal CreatePortal()
        {
            var portal = new Tile_Portal();
            portal.IsBlock = false;

            return portal;
        }

        public Tile_Npc CreateNpc(uint id)
        {
            var npc = new Tile_Npc(id);
            npc.IsBlock = false;

            return npc;
        }

        public Tile_Item CreateItem(uint id)
        {
            var item = new Tile_Item(id);
            item.IsBlock = false;

            return item;
        }

        public Tile CreateTerrainTile(Tile.EType type)
        {
            var tile = new Tile(type);
            tile.IsBlock = false;

            return tile;
        }

        public Tile CreateTerrainBlockTile(Tile.EType type)
        {
            var tile = new Tile(type);
            tile.IsBlock = true;

            return tile;
        }

        public Tile CreateDoor(EDoorType doorType)
        {
            var tile = new Tile_Door(doorType);
            tile.IsBlock = false;

            return tile;
        }

        public Tile CreateTile(char tile_type)
        {
            if (tile_type == '1')
            {
                return CreateTerrainTile(Tile.EType.Floor);
            }
            else if (tile_type == '2')
            {
                return CreateTerrainBlockTile(Tile.EType.Wall);
            }
            else if (tile_type == '3')
            {
                return CreateTerrainBlockTile(Tile.EType.Water);
            }
            else if (tile_type == '4')
            {
                return CreateTerrainBlockTile(Tile.EType.Sky);
            }
            else if (tile_type == 'R')
            {
                return CreateDoor(EDoorType.Red);
            }
            else if (tile_type == 'B')
            {
                return CreateDoor(EDoorType.Blue);
            }
            else if (tile_type == 'Y')
            {
                return CreateDoor(EDoorType.Yellow);
            }
            else if (tile_type == 'D')
            {
                return CreateDoor(EDoorType.Fences);
            }
            else if (tile_type == 'S')
            {
                return CreateDoor(EDoorType.Trigger);
            }
            else
            {
                return null;
            }
        }
    }
}

