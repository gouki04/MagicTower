using System;
using UnityEngine;

public class TileObjFactory : Singleton<TileObjFactory>
{
	public TileObjFactory ()
	{
	}

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

	public GameObject CreateFloor(mt.Tile tile_data)
	{
		return _createBasicTile (tile_data, "floor_0", "Floor");
	}

	public GameObject CreateWall(mt.Tile tile_data)
	{
		return _createBasicTile (tile_data, "wall_7", "Collide");
	}

	public GameObject CreateSky(mt.Tile tile_data)
	{
		return _createBasicTile (tile_data, "wall_13", "Collide");
	}

	public GameObject CreateWater(mt.Tile tile_data)
	{
		return _createBasicTile (tile_data, "water_0", "Collide");
	}

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
            default:
                return null;
        }
	}
}

