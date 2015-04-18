using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NAT
{
    public class Tile
    {
        public virtual bool ValidateMove(Tile target)
        {
            return true;
        }

        public virtual IEnumerator BeginTrigger(Tile target)
        {
            return null;
        }

        public virtual IEnumerator MoveTo(uint row, uint col)
        {
            return null;
        }

        private bool mIsMoving = false;
        public bool IsMoving
        {
            get { return mIsMoving; }
            set { mIsMoving = value; }
        }
    }

    public class Tile_Player
    {
        public uint Attack
        {
            get {}
        }

        public uint Defend
        {
            get {}
        }

        public uint Hp
        {
            get {}
        }
    }

    public class Tile_Monster
    {
        public uint Attack
        {
            get {}
        }

        public uint Defend
        {
            get {}
        }

        public uint Hp
        {
            get {}
        }

        public override bool ValidateMove(Tile target)
        {
            if (target is Tile_Player)
            {
                var player = target as Tile_Player;

                // valide the atk,def,hp
                return true;
            }
            return false;
        }

        public override IEnumerator BeginTrigger(Tile target)
        {
            yield return Battle.Instance.BeginBattle(target as Tile_Player, this);
        }
    }

    public class Battle : Singleton<Battle>
    {
        public interface BattleDisplay
        {
            IEnumerator InitBattle(Tile_Player player, Tile_Monster monster);
            IEnumerator BattleBegin();
            IEnumerator BattleAttack(Tile atk, Tile def, Damage dam);
            IEnumerator BattleEnd(Tile winner);
        }

        private Tile_Player mPlayer;
        private Tile_Monster mMonster;

        private BattleDisplay mDisplay;
        public BattleDisplay Display
        {
            set { mDisplay = value; }
        }

        protected IEnumerator battleInit()
        {
            yield return mDisplay.InitBattle(mPlayer, mMonster);
        }

        public IEnumerator BeginBattle(Tile_Player player, Tile_Monster monster)
        {
            mPlayer = player;
            mMonster = monster;

            yield return battleInit();
            
            Tile attaker = mPlayer;
            Tile defender = mMonster;
            Tile winner = null;
            while (true)
            {
                Damage dam = attaker.Attack(defender);
                yield return mDisplay.BattleAttack(attaker, defender, dam);

                if (defender.IsDead || attaker.IsDead)
                {
                    winner = defender.IsDead ? attaker : defender;
                    break;
                }
                else
                {
                    var tmp = attaker;
                    attaker = defender;
                    defender = tmp;
                }
            }

            yield return mDisplay.BattleEnd(winner);
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
            if (tile.IsMoving)
                return null;

            var dst_tile = GetTile(row, col);
            if (tile == dst_tile)
                return null;

            tile.IsMoving = true;
            if (dst_tile.ValidateMove(tile))
            {
                var routine = new SafeCoroutine(dst_tile.BeginTrigger(tile));
                yield return routine;

                var result = (bool)routine.Result;
                if (result == true)
                {
                    routine = new SafeCoroutine(tile.MoveTo(row, col));
                    yield return routine;
                }
            }
            tile.IsMoving = false;

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
