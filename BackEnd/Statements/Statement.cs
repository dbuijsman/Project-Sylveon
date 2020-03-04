using System;
using System.Collections.Generic;
using MongoDB.Entities;
using MongoDB.Entities.Core;

namespace BackEnd
{
	public class Statement : Entity
	{
		[Ignore]
		public static uint MaxStates { get; } = 100;
		[Ignore]
		internal bool IsInfiniteLoop { get; set; } = false;
		public string StatementType => this.GetType().ToString();
		public ConditionEntity Condition;
		public Statement[] Code;
		public string Command;
		internal virtual List<State> ExecuteCommand(Puzzle puzzle)
		{
			throw new Exception();
		}
		internal virtual int GetLines()
		{
			throw new Exception();
		}
		///<summary>
		/// This method converts a statement that came from the database to its original type.
		///</summary>
		///<returns>Child of statement that is an instance of the current representation.</returns>
		internal Statement GetStatementAsOriginalType()
		{
			List<object> inputForConstructor = new List<object>();
			if (Condition != null)
			{
				inputForConstructor.AddRange(Condition.ToObjectList());
			}
			if (Command != null)
			{
				if (uint.TryParse(Command, out uint number))
				{
					inputForConstructor.Add(number);
				}
				else
				{
					inputForConstructor.Add((BackEnd.Command)BackEnd.Command.Parse(typeof(BackEnd.Command), Command));
				}
			}
			if (Code != null)
			{
				List<Statement> codeBlock = new List<Statement>();
				foreach (Statement statement in Code)
				{
					codeBlock.Add(statement.GetStatementAsOriginalType());
				}
				inputForConstructor.Add(codeBlock.ToArray());
			}
			return (Statement)Activator.CreateInstance(Type.GetType(StatementType), inputForConstructor.ToArray());
		}
	}
}
