using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicTower
{
    public sealed class GameData : Utils.Singleton<GameData>
    {

        /// <summary>
        /// 存档文件名
        /// </summary>
        public readonly string DATA_TILE_NAME = "save.dat";

        public bool IsTileExist(uint lv, uint row, uint col)
        {
            // TODO　
            return true;
        }

        public void SaveDataToFile()
        {
        }

        public void LoadDataFromFile()
        {
            //try
            //{
            //    using (var stream_reader = new StreamReader(DATA_TILE_NAME))
            //    {
            //        var content_text = stream_reader.ReadToEnd();
            //        var content = content_text.Split('\n');

            //        Lv = Convert.ToUInt32(content[0]);
            //        Attack = Convert.ToUInt32(content[1]);
            //        Defend = Convert.ToUInt32(content[2]);
            //        Hp = Convert.ToUInt32(content[3]);
            //        Gold = Convert.ToUInt32(content[4]);
            //        Exp = Convert.ToUInt32(content[5]);

            //        mKeys[EDoorKeyType.YellowKey] = Convert.ToUInt32(content[6]);
            //        mKeys[EDoorKeyType.BlueKey] = Convert.ToUInt32(content[7]);
            //        mKeys[EDoorKeyType.RedKey] = Convert.ToUInt32(content[8]);
            //    }
            //}
            //catch (Exception)
            //{
            //    Logger.LogDebug("Open {0} Failed!", DATA_TILE_NAME);
            //}
        }
    }
}
