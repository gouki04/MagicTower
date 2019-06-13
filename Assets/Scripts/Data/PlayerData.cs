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
    public class PlayerData : Utils.Singleton<PlayerData>
    {
        /// <summary>
        /// 上次游戏时最后停留的层数
        /// </summary>
        public uint LastTileMapLevel { get; set; }

        /// <summary>
        /// 上次游戏时最后所站的位置
        /// </summary>
        public TilePosition LastPlayerPosition { get; set; }

        public uint Lv { get; set; }

        /// <summary>
        /// 玩家的攻击力
        /// </summary>
        public uint Attack { get; set; }

        /// <summary>
        /// 玩家的防御力
        /// </summary>
        public uint Defend { get; set; }

        /// <summary>
        /// 玩家的血量
        /// </summary>
        public uint Hp { get; set; }

        /// <summary>
        /// 玩家的金币
        /// </summary>
        public uint Gold { get; set; }

        /// <summary>
        /// 玩家的经验
        /// </summary>
        public uint Exp { get; set; }

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
            Lv = 1;
            Hp = 100;
            Gold = 0;
            Exp = 0;
            Attack = 10;
            Defend = 10;

            mKeys[EDoorKeyType.YellowKey] = 0;
            mKeys[EDoorKeyType.BlueKey] = 0;
            mKeys[EDoorKeyType.RedKey] = 0;
        }
    }
}
