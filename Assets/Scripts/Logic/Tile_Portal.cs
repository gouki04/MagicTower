using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicTower.Logic
{
    [Serializable]
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

        public Data.PortalData Data { get; set; }
 
        public EProtalDirection Direction
        {
            get { return Data.PortalType; }
        }

        public uint DestinationLevel
        {
            get { return Data.DestinationLevel; }
        }

        public TilePosition DestinationPosition
        {
            get { return Data.DestinationPosition; }
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
            EventQueue.Instance.AddEvent(EEventType.CHANGE_LEVEL, DestinationLevel, DestinationPosition);

            yield return null;
        }
    }
}
