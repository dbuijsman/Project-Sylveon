using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace BackEnd
{
    public interface IRepository
    {
        Task<string> ValidateHR(string name);

        Task<string> AddCandidate(string name);

        Task SaveCandidate(CandidateEntity candidate);

        Task<IState> GetTutorialLevel();

        Task<CandidateEntity> GetCandidate(string ID);

        Task<List<CandidateEntity>> GetCandidates();

        Task<List<CandidateEntity>> GetCandidatesAfterTime(DateTime dateTime);

        Task<CandidateEntity> GetLastCandidateBefore(DateTime dateTime);

        Task<List<CandidateEntity>> GetListOfCandidatesWithScores(int LevelNumber);

        Task<int> GetAmountUnsolved(int levelNumber);
    }
}
