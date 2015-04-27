using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MagicTower.Display
{
    public interface IBattleDisplay
    {
        IEnumerator InitBattle(Logic.Tile_Actor player, Logic.Tile_Actor monster);
        IEnumerator BattleBegin();
        IEnumerator BattleAttackBegin(Logic.Tile_Actor atk, Logic.Tile_Actor def, uint dam);
        IEnumerator BattleAttackEnd(Logic.Tile_Actor atk, Logic.Tile_Actor def, uint dam);
        IEnumerator BattleEnd(Logic.Tile_Actor winner);
    }

    public class BattleDisplay : MonoBehaviour, IBattleDisplay
    {
        private Logic.Tile_Actor mPlayer;
        private Logic.Tile_Actor mMonster;

        public IEnumerator InitBattle(Logic.Tile_Actor player, Logic.Tile_Actor monster)
        {
            mPlayer = player;
            mMonster = monster;

            // TODO change to battle scene
            yield return null;
        }

        public IEnumerator BattleBegin()
        {
            yield return null;
        }

        public IEnumerator BattleAttackBegin(Logic.Tile_Actor atk, Logic.Tile_Actor def, uint dam)
        {
            // TODO show the attack effect
            yield return null;
        }

        public IEnumerator BattleEnd(Logic.Tile_Actor winner)
        {
            // TODO back to level scene
            yield return null;
        }

        public IEnumerator BattleAttackEnd(Logic.Tile_Actor atk, Logic.Tile_Actor def, uint dam)
        {
            // TODO update the data on the gui
            yield return null;
        }
    }
}
