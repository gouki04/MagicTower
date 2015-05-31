using MagicTower.Logic;
using System;

namespace MagicTower.Data
{
    [Serializable]
    public class PortalData : TileData
    {
        public EProtalDirection PortalType;
        public uint DestinationLevel;
        public TilePosition DestinationPosition;
    }
}
