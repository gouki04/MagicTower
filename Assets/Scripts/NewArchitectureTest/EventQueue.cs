using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NAT
{
    public class Tile
    {
        public bool ValidateMove(Tile target)
        {
            return true;
        }

        public IEnumerator BeginTrigger(Tile target)
        {
            return null;
        }

        public IEnumerator MoveTo(uint row, uint col)
        {
            return null;
        }
    }

    public class TileMap
    {
        public Tile GetTile(uint row, uint col)
        {
            return null;
        }

        public IEnumerator MoveTile(Tile tile, uint row, uint col)
        {
            var dst_tile = GetTile(row, col);
            if (tile == dst_tile)
                return null;

            if (dst_tile.ValidateMove(tile))
            {

            }

            return null;
        }
    }

    public class Game : Singleton<Game>
    {
        private TileMap mTileMap;
        public TileMap TileMap
        {
            get { return mTileMap; }
        }
    }

    public enum EEventType
    {
        TILE_MOVE = 1,
        ENTER_LEVEL = 2,
        CHANGE_LEVEL = 3,
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
                var evt = mEventQ.Dequeue();
                if (evt == null)
                {
                    break;
                }

                switch (evt.Type)
                {
                    case EEventType.TILE_MOVE:
                        {
                            var tile = evt.Params[0] as Tile;
                            var row = (uint)evt.Params[1];
                            var col = (uint)evt.Params[2];
                            Coroutine.Start(Game.Instance.TileMap.MoveTile(tile, row, col));
                            break;
                        }
                }
            }
        }
    }
}
