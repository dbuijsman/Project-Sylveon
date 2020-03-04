using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using BackEnd;

namespace UnitTest
{
    [TestClass]
    public class SingleStatementTest
    {
        [TestMethod]
        public void ExecutingSingleCommandsTest()
        {
            Statement[] singleStatement = new Statement[]{new SingleCommand(Command.RotateLeft)};
            List<IState> states = (new LevelSolution(1, singleStatement)).States;
            Assert.AreEqual(2, states.Count);
        }

        [TestMethod]
        public void CountLineSingleCommandsTest()
        {
            Statement[] singleStatement = new Statement[]{new SingleCommand(Command.RotateLeft)};
            LevelSolution solution = new LevelSolution(1, singleStatement);
            Assert.AreEqual(1, solution.Lines);
        }

        [TestMethod]
        public void ExecutingHardCodedSolutionTest()
        {
            List<Statement> allSingleCommands = new List<Statement>();
            Command[] allCommands = (Command[]) Enum.GetValues(typeof(Command));
            foreach(Command command in allCommands)
            {
                allSingleCommands.Add(new SingleCommand(command));
            }
            List<IState> states = (new LevelSolution(1, allSingleCommands.ToArray())).States;
            Assert.AreEqual(allSingleCommands.Count+1, states.Count);
        }

        [TestMethod]
        public void CountLinesHardCodedSolutionTest()
        {
            List<Statement> allSingleCommands = new List<Statement>();
            Command[] allCommands = (Command[]) Enum.GetValues(typeof(Command));
            foreach(Command command in allCommands)
            {
                allSingleCommands.Add(new SingleCommand(command));
            }
            LevelSolution solution = new LevelSolution(1, allSingleCommands.ToArray());
            Assert.AreEqual(allSingleCommands.Count, solution.Lines);
        }

        [TestMethod]
        public void IfTrueElseTest()
        {
            Puzzle puzzle = new Puzzle(Level.Get(1));
            ICharacter character = puzzle.Character;
            bool testTrue = character.CheckCondition(ConditionParameter.TileFront, ConditionValue.Passable);
            Direction currentDirection = character.Direction;
            Statement[] statements = new Statement[] { 
                new IfElse(ConditionParameter.TileFront, ConditionValue.Passable, testTrue, 
                    new Statement[] { new SingleCommand(Command.RotateLeft) }, 
                    new Statement[] { new SingleCommand(Command.RotateRight) }
                )};
            List<IState> states = (new LevelSolution(1, statements)).States;
            IState finalState = states[states.Count - 1];
            Direction newDirection = finalState.Character.DirectionCharacter;
            Assert.AreEqual(currentDirection.Left(), newDirection);
        }

        [TestMethod]
        public void IfTrueNoElseTest()
        {
            Puzzle puzzle = new Puzzle(Level.Get(1));
            ICharacter character = puzzle.Character;
            bool testTrue = character.CheckCondition(ConditionParameter.TileFront, ConditionValue.Passable);
            Direction currentDirection = character.Direction;
            Statement[] statements = new Statement[] { 
                new IfElse(ConditionParameter.TileFront, ConditionValue.Passable, testTrue, 
                    new Statement[] { new SingleCommand(Command.RotateLeft) }
                )};
            List<IState> states = (new LevelSolution(1, statements)).States;
            IState finalState = states[states.Count - 1];
            Direction newDirection = finalState.Character.DirectionCharacter;
            Assert.AreEqual(currentDirection.Left(), newDirection);
        }

        [TestMethod]
        public void IfFalseElseTest()
        {
            Puzzle puzzle = new Puzzle(Level.Get(1));
            ICharacter character = puzzle.Character;
            bool testFalse = !character.CheckCondition(ConditionParameter.TileFront, ConditionValue.Passable);
            Direction currentDirection = character.Direction;
            Statement[] statements = new Statement[] { 
                new IfElse(ConditionParameter.TileFront, ConditionValue.Passable, testFalse, 
                    new Statement[] { new SingleCommand(Command.RotateLeft) }, 
                    new Statement[] { new SingleCommand(Command.RotateRight) }
                )};
            List<IState> states = (new LevelSolution(1, statements)).States;
            IState finalState = states[states.Count - 1];
            Direction newDirection = finalState.Character.DirectionCharacter;
            Assert.AreEqual(currentDirection.Right(), newDirection);
        }

        [TestMethod]
        public void IfFalseNoElseTest()
        {
            Puzzle puzzle = new Puzzle(Level.Get(1));
            ICharacter character = puzzle.Character;
            bool testFalse = !character.CheckCondition(ConditionParameter.TileFront, ConditionValue.Passable);
            Statement[] statements = new Statement[] { 
                new IfElse(ConditionParameter.TileFront, ConditionValue.Passable, testFalse,
                    new Statement[] { new SingleCommand(Command.RotateLeft) }
                )};
            List<IState> states = (new LevelSolution(1, statements)).States;
            Assert.AreEqual(1, states.Count);
        }
        
