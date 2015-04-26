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
        }

        public CSVLine CsvData
        {
            get
            {
                return CSVManager.Instance["monster"][mId];
            }
        }

		public override bool ValidateMove(Tile target)
		{
			if (target is Tile_Player)
			{
				return true;

				var player = target as Tile_Player;
				
				var damage = CalcDamage(player);
				return player.Hp > damage;
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
            mParent.LayerCollide[mPosition] = null;
            yield return mDisplay.Destroy();

            Game.Instance.IsInBattle = false;

            yield return true;
        }
    }
}
