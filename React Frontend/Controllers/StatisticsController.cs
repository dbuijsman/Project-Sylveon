using System;
using System.Linq;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BackEnd;

namespace React_Frontend.Controllers
{
	[ApiController]
	[Route("api/statistics"), Authorize]
	public class StatisticsController : Controller
	{
		
		private IRepository _repo;
		public StatisticsController(IRepository repo = null) : base()
		{
			_repo = repo == null ? new MongoDataBase() : repo;
		}

        /// <summary>
        /// Creates a list of IDs of candidates that are finished after a given time.
        /// </summary>
        /// <returns>List of IDs. 
		/// Returns Bad Request if the time can not be parsed.
		/// Returns Not Found if there is no such candidate.</returns>
		[HttpGet("newFinished")]
		async public Task<ActionResult<string>> GetNewFinished(string time)
		{
			string timeReceivedRequest = DateTime.UtcNow.ToString(); // The time of receiving will be send in the response
			if(time==null){ // If no time is given, then this method will get the time from the token
				ClaimsIdentity identity = (ClaimsIdentity)User.Identity;
				time = identity.FindFirst("Time").Value;
			}
			if(!DateTime.TryParse(time, out DateTime givenTime))
			{
				return BadRequest(timeReceivedRequest);
			}
			List<CandidateEntity> newFinishedCandidates = await _repo.GetCandidatesAfterTime(givenTime);
			if(newFinishedCandidates.Count==0)
			{
				return NotFound(timeReceivedRequest);
			}
			List<string> newFinishedIDs = newFinishedCandidates.Select(candidate => candidate.ID).ToList();
			return JSON.Serialize(new {IDs = newFinishedIDs , time = timeReceivedRequest});
		}

        /// <summary>
        /// Finds the ID of the last candidate that is finished before a given candidate.
        /// </summary>
        /// <returns> ID of the found candidate.
		/// Returns Bad Request if ID is incorrect or the candidate is not finished yet.
		/// Returns Not Found if there is no such candidate.</returns>
		[HttpGet("previousFinished")]
		async public Task<ActionResult<string>> GetPreviousFinished(string ID)
		{
			CandidateEntity currentCandidate = await _repo.GetCandidate(ID);
			if (currentCandidate == null || !currentCandidate.IsFinished())
			{
				return BadRequest();
			}
			CandidateEntity lastFinishedCandidate = await _repo.GetLastCandidateBefore(currentCandidate.finished);
			if (lastFinishedCandidate == null)
			{
				return NotFound();
			}
			else
			{
				return lastFinishedCandidate.ID;
			}
		}

        /// <summary>
        /// Finds the ID of the first candidate that is finished after a given candidate.
        /// </summary>
        /// <returns> ID of the found candidate.
		/// Returns Bad Request if ID is incorrect or the candidate is not finished yet.
		/// Returns Not Found if there is no such candidate.</returns>
		[HttpGet("nextFinished")]
		async public Task<ActionResult<string>> GetNextFinished(string ID)
		{
			CandidateEntity currentCandidate = await _repo.GetCandidate(ID);
			if (currentCandidate == null || !currentCandidate.IsFinished())
			{
				return BadRequest();
			}
			CandidateEntity nextFinishedCandidate = (await _repo.GetCandidatesAfterTime(currentCandidate.finished)).FirstOrDefault();
			if (nextFinishedCandidate == null)
			{
				return NotFound();
			}
			else
			{
				return nextFinishedCandidate.ID;
			}
		}
		
        /// <summary>
        /// Finds the ID of the last candidate that is finished.
        /// </summary>
        /// <returns> ID of the found candidate.
		/// Returns Not Found if there is no such candidate (i.e. the database does not contains any finished candidates).</returns>
		[HttpGet("lastFinished")]
		async public Task<ActionResult<string>> GetLastFinished()
		{
			CandidateEntity lastFinishedCandidate = await _repo.GetLastCandidateBefore(DateTime.UtcNow);
			if (lastFinishedCandidate == null)
			{
				return NotFound();
			}
			else
			{
				return lastFinishedCandidate.ID;
			}
		}

		/// Making statistics

