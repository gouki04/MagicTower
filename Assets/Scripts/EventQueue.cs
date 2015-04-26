using SafeCoroutine;
using System.Collections;
using System.Collections.Generic;
using Utils;

namespace MagicTower
{
    public class NullGameDisplay : Display.IGameDisplay
    {

    }

    public class NullTileMapDisplay : Display.ITileMapDisplay
    {
        public IEnumerator BeginEnter()
        {
            Logger.LogDebug("NullTileMapDisplay.BeginEnter");
            yield return null;
        }

        public IEnumerator EndEnter()
        {
            Logger.LogDebug("NullTileMapDisplay.EndEnter");
            yield return null;
        }
    }

    public class NullTileDisplay : Display.ITileDisplay
    {
        public IEnumerator Enter()
        {
            Logger.LogDebug("NullTileDisplay.Enter");
            yield return null;
        }
    }

    public class NullDisplayFacotry : Display.IDisplayFactory
    {
        public Display.IGameDisplay GetGameDisplay(Logic.Game game)
        {
            return new NullGameDisplay();
        }

        public Display.ITileMapDisplay GetTileMapDisplay(Logic.TileMap tile_map)
        {
            return new NullTileMapDisplay();
        }

        public Display.ITileDisplay GetTileDisplay(Logic.Tile tile)
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
        TILE_MOVE_RELATIVETY = 5,
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

    enum EDirection
    {
        UP = 0,
        DOWN = 1,
        LEFT = 2,
        RIGHT = 3,
    }

    public class EventQueue : Singleton<EventQueue>
    {
        private Queue<EventData> mEventQ;
        private uint mIdGenerator = 0;

        public EventQueue()
        {
            mEventQ = new Queue<EventData>();
        }

        private void translateEvent(EEventType type, object[] args, out EEventType out_type, out object[] out_args)
        {
            switch (type)
            {
                case EEventType.TILE_MOVE_RELATIVETY:
                    {
                        var tile = args[0] as Logic.Tile;
                        var direction = (EDirection)args[1];
                        
                        out_type = EEventType.TILE_MOVE;
                        
                        out_args = new object[2];
                        out_args[0] = tile;

                        if (direction == EDirection.UP)
                            out_args[1] = new Logic.TilePosition(tile.Position.Row + 1, tile.Position.Col);
                        else if (direction == EDirection.DOWN)
                            out_args[1] = new Logic.TilePosition(tile.Position.Row - 1, tile.Position.Col);
                        else if (direction == EDirection.LEFT)
                            out_args[1] = new Logic.TilePosition(tile.Position.Row, tile.Position.Col - 1);
                        else if (direction == EDirection.RIGHT)
                            out_args[1] = new Logic.TilePosition(tile.Position.Row, tile.Position.Col + 1);

                        break;
                    }
            }

            out_type = type;
            out_args = args;
        }

        public void AddEvent(EEventType type, params object[] args)
        {
            translateEvent(type, args, out type, out args);

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
                            var tile = evt.Params[0] as Logic.Tile;
                            var row = (uint)evt.Params[1];
                            var col = (uint)evt.Params[2];
                            CoroutineManager.StartCoroutine(Logic.Game.Instance.MoveTileTo(tile, new Logic.TilePosition(row, col)));
                            break;
                        }
                    case EEventType.ENTER_GAME:
                        {
                            CoroutineManager.StartCoroutine(Logic.Game.Instance.Init(new Display.DisplayFactory()));
                            break;
                        }
                }
            }
        }
    }
}
