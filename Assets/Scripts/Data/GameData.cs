using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Utils;

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

        public void SaveDataToFile(uint index = 0u)
        {
            try
            {
                var file_name = string.Format("save{0}.dat", index);
                using (var stream_writer = new StreamWriter(file_name, false))
                {
                    var player_data = Logic.PlayerData.Instance;

                    stream_writer.Write(player_data.Lv);
                    stream_writer.Write(player_data.Attack);
                    stream_writer.Write(player_data.Defend);
                    stream_writer.Write(player_data.Hp);
                    stream_writer.Write(player_data.Gold);
                    stream_writer.Write(player_data.Exp);
                    stream_writer.Write(player_data.RedKeys);
                    stream_writer.Write(player_data.BlueKeys);
                    stream_writer.Write(player_data.YellowKeys);
                }
            }
            catch (Exception e)
            {
                Logger.LogError("Save File Failed! : " + e.Message);
            }
        }

        public void LoadDataFromFile(uint index = 0u)
        {
            try
            {
                var file_name = string.Format("save{0}.dat", index);
                using (var stream_reader = new StreamReader(file_name))
                {
                    var player_data = Logic.PlayerData.Instance;

                    player_data.Lv = uint.Parse(stream_reader.ReadLine());

                    var content_text = stream_reader.ReadToEnd();
                    var content = content_text.Split('\n');

                    //Lv = Convert.ToUInt32(content[0]);
                    //Attack = Convert.ToUInt32(content[1]);
                    //Defend = Convert.ToUInt32(content[2]);
                    //Hp = Convert.ToUInt32(content[3]);
                    //Gold = Convert.ToUInt32(content[4]);
                    //Exp = Convert.ToUInt32(content[5]);

                    //mKeys[EDoorKeyType.YellowKey] = Convert.ToUInt32(content[6]);
                    //mKeys[EDoorKeyType.BlueKey] = Convert.ToUInt32(content[7]);
                    //mKeys[EDoorKeyType.RedKey] = Convert.ToUInt32(content[8]);
                }
            }
            catch (Exception)
            {
                Logger.LogDebug("Open {0} Failed!", DATA_TILE_NAME);
            }
        }
    }
}
