using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicTower.Logic
{
	public class Tile_Player : Tile_Actor
    {
        public Tile_Player()
            : base(EType.Player)
        {

        }

        public override uint Attack
        {
            get { return PlayerData.Instance.Attack; }
        }

        public override uint Defend
        {
            get { return PlayerData.Instance.Defend; }
        }

        public override uint Hp
        {
            get { return PlayerData.Instance.Hp; }
            set { PlayerData.Instance.Hp = value; }
        }

//        public override IEnumerator Exit()
//        {
//            yield return Exit();
//        }
    }
}
