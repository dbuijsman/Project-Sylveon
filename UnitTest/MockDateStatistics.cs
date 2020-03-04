using Microsoft.AspNetCore.Mvc;
using BackEnd;
using React_Frontend.Controllers;

namespace UnitTest
{
    public class MockDataStatistics
    {
        public static Statement[] GetAnswerLevel1HardCoded()
        {
            return new Statement[]
			{
					new SingleCommand(Command.RotateRight),
					new SingleCommand(Command.PickUp),
					new SingleCommand(Command.RotateLeft),
					new SingleCommand(Command.RotateLeft),
					new SingleCommand(Command.Drop),
					new SingleCommand(Command.RotateRight),
					new SingleCommand(Command.MoveForward),
					new SingleCommand(Command.MoveForward)
			};
        }
        public static Statement[] GetAnswerLevel1LongerWithLoop()
        {
            return new Statement[]
			{
				new While(ConditionParameter.TileCurrent, ConditionValue.Finish, false, new Statement[]
				{
					new SingleCommand(Command.RotateRight),
					new SingleCommand(Command.PickUp),
					new SingleCommand(Command.RotateLeft),
					new SingleCommand(Command.RotateLeft),
					new SingleCommand(Command.Drop),
					new SingleCommand(Command.RotateRight),
					new SingleCommand(Command.MoveForward),
					new SingleCommand(Command.MoveBackward),
					new SingleCommand(Command.MoveForward)
				})
			};
        }
        public static Statement[] GetAnswerLevel2WhileLongRoute()
        {
            return new Statement[]
			{
				new While(ConditionParameter.TileLeft, ConditionValue.Impassable, true, new Statement[]
				{
					new SingleCommand(Command.RotateLeft),
					new SingleCommand(Command.PickUp),
					new SingleCommand(Command.RotateRight),
					new SingleCommand(Command.RotateRight),
					new SingleCommand(Command.Drop),
					new SingleCommand(Command.RotateLeft),
					new SingleCommand(Command.MoveForward)
				}),
				new While(ConditionParameter.TileCurrent, ConditionValue.Finish, false, new Statement[]
				{
					new While(ConditionParameter.TileFront, ConditionValue.Passable, true, new Statement[]
					{
						new SingleCommand(Command.MoveForward)
					}),
					new SingleCommand(Command.RotateRight)
				})
			};
        }
        public static Statement[] GetAnswerLevel3WhileIf()
        {
            return new Statement[]
            {
                new While(ConditionParameter.TileCurrent, ConditionValue.Finish, false, new Statement[]
                {
                    new IfElse(ConditionParameter.TileFront, ConditionValue.Passable, true, 
                        new Statement[]{ new SingleCommand(Command.MoveForward)}, 
                        new Statement[]{ new SingleCommand(Command.RotateLeft)}
                    )
                })
            };
        }
        public static Statement[] GetAnswerLevel3WhileOnly()
        {
            return new Statement[]
            {
                new While(ConditionParameter.TileCurrent, ConditionValue.Finish, false, new Statement[]
                {
                    new SingleCommand(Command.MoveForward),
                    new SingleCommand(Command.RotateLeft),
                    new SingleCommand(Command.MoveForward),
                    new SingleCommand(Command.RotateRight)
                })
            };
        }
        public static Statement[] GetInfiniteLoop()
        {
            return new Statement[] 
            { 
                new While(ConditionParameter.TileCurrent, ConditionValue.Passable, true, new Statement[] { new SingleCommand(Command.RotateLeft) }) 
            };
        }
        public static Statement[] AddSingleCommand(Statement[] statements, SingleCommand command)
        {
            Statement[] longerArray = new Statement[statements.Length+1];
            for(int index=0; index< statements.Length; index++)
            {
                longerArray[index] = statements[index];
            }
            longerArray[statements.Length] = command;
            return longerArray;
        }
    }
}
