using System.Collections.Generic;

namespace BackEnd
{
    public class DoWhile : ConditionalStatement
    {
        public DoWhile(ConditionParameter parameter, ConditionValue value, bool isTrue, Statement[] statements)
            : this(parameter, value, isTrue, new StatementBlock(statements)) { }
        public DoWhile(ConditionParameter parameter, ConditionValue value, bool isTrue, StatementBlock statements) : base(parameter, value, isTrue)
        {
            this._statements = statements;
            this.Code = statements._statements;
        }
        internal override List<State> ExecuteCommand(Puzzle puzzle)
        {
            List<State> states = new List<State>();
            List<State> oldStates = new List<State>();
            State newState = null;
            do
            {
                states.AddRange(_statements.ExecuteCommand(puzzle));
				newState = states.Count>0 ? states[states.Count - 1] : null;
				if (newState == null || oldStates.Contains(newState) || _statements.IsInfiniteLoop)
                {
                    this.IsInfiniteLoop = true;
                    return states;
                }
                oldStates.Add(newState);
            } while (puzzle.Character.CheckCondition(_parameter, _value) == _isTrue);
            return states;
        }

        internal override int GetLines()
        {
            return 1 + _statements.GetLines();
        }
    }
}
