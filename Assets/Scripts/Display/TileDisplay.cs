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
        private Logic.TilePosition mDestination;
        private float mSpeed;

        public WaitForMoveTo(GameObject gameobject, Logic.TilePosition destination, float speed)
        {
            mGameObject = gameobject;
            mDestination = destination;
            mSpeed = speed;
        }

        public bool IsComplete(float delta_time)
        {
            var cur_position = mGameObject.transform.localPosition;
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
            mDestination = dest;
        }
    }
}
