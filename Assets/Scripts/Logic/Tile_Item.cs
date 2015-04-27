using System.Collections;
using Utils;

namespace MagicTower.Logic
{
    public class Tile_Item : Tile
    {
        public Tile_Item(uint id)
            : base(EType.Item)
        {
            mId = id;
        }

        private uint mId;
        public uint Id
        {
            get { return mId; }
            set { mId = value; }
        }

        public CSVLine CsvData
        {
            get
            {
                return CSVManager.Instance["item"][mId];
            }
        }

        public override IEnumerator BeginTrigger(Tile target)
        {
            // add property to the target
            // property = CsvData["hp"] .. "mp"

            yield return null;
        }
    }
}
