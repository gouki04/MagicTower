using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils;

namespace MagicTower.Logic
{
    public class Tile_Monster : Tile_Actor
    {
        private uint mId;
        public uint Id
        {
            get { return mId; }
        }

        public Tile_Monster(uint id)
            : base(EType.Monster)
        {
            mId = id;
            mHp = CsvData.GetUIntValue("hp");
        }

        public CSVLine CsvData
        {
            get
            {
                return CSVManager.Instance["monster"][mId];
            }
        }

        public uint Gold
        {
            get { return CsvData.GetUIntValue("gold"); }
        }

        public uint Exp
        {
            get { return CsvData.GetUIntValue("exp"); }
        }

        public override uint Attack
        {
            get { return CsvData.GetUIntValue("atk"); }
        }

        public override uint Defend
        {
            get { return CsvData.GetUIntValue("def"); }
        }

        private uint mHp;
        public override uint Hp
        {
            get { return mHp; }
            set { mHp = value; }
        }

        public uint Damage
        {
            get
            {
                var hp = (float)Hp;
                var atk = (float)Attack;
                var def = (float)Defend;
                var player_atk = (float)PlayerData.Instance.Attack;
                var player_def = (float)PlayerData.Instance.Defend;

                var damage = Math.Floor(hp / (player_atk - def)) * (atk - player_def);
                return (uint)damage;
            }
        }

		public override bool ValidateMove(Tile target)
		{
			if (target is Tile_Player)
			{
				var player = target as Tile_Player;
				return player.Hp > Damage;
			}

			return false;
		}

		public float CalcDamage(Tile_Player player)
		{
			var damage_value = (float)Math.Floor((float)Hp / ((float)player.Attack - (float)Defend)) * ((float)Attack - (float)player.Defend);
			return damage_value;
		}
		
		public override IEnumerator BeginTrigger(Tile target)
		{
            // start the battle
            Game.Instance.IsInBattle = true;

            //yield return Battle.Instance.BeginBattle(target as Tile_Player, this);

            // remove self from tile map
            yield return Exit();

            Game.Instance.IsInBattle = false;

            yield return true;
        }
    }
}
