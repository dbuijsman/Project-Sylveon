using System;
using System.Collections.Generic;
using MongoDB.Entities;
using MongoDB.Entities.Core;

namespace BackEnd
{
    public class LevelSession : Entity
    {
        public int LevelNumber { get; protected set; }
		public DateTime StartTime { get; protected set; }
		/// <summary>
		/// The duration of the session once it has been stopped or paused
		/// </summary>
		public long TotalDuration { get; protected set; }
        private long CurrentDuration {get
            {
                return InProgress ? TotalDuration + (long) Math.Ceiling((DateTime.UtcNow - StartTime).TotalSeconds) : TotalDuration;
            }
        }
		public bool InProgress { get; private set; }
        public List<LevelSolution> Solutions = new List<LevelSolution>();
        [Ignore]
        public int NumberOfAttemptsForFirstSolved => Solutions.FindIndex(s => s.Solved) + 1;
        public bool Solved;

        public LevelSession(int levelNumber)
        {
            LevelNumber = levelNumber;
        }
        public void Start()
        {
			if (!InProgress)
			{
				StartTime = DateTime.UtcNow;
				InProgress = true;
			}
        }
        public void Stop()
        {
			if (InProgress)
			{
				InProgress = false;
				TotalDuration += (long) Math.Ceiling((DateTime.UtcNow - StartTime).TotalSeconds);
			}
        }
        public LevelSolution Attempt(Statement[] statements)
        {
            if (InProgress)
            {
                LevelSolution solution = new LevelSolution(LevelNumber, statements, CurrentDuration);
                Solutions.Add(solution);
                Solved = Solved || solution.Solved;
                return solution;
            }
            else
            {
                throw new InvalidOperationException("Level session has already ended.");
            }
        }


        public LevelSolution GetLeastLinesOfCodeSolution()
        {
            LevelSolution solution = Util.Min(Solutions.FindAll(s => s.Solved), s => s.Lines);
            if(solution != null)
            {
                try{
                    solution.ConvertCodeToOriginalTypes();
                }
                catch(Exception)
                {
                }
            }
            return solution;
        }
        public static int GetLines(LevelSession session) => session.GetLeastLinesOfCodeSolution().Lines;
        public static Func<LevelSession,int> GetDurationPerPeriod(int period) => (session => GetDuration(session)/period*period);
        public static int GetDuration(LevelSession session) => (int)session.GetLeastLinesOfCodeSolution().Duration;
    }
}
