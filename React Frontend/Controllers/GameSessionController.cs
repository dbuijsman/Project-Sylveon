using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BackEnd;

namespace React_Frontend.Controllers
{
	[ApiController]
	[Route("api/session")]
	public class GameSessionController : Controller
	{
		private IRepository _repo;
		public GameSessionController(IRepository repo = null) : base()
		{
			_repo = repo == null ? new MongoDataBase() : repo;
		}
        /// <summary>
        /// Starts a gamesession.
        /// </summary>
        /// <returns> 
		/// Returns Bad Request if the ID is invalid or the candidate is already started.
		/// Returns Server Error if there went something wrong with saving the candidate.</returns>
		[HttpGet("startsession")]
		async public Task<ActionResult> StartSession([FromHeader(Name="Authorization")] string sessionID)
		{
            CandidateEntity candidate = await _repo.GetCandidate(sessionID);
            if(candidate == null || candidate.IsStarted())
            {
                return BadRequest();
            }
            candidate.GameResults = CandidateEntity.newGameResults();
            candidate.started = DateTime.UtcNow;
            await _repo.SaveCandidate(candidate);
            CandidateEntity candidateCheck = await _repo.GetCandidate(sessionID);
			if (candidateCheck.IsStarted())
			{
				return Ok();
			}
			else
			{
				return new StatusCodeResult(500);
			}
		}

        /// <summary>
        /// Ends a gamesession.
        /// </summary>
        /// <returns> 
		/// Returns Bad Request if the ID is invalid or the session of the candidate is not in progress.
		/// Returns Server Error if there went something wrong with saving the candidate.</returns>
		[HttpPost("endSession")]
		async public Task<StatusCodeResult> EndSession([FromHeader(Name="Authorization")] string sessionID)
		{
            CandidateEntity candidate = await _repo.GetCandidate(sessionID);
            if(candidate == null || !candidate.IsStarted() || candidate.IsFinished())
            {
                return BadRequest();
            }
            candidate.finished = DateTime.UtcNow;
            await _repo.SaveCandidate(candidate);
            CandidateEntity candidateCheck = await _repo.GetCandidate(sessionID);
			if (candidateCheck.IsFinished())
			{
				return Ok();
			}
			else
			{
				return new StatusCodeResult(500);
			}
		}
        
        /// <summary>
        /// Total amount of levels.
        /// </summary>
        /// <returns> Returns amount of levels.</returns>
		[HttpGet("totalAmountLevels")]
		public int GetTotalAmountLevels()
		{
			return Level.TotalLevels;
		}

        /// <summary>
        /// Creates an overview with for each level the number of lines, par and if it is solved.
        /// </summary>
        /// <returns> 
		/// Returns Bad Request if the ID is invalid or not started.</returns>
		[HttpGet("getOverview")]
		async public Task<ActionResult<string>> GetOverview([FromHeader(Name="Authorization")] string sessionID)
		{
            CandidateEntity candidate = await _repo.GetCandidate(sessionID);
            if(candidate == null || !candidate.IsStarted())
            {
                return BadRequest();
            }
			Overview overview = new Overview(candidate.GameResults);
			return JSON.Serialize(overview);
		}

		/// <summary>
		/// Returns the remaining time the session has (starting at 20 minutes) in milliseconds.
		/// </summary>
		/// <returns> Remaining time in milliseconds.
        /// Returns Bad Request if the ID is invalid or not started.
        /// Returns Gone if there is no time left (i.e. time is up or session is over)./</returns>
		[HttpGet("remainingTime")]
		async public Task<ActionResult<long>> GetRemainingTime([FromHeader(Name="Authorization")] string sessionID)
		{
            CandidateEntity candidate = await _repo.GetCandidate(sessionID);
            if(candidate == null || !candidate.IsStarted())
            {
                return BadRequest();
            }
			TimeSpan remainingTime = candidate.GetRemainingTime();
			if(remainingTime != TimeSpan.Zero)
			{
				return (long) Math.Floor(remainingTime.TotalMilliseconds);
			}
			else
			{
				foreach(LevelSession levelSession in candidate.GameResults)
				{
					levelSession.Stop();
				}
				await _repo.SaveCandidate(candidate);
				await EndSession(sessionID);
				return new StatusCodeResult(410);
			}
		}
	}
}
