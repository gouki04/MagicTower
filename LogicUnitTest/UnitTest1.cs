using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using Utils;

namespace LogicUnitTest
{
    public class TestGameDisplay : MagicTower.Display.IGameDisplay
    {

    }

    public class TestTileMapDisplay : MagicTower.Display.ITileMapDisplay
    {
        public IEnumerator BeginEnter()
        {
            yield return null;
        }

        public IEnumerator EndEnter()
        {
            yield return null;
        }

        public IEnumerator BeginExit()
        {
            yield return null;
        }

        public IEnumerator EndExit()
        {
            yield return null;
        }
    }

    public class TestTileDisplay : MagicTower.Display.ITileDisplay
    {
        public IEnumerator Enter()
        {
            yield return null;
        }

        public IEnumerator MoveTo(MagicTower.Logic.TilePosition dest)
        {
            yield return null;
        }

        public IEnumerator Exit()
        {
            yield return null;
        }
    }

    public class TestDisplayFacotry : MagicTower.Display.IDisplayFactory
    {
        public MagicTower.Display.IGameDisplay GetGameDisplay(MagicTower.Logic.Game game)
        {
            return new TestGameDisplay();
        }

        public MagicTower.Display.ITileMapDisplay GetTileMapDisplay(MagicTower.Logic.TileMap tile_map)
        {
            return new TestTileMapDisplay();
        }

        public MagicTower.Display.ITileDisplay GetTileDisplay(MagicTower.Logic.Tile tile)
        {
            return new TestTileDisplay();
        }
    }

    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            MagicTower.EventQueue.Instance.AddEvent(MagicTower.EEventType.ENTER_GAME);
        }
    }
}
