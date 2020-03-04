using Microsoft.VisualStudio.TestTools.UnitTesting;
using BackEnd;

namespace UnitTest
{
    [TestClass]
    public class PuzzleTest
    {
        [TestMethod]
        public void PuzzleHasCorrectLevelNumberTest()
        {
            Puzzle puzzle = new Puzzle(Level.Get(1));
            Assert.AreEqual(1, puzzle.LevelNumber);
        }   

        [TestMethod]
        public void PuzzleHasCorrectSizeTest()
        {
            Level level = Level.CreateFromString("" +
                "!#a#\n" +
                "A.*_\n" +
                "1,1");
            Puzzle puzzle = new Puzzle(level);
            Tile[,] puzzleTiles = puzzle.AllTiles;
            Assert.AreEqual(level.GridSize[0], puzzleTiles.GetLength(0));
            Assert.AreEqual(level.GridSize[1], puzzleTiles.GetLength(1));
        }  

        [TestMethod]
        public void PuzzlePlacesWallsCorrectlyTest()
        {
            Level level = Level.CreateFromString("" +
                "!#a#\n" +
                "A.*_\n" +
                "1,1");
            Puzzle puzzle = new Puzzle(level);
            Tile[,] puzzleTiles = puzzle.AllTiles;
            Assert.IsInstanceOfType(puzzleTiles[0,1], typeof(WallTile));
            Assert.IsInstanceOfType(puzzleTiles[0,3], typeof(WallTile));
        }  
        
        [TestMethod]
        public void PuzzlePlacesButtonsCorrectlyTest()
        {
            Level level = Level.CreateFromString("" +
                "!#a#\n" +
                "A.*_\n" +
                "1,1");
            Puzzle puzzle = new Puzzle(level);
            Tile[,] puzzleTiles = puzzle.AllTiles;
            Assert.IsInstanceOfType(puzzleTiles[0,2], typeof(ButtonTile));
        } 
        
        [TestMethod]
        public void PuzzlePlacesDoorsCorrectlyTest()
        {
            Level level = Level.CreateFromString("" +
                "!#a#\n" +
                "A.*_\n" +
                "1,1");
            Puzzle puzzle = new Puzzle(level);
            Tile[,] puzzleTiles = puzzle.AllTiles;
            Assert.IsInstanceOfType(puzzleTiles[1,0], typeof(DoorTile));
        }
        
        [TestMethod]
        public void PuzzleConnectButtonsAndDoorsCorrectlyTest()
        {
            Level level = Level.CreateFromString("" +
                "!#a#\n" +
                "A.*_\n" +
                "B.*b\n" +
                "1,1");
            Puzzle puzzle = new Puzzle(level);
            Tile[,] puzzleTiles = puzzle.AllTiles;
            Assert.AreEqual(puzzleTiles[1,0], ((ButtonTile) puzzleTiles[0,2]).door);
        } 
        
        [TestMethod]
        public void PuzzlePlacesFinishCorrectlyTest()
        {
            Level level = Level.CreateFromString("" +
                ">##!\n" +
                "*.aA\n" +
                "1,1");
            Puzzle puzzle = new Puzzle(level);
            Tile[,] puzzleTiles = puzzle.AllTiles;
            Assert.AreEqual(puzzleTiles[0,3], puzzle.Finish);
        }   
        [TestMethod]
        public void PuzzlePlacesPassableTilesCorrectlyTest()
        {
            Level level = Level.CreateFromString("" +
                ">##!\n" +
                "*.aA\n" +
                "1,1");
            Puzzle puzzle = new Puzzle(level);
            Tile[,] puzzleTiles = puzzle.AllTiles;
            Assert.AreEqual(typeof(Tile), puzzleTiles[0,0].GetType());
            Assert.AreEqual(typeof(Tile), puzzleTiles[1,0].GetType());
            Assert.AreEqual(typeof(Tile), puzzleTiles[1,1].GetType());
        }   

        [TestMethod]
        public void PuzzlePlacesCharacterCorrectlyTest()
        {
            Level level = Level.CreateFromString("" +
                "!#a#\n" +
                "A*<.\n" +
                "1,1");
            Puzzle puzzle = new Puzzle(level);
            Tile[,] puzzleTiles = puzzle.AllTiles;
            Assert.AreEqual(puzzleTiles[1,2], puzzle.Character.Position);
            Assert.AreEqual(Direction.West, puzzle.Character.Direction);
        }   
    }
}
