using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class TileComponent : MonoBehaviour
{
    public mt.Tile DataTile;

    public void Setup(mt.Tile data)
    {
        DataTile = data;

        SpriteRenderer spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        mt.SpriteSheetManager sheet = mt.SpriteSheetManager.Instance;
        switch (data.Type)
        {
            case mt.Tile.EType.Floor:
                {
                    spriteRenderer.sprite = sheet["floor_0"];
                    spriteRenderer.sortingLayerName = "Floor";
                    break;
                }
            case mt.Tile.EType.Wall:
                {
                    spriteRenderer.sprite = sheet["wall_7"];
                    spriteRenderer.sortingLayerName = "Collide";
                    break;
                }
            case mt.Tile.EType.Water:
                {
                    spriteRenderer.sprite = sheet["water_0"];
                    spriteRenderer.sortingLayerName = "Collide";
                    break;
                }
            case mt.Tile.EType.Sky:
                {
                    spriteRenderer.sprite = sheet["wall_13"];
                    spriteRenderer.sortingLayerName = "Collide";
                    break;
                }
        }

        transform.parent = transform;
        transform.localPosition = new Vector3(data.Position.Col, data.Position.Row, 0);
    }

    void Start()
    {
        
    }
}
