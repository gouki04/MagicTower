using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicTower.Logic
{
    [Serializable]
    public struct TilePosition
    {
        public uint Row;
        public uint Col;

        public TilePosition(uint r, uint c)
        {
            Row = r;
            Col = c;
        }

        //[NonSerialized]
        //static readonly public TilePosition Zero = new TilePosition(0, 0);

        public override bool Equals(object obj)
        {
            return obj is TilePosition && this == (TilePosition)obj;
        }

        public override int GetHashCode()
        {
            return (int)(Row * Col);
        }

        public static bool operator ==(TilePosition x, TilePosition y)
        {
            return (x.Row == y.Row) && (x.Col == y.Col);
        }

        public static bool operator !=(TilePosition x, TilePosition y)
        {
            return !(x == y);
        }
    }
}