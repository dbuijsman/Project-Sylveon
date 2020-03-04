
namespace BackEnd
{
    public enum Direction
    {
        North = 0,
        East = 1,
        South = 2,
        West = 3
    }

    public static class DirectionFunctionality
    {
        public static Direction Right(this Direction dir)
        {
            return dir.NthDirectionClockwise(1);
        }
        public static Direction Opposite(this Direction dir)
        {
            return dir.NthDirectionClockwise(2);
        }
        public static Direction Left(this Direction dir)
        {
            return dir.NthDirectionClockwise(3);
        }

        private static Direction NthDirectionClockwise(this Direction dir, uint n)
        {
            return (Direction)(((int)dir + n) % 4);
        }

        public static (int, int) OfPosition(this Direction dir, int row, int col)
        {
            switch (dir)
            {
                case Direction.North:
                    return (row - 1, col);
                case Direction.West:
                    return (row, col - 1);
                case Direction.South:
                    return (row + 1, col);
                case Direction.East:
                    return (row, col + 1);
                default:
                    throw new System.Exception("This direction is not implemented.");
            }
        }
    }
}