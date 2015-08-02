using System;
using System.Collections.Generic;

namespace MagicTower.Data
{
    [Serializable]
    public class TileMapData
    {
        public uint Level;
        public uint Width;
        public uint Height;
        public Logic.Tile.EType[,] FloorLayer;
        public List<PortalData> PortalDatas;
        public List<MonsterData> MonsterDatas;
        public List<ItemData> ItemDatas;
        public List<DoorData> DoorDatas;
    }
}
