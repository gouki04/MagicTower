
using UnityEditorInternal;
using UnityEngine;
using Utils;

namespace MagicTower.Display
{
    public interface IDisplayFactory
    {
        IGameDisplay GetGameDisplay(Logic.Game game);

        ITileMapDisplay GetTileMapDisplay(Logic.TileMap tile_map);

        IBattleDisplay GetBattleDisplay(Logic.Battle battle);

        ITileDisplay GetTileDisplay(Logic.Tile tile);
    }

    public class DisplayFactory : IDisplayFactory
    {

        private GameObject mTileMapObj;
        private GameObject mBattleObj;

        public IGameDisplay GetGameDisplay(Logic.Game game)
        {
            var game_game_object = new GameObject("Game");
            var game_display = game_game_object.AddComponent<GameDisplay>();

            return game_display;
        }

        public ITileMapDisplay GetTileMapDisplay(Logic.TileMap tile_map)
        {
            if (mTileMapObj == null)
            {
                mTileMapObj = new GameObject("TileMap");
                mTileMapObj.transform.localPosition = new Vector3(0, -5, 0);
            }

            var tile_map_display = mTileMapObj.AddComponent<TileMapDisplay>();
            return tile_map_display;
        }

        public IBattleDisplay GetBattleDisplay(Logic.Battle battle)
        {
            if (mBattleObj == null)
            {
                mBattleObj = new GameObject("Battle");
                mBattleObj.transform.localPosition = new Vector3(0, -5, 0);
            }

            var battle_display = mBattleObj.AddComponent<BattleDisplay>();
            return battle_display;
        }

        public ITileDisplay GetTileDisplay(Logic.Tile tile)
        {
            switch (tile.Type)
            {
                case Logic.Tile.EType.Floor:
                    {
                        return CreateFloor(tile).GetComponent<TileDisplay>();
                    }
                case Logic.Tile.EType.Wall:
                    {
                        return CreateWall(tile).GetComponent<TileDisplay>();
                    }
                case Logic.Tile.EType.Water:
                    {
                        return CreateWater(tile).GetComponent<TileDisplay>();
                    }
                case Logic.Tile.EType.Sky:
                    {
                        return CreateSky(tile).GetComponent<TileDisplay>();
                    }
                case Logic.Tile.EType.Monster:
                    {
                        return CreateMonster(tile).GetComponent<TileDisplay>();
                    }
                case Logic.Tile.EType.Player:
                    {
                        return CreatePlayer(tile).GetComponent<TileDisplay>();
                    }
                case Logic.Tile.EType.Portal:
                    {
                        return CreatePortal(tile).GetComponent<TileDisplay>();
                    }
                case Logic.Tile.EType.Npc:
                    {
                        return CreateNpc(tile).GetComponent<TileDisplay>();
                    }
                case Logic.Tile.EType.Item:
                    {
                        return CreateItem(tile).GetComponent<TileDisplay>();
                    }
                case Logic.Tile.EType.Door:
                    {
                        return CreateDoor(tile).GetComponent<TileDisplay>();
                    }
                default:
                    return null;
            }
        }

        /// <summary>
        /// 创建基本的tile
        /// </summary>
        /// <param name="tile_data">tile数据</param>
        /// <param name="sprite_name"></param>
        /// <param name="sorting_layer_name"></param>
        /// <returns></returns>
        private GameObject _createBasicTile(Logic.Tile tile_data, string sprite_name, string sorting_layer_name)
        {
            // create the gameobject
            var obj = new GameObject(tile_data.Type.ToString());

            // set the sprite
            var sprite_renderer = obj.AddComponent<SpriteRenderer>();
            sprite_renderer.sprite = SpriteSheetManager.Instance.GetSprite(sprite_name);

            // set the sorting layer
            sprite_renderer.sortingLayerName = sorting_layer_name;

            // set the tile data
            var tile_component = obj.AddComponent<TileDisplay>();
            tile_component.Tile = tile_data;

            // set the position
            obj.transform.parent = mTileMapObj.transform;
            obj.transform.localPosition = new Vector3(tile_data.Position.Col, tile_data.Position.Row, 0);

            return obj;
        }

