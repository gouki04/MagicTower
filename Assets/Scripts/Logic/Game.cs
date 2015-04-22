using System.Collections;
using UnityEngine;
using SafeCoroutine;

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
                yield return CoroutineManager.StartCoroutine(_loadGameData());

                // enter the map
                yield return CoroutineManager.StartCoroutine(_enterTileMap(PlayerData.Instance.CurTileMapLevel));

                yield return null;
            }

            private IEnumerator _loadGameData()
            {
                PlayerData.Instance.LoadDataFromFile();
                yield return null;
            }

            private IEnumerator _enterTileMap(uint lv)
            {
                var load_map_routine = CoroutineManager.StartCoroutine(TileMapManager.Instance.LoadMap(lv));
                yield return load_map_routine;

                if (load_map_routine.HasResult == false)
                {
                    Logger.LogFatal("Load Tile Map ({0}) Failed!", lv);
                    yield return null;
                }
                
                mCurTileMap = load_map_routine.Result as TileMap;

                var tile_map_enter_routine = CoroutineManager.StartCoroutine(mCurTileMap.Enter());
                yield return tile_map_enter_routine;
            }
        } 
    }
}
