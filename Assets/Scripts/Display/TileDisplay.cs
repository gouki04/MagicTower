using System.Collections;
using UnityEngine;

namespace MagicTower.Display
{

    public interface ITileDisplay
    {
        IEnumerator Enter();

        IEnumerator MoveTo(Logic.TilePosition dest);
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

        public IEnumerator MoveTo(Logic.TilePosition dest)
        {
            yield return new WaitForMoveTo(gameObject, dest, 0.2f);
        }
    }
}
