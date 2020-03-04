using Microsoft.VisualStudio.TestTools.UnitTesting;
using BackEnd;
using System.Collections.Generic;

namespace UnitTest
{
    [TestClass]
    public class StateTest
    {
        [TestMethod]
        public void StateHasCharacterTest()
        {
            IState state = new State(new Puzzle(Level.Get(1)));
            Assert.IsNotNull(state.Character);
        }

        [TestMethod]
        public void CharacterHasTileTest()
        {
            IState state = new State(new Puzzle(Level.Get(1)));
            Assert.IsNotNull(state.Character.Tile);
        }

        [TestMethod]
        public void StateSetsCharacterCorrectlyTest()
        {
            Level level = Level.CreateFromString("" +
				"!#.\n" +
				">..\n" +
				"2,15");
            IState state = new State(new Puzzle(level));
            Assert.AreEqual(state.PuzzleTiles[3], state.Character.Tile);
        }

        [TestMethod]
        public void StateHasCorrectWidthTest()
        {
            Level level = Level.CreateFromString("" +
				"!#.aA\n" +
				"#...<\n" +
				"2,15");
            IState state = new State(new Puzzle(level));
            Assert.AreEqual(5, state.PuzzleWidth);
        }

        [TestMethod]
        public void StateHasCorrectHeigthTest()
        {
            Level level = Level.CreateFromString("" +
				"!#.aA\n" +
				"#...<\n" +
				"2,15");
            IState state = new State(new Puzzle(level));
            Assert.AreEqual(2, state.PuzzleHeight);
        }

        [TestMethod]
        public void LengthOfTilesCorrespondsWithGivenSizeFromHeightAndWidthTest()
        {
            IState state = new State(new Puzzle(Level.Get(1)));
            Assert.AreEqual(state.PuzzleHeight*state.PuzzleWidth, state.PuzzleTiles.Count);
        }

        [TestMethod]
        public void EveryTileHasAnUniqueIdTest()
        {
            IState state = new State(new Puzzle(Level.Get(1)));
            HashSet<int> IDs = new HashSet<int>();
            foreach (TileState tile in state.PuzzleTiles)
            {
                IDs.Add(tile.ID);
            }
            Assert.AreEqual(state.PuzzleTiles.Count, IDs.Count);
        }

        [TestMethod]
        public void StateSetsEndCorrectlyTest()
        {
            Level level = Level.CreateFromString("" +
				"!#.aA\n" +
				"#...<\n" +
				"2,15");
            IState state = new State(new Puzzle(level));
            Assert.AreEqual(StateOfTile.End, state.PuzzleTiles[0].State);
        }

        [TestMethod]
        public void StateSetsButtonCorrectlyTest()
        {
            Level level = Level.CreateFromString("" +
				"!#.aA\n" +
				"#...<\n" +
				"2,15");
            IState state = new State(new Puzzle(level));
            TileState buttonTile = state.PuzzleTiles[3];
            Assert.AreEqual(StateOfTile.Button, buttonTile.State);
            Assert.AreEqual(typeof(ButtonTileState), buttonTile.GetType());
        }

        [TestMethod]
        public void StateSetsDoorCorrectlyTest()
        {
            Level level = Level.CreateFromString("" +
				"!#.aA\n" +
				"#...<\n" +
				"2,15");
            IState state = new State(new Puzzle(level));
            TileState doorTile = state.PuzzleTiles[4];
            Assert.AreEqual(StateOfTile.Door, doorTile.State);
            Assert.AreEqual(typeof(DoorTileState), doorTile.GetType());
        }

        [TestMethod]
        public void StateLinksButtonAndDoorCorrectlyTest()
        {
            Level level = Level.CreateFromString("" +
				"!#.aA\n" +
				"#bB.<\n" +
				"2,15");
            IState state = new State(new Puzzle(level));
            ButtonTileState buttonTile = (ButtonTileState) state.PuzzleTiles[3];
            TileState doorTile = state.PuzzleTiles[4];
            Assert.AreEqual(doorTile, buttonTile.Door);
        }

        [TestMethod]
        public void StateSetsWallCorrectlyTest()
        {
            Level level = Level.CreateFromString("" +
				"!#.aA\n" +
				"#...<\n" +
				"2,15");
            IState state = new State(new Puzzle(level));
            Assert.AreEqual(StateOfTile.Wall, state.PuzzleTiles[1].State);
        }

        [TestMethod]
        public void StateSetsBoxCorrectlyTest()
        {
            Level level = Level.CreateFromString("" +
				"!#*aA\n" +
				"#...<\n" +
				"2,15");
            IState state = new State(new Puzzle(level));
            Assert.AreEqual(MovableItem.Box, state.PuzzleTiles[2].Movable);
        }

        [TestMethod]
        public void StateSetsEmptyTileCorrectlyTest()
        {
            Level level = Level.CreateFromString("" +
				"!#.aA\n" +
				"#...<\n" +
				"2,15");
            IState state = new State(new Puzzle(level));
            Assert.AreEqual(StateOfTile.Empty, state.PuzzleTiles[2].State);
        }
    }
}
