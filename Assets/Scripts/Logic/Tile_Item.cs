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
            var data = CsvData;

            var atk = data.GetUIntValue("atk");
            if (atk > 0)
                PlayerData.Instance.Attack += atk;

            var def = data.GetUIntValue("def");
            if (def > 0)
                PlayerData.Instance.Defend += def;

            var hp = data.GetUIntValue("hp");
            if (hp > 0)
                PlayerData.Instance.Hp += hp;

            var gold = data.GetUIntValue("gold");
            if (gold > 0)
                PlayerData.Instance.Gold += gold;

            var exp = data.GetUIntValue("exp");
            if (exp > 0)
                PlayerData.Instance.Exp += exp;

            var key_yellow = data.GetUIntValue("key_y");
            if (key_yellow > 0)
                PlayerData.Instance.ObtainKey(EDoorKeyType.YellowKey);

            var key_blue = data.GetUIntValue("key_b");
            if (key_blue > 0)
                PlayerData.Instance.ObtainKey(EDoorKeyType.BlueKey);

            var key_red = data.GetUIntValue("key_r");
            if (key_red > 0)
                PlayerData.Instance.ObtainKey(EDoorKeyType.RedKey);

            yield return Exit();
        }
    }
}
