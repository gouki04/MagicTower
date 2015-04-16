using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class TileComponent : MonoBehaviour
{
    public mt.Tile TileData;

    public T GetTileData<T>() where T : mt.Tile
    {
        return TileData as T;
    }

    public mt.Tile GetTileData()
    {
        return TileData;
    }

    void Start()
    {
        
    }
}
