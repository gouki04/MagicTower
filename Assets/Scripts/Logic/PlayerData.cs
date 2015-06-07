using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Utils;

namespace MagicTower.Logic
{
    public enum EDoorKeyType
    {
        RedKey = 0,
        BlueKey = 1,
        YellowKey = 2,
    }

    /// <summary>
    /// 玩家数据
    /// 
    /// 负责从文件读入数据，写入数据到文件
    /// 保存玩家相关的最新数据
    /// </summary>
    public sealed class PlayerData : Singleton<PlayerData>
    {
        /// <summary>
        /// 存档文件名
        /// </summary>
        public readonly string DATA_TILE_NAME = "save.dat";

        /// <summary>
        /// 上次游戏时最后停留的层数
        /// </summary>
        private uint mLastTileMapLevel = 1;
        public uint LastTileMapLevel
        {
            get { return mLastTileMapLevel; }
            set { mLastTileMapLevel = value; }
        }

        /// <summary>
        /// 上次游戏时最后所站的位置
        /// </summary>
        private TilePosition mLastPlayerPosition = new TilePosition(0, 5);
        public TilePosition LastPlayerPosition
        {
            get { return mLastPlayerPosition; }
            set { mLastPlayerPosition = value; }
        }

        private uint mLv = 1;
        public uint Lv
        {
            get { return mLv; }
            set { mLv = value; }
        }

        /// <summary>
        /// 玩家的攻击力
        /// </summary>
        private uint mAttack = 50;
        public uint Attack
        {
            get { return mAttack; }
            set { mAttack = value; }
        }

        /// <summary>
        /// 玩家的防御力
        /// </summary>
        private uint mDefend = 50;
        public uint Defend
        {
            get { return mDefend; }
            set { mDefend = value; }
        }

        /// <summary>
        /// 玩家的血量
        /// </summary>
        private uint mHp = 5000;
        public uint Hp
        {
            get { return mHp; }
            set { mHp = value; }
        }

        /// <summary>
        /// 玩家的金币
        /// </summary>
        private uint mGold = 0;
        public uint Gold
        {
            get { return mGold; }
            set { mGold = value; }
        }

        /// <summary>
        /// 玩家的经验
        /// </summary>
        private uint mExp = 0;
        public uint Exp
        {
            get { return mExp; }
            set { mExp = value; }
        }

        private Dictionary<EDoorKeyType, uint> mKeys = new Dictionary<EDoorKeyType, uint>();
        public Dictionary<EDoorKeyType, uint> Keys
        {
            get { return mKeys; }
        }

        public uint RedKeys
        {
            get { return mKeys[EDoorKeyType.RedKey]; }
        }

        public uint BlueKeys
        {
            get { return mKeys[EDoorKeyType.BlueKey]; }
        }

        public uint YellowKeys
        {
            get { return mKeys[EDoorKeyType.YellowKey]; }
        }

        public void UseKey(EDoorKeyType key_type)
        {
            var origin_count = mKeys[key_type];
            if (origin_count > 0)
                mKeys[key_type] = origin_count - 1;
        }

        public void ObtainKey(EDoorKeyType key_type)
        {
            var origin_count = mKeys[key_type];
            mKeys[key_type] = origin_count + 1;
        }

        public PlayerData()
        {
            mKeys[EDoorKeyType.YellowKey] = 0;
            mKeys[EDoorKeyType.BlueKey] = 0;
            mKeys[EDoorKeyType.RedKey] = 0;
        }

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
            try
            {
                using (var stream_reader = new StreamReader(DATA_TILE_NAME))
                {
                    var content_text = stream_reader.ReadToEnd();
                    var content = content_text.Split('\n');

                    mLv = Convert.ToUInt32(content[0]);
                    mAttack = Convert.ToUInt32(content[1]);
                    mDefend = Convert.ToUInt32(content[2]);
                    mHp = Convert.ToUInt32(content[3]);
                    mGold = Convert.ToUInt32(content[4]);
                    mExp = Convert.ToUInt32(content[5]);
                    mKeys[EDoorKeyType.YellowKey] = Convert.ToUInt32(content[6]);
                    mKeys[EDoorKeyType.BlueKey] = Convert.ToUInt32(content[7]);
                    mKeys[EDoorKeyType.RedKey] = Convert.ToUInt32(content[8]);
                }
            }
            catch (Exception)
            {
                Logger.LogDebug("Open {0} Failed!", DATA_TILE_NAME);
            }
        }
    }
}
