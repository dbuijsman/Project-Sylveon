using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Entities;
using MongoDB.Driver;
using MongoDB.Driver.Linq;


namespace BackEnd {

    public class MongoDataBase : IRepository
    {
        
        private static DB MongoDB = new DB(MongoClientSettings.FromConnectionString(
		"mongodb+srv://sylveon-client:development@sylveon-xf66r.azure.mongodb.net/test?retryWrites=true&w=majority"),
		"sylveon");

        /// <summary>
		/// This method should validate the credentials of HR. This will later be implemented!!!!!!
		/// </summary>
		/// <returns></returns>
        async public Task<string> ValidateHR(string name){
            await emptyTask();
            return "hashedPassword";
        }

        async public Task<string> AddCandidate(string name)
		{
			CandidateEntity candidate = new CandidateEntity(name);
			await MongoDB.SaveAsync(candidate);
			return candidate.ID;
		}

        async public Task SaveCandidate(CandidateEntity candidate){
            await MongoDB.SaveAsync(candidate);
        }

        async public Task<IState> GetTutorialLevel(){
            await emptyTask();
            return new State(new Puzzle(Level.Get(0)));
        }

        async public Task<CandidateEntity> GetCandidate(string ID){
            try
			{
				return await MongoDB.Find<CandidateEntity>().OneAsync(ID);
			} catch(Exception error)
			{
				Console.WriteLine("Can't find any user with ID: " + ID);
				Console.WriteLine(error.StackTrace);
				return null;
			}
        }

        async public Task<List<CandidateEntity>> GetCandidates(){
            return (await MongoDB.Find<CandidateEntity>()
			.ManyAsync(a => a.started == new DateTime())).ToList();
        }

        async public Task<List<CandidateEntity>> GetCandidatesAfterTime(DateTime dateTime){
            IEnumerable<CandidateEntity> newFinishedCandidates = await MongoDB.Find<CandidateEntity>()
                .Match(candidate => (candidate.finished > dateTime))
                .Sort(candidate => candidate.finished, Order.Ascending)
                .ExecuteAsync();
			return newFinishedCandidates.ToList();
        }


        async public Task<CandidateEntity> GetLastCandidateBefore(DateTime dateTime){
            IEnumerable<CandidateEntity> newFinishedCandidates = await MongoDB.Find<CandidateEntity>()
                .Match(candidate => (candidate.finished < dateTime && candidate.finished > new DateTime()))
                .Sort(candidate => candidate.finished, Order.Descending)
                .ExecuteAsync();
			return newFinishedCandidates.ToList().FirstOrDefault();
        }

        /// <summary>
        /// Creates a dictionary consisting of all statistics of all candidates.
        /// </summary>
        /// <returns>Dictionary with for every level has a dictionary of name of the statistic and the combination of data and number of candidates.</returns>
        async public Task<List<CandidateEntity>> GetListOfCandidatesWithScores(int levelNumber){
            return await MongoDB.Collection<CandidateEntity>().AsQueryable()
                .Where(candidate => candidate.finished > new DateTime() && candidate.GameResults[levelNumber-1].Solved)
                .ToListAsync();
        }

        /// <summary>
        /// This method creates a dictionary consisting of the number of candidates that did not solve a certain level.
        /// </summary>
        /// <returns></returns>
        async public Task<int> GetAmountUnsolved(int levelNumber){
            return await MongoDB.Collection<CandidateEntity>().AsQueryable()
					.CountAsync(candidate => candidate.finished > new DateTime() && !candidate.GameResults[levelNumber-1].Solved);
        }

        private Task emptyTask(){
            return Task.Factory.StartNew(() => {});
        }
    }
}