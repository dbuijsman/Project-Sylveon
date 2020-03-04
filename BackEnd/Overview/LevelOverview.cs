
namespace BackEnd
{
    public class LevelOverview{
        public int LevelNumber;
        public bool Solved;
        public int Par;
        public int Lines;
        public LevelOverview(int levelNumber){
            this.LevelNumber = levelNumber;
            this.Solved = false;
        }
        public LevelOverview(LevelSession level){
            this.LevelNumber = level.LevelNumber;
            this.Solved = level.Solved;
            if(level.Solved){
                this.Par = Level.Get(level.LevelNumber).Par;
                this.Lines = level.GetLeastLinesOfCodeSolution().Lines;
            }

        }

    }
}