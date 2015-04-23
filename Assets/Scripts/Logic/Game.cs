using System.Collections;
using UnityEngine;
using SafeCoroutine;
using Utils;

namespace MagicTower
{
    namespace Logic
    {
        public class Game : Singleton<Game>
        {
            private Display.DisplayFactory mDisplayFactory;
            public Display.DisplayFactory DisplayFactory
            {
                get { return mDisplayFactory; }
            }

            private Display.GameDisplay mDisplay;
            public Display.GameDisplay Display
            {
                get { return mDisplay; }
            }

            private TileMap mCurTileMap;

            public Game()
            {

            }

            public IEnumerator Init(Display.DisplayFactory factory)
            {
                mDisplayFactory = factory;

                // set the display
                mDisplay = mDisplayFactory.GetGameDisplay(this);

                // load the save data
                yield return _loadGameData();

                // enter the map
                yield return _enterTileMap(PlayerData.Instance.CurTileMapLevel);

                yield return null;
            }

            private IEnumerator _loadGameData()
            {
                PlayerData.Instance.LoadDataFromFile();
                yield return null;
            }

            private IEnumerator _enterTileMap(uint lv)
            {
                yield return TileMapManager.Instance.LoadMap(lv);

                if (SafeCoroutine.Coroutine.GlobalResult == null)
                {
                    Logger.LogFatal("Load Tile Map ({0}) Failed!", lv);
                    yield return null;
                }

                mCurTileMap = SafeCoroutine.Coroutine.GlobalResult as TileMap;

                yield return mCurTileMap.Enter();
            }
        } 
    }
}
