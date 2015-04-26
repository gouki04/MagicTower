using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicTower.Logic
{
    public enum EProtalDirection
    {
        Up = 0,
        Down = 1,
    }

    public class Tile_Portal : Tile
    {
        public Tile_Portal()
            : base (EType.Portal)
        { }

        private EProtalDirection mDirection;    
        public EProtalDirection Direction
        {
            get { return mDirection; }
            set { mDirection = value; }
        }

        private uint mDestinationLevel;
        public uint DestinationLevel
        {
            get { return mDestinationLevel; }
            set { mDestinationLevel = value; }
        }

        private TilePosition mDestinationPosition;
        public TilePosition DestinationPosition
        {
            get { return mDestinationPosition; }
            set { mDestinationPosition = value; }
        }

        public override bool ValidateMove(Tile target)
        {
            if (target is Tile_Player)
            {
                return true;
            }

            return false;
        }

        public override IEnumerator BeginTrigger(Tile target)
        {
            yield return true;
        }

        public override IEnumerator EndTrigger(Tile target)
        {
            EventQueue.Instance.AddEvent(EEventType.CHANGE_LEVEL, mDestinationLevel, mDestinationPosition);

            yield return null;
        }
    }
}
