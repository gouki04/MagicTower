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

        public enum EDirection
        {
            Up = 0,
            Down = 1,
            Left = 2,
            Right = 3,
        }

        private EType mType;
        public EType Type
        {
            get { return mType; }
        }

        private TileMap mParent = null;
        public TileMap Parent
        {
            get { return mParent; }
            set { mParent = value; }
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

		public virtual bool ValidateMove(Tile target)
		{
			return true;
		}
		
		public virtual IEnumerator BeginTrigger(Tile target)
		{
			return null;
		}
		
		public virtual IEnumerator MoveTo(uint row, uint col)
		{
			return null;
		}
		
		private bool mIsMoving = false;
		public bool IsMoving
		{
			get { return mIsMoving; }
			set { mIsMoving = value; }
		}
    }
}

