using System;

namespace MagicTower.Data
{
    [Serializable]
    public class PortalData : TileData
    {
        public int PortalType;
        public int DestinationLevel;
        public uint DestinationRow;
        public uint DestinationCol;
    }
}
