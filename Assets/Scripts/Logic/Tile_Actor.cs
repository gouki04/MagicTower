using System;

namespace MagicTower.Logic
{
	public abstract class Tile_Actor : Tile
	{
		public Tile_Actor (EType type)
            : base(type)
		{
		}

        public abstract uint Attack
        {
            get;
        }

        public abstract uint Defend
        {
            get;
        }

        public abstract uint Hp
        {
            get;
            set;
        }

		public override bool IsDead
		{
			get { return Hp <= 0; }
		}

        public virtual uint CalcOneHitDamage(Tile_Actor defender)
        {
            if (Attack < defender.Defend)
            {
                return 0;
            }

            return Attack - defender.Defend;
        }

        public virtual void TakeDamage(uint dam)
        {
            if (Hp < dam)
            {
                Hp = 0;
            }
            else
            {
                Hp -= dam;
            }
        }
	}
}

