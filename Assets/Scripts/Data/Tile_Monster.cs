using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mt
{
    public class Tile_Monster : Tile_Actor
    {
        private uint mId;
        public uint Id
        {
            get { return mId; }
        }

        public string SpriteName
        {
            get
            {
                return CsvData["sprite"];
            }
        }

        public Tile_Monster(uint id)
            : base(EType.Monster)
        {
            mId = id;
        }

        private CSVLine CsvData
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
				var player = target as Tile_Player;
				
				var damage = CalcDamage(player);
				return player.Hp > damage;
			}
			return false;
		}

		public float CalcDamage(Tile_Player player)
		{
			var damage_value = Mathf.Floor((float)Hp / ((float)player.Attack - (float)Defend)) * ((float)Attack - (float)player.Defend);
			return damage_value;
		}
		
		public override IEnumerator BeginTrigger(Tile target)
		{
			yield return Battle.Instance.BeginBattle(target as Tile_Player, this);
		}
    }
}
