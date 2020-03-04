using System.Collections.Generic;

namespace BackEnd
{
    public class SingleCommand : Statement
    {
        private readonly Command _command;
        public SingleCommand(Command command)
        {
            this._command = command;
            this.Command = command.ToString();
        }

        internal override List<State> ExecuteCommand(Puzzle puzzle)
        {
            puzzle.Character.ExecuteCommand(_command);
            return new List<State> { new State(puzzle) };
        }

        internal override int GetLines()
        {
            return 1;
        }
    }
}
