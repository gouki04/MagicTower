using UnityEngine;
using System.Collections;
using SafeCoroutine;

namespace mt
{
    public class TileComponent
    {
        protected Tile mOnwer;
        public Tile Onwer
        {
            get { return mOnwer; }
        }

        public TileComponent(Tile onwer)
        {
            mOnwer = onwer;
        }
    }

    public class TileMoveComponent : TileComponent
    {
        public TileMoveComponent(Tile onwer)
            : base(onwer)
        { }

        IEnumerator MoveTo(TilePosition dst);

        bool IsMoving { get; set; }
    }

    public class TileNullMoveComponent : TileMoveComponent
    {
        public TileNullMoveComponent(Tile onwer)
            : base(onwer)
        { }

        public IEnumerator MoveTo(TilePosition dst)
        {
            yield return null;
        }

        public bool IsMoving
        {
            get
            {
                return false;
            }
            set
            {
                // do nothing
            }
        }
    }

    public class TileActorMoveComponent : TileMoveComponent
    {
        public class WaitForMoveTo : IYieldInstruction
        {
            private Tile mTile;
            private TilePosition mDestination;

            public WaitForMoveTo(Tile tile, TilePosition dst)
            {
                mTile = tile;
                mDestination = dst;
            }

            private bool isNealyEqual(float a, float b, float epsilon = 0.0001f)
            {
                return Mathf.Abs(a - b) <= epsilon;
            }

            public bool IsComplete(float delta_time)
            {
                var src_position = mTile.Position;
                if (src_position.Equals(mDestination))
                    return true;

                if (!isNealyEqual(src_position.Row, mDestination.Row))
                {
                    int direction = src_position.Row > mDestination.Row ? -1 : 1;
                    float distance = mTile.Speed * delta_time * direction;
                    mTile.Position = new TilePosition(src_position.Row + distance, src_position.Col);

                    mTile.Display.UpdateMove(mTile.Position);
                }
                else if (!isNealyEqual(src_position.Col, mDestination.Col))
                {
                    int direction = src_position.Col > mDestination.Col ? -1 : 1;
                    float distance = mTile.Speed * delta_time * direction;
                    mTile.Position = new TilePosition(src_position.Row, src_position.Col + distance);

                    mTile.Display.UpdateMove(mTile.Position);
                }

                return false;
            }
        }

        public TileActorMoveComponent(Tile onwer)
            : base(onwer)
        { }

        private TilePosition mDstPosition;
        private bool mIsMoving = false;

        public IEnumerator MoveTo(TilePosition dst)
        {
            mOnwer.Display.BeginMove(dst);
            
            yield return new WaitForMoveTo(mOnwer, dst);

            mOnwer.Display.EndMove();
        }

        public bool IsMoving
        {
            get
            {
                return mIsMoving;
            }
            set
            {
                mIsMoving = value;
            }
        }
    }

    public class Tile
    {
        public interface ITileDisplay
        {
            void BeginMove(TilePosition dst_position);
            void UpdateMove(TilePosition new_position);
            void EndMove();
        }

        private ITileDisplay mDisplay;
        public ITileDisplay Display
        {
            get { return mDisplay; }
        }

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

        public float Speed
        {
            get { return 1.0f; }
        }
    }
}

