using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace BackEnd
{
	public class Level
	{
		private const char WALL = '#';
		private const char FINISH = '!';
		private static readonly char[] START_BY_DIRECTION = { '^', '>', '_', '<' }; //v needs to be reserved for buttons; the order of the array matches the int values of Direction
		private const char MOVABLE_BOX = '*';

		public readonly int LevelNumber;
		public readonly int[] GridSize; //height, width
		public readonly int Par;
		public readonly int[][] Walls;
		public readonly int[] PositionCharacter;
		public readonly Direction DirectionCharacter;
		public readonly int[] End;
		public readonly int[][] Buttons;
		public readonly int[][] Doors;
		public readonly int[][] Boxes;
		public static int TotalLevels => levels.Length - 1; // Returns amount of levels excluding the tutorial.

		[JsonConstructor]
		private Level(int level, int[] gridSize, int par, int[][] walls, int[] positionCharacter, Direction directionCharacter, int[] end, int[][] buttons, int[][] doors, int[][] boxes)
		{
			if(gridSize == null || walls == null || positionCharacter == null || end == null || buttons == null || doors == null || boxes == null)
			{
				throw new ArgumentNullException("Can not set any variables on null.");
			}
			if(level<1 || par <1)
			{
				throw new ArgumentException("All given integers must be positive.");
			}
			if(gridSize.Length != 2)
			{
				throw new ArgumentException("Size must have length and heigth.");
			}
			List<int[]> positions = new List<int[]>{positionCharacter, end};
			positions.AddRange(walls);
			positions.AddRange(boxes);
			foreach(int[] position in positions)
			{
				if(position.Length != 2 || position[0]<0 || position[0]>=gridSize[0] || position[1]<0 || position[1]>=gridSize[1])
				{
					throw new ArgumentException("Positions must have length 2.");
				}
			}
			List<int[]> doorsAndButtons = new List<int[]>();
			doorsAndButtons.AddRange(buttons);
			doorsAndButtons.AddRange(doors);
			foreach(int[] position in doorsAndButtons)
			{
				if(position.Length != 3 || position[1]<0 || position[1]>=gridSize[0] || position[2]<0 || position[2]>=gridSize[1])
				{
					throw new ArgumentException("Positions must have length 2.");
				}
			}
			this.LevelNumber = level;
			this.GridSize = gridSize;
			this.Par = par;
			this.Walls = walls;
			this.PositionCharacter = positionCharacter;
			this.DirectionCharacter = directionCharacter;
			this.End = end;
			this.Buttons = buttons;
			this.Doors = doors;
			this.Boxes = boxes;
		}

		private readonly static Level[] levels = new Level[] {
			CreateFromString("" +
				".!.\n" +
				"#A#\n" +
				"a^*\n" +
				"1,8"),
			CreateFromString("" +
				".!.\n" +
				"#A#\n" +
				"a^*\n" +
				"1,8"),
			CreateFromString("" +
				"!#ab\n" +
				"A#.<\n" +
				"B#**\n" +
				"....\n" +
				"2,15"),
			CreateFromString("" +
				"...#....#.\n" +
				"..........\n" +
				".....#.#..\n" +
				".!.......#\n" +
				"..........\n" +
				"..........\n" +
				"..........\n" +
				"..#..^....\n" +
				"........#.\n" +
				"3,6")
		};
		public static Level Get(int level)
		{
			return levels[level];
		}

		/// <summary>
		/// Create a level from a file that is more readable.
		/// Values are as the above constants. Lowercase letters represent buttons and uppercase letters doors, where button a is matched to door A, etc.
		/// The character position and direction is stated in START_BY_DIRECTION, where ^ = North, > = East, _ = South, and < = West.
		/// </summary>
		/// <param name="data">A String representing the level. The last line should be two integers separated by a comma: the first is the level number, the second is the amount of moves considered par.</param>
		/// <returns>A level object representing the provided data.</returns>
		public static Level CreateFromString(string data)
		{
			string[] lines = data.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

			int[] levelDetails = (new List<string>(lines[lines.Length - 1].Split(','))).ConvertAll<int>(s => int.Parse(s)).ToArray();
			int level = levelDetails[0];
			int[] gridSize = new int[] { lines.Length - 1, lines[1].Length };
			int par = levelDetails[1];

			List<int[]> walls = new List<int[]>();
			int[] positionCharacter = new int[2];
			Direction directionCharacter = Direction.North;
			int[] end = new int[2];
			List<int[]> buttons = new List<int[]>();
			List<int[]> doors = new List<int[]>();
			List<int[]> boxes = new List<int[]>();

			for (int r = 0; r < lines.Length - 1; r++)
			{
				for (int c = 0; c < lines[r].Length; c++)
				{
					char label = lines[r][c];
					switch (label)
					{
						case WALL:
							walls.Add(new int[] { r, c });
							break;
						case FINISH:
							end = new int[] { r, c };
							break;
						case MOVABLE_BOX:
							boxes.Add(new int[] { r, c });
							break;
						default:
							if (char.IsLower(label))
							{
								buttons.Add(new int[] { label - 'a', r, c });
							}
							if (char.IsUpper(label))
							{
								doors.Add(new int[] { label - 'A', r, c });
							}
							int start = Array.IndexOf(START_BY_DIRECTION, label);
							if (start > -1)
							{
								positionCharacter = new int[] { r, c };
								directionCharacter = (Direction)start;
							}
							break;
					}
				}
			}

			return new Level(level, gridSize, par, walls.ToArray(), positionCharacter, directionCharacter, end, buttons.ToArray(), doors.ToArray(), boxes.ToArray());
		}
	}
}
