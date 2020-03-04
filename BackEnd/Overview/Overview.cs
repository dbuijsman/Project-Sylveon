using System.Collections.Generic;

namespace BackEnd
{
    public class Overview{
        public List<LevelOverview> levels = new List<LevelOverview>();
        public Overview(LevelSession[] levelSessions)
        {
            foreach(LevelSession levelSession in levelSessions)
            {
                levels.Add(new LevelOverview(levelSession));
            }
        }

    }
}