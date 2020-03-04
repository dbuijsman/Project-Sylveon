using Microsoft.VisualStudio.TestTools.UnitTesting;
using BackEnd;
using System.Collections.Generic;

namespace UnitTest
{
    [TestClass]
    public class StateEqualityTest
    {
        [TestMethod]
        public void CharactersAreEqualTest()
        {
            Statement[] singleStatement = new Statement[]{new SingleCommand(Command.RotateLeft)};
            List<IState> states = (new LevelSolution(1, singleStatement)).States;
            CharacterState character1 = states[1].Character;
            states = (new LevelSolution(1, singleStatement)).States;
            CharacterState character2 = states[1].Character;
            Assert.AreEqual(character1,character2);
        }
        
        [TestMethod]
        public void CharactersWithDifferentDirectionAreNotEqualTest()
        {
            List<IState> states = (new LevelSolution(1, new Statement[]{new SingleCommand(Command.RotateLeft)})).States;
            CharacterState character1 = states[1].Character;
            states = (new LevelSolution(1, new Statement[]{new SingleCommand(Command.RotateRight)})).States;
            CharacterState character2 = states[1].Character;
            Assert.AreNotEqual(character1,character2);
        }

        [TestMethod]
        public void CharactersWithDifferentPositionsAreNotEqualTest()
        {
            Level level = Level.CreateFromString("" +
				"!#.aA\n" +
				"#...<\n" +
				"2,15");
            Statement[] statements = new Statement[]{new SingleCommand(Command.MoveForward)};
            List<IState> states = (new LevelSolution(new Puzzle(level), statements)).States;
            Assert.AreNotEqual(states[0].Character,states[1].Character);
        }
        
        [TestMethod]
        public void TileStatesNoMovableAreEqualTest()
        {
            TileState tile1 = new TileState(0,StateOfTile.Empty);
            TileState tile2 = new TileState(0,StateOfTile.Empty);
            Assert.AreEqual(tile1,tile2);
        }
        
        [TestMethod]
        public void TileStatesWithMovableAreEqualTest()
        {
            Level level = Level.CreateFromString("" +
				"!#*aA\n" +
				"#...<\n" +
				"2,15");
            TileState tile1 = (new State(new Puzzle(level))).PuzzleTiles[2];
            TileState tile2 = (new State(new Puzzle(level))).PuzzleTiles[2];
            Assert.AreEqual(tile1,tile2);
        }
        
        [TestMethod]
        public void TileStatesDifferentMovableAreNotEqualTest()
        {
            Level level = Level.CreateFromString("" +
				"!#*aA\n" +
				"#...<\n" +
				"2,15");
            TileState tile1 = (new State(new Puzzle(level))).PuzzleTiles[2];
            TileState tile2 = new TileState(tile1.ID,StateOfTile.Empty);
            Assert.AreNotEqual(tile1,tile2);
        }
        
        [TestMethod]
        public void TileStatesDifferentIDAreNotEqualTest()
        {
            TileState tile1 = new TileState(0,StateOfTile.Empty);
            TileState tile2 = new TileState(1,StateOfTile.Empty);
            Assert.AreNotEqual(tile1,tile2);
        }
        
        [TestMethod]
        public void TileStatesDifferentTypeOfTileAreNotEqualTest()
        {
            TileState tile1 = new TileState(0,StateOfTile.Empty);
            TileState tile2 = new TileState(0,StateOfTile.End);
            Assert.AreNotEqual(tile1,tile2);
        }
        
        [TestMethod]
        public void DoorTileStatesAreEqualTest()
        {
            TileState tile1 = new DoorTileState(0,StateOfTile.Empty, true);
            TileState tile2 = new DoorTileState(0,StateOfTile.Empty, true);
            Assert.AreEqual(tile1,tile2);
        }
        
        [TestMethod]
        public void DoorTileStatesOpenAndClosedAreNotEqualTest()
        {
            TileState tile1 = new DoorTileState(0,StateOfTile.Empty, true);
            TileState tile2 = new DoorTileState(0,StateOfTile.Empty, false);
            Assert.AreNotEqual(tile1,tile2);
        }
        
        [TestMethod]
        public void ButtonTileStatesAreEqualTest()
        {
            Level level = Level.CreateFromString("" +
				"!A*a#\n" +
				"#...<\n" +
				"2,15");
            TileState tile1 = new State(new Puzzle(level)).PuzzleTiles[3];
            Puzzle puzzle2 = new Puzzle(level);
            TileState tile2 = new State(new Puzzle(level)).PuzzleTiles[3];
            Assert.AreEqual(tile1,tile2);
        }
        
        [TestMethod]
        public void ButtonTileStatesWithDoorOpenAndClosedAreNotEqualTest()
        {
            Level level = Level.CreateFromString("" +
				"!A*a#\n" +
				"#...<\n" +
				"2,15");
            TileState tile1 = new State(new Puzzle(level)).PuzzleTiles[3];
            Puzzle puzzle2 = new Puzzle(level);
            ((DoorTile) puzzle2.AllTiles[0,1]).Open();
            TileState tile2 = new State(puzzle2).PuzzleTiles[3];
            Assert.AreNotEqual(tile1,tile2);
        }

        [TestMethod]
        public void StatesEqualsTest()
        {
            Puzzle puzzle = new Puzzle(Level.Get(1));
            State state1 = new State(puzzle);
            State state2 = new State(puzzle);
            Assert.AreEqual(state1,state2);
        }

        [TestMethod]
        public void StatesAreEqualAfterExecutingCommandReturnsSamePuzzleTest()
        {
            Statement[] statements = new Statement[]{new SingleCommand(Command.RotateLeft),new SingleCommand(Command.RotateRight)};
            List<IState> states = (new LevelSolution(1, statements)).States;
            Assert.AreEqual(states[0],states[2]);
        }
    }
}
