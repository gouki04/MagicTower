using SafeCoroutine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace MagicTower
{
    //public class NullGameDisplay : Display.IGameDisplay
    //{

    //}

    //public class NullTileMapDisplay : Display.ITileMapDisplay
    //{
    //    public IEnumerator BeginEnter()
    //    {
    //        Logger.LogDebug("NullTileMapDisplay.BeginEnter");
    //        yield return null;
    //    }

    //    public IEnumerator EndEnter()
    //    {
    //        Logger.LogDebug("NullTileMapDisplay.EndEnter");
    //        yield return null;
    //    }
    //}

    //public class NullTileDisplay : Display.ITileDisplay
    //{
    //    public IEnumerator Enter()
    //    {
    //        Logger.LogDebug("NullTileDisplay.Enter");
    //        yield return null;
    //    }

    //    public IEnumerator MoveTo(Logic.TilePosition dest)
    //    {
    //        throw new System.NotImplementedException();
    //    }

    //    public IEnumerator Exit()
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    //public class NullDisplayFacotry : Display.IDisplayFactory
    //{
    //    public Display.IGameDisplay GetGameDisplay(Logic.Game game)
    //    {
    //        return new NullGameDisplay();
    //    }

    //    public Display.ITileMapDisplay GetTileMapDisplay(Logic.TileMap tile_map)
    //    {
    //        return new NullTileMapDisplay();
    //    }

    //    public Display.ITileDisplay GetTileDisplay(Logic.Tile tile)
    //    {
    //        return new NullTileDisplay();
    //    }
    //}

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

        private bool translateEvent(EEventType type, object[] args, out EEventType out_type, out object[] out_args)
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
                        {
                            if (tile.Position.Row >= tile.Parent.LayerCollide.Height)
                                return false;
                    
                            out_args[1] = new Logic.TilePosition(tile.Position.Row + 1, tile.Position.Col);
                        }
                        else if (direction == EDirection.DOWN)
                        {
                            if (tile.Position.Row <= 0)
                                return false;

                            out_args[1] = new Logic.TilePosition(tile.Position.Row - 1, tile.Position.Col);
                        }
                        else if (direction == EDirection.LEFT)
                        {
                            if (tile.Position.Col <= 0)
                                return false;

                            out_args[1] = new Logic.TilePosition(tile.Position.Row, tile.Position.Col - 1);
                        }
                        else if (direction == EDirection.RIGHT)
                        {
                            if (tile.Position.Col >= tile.Parent.LayerCollide.Width)
                                return false;

                            out_args[1] = new Logic.TilePosition(tile.Position.Row, tile.Position.Col + 1);
                        }

                        return true;
                    }
            }

            out_type = type;
            out_args = args;

            return true;
        }

        public void AddEvent(EEventType type, params object[] args)
        {
            if (translateEvent(type, args, out type, out args))
            {
                var data = new EventData(mIdGenerator++, type, args);
                mEventQ.Enqueue(data);
            }
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
                            var destination = (Logic.TilePosition)evt.Params[1];
                            CoroutineManager.StartCoroutine(Logic.Game.Instance.MoveTileTo(tile, destination));
                            break;
                        }
                    case EEventType.ENTER_GAME:
                        {
                            CoroutineManager.StartCoroutine(Logic.Game.Instance.Init(new Display.DisplayFactory()));
                            break;
                        }
                    case EEventType.CHANGE_LEVEL:
                        {
                            var level = (uint)evt.Params[0];
                            var position = (Logic.TilePosition)evt.Params[1];
                            CoroutineManager.StartCoroutine(Logic.Game.Instance.ChangeLevel(level, position));
                            break;
                        }
                }
            }
        }
    }
}
