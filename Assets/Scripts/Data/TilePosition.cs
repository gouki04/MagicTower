using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mt
{
    public struct TilePosition
    {
        public uint Row;
        public uint Col;

        public TilePosition(uint r, uint c)
        {
            Row = r;
            Col = c;
        }

        static public TilePosition Zero
        {
            get { return new TilePosition(0, 0); }
        }
    }
}
