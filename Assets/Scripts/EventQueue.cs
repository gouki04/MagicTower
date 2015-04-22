using SafeCoroutine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicTower
{
    public class NullGameDisplay : Display.GameDisplay
    {

    }

    public class NullTileMapDisplay : Display.TileMapDisplay
    {
        public IEnumerator BeginEnter()
        {
            yield return null;
        }

        public IEnumerator EndEnter()
        {
            yield return null;
        }
    }

    public class NullTileDisplay : Display.TileDisplay
    {
        public IEnumerator Enter()
        {
            yield return null;
        }
    }

    public class NullDisplayFacotry : Display.DisplayFactory
    {
        public Display.GameDisplay GetGameDisplay(Logic.Game game)
        {
            return new NullGameDisplay();
        }

        public Display.TileMapDisplay GetTileMapDisplay(Logic.TileMap tile_map)
        {
            return new NullTileMapDisplay();
        }

        public Display.TileDisplay GetTileDisplay(Logic.Tile tile)
        {
            return new NullTileDisplay();
        }
    }

    public enum EEventType
    {
        TILE_MOVE = 1,
        ENTER_LEVEL = 2,
        CHANGE_LEVEL = 3,
        ENTER_GAME = 4,
    }

    public class EventData
    {
        public uint ID;
        public EEventType Type;
        public object[] Params;

        public EventData(uint id, EEventType type, object[] param)
        {
            ID = id;
            Type = type;
            Params = param;
        }
    }

    public class EventQueue : Singleton<EventQueue>
    {
        private Queue<EventData> mEventQ;
        private uint mIdGenerator = 0;

        public EventQueue()
        {
            mEventQ = new Queue<EventData>();
        }

        public void AddEvent(EEventType type, params object[] args)
        {
            var data = new EventData(mIdGenerator++, type, args);
            mEventQ.Enqueue(data);
        }

        public void Update(float dt)
        {
            while (true)
            {
                if (mEventQ.Count <= 0)
                {
                    break;
                }

                var evt = mEventQ.Dequeue();
                switch (evt.Type)
                {
                    case EEventType.TILE_MOVE:
                        {
                            //var tile = evt.Params[0] as Tile;
                            //var row = (uint)evt.Params[1];
                            //var col = (uint)evt.Params[2];
                            //Coroutine.Start(Game.Instance.TileMap.MoveTile(tile, row, col));
                            break;
                        }
                    case EEventType.ENTER_GAME:
                        {
                            CoroutineManager.StartCoroutine(Logic.Game.Instance.Init(new NullDisplayFacotry()));
                            break;
                        }
                }
            }
        }
    }
}
