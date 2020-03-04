using System;
using System.Collections.Generic;

namespace BackEnd
{
	public class While : ConditionalStatement
	{
		public While(ConditionParameter parameter, ConditionValue value, bool isTrue, Statement[] statements)
			: this(parameter, value, isTrue, new StatementBlock(statements)) { }
		public While(ConditionParameter parameter, ConditionValue value, bool isTrue, StatementBlock statements) : base(parameter, value, isTrue)
		{
			this._statements = statements;
            this.Code = statements._statements;
		}
		internal override List<State> ExecuteCommand(Puzzle puzzle)
		{
			List<State> states = new List<State>();
			List<State> oldStates = new List<State>();
			State newState = null;
			while (puzzle.Character.CheckCondition(_parameter, _value) == _isTrue)
			{
				states.AddRange(_statements.ExecuteCommand(puzzle));
				newState = states.Count>0 ? states[states.Count - 1] : null;
				if (newState == null || oldStates.Contains(newState) || _statements.IsInfiniteLoop)
				{
					this.IsInfiniteLoop = true;
					return states;
				}
				oldStates.Add(newState);
				if (states.Count >= MaxStates)
				{
					return states;
				}
			}
			return states;
		}

		internal override int GetLines()
		{
			return 1 + _statements.GetLines();
		}
	}
}
