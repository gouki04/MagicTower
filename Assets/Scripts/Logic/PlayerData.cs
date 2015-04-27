using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils;

namespace MagicTower.Logic
{
    public class PlayerData : Singleton<PlayerData>
    {
        public readonly string DATA_TILE_NAME = "save.dat";

        private uint mLastTileMapLevel = 0;
        public uint LastTileMapLevel
        {
            get { return mLastTileMapLevel; }
            set { mLastTileMapLevel = value; }
        }

        private TilePosition mLastPlayerPosition = new TilePosition(0, 5);
        public TilePosition LastPlayerPosition
        {
            get { return mLastPlayerPosition; }
            set { mLastPlayerPosition = value; }
        }

        private uint mAttack;
        public uint Attack
        {
            get { return mAttack; }
            set { mAttack = value; }
        }

        private uint mDefend;
        public uint Defend
        {
            get { return mDefend; }
            set { mDefend = value; }
        }

        private uint mHp;
        public uint Hp
        {
            get { return mHp; }
            set { mHp = value; }
        }

        private uint mGold;
        public uint Gold
        {
            get { return mGold; }
            set { mGold = value; }
        }

        private uint mExp;
        public uint Exp
        {
            get { return mExp; }
            set { mExp = value; }
        }

        public bool IsTileExist(uint lv, uint row, uint col)
        {
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
