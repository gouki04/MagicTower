using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mt
{
    public struct TilePosition
    {
        public float Row;
        public float Col;

        public TilePosition(float r, float c)
        {
            Row = r;
            Col = c;
        }

        static public TilePosition Zero
        {
            get { return new TilePosition(0.0f, 0.0f); }
        }

        public override bool Equals(object obj)
        {
            return obj is TilePosition && this == (TilePosition)obj;
        }

        public static bool operator==(TilePosition x, TilePosition y)
        {
            return (x.Row - y.Row) <= 0.0001f && (x.Col - y.Col) <= 0.0001f;
        }

        public static bool operator!=(TilePosition x, TilePosition y)
        {
            return !(x == y);
        }
    }
}
