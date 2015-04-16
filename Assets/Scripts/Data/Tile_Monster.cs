using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mt
{
    public class Tile_Monster : Tile
    {
        private uint mId;
        public uint Id
        {
            get { return mId; }
        }

        public string SpriteName
        {
            get
            {
                return CsvData["sprite"];
            }
        }

        public Tile_Monster(uint id)
            : base(EType.Monster)
        {
            mId = id;
        }

        private CSVLine CsvData
        {
            get
            {
                return CSVManager.Instance["monster"][mId];
            }
        }
    }
}