        /// <summary>
        /// Creates a dictionary consisting of all statistics of a candidate.
        /// </summary>
        /// <returns>Dictionary with for every level has a dictionary of name of the statistic and the data.
		/// Returns Bad Request if the ID is invalid or the candidate is not finished.</returns>
		[HttpGet("candidate")]
		async public Task<ActionResult<string>> MakeStatisticsCandidate(string ID)
		{
			CandidateEntity candidate = await _repo.GetCandidate(ID);
			if(candidate == null || !candidate.IsFinished())
			{
				return BadRequest();
			}
            Dictionary<string,Func<LevelSession,int>> statisticsFunctions = GetStatisticsFunctions();
            Dictionary<int,Dictionary<string,int>> dataCandidate = new Dictionary<int,Dictionary<string,int>>();
            for (int levelNumber=1; levelNumber<=Level.TotalLevels;levelNumber++)
            {
                LevelSession levelSession = candidate.GetLevelSession(levelNumber);
                if(!levelSession.Solved)
                {
                    continue;
                }
                Dictionary<string,int> dataSingleLevel = new Dictionary<string, int>();
                foreach(KeyValuePair<string,Func<LevelSession,int>> nameDataAndFunctionToIntPair in statisticsFunctions){
                    dataSingleLevel.Add(nameDataAndFunctionToIntPair.Key, nameDataAndFunctionToIntPair.Value(levelSession));
                }
                dataCandidate.Add(levelNumber,dataSingleLevel);
                
            }
			return JSON.Serialize(dataCandidate);
		}
		
        /// <summary>
        /// Creates a dictionary consisting of all statistics of all candidates.
        /// </summary>
        /// <returns>Dictionary with for every level has a dictionary of name of the statistic and the combination of data and number of candidates.</returns>
		[HttpGet("tally")]
		async public Task<string> MakeStatisticsEveryone()
		{
            Dictionary<string,Func<LevelSession,int>> statisticsFunctions = GetStatisticsFunctions();
            Dictionary<int,Dictionary<string, Dictionary<int, int>>> dataEveryone = new Dictionary<int,Dictionary<string, Dictionary<int, int>>>();
            for (int levelNumber=1; levelNumber<=Level.TotalLevels;levelNumber++)
            {
                Dictionary<string,Dictionary<int, int>> dataSingleLevel = new Dictionary<string, Dictionary<int, int>>();
                foreach(KeyValuePair<string,Func<LevelSession,int>> nameDataAndFunctionToIntPair in statisticsFunctions){
                    Func<LevelSession,int> functionStatistic = nameDataAndFunctionToIntPair.Value;
					List<CandidateEntity> listOfCandidatesWithScores = await _repo.GetListOfCandidatesWithScores(levelNumber);
                    var pairingScoreAmountCandidates = listOfCandidatesWithScores.GroupBy(candidate => functionStatistic(candidate.GameResults[levelNumber-1]))
						.Select(result => new { Score = result.Key, AmountCandidates = result.Count() })
                        .OrderBy(result => result.Score);
					Dictionary<int,int> levelStatistic = pairingScoreAmountCandidates.ToDictionary(result => result.Score, result => result.AmountCandidates);
                    dataSingleLevel.Add(nameDataAndFunctionToIntPair.Key, levelStatistic);
                }
                dataEveryone.Add(levelNumber, dataSingleLevel);
            }
			return JSON.Serialize(dataEveryone);
		}
		
        /// <summary>
        /// This method creates a dictionary consisting of the number of candidates that did not solve a certain level.
        /// </summary>
        /// <returns></returns>
		[HttpGet("unsolved")]
		async public Task<string> GetAmountUnsolvedPerLevel()
		{
            Dictionary<int,int> amountUnsolved = new Dictionary<int, int>();
            for(int levelNumber=1; levelNumber<=Level.TotalLevels;levelNumber++)
            {
				amountUnsolved[levelNumber] = await _repo.GetAmountUnsolved(levelNumber);
            }
			return JSON.Serialize(amountUnsolved);
		}

        /// <summary>
        /// Creates a list of functions that maps a levelSession to an int. This function are used in constructing dictionaries that represents the statistics.
        /// Add here extra functions in order to add extra statistics.
        /// </summary>
        /// <returns>Dictionary with for name of the statistic creates data that represents the statistic of the given level.</returns>
        private static Dictionary<string,Func<LevelSession,int>> GetStatisticsFunctions()
        {
            Dictionary<string,Func<LevelSession,int>> statisticsFunctions = new Dictionary<string,Func<LevelSession, int>>();
            statisticsFunctions.Add("Regels code kortste oplossing", LevelSession.GetLines);
            statisticsFunctions.Add("Tijd tot korste oplossing", LevelSession.GetDurationPerPeriod(30));
            statisticsFunctions.Add("Pogingen tot korste oplossing", session => session.NumberOfAttemptsForFirstSolved);
            return statisticsFunctions;
        }
	}
}