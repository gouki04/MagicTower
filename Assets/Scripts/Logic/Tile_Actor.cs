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
        }

		public bool IsDead
		{
			get { return Hp <= 0; }
		}

        //public abstract Damage AttackTo(Tile_Actor actor);

        //public abstract Damage TakeDamage(Damage dam);
	}
}

