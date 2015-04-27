using System;
using System.Collections.Generic;
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
        private uint mLastTileMapLevel = 0;
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

        /// <summary>
        /// 玩家的攻击力
        /// </summary>
        private uint mAttack;
        public uint Attack
        {
            get { return mAttack; }
            set { mAttack = value; }
        }

        /// <summary>
        /// 玩家的防御力
        /// </summary>
        private uint mDefend;
        public uint Defend
        {
            get { return mDefend; }
            set { mDefend = value; }
        }

        /// <summary>
        /// 玩家的血量
        /// </summary>
        private uint mHp;
        public uint Hp
        {
            get { return mHp; }
            set { mHp = value; }
        }

        /// <summary>
        /// 玩家的金币
        /// </summary>
        private uint mGold;
        public uint Gold
        {
            get { return mGold; }
            set { mGold = value; }
        }

        /// <summary>
        /// 玩家的经验
        /// </summary>
        private uint mExp;
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
        }
    }
}
