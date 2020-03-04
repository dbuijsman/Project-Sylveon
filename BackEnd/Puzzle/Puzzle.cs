using System;

namespace BackEnd
{
    public class Puzzle
    {
        public Tile[,] AllTiles { get; private set; }
        public ICharacter Character { get; private set; }
        public FinishTile Finish { get; private set; }
        public bool Finished => Character.Position == Finish;
        public int LevelNumber { get; private set; }

        public Puzzle(Level level)
        {
            ConstructPuzzle(level);
        }

        void ConstructPuzzle(Level level)
        {
            int height = level.GridSize[0];
            int width = level.GridSize[1];
            AllTiles = new Tile[height, width];
            LevelNumber = level.LevelNumber;

            BuildWalls(level);
            BuildButtonsAndDoors(level);
            CreateFinish(level);
            CreatePassableTiles();
            CreateCharacter(level);
            PlaceBoxes(level);
            SetNeighbours();
        }

        void BuildWalls(Level level)
        {
            int[][] walls = level.Walls;
            foreach (var wall in walls)
            {
                AllTiles[wall[0], wall[1]] = new WallTile();
            }
        }

        void BuildButtonsAndDoors(Level level)
        {
            int[][] buttons = level.Buttons;
            int[][] doors = level.Doors;
            foreach (var button in buttons)
            {
                bool foundDoor = false;
                foreach (var door in doors)
                {
                    if (button[0] == door[0])
                    {
                        DoorTile doorTile = new DoorTile();
                        AllTiles[door[1], door[2]] = doorTile;
                        AllTiles[button[1], button[2]] = new ButtonTile(doorTile);
                        foundDoor = true;
                        break;
                    }
                }
                if(!foundDoor){
                    throw new ArgumentException("Can not find the right door for every button.");
                }
            }
        }

        void CreateFinish(Level level)
        {
            int[] finish = level.End;
            AllTiles[finish[0], finish[1]] = Finish = new FinishTile();
        }

        void CreatePassableTiles()
        {
            for (int r = 0; r < AllTiles.GetLength(0); r++)
            {
                for (int c = 0; c < AllTiles.GetLength(1); c++)
                {
                    if (AllTiles[r, c] == null)
                    {
                        AllTiles[r, c] = new Tile();
                    }
                }
            }
        }

        void CreateCharacter(Level level)
        {
            int[] start = level.PositionCharacter;
            if(!AllTiles[start[0], start[1]].Passable)
            {
                throw new ArgumentException("Character can only be placed on passable tiles.");
            }
            Character = new Character(AllTiles[start[0], start[1]], level.DirectionCharacter);
        }

        void PlaceBoxes(Level level)
        {
            int[][] boxes = level.Boxes;
            foreach (var box in boxes)
            {
                Tile tile = AllTiles[box[0], box[1]];
                if (Character.Position == tile)
                {
                    Character.HeldItem = new Box();
                }
                else
                {
                    if(!tile.DropOnto(new Box()))
                    {
                        throw new ArgumentException("Can not correctly place all the boxes.");
                    }
                }
            }
        }

        void SetNeighbours()
        {
            for (int r = 0; r < AllTiles.GetLength(0); r++)
            {
                for (int c = 0; c < AllTiles.GetLength(1); c++)
                {
                    foreach (Direction dir in Enum.GetValues(typeof(Direction)))
                    {
                        (int row, int col) = dir.OfPosition(r, c);
                        if (row >= 0 && col >= 0 && row < AllTiles.GetLength(0) && col < AllTiles.GetLength(1))
                        {
                            AllTiles[r, c].SetNeighbour(AllTiles[row, col], dir);
                        }
                    }
                }
            }
        }
    }
}
