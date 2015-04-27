using System;
using System.Collections;
using Utils;

namespace MagicTower.Logic
{
	public class Battle : Singleton<Battle>
	{
		private Tile_Actor mAttacker;
		private Tile_Actor mDefender;
		
		private Display.IBattleDisplay mDisplay;
        public Display.IBattleDisplay Display
		{
			set { mDisplay = value; }
		}
		
		protected IEnumerator battleInit()
		{
			yield return mDisplay.InitBattle(mAttacker, mDefender);
		}

        public IEnumerator BeginBattle(Tile_Actor atk, Tile_Actor def)
		{
            if (mDisplay == null)
            {
                mDisplay = Game.Instance.DisplayFactory.GetBattleDisplay(this);
            }

			mAttacker = atk;
			mDefender = def;
			
			yield return battleInit();
			
			Tile_Actor attaker = mAttacker;
			Tile_Actor defender = mDefender;
			Tile_Actor winner = null;

			while (true)
			{
				var dam = attaker.CalcOneHitDamage(defender);
				yield return mDisplay.BattleAttackBegin(attaker, defender, dam);

                defender.TakeDamage(dam);
                yield return mDisplay.BattleAttackEnd(attaker, defender, dam);

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

