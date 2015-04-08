using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class TileMapComponent : MonoBehaviour
{
    public mt.TileMap DataTileMap;

    private void AddTile(mt.Tile tile)
    {
        GameObject prefab = new GameObject();
        SpriteRenderer spriteRenderer = prefab.AddComponent<SpriteRenderer>();
        mt.SpriteSheetManager sheet = mt.SpriteSheetManager.Instance;
        switch (tile.Type)
        {
            case mt.Tile.EType.Floor:
                {
                    spriteRenderer.sprite = sheet["floor_0"];
                    spriteRenderer.sortingLayerName = "Floor";
                    //prefab = Resources.Load("Prefabs/Floor") as GameObject;
                    break;
                }
            case mt.Tile.EType.Wall:
                {
                    spriteRenderer.sprite = sheet["wall_7"];
                    spriteRenderer.sortingLayerName = "Collide";
                    //prefab = Resources.Load("Prefabs/Wall") as GameObject;
                    break;
                }
            case mt.Tile.EType.Water:
                {
                    spriteRenderer.sprite = sheet["water_0"];
                    spriteRenderer.sortingLayerName = "Collide";
                    //prefab = Resources.Load("Prefabs/Water") as GameObject;
                    break;
                }
            case mt.Tile.EType.Sky:
                {
                    spriteRenderer.sprite = sheet["wall_13"];
                    spriteRenderer.sortingLayerName = "Collide";
                    //prefab = Resources.Load("Prefabs/Sky") as GameObject;
                    break;
                }
        }

        if (prefab != null)
        {
            var tileObj = (GameObject)GameObject.Instantiate(prefab);
            tileObj.transform.parent = transform;
            tileObj.transform.localPosition = new Vector3(tile.Position.Col, tile.Position.Row, 0);
        }
    }

    private void AddLayer(mt.TileLayer layer)
    {
        for (uint r = 0; r < layer.Height; ++r)
        {
            for (uint c = 0; c < layer.Width; ++c)
            {
                var tile = layer[r, c];
                if (tile != null)
                {
                    AddTile(tile);
                }
            }
        }
    }

    void Start()
    {
        AddLayer(DataTileMap.LayerFloor);
        AddLayer(DataTileMap.LayerCollide);
    }
}
