using System;
namespace mt
{
	public class Tile_Actor : Tile
	{
		public Tile_Actor (EType type)
            : base(type)
		{
		}

		private uint mAttack;
		public uint Attack
		{
			get { return mAttack; }
			set { mAttack = value; }
		}
		
		private uint mDefend;
		public uint Defend
		{
			get { return mDefend; }
			set { mDefend = value; }
		}
		
		private uint mHp;
		public uint Hp
		{
			get { return mHp; }
			set { mHp = value; }
		}

		public bool IsDead
		{
			get { return Hp <= 0; }
		}

		public abstract Damage AttackTo(Tile_Actor actor);

		public abstract Damage TakeDamage(Damage dam);
	}
}

