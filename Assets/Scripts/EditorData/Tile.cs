using System.Collections.Generic;
using UnityEngine;

namespace MagicTower.EditorData
{
    public class Tile : MonoBehaviour
    {
        public Logic.Tile.EType TileType;

        public Dictionary<string, object> Properties = new Dictionary<string, object>();
    }
}
