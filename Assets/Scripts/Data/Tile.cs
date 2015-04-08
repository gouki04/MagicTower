using UnityEngine;
using System.Collections;

namespace mt
{
    public class Tile
    {
        public enum EType
        {
            None = 0,
            Floor = 1,
            Wall = 2,
            Water = 3,
            Sky = 4,
            Monster = 5,
            Npc = 6,
            Item = 7,
            Trigger = 8,
            Door = 9,
            Portal = 10,
            Player = 11,
        }

        private EType mType;
        public EType Type
        {
            get { return mType; }
        }

        public Tile(EType type)
        {
            mType = type;
        }

        private TilePosition mPosition = new TilePosition(0, 0);
        public TilePosition Position
        {
            get { return mPosition; }
            set { mPosition = value; }
        }
    }
}

