using System;

namespace MagicTower.Data
{
    [Serializable]
    public class TileMapData
    {
        public int Level;
        public int Width;
        public int Height;
        public int[,] FloorLayer;
        public PortalData[] PortalDatas;
        public MonsterData[] MonsterDatas;
        public ItemData[] ItemDatas;
    }
}
