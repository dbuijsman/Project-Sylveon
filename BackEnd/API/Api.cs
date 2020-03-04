using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BackEnd
{
	public partial class Api
	{
		public static bool ValidateUser(string username, string hashedPass)
		{
			return Repository.ValidateUser(username, hashedPass);
		}
		public static IState GetTutorialLevel()
		{
			return new State(new Puzzle(Level.Get(0)));
		}
		public static LevelSolution SubmitSolutionTutorial(Statement[] statements)
		{
			return new LevelSolution(0, statements, 0);
		}
	}
}
