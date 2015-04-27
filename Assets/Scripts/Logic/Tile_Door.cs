using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicTower.Logic
{
    public enum EDoorType
    {
        Yellow = 0,
        Blue = 1,
        Red = 2,
        Trigger = 3,
        Fences = 4,
    }

    public class Tile_Door : Tile
    {
        public Tile_Door(EDoorType type)
            : base(EType.Door)
        {
            mType = type;
        }

        private EDoorType mType;
        public EDoorType Type
        {
            get { return mType; }
            set { mType = value; }
        }

        public override bool ValidateMove(Tile target)
        {
            // TODO check if the player get the key
            return false;
        }

        public override IEnumerator BeginTrigger(Tile target)
        {
            // TODO reduces the key of the player
            yield return true;
        }
    }
}
