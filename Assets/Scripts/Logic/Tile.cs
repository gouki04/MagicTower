using UnityEngine;
using System.Collections;
using SafeCoroutine;

namespace MagicTower
{
    namespace Logic
    {
        public class Tile
        {
            private Display.ITileDisplay mDisplay;
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
                yield return false;
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

