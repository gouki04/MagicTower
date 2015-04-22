using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicTower.Display
{
    public interface TileMapDisplay
    {
        IEnumerator BeginEnter();

        IEnumerator EndEnter();
    }
}
