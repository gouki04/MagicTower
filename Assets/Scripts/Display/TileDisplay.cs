using System.Collections;
using UnityEngine;

namespace MagicTower.Display
{

    public interface ITileDisplay
    {
        IEnumerator Enter();

        void BeginMove();
        void EndMove();

        IEnumerator MoveTo(Logic.TilePosition dest);

        IEnumerator Exit();
    }

    public class WaitForMoveTo : SafeCoroutine.IYieldInstruction
    {
        private GameObject mGameObject;
        private Vector3 mBeginPosition;
        private Vector3 mEndPosition;
        private float mTime;
        private float mDuration;

        public WaitForMoveTo(GameObject gameobject, Logic.TilePosition destination, float time)
        {
            mGameObject = gameobject;
            mBeginPosition = gameobject.transform.localPosition;
            mEndPosition = new Vector3(destination.Col, destination.Row, 0);
            mTime = 0;
            mDuration = time;
        }

        public bool IsComplete(float delta_time)
        {
            mTime += delta_time;
            if (mTime >= mDuration)
            {
                mGameObject.transform.localPosition = mEndPosition;
                return true;
            }

            var cur_progress = mTime / mDuration;
            var distance = mEndPosition - mBeginPosition;
            var cur_position = mBeginPosition + distance * cur_progress;
            mGameObject.transform.localPosition = cur_position;

            return false;
        }
    }

    public class TileDisplay : MonoBehaviour, ITileDisplay
    {
        private Logic.Tile mTile;
        public Logic.Tile Tile
        {
            get { return mTile; }
            set { mTile = value; }
        }

        private Logic.TilePosition mDestination;

        public IEnumerator Enter()
        {
            gameObject.SetActive(true);
            yield return null;
        }

        public enum EDisplayDirection
        {
            Up = 0,
            Down = 1,
            Left = 2,
            Right = 3
        }

        public IEnumerator MoveTo(Logic.TilePosition dest)
        {
            var animator = GetComponent<Animator>();
            if (animator != null)
            {
                if (mTile.Position.Row > dest.Row)
					animator.SetInteger("direction", (int)EDisplayDirection.Down);
                else if (mTile.Position.Row < dest.Row)
                    animator.SetInteger("direction", (int)EDisplayDirection.Up);

                if (mTile.Position.Col > dest.Col)
                    animator.SetInteger("direction", (int)EDisplayDirection.Left);
                else if (mTile.Position.Col < dest.Col)
                    animator.SetInteger("direction", (int)EDisplayDirection.Right);
            }

            yield return new WaitForMoveTo(gameObject, dest, 0.2f);
        }

        public IEnumerator Exit()
        {
            Destroy(gameObject);
            yield return null;
        }

        public void BeginMove()
        {
            var animator = GetComponent<Animator>();
            if (animator != null)
                animator.SetBool("moving", true);
        }

        public void EndMove()
        {
            var animator = GetComponent<Animator>();
            if (animator != null)
                animator.SetBool("moving", false);
        }
    }
}
