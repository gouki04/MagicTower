using System.Collections;
using UnityEngine;

namespace MagicTower.Display
{
    public interface ITileMapDisplay
    {
        IEnumerator BeginEnter();

        IEnumerator EndEnter();

        IEnumerator BeginExit();

        IEnumerator EndExit();
    }

    public class TileMapDisplay : MonoBehaviour, ITileMapDisplay
    {
        private Logic.TileMap mTileMap;
        public Logic.TileMap TileMap
        {
            get { return mTileMap; }
            set { mTileMap = value; }
        }

        public IEnumerator BeginEnter()
        {
            gameObject.SetActive(true);
            yield return null;
        }

        public IEnumerator EndEnter()
        {
            yield return null;
        }

        public IEnumerator BeginExit()
        {
            yield return null;
        }

        public IEnumerator EndExit()
        {
            Destroy(gameObject);

            yield return null;
        }
    }
}
