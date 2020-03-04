using Microsoft.VisualStudio.TestTools.UnitTesting;
using BackEnd;
using System.Collections.Generic;

namespace UnitTest
{
    [TestClass]
    public class LevelSolutionTest
    {
        [TestMethod]
        public void RunSingleCommandTest()
        {
            LevelSolution solution = new LevelSolution(1, new Statement[] { new SingleCommand(Command.RotateLeft) });
            List<IState> states = solution.States;
            Assert.AreEqual(2, states.Count);
        }

        [TestMethod]
        public void RunMultipleCommandsTest()
        {
            LevelSolution solution = new LevelSolution(1, new Statement[] { new SingleCommand(Command.RotateLeft), new SingleCommand(Command.MoveForward), new SingleCommand(Command.RotateRight) });
            List<IState> states = solution.States;
            Assert.AreEqual(4, states.Count);
        }

        [TestMethod]
        public void LevelSolutionAutoCalculatesLinesTest()
        {
            LevelSolution solution = new LevelSolution(1, new Statement[] { new SingleCommand(Command.RotateLeft), new SingleCommand(Command.MoveForward), new SingleCommand(Command.RotateRight) });
            Assert.AreEqual(3, solution.Lines);
        }

        [TestMethod]
        public void LevelSolutionCalculatesStatesTest()
        {
            LevelSolution solution = new LevelSolution(1, new Statement[] { new SingleCommand(Command.RotateLeft), new SingleCommand(Command.MoveForward), new SingleCommand(Command.RotateRight) });
            Assert.AreEqual(3, solution.NumberOfStates);
        }
        
        [TestMethod]
        public void LevelSolutionSolvedTrueTest()
        {
            Level level = Level.CreateFromString("" +
				".#.aA\n" +
				"#!..<\n" +
				"2,15");
            Puzzle puzzle = new Puzzle(level);
            Statement[] statements = new Statement[] { 
                new While(ConditionParameter.TileFront, ConditionValue.Passable, true, new Statement[] { new SingleCommand(Command.MoveForward) }) };
            LevelSolution solution = new LevelSolution(puzzle, statements);
            Assert.IsTrue(solution.Solved);
        }
        
        [TestMethod]
        public void LevelSolutionSolvedFalseTest()
        {
            Level level = Level.CreateFromString("" +
				".#.aA\n" +
				"#!..<\n" +
				"2,15");
            Puzzle puzzle = new Puzzle(level);
            Statement[] statements = new Statement[] { 
                new SingleCommand(Command.RotateLeft) };
            LevelSolution solution = new LevelSolution(puzzle, statements);
            Assert.IsFalse(solution.Solved);
        }
        
        [TestMethod]
        public void LevelSolutionInfiniteLoopFalseTest()
        {
            Level level = Level.CreateFromString("" +
				".#.aA\n" +
				"#!..<\n" +
				"2,15");
            Puzzle puzzle = new Puzzle(level);
            Statement[] statements = new Statement[] { 
                new While(ConditionParameter.TileFront, ConditionValue.Passable, true, new Statement[] { new SingleCommand(Command.MoveForward) }) };
            LevelSolution solution = new LevelSolution(puzzle, statements);
            List<IState> states = solution.States;
            Assert.IsFalse(solution.IsInfiteLoop);
        }
        
        [TestMethod]
        public void LevelSolutionInfiniteLoopTrueTest()
        {
            Level level = Level.CreateFromString("" +
				".#.aA\n" +
				"#!..<\n" +
				"2,15");
            Puzzle puzzle = new Puzzle(level);
            Statement[] statements = new Statement[] { 
                new While(ConditionParameter.TileCurrent, ConditionValue.Passable, true, new Statement[] { new SingleCommand(Command.RotateLeft) }) };
            LevelSolution solution = new LevelSolution(puzzle, statements);
            Assert.IsTrue(solution.IsInfiteLoop);
        }
    }
}
