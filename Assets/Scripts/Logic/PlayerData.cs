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

        public uint LastTileMapLevel
        {
            get { return 0; }
        }

        public TilePosition LastPlayerPosition
        {
            get;
            set;
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