        [TestMethod]
        public void CountLinesIfElseTest()
        {
            Statement[] statements = new Statement[] { 
                new IfElse(ConditionParameter.TileFront, ConditionValue.Passable, true, 
                    new Statement[] { new SingleCommand(Command.RotateLeft), new SingleCommand(Command.RotateLeft) }, 
                    new Statement[] { new SingleCommand(Command.RotateRight) }
                )};
            LevelSolution solution = new LevelSolution(1, statements);
            Assert.AreEqual(4, solution.Lines);
        }

        [TestMethod]
        public void CountLinesIfNoElseTest()
        {
            Statement[] statements = new Statement[] { 
                new IfElse(ConditionParameter.TileFront, ConditionValue.Passable, true, 
                    new Statement[] { new SingleCommand(Command.RotateLeft), new SingleCommand(Command.RotateLeft) }
                )};
            LevelSolution solution = new LevelSolution(1, statements);
            Assert.AreEqual(3, solution.Lines);
        }

        [TestMethod]
        public void RepeatMultipleTimesTest()
        {
            Puzzle puzzle = new Puzzle(Level.Get(1));
            Direction initialDir = puzzle.Character.Direction;
            int repeatingTimes = 3;
            Statement[] statements = new Statement[] { new Repeat((uint)repeatingTimes, new Statement[] { new SingleCommand(Command.RotateLeft) }) };
            List<IState> states = (new LevelSolution(1, statements)).States;
            Assert.AreEqual(repeatingTimes+1, states.Count);
            Assert.AreEqual(initialDir.Right(), states[3].Character.DirectionCharacter);
        }

        [TestMethod]
        public void CountLinesRepeatTest()
        {
            Statement[] statements = new Statement[] { 
                new Repeat(10, new Statement[] { new SingleCommand(Command.RotateLeft), new SingleCommand(Command.RotateLeft) }
                )};
            LevelSolution solution = new LevelSolution(1, statements);
            Assert.AreEqual(3, solution.Lines);
        }
        
        [TestMethod]
        public void WhileWillRepeatUntillDoneTest()
        {
            Level level = Level.CreateFromString("" +
				"!#.aA\n" +
				"#...<\n" +
				"2,15");
            Puzzle puzzle = new Puzzle(level);
            ICharacter character = puzzle.Character;
            Statement[] statements = new Statement[] { 
                new While(ConditionParameter.TileFront, ConditionValue.Passable, true, new Statement[] { new SingleCommand(Command.MoveForward) }) };
            List<IState> states = (new LevelSolution(puzzle, statements,0)).States;
            Assert.AreEqual(4, states.Count);
        }
        
        [TestMethod]
        public void WhileWithIsTrueEqualsFalseTest()
        {
            Level level = Level.CreateFromString("" +
				"!#.aA\n" +
				"#...<\n" +
				"2,15");
            Puzzle puzzle = new Puzzle(level);
            ICharacter character = puzzle.Character;
            Statement[] statements = new Statement[] { 
                new While(ConditionParameter.TileFront, ConditionValue.Impassable, false, new Statement[] { new SingleCommand(Command.MoveForward) }) };
            List<IState> states = (new LevelSolution(puzzle, statements)).States;
            Assert.AreEqual(4, states.Count);
        }
        
        [TestMethod]
        public void WhileWithFalseConditionTest()
        {
            Puzzle puzzle = new Puzzle(Level.Get(1));
            ICharacter character = puzzle.Character;
            bool testFalse = !character.CheckCondition(ConditionParameter.TileFront, ConditionValue.Passable);
            Statement[] statements = new Statement[] { 
                new While(ConditionParameter.TileFront, ConditionValue.Passable, testFalse, new Statement[] { new SingleCommand(Command.RotateLeft) }) };
            List<IState> states = (new LevelSolution(1, statements)).States;
            Assert.AreEqual(1, states.Count);
        }

        [TestMethod]
        public void CountLinesWhileTest()
        {
            Statement[] statements = new Statement[] { 
                new While(ConditionParameter.TileCurrent, ConditionValue.Passable, true, 
                    new Statement[] { new SingleCommand(Command.RotateLeft),new SingleCommand(Command.MoveForward) }) };
            LevelSolution solution = new LevelSolution(1, statements);
            Assert.AreEqual(3, solution.Lines);
        }
        
