using UnityEngine;
using System.Collections;
using SafeCoroutine;

namespace MagicTower
{
    namespace Logic
    {
        public class Tile
        {
            protected Display.ITileDisplay mDisplay;
            public Display.ITileDisplay Display
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

            protected EType mType;
            public EType Type
            {
                get { return mType; }
            }

            protected TileMap mParent = null;
            public TileMap Parent
            {
                get { return mParent; }
                set { mParent = value; }
            }

            public Tile(EType type)
            {
                mType = type;
            }

            protected TilePosition mPosition = new TilePosition(0, 0);
            public TilePosition Position
            {
                get { return mPosition; }
                set { mPosition = value; }
            }

            protected bool mIsBlock = true;
            public bool IsBlock
            {
                get { return mIsBlock; }
                set { mIsBlock = value; }
            }

            public virtual bool ValidateMove(Tile target)
            {
                return !mIsBlock;
            }

            public virtual IEnumerator BeginTrigger(Tile target)
            {
                yield return true;
            }

            public virtual IEnumerator EndTrigger(Tile target)
            {
                yield return null;
            }

            public virtual IEnumerator MoveTo(uint row, uint col)
            {
                yield return null;
            }

            public virtual IEnumerator MoveTo(TilePosition destination)
            {
                yield return mDisplay.MoveTo(destination);

                mParent.LayerCollide[Position] = null;
                mParent.LayerCollide[destination] = this;

                yield return null;
            }

            public virtual IEnumerator Enter()
            {
                if (mDisplay == null)
                {
                    mDisplay = Game.Instance.DisplayFactory.GetTileDisplay(this);
                }

                yield return mDisplay.Enter();
            }

            public virtual IEnumerator Exit()
            {
                mParent.LayerCollide[mPosition] = null;
                yield return mDisplay.Exit();
                mDisplay = null;
				yield return null;
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
}

