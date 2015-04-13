using System;
using UnityEngine;

/// <summary>
/// tile gameobj 工厂
/// </summary>
public class TileObjFactory : Singleton<TileObjFactory>
{
	public TileObjFactory ()
	{
	}

    /// <summary>
    /// 创建基本的tile
    /// </summary>
    /// <param name="tile_data">tile数据</param>
    /// <param name="sprite_name"></param>
    /// <param name="sorting_layer_name"></param>
    /// <returns></returns>
	private GameObject _createBasicTile(mt.Tile tile_data, string sprite_name, string sorting_layer_name)
	{
		// create the gameobject
		var obj = new GameObject ();
		
		// set the sprite
		var sprite_renderer = obj.AddComponent<SpriteRenderer> ();
		sprite_renderer.sprite = mt.SpriteSheetManager.Instance.GetSprite (sprite_name);
		
		// set the sorting layer
		sprite_renderer.sortingLayerName = sorting_layer_name;
		
		// set the tile data
		var tile_component = obj.AddComponent<TileComponent> ();
		tile_component.DataTile = tile_data;
		
		// set the position
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
    private GameObject _createTerrainBlockTile(mt.Tile tile_data, string sprite_name, string sorting_layer_name)
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
	public GameObject CreateFloor(mt.Tile tile_data)
	{
		return _createBasicTile (tile_data, "floor_0", "Floor");
	}

    /// <summary>
    /// 创建墙壁
    /// </summary>
    /// <param name="tile_data"></param>
    /// <returns></returns>
	public GameObject CreateWall(mt.Tile tile_data)
	{
        return _createTerrainBlockTile(tile_data, "wall_7", "Collide");
	}

    /// <summary>
    /// 创建星空
    /// </summary>
    /// <param name="tile_data"></param>
    /// <returns></returns>
	public GameObject CreateSky(mt.Tile tile_data)
	{
        return _createTerrainBlockTile(tile_data, "wall_13", "Collide");
	}

    /// <summary>
    /// 创建岩浆
    /// </summary>
    /// <param name="tile_data"></param>
    /// <returns></returns>
	public GameObject CreateWater(mt.Tile tile_data)
	{
        return _createTerrainBlockTile(tile_data, "water_0", "Collide");
	}

    /// <summary>
    /// 创建怪物
    /// </summary>
    /// <param name="tile_data"></param>
    /// <returns></returns>
    public GameObject CreateMonster(mt.Tile tile_data)
    {
        var tile_monster = tile_data as mt.Tile_Monster;

        var obj = _createBasicTile(tile_data, tile_monster.SpriteName, "Collide");

        // add collider
        var collider_component = obj.AddComponent<BoxCollider2D>();
        collider_component.isTrigger = true;

        return obj;
    }

    /// <summary>
    /// 工厂方法
    /// 根据tile data创建对应的tile obj
    /// </summary>
    /// <param name="tile_data"></param>
    /// <returns></returns>
	public GameObject CreateTile(mt.Tile tile_data)
    {
        switch (tile_data.Type)
        {
            case mt.Tile.EType.Floor:
                {
                    return CreateFloor(tile_data);
                }
            case mt.Tile.EType.Wall:
                {
                    return CreateWall(tile_data);
                }
            case mt.Tile.EType.Water:
                {
                    return CreateWater(tile_data);
                }
            case mt.Tile.EType.Sky:
                {
                    return CreateSky(tile_data);
                }
            case mt.Tile.EType.Monster:
                {
                    return CreateMonster(tile_data);
                }
            default:
                return null;
        }
	}
}

