using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mt
{
    public class Tile_Player : Tile
    {
        public Tile_Player()
            : base(EType.Player)
        {

        }

        public string SpriteName
        {
            get { return "monster_1"; }
        }
    }
}
