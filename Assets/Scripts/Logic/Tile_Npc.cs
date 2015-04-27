using System.Collections;
using Utils;

namespace MagicTower.Logic
{
    public class Tile_Npc : Tile
    {
        public Tile_Npc(uint id)
            : base(EType.Npc)
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
                return CSVManager.Instance["npc"][mId];
            }
        }

        public override bool ValidateMove(Tile target)
        {
            // 可以移动，但在trigger里阻挡他
            return true;
        }

        public override IEnumerator BeginTrigger(Tile target)
        {
            // show npc talking
            // msg = CsvData["msg"]

            // do not let player pass
            yield return false;
        }
    }
}
