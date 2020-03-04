using System.Collections.Generic;

namespace BackEnd
{
    public class StatementBlock : Statement
    {
        public readonly Statement[] _statements;

        public StatementBlock(Statement[] statements)
        {
            _statements = statements;
        }

        internal override List<State> ExecuteCommand(Puzzle puzzle)
        {
            List<State> states = new List<State>();
            foreach (Statement statement in _statements)
            {
                if (statement == null) continue;
                states.AddRange(statement.ExecuteCommand(puzzle));
                if(statement.IsInfiniteLoop)
                {
                    this.IsInfiniteLoop = true;
                    return (states.Count>MaxStates) ? states.GetRange(0,(int)MaxStates) : states;
                }
            }
            return (states.Count>MaxStates) ? states.GetRange(0,(int)MaxStates) : states;
        }

        internal override int GetLines()
        {
            int lines = 0;
            foreach (Statement statement in _statements)
            {
                if (statement == null) continue;
                lines += statement.GetLines();
            }
            return lines;
        }
    }
}