        /// <summary>
        /// 创建基本的阻挡地形tile
        /// </summary>
        /// <param name="tile_data">tile数据</param>
        /// <param name="sprite_name"></param>
        /// <param name="sorting_layer_name"></param>
        /// <returns></returns>
        private GameObject _createTerrainBlockTile(Logic.Tile tile_data, string sprite_name, string sorting_layer_name)
        {
            var obj = _createBasicTile(tile_data, sprite_name, sorting_layer_name);

            // add collider
            var collider_component = obj.AddComponent<BoxCollider2D>();
            collider_component.isTrigger = false;

            return obj;
        }

        /// <summary>
        /// 创建地板
        /// </summary>
        /// <param name="tile_data"></param>
        /// <returns></returns>
        public GameObject CreateFloor(Logic.Tile tile_data)
        {
            return _createBasicTile(tile_data, "floor_0", "Floor");
        }

        /// <summary>
        /// 创建墙壁
        /// </summary>
        /// <param name="tile_data"></param>
        /// <returns></returns>
        public GameObject CreateWall(Logic.Tile tile_data)
        {
            return _createTerrainBlockTile(tile_data, "wall_0", "Floor");
        }

        /// <summary>
        /// 创建星空
        /// </summary>
        /// <param name="tile_data"></param>
        /// <returns></returns>
        public GameObject CreateSky(Logic.Tile tile_data)
        {
            return _createTerrainBlockTile(tile_data, "sky_0", "Floor");
        }

        /// <summary>
        /// 创建岩浆
        /// </summary>
        /// <param name="tile_data"></param>
        /// <returns></returns>
        public GameObject CreateWater(Logic.Tile tile_data)
        {
            return _createTerrainBlockTile(tile_data, "water_0", "Floor");
        }

        /// <summary>
        /// 创建怪物
        /// </summary>
        /// <param name="tile_data"></param>
        /// <returns></returns>
        public GameObject CreateMonster(Logic.Tile tile_data)
        {
            var tile_monster = tile_data as Logic.Tile_Monster;

            var obj = _createBasicTile(tile_data, tile_monster.CsvData["sprite"], "Collide");

            // add collider
            var collider_component = obj.AddComponent<BoxCollider2D>();
            collider_component.isTrigger = true;

            return obj;
        }

        public GameObject CreatePlayer(Logic.Tile tile_data)
        {
            var tile_player = tile_data as Logic.Tile_Player;

            var obj = _createBasicTile(tile_data, "player", "Collide");

            var animator = obj.AddComponent<Animator>();
            var animtor_controller = Resources.Load("Animation/player_controller") as AnimatorController;
            animator.runtimeAnimatorController = animtor_controller;

            // add collider
            var collider_component = obj.AddComponent<BoxCollider2D>();
            collider_component.isTrigger = true;

            return obj;
        }

        public GameObject CreatePortal(Logic.Tile tile_data)
        {
            var tile_portal = tile_data as Logic.Tile_Portal;

            var obj = _createBasicTile(tile_data, tile_portal.Direction == Logic.EProtalDirection.Up ? "stair_up" : "stair_down", "Floor");

            return obj;
        }

        public GameObject CreateNpc(Logic.Tile tile_data)
        {
            var tile_npc = tile_data as Logic.Tile_Npc;

            var obj = _createBasicTile(tile_data, tile_npc.CsvData["sprite"], "Collide");

            return obj;
        }

        public GameObject CreateItem(Logic.Tile tile_data)
        {
            var tile_item = tile_data as Logic.Tile_Item;

            var obj = _createBasicTile(tile_data, tile_item.CsvData["sprite"], "Collide");

            return obj;
        }

        public GameObject CreateDoor(Logic.Tile tile_data)
        {
            var tile_door = tile_data as Logic.Tile_Door;

            var sprite = "door_yellow";
            switch (tile_door.DoorType)
            {
                case Logic.EDoorType.Yellow:
                    {
                        sprite = "door_yellow";
                        break;
                    }
                case Logic.EDoorType.Blue:
                    {
                        sprite = "door_blue";
                        break;
                    }
                case Logic.EDoorType.Red:
                    {
                        sprite = "door_red";
                        break;
                    }
                case Logic.EDoorType.Trigger:
                    {
                        sprite = "door_special";
                        break;
                    }
                case Logic.EDoorType.Fences:
                    {
                        sprite = "door_fences";
                        break;
                    }
            }

            var obj = _createBasicTile(tile_data, sprite, "Floor");

            return obj;
        }
    }
}