        [TestMethod]
        public void DoWhileWillRepeatUntillDoneTest()
        {
            Level level = Level.CreateFromString("" +
				"!#.aA\n" +
				"#...<\n" +
				"2,15");
            Puzzle puzzle = new Puzzle(level);
            ICharacter character = puzzle.Character;
            Statement[] statements = new Statement[] { 
                new DoWhile(ConditionParameter.TileFront, ConditionValue.Passable, true, new Statement[] { new SingleCommand(Command.MoveForward) }) };
            List<IState> states = (new LevelSolution(puzzle, statements)).States;
            Assert.AreEqual(4, states.Count);
        }
        
        [TestMethod]
        public void DoWhileWithIsTrueEqualsFalseTest()
        {
            Level level = Level.CreateFromString("" +
				"!#.aA\n" +
				"#...<\n" +
				"2,15");
            Puzzle puzzle = new Puzzle(level);
            ICharacter character = puzzle.Character;
            Statement[] statements = new Statement[] { 
                new DoWhile(ConditionParameter.TileFront, ConditionValue.Impassable, false, new Statement[] { new SingleCommand(Command.MoveForward) }) };
            List<IState> states = (new LevelSolution(puzzle, statements)).States;
            Assert.AreEqual(4, states.Count);
        }
        
        [TestMethod]
        public void DoWhileWithFalseConditionTest()
        {
            Puzzle puzzle = new Puzzle(Level.Get(1));
            ICharacter character = puzzle.Character;
            bool testFalse = !character.CheckCondition(ConditionParameter.TileFront, ConditionValue.Passable);
            Statement[] statements = new Statement[] { 
                new DoWhile(ConditionParameter.TileFront, ConditionValue.Passable, testFalse, new Statement[] { new SingleCommand(Command.RotateLeft) }) };
            List<IState> states = (new LevelSolution(1, statements)).States;
            Assert.AreEqual(3, states.Count);
        }

        [TestMethod]
        public void CountLinesDoWhileTest()
        {
            Statement[] statements = new Statement[] { 
                new DoWhile(ConditionParameter.TileCurrent, ConditionValue.Passable, true, 
                    new Statement[] { new SingleCommand(Command.RotateLeft),new SingleCommand(Command.MoveForward) }) };
            LevelSolution solution = new LevelSolution(1, statements);
            Assert.AreEqual(3, solution.Lines);
        }
        
        [TestMethod]
        public void InfiniteLoopTest()
        {
            Statement[] statements = new Statement[] { 
                new While(ConditionParameter.TileCurrent, ConditionValue.Passable, true, 
                    new Statement[] { new SingleCommand(Command.RotateLeft),new SingleCommand(Command.RotateRight) }) };
            List<IState> states = (new LevelSolution(1, statements)).States;
            Assert.IsTrue(states.Count<6); // 6 because original state is included and the loop should execute at most twice with two statements
        }
        
        [TestMethod]
        public void InfiniteLoopAfterMultipleIterationsTest()
        {
            Statement[] statements = new Statement[] { 
                new While(ConditionParameter.TileCurrent, ConditionValue.Passable, true, new Statement[] { new SingleCommand(Command.RotateLeft) }) };
            List<IState> states = (new LevelSolution(1, statements)).States;
            Assert.IsTrue(states.Count<7); // 7 because original state is included and loop is back at original state after 4 iterations and loop must see the infinite loop after the next iteration.
        }
        
        [TestMethod]
        public void InfiniteLoopShouldNotExecuteCommandsAfterTheLoopTest()
        {
            Statement[] statements = new Statement[] { 
                new While(ConditionParameter.TileCurrent, ConditionValue.Passable, true, new Statement[] { new SingleCommand(Command.RotateLeft),new SingleCommand(Command.RotateRight) }),
                new SingleCommand(Command.RotateLeft) };
            List<IState> states = (new LevelSolution(1, statements)).States;
            Assert.IsTrue(states.Count<6); // 6 because original state is included and the loop should execute at most twice with two statements
        }

        [TestMethod]
        public void InfiniteLoopInNestedLoopTest()
        {
            Statement[] statements = new Statement[] {
                new Repeat(10, new Statement[] {
                    new While(ConditionParameter.TileCurrent, ConditionValue.Passable, true, new Statement[] { new SingleCommand(Command.RotateLeft) })}) };
            List<IState> states = (new LevelSolution(1, statements)).States;
            Assert.IsTrue(states.Count<7);
        }

        [TestMethod]
        public void ExecutingShouldNotGiveMoreThanMaxStatesTest()
        {
            Statement[] statements = new Statement[]{new Repeat(Statement.MaxStates+2, new Statement[]{new SingleCommand(Command.MoveForward)})};
            List<IState> states = (new LevelSolution(1, statements)).States;
            Assert.AreEqual((int)Statement.MaxStates + 1, states.Count);
        }
    }
}
