using System.Collections.Generic;

namespace BackEnd
{
    public interface IState
    {
        /// <summary>
        /// Returns game's character object
        /// </summary>
        CharacterState Character { get; }

        /// <summary>
        /// Returns list that contains tiles with information from top-left of grid on pos 0, 
        /// bottom-right tile on last pos.
        /// </summary>
        List<TileState> PuzzleTiles { get; }

        /// <summary>
        /// Returns int with puzzle's width
        /// </summary>
        int PuzzleWidth { get; }

        /// <summary>
        /// Returns in with puzzle's height
        /// </summary>
        int PuzzleHeight { get;  }

        int PuzzleLevel { get; }
    }
}
