using System;
using UnityEngine;
using Utils;

namespace MagicTower.Logic
{
    public class TileFactory : Singleton<TileFactory>
    {
        public enum ETileChar
        {
            Floor = '1',
            Wall = '2',
            Water = '3',
            Sky = '4',
            RedDoor = 'R',
            BlueDoor = 'B',
            YellowDoor = 'Y',
            FencesDoor = 'D',
            TriggerDoor = 'S',
        }

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

        public Tile CreateTile(char tile_char)
        {
            var tile_type = (ETileChar)tile_char;
            switch (tile_type)
            {
                case ETileChar.Floor:
                    {
                        return CreateTerrainTile(Tile.EType.Floor);
                    }
                case ETileChar.Wall:
                    {
                        return CreateTerrainBlockTile(Tile.EType.Wall);
                    }
                case ETileChar.Water:
                    {
                        return CreateTerrainBlockTile(Tile.EType.Water);
                    }
                case ETileChar.Sky:
                    {
                        return CreateTerrainBlockTile(Tile.EType.Sky);
                    }
                case ETileChar.RedDoor:
                    {
                        return CreateDoor(EDoorType.Red);
                    }
                case ETileChar.BlueDoor:
                    {
                        return CreateDoor(EDoorType.Blue);
                    }
                case ETileChar.YellowDoor:
                    {
                        return CreateDoor(EDoorType.Yellow);
                    }
                case ETileChar.FencesDoor:
                    {
                        return CreateDoor(EDoorType.Fences);
                    }
                case ETileChar.TriggerDoor:
                    {
                        return CreateDoor(EDoorType.Trigger);
                    }
                default:
                    {
                        return null;
                    }
            }
        }
    }
}

