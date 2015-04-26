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
