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

        public override IEnumerator Exit()
        {
            yield return base.Exit();
        }
    }
}
