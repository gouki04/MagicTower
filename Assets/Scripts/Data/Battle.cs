using System;
using System.Collections;

namespace mt
{
	public class Battle : Singleton<Battle>
	{
		public interface BattleDisplay
		{
            IEnumerator InitBattle(Tile_Actor player, Tile_Actor monster);
			IEnumerator BattleBegin();
			IEnumerator BattleAttack(Tile atk, Tile def, Damage dam);
			IEnumerator BattleEnd(Tile winner);
		}
		
		private Tile_Actor mPlayer;
		private Tile_Actor mMonster;
		
		private BattleDisplay mDisplay;
		public BattleDisplay Display
		{
			set { mDisplay = value; }
		}
		
		protected IEnumerator battleInit()
		{
			yield return mDisplay.InitBattle(mPlayer, mMonster);
		}
		
		public IEnumerator BeginBattle(Tile_Actor player, Tile_Actor monster)
		{
			mPlayer = player;
			mMonster = monster;
			
			yield return battleInit();
			
			Tile_Actor attaker = mPlayer;
			Tile_Actor defender = mMonster;
			Tile_Actor winner = null;

			while (true)
			{
				Damage dam = attaker.AttackTo(defender);
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
}

