using System.Collections;
using UnityEngine;

namespace MagicTower.Display
{
    public interface ITileMapDisplay
    {
        IEnumerator BeginEnter();

        IEnumerator EndEnter();
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
    }
}
