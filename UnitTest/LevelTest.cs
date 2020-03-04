using Microsoft.VisualStudio.TestTools.UnitTesting;
using BackEnd;

namespace UnitTest
{
    [TestClass]
    public class LevelTest
    {
        [TestMethod]
        public void LevelHasCorrectLevelNumberTest()
        {
            Level level = Level.Get(1);
            Assert.AreEqual(1, level.LevelNumber);
        }

        [TestMethod]
        public void NewLevelHasLevelNumberTest()
        {
            Level level = Level.CreateFromString("##\n^!\n3,2");
            Assert.AreEqual(3, level.LevelNumber);
        }
        [TestMethod]
        public void GridSizeSquareTest()
        {
            Level level = Level.CreateFromString("" + 
                "###\n" +
                "#!^\n" +
                "###\n" +
                "1,1");
            Assert.AreEqual(3, level.GridSize[0]);
            Assert.AreEqual(3, level.GridSize[1]);
        }

        [TestMethod]
        public void GridSizeNotSquareTest()
        {
            Level level = Level.CreateFromString("" +
                "!#a#\n" +
                "A.*_\n" +
                "1,1");
            Assert.AreEqual(2, level.GridSize[0]);
            Assert.AreEqual(4, level.GridSize[1]);
        }

        [TestMethod]
        public void LevelHasParTest()
        {
            Level level = Level.CreateFromString("#!\n^#\n3,2");
            Assert.AreEqual(2, level.Par);
        }

        [TestMethod]
        public void WallIsSetOnRightPositionTest()
        {
            Level level = Level.CreateFromString("" +
				".!.\n" +
				"#^.\n" +
				"1,8");
            CollectionAssert.AreEqual(new int[] { 1, 0 }, level.Walls[0]);
        }

        [TestMethod]
        public void CharacterDirectionNorthTest()
        {
            Level level = Level.CreateFromString("" +
				"^#\n" + 
                "#!\n" + 
                "1,1");
            Assert.AreEqual(Direction.North, level.DirectionCharacter);
            CollectionAssert.AreEqual(new int[] { 0, 0 }, level.PositionCharacter);
        }

        [TestMethod]
        public void CharacterDirectionEastTest()
        {
            Level level = Level.CreateFromString("" +
				"#>\n" + 
                "!#\n" + 
                "1,1");
            Assert.AreEqual(Direction.East, level.DirectionCharacter);
            CollectionAssert.AreEqual(new int[] { 0, 1 }, level.PositionCharacter);
        }

        [TestMethod]
        public void CharacterDirectionSouthTest()
        {
            Level level = Level.CreateFromString("" +
				"!#\n" + 
                "_#\n" + 
                "1,1");
            Assert.AreEqual(Direction.South, level.DirectionCharacter);
            CollectionAssert.AreEqual(new int[] { 1, 0 }, level.PositionCharacter);
        }

        [TestMethod]
        public void CharacterDirectionWestTest()
        {
            Level level = Level.CreateFromString("" +
				"!#\n" + 
                "#<\n" + 
                "1,1");
            Assert.AreEqual(Direction.West, level.DirectionCharacter);
            CollectionAssert.AreEqual(new int[] { 1, 1 }, level.PositionCharacter);
        }

        [TestMethod]
        public void FinishIsSetOnRightPositionTest()
        {
            Level level = Level.CreateFromString(""+ 
                "#^\n" + 
                "!.\n" + 
                "1,1");
            CollectionAssert.AreEqual(new int[] { 1, 0 }, level.End);
        }

        [TestMethod]
        public void ButtonTest()
        {
            Level level = Level.CreateFromString(""+ 
                "ab\n" + 
                "##\n" + 
                "^!\n" + 
                "1,1");
            Assert.AreEqual(2, level.Buttons.Length);
            CollectionAssert.AreEqual(new int[] { 0, 0, 0 }, level.Buttons[0]);
            CollectionAssert.AreEqual(new int[] { 1, 0, 1 }, level.Buttons[1]);
        }

        [TestMethod]
        public void DoorTest()
        {
            Level level = Level.CreateFromString(""+ 
                "AB\n" + 
                "##\n" + 
                "^!\n" + 
                "1,1");
            Assert.AreEqual(2, level.Doors.Length);
            CollectionAssert.AreEqual(new int[] { 0, 0, 0 }, level.Doors[0]);
            CollectionAssert.AreEqual(new int[] { 1, 0, 1 }, level.Doors[1]);
        }

        [TestMethod]
        public void ButtonDoorTest()
        {
            Level level = Level.CreateFromString(""+ 
                "a.\n" + 
                ".A\n" + 
                "^!\n" + 
                "1,1");
            Assert.AreEqual(level.Doors[0][0], level.Buttons[0][0]);
        }

        [TestMethod]
        public void BoxTest()
        {
            Level level = Level.CreateFromString(""+ 
                "*#\n" + 
                ".#\n" + 
                "^!\n" + 
                "1,1");
            CollectionAssert.AreEqual(new int[] { 0, 0 }, level.Boxes[0]);
        }
    }
}
