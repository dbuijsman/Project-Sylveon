using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BackEnd;

namespace React_Frontend.Controllers
{
	[ApiController]
	[Route("api/session")]
	public class LevelSessionController : Controller
	{
		private IRepository _repo;
		public LevelSessionController(IRepository repo = null) : base()
		{
			_repo = repo == null ? new MongoDataBase() : repo;
		}
        /// <summary>
        /// Starts a levelSession.
        /// </summary>
        /// <returns> Returns a state of the requested level.
		/// Returns Bad Request if the ID is invalid or the levelNumber is invalid.
        /// Returns Gone if there is no time left (i.e. time is up or session is over).
		/// Returns Server Error if there went something wrong with saving the candidate.</returns>
		[HttpGet("retrieveLevel/{levelNumber}")]
		async public Task<ActionResult<string>> StartLevel([FromHeader(Name="Authorization")] string sessionID, string levelNumber)
		{
            if(!int.TryParse(levelNumber, out int level))
            {
                return BadRequest();
            }
            CandidateEntity candidate = await _repo.GetCandidate(sessionID);
			if(candidate == null || !candidate.IsStarted()){
				return BadRequest();
			}
            if(!candidate.HasTimeLeft())
            {
				GameSessionController controller = new GameSessionController(_repo);
				await StopLevel(sessionID, level);
				await controller.EndSession(sessionID);
                return new StatusCodeResult(410);
            }
			LevelSession levelSession = candidate.GetLevelSession(level);
			if(levelSession == null)
			{
				return BadRequest();
			}
            levelSession.Start();
			await _repo.SaveCandidate(candidate);
			CandidateEntity foundCandidate = await _repo.GetCandidate(sessionID);
			if(foundCandidate.GetLevelSession(level).InProgress)
            {
                return JSON.Serialize(new State(new Puzzle(Level.Get(level))));
            } else 
			{
				return new StatusCodeResult(500);
			}
		}
        /// <summary>
        /// Ends a levelSession.
        /// </summary>
        /// <returns>
		/// Returns Bad Request if the ID is invalid or the levelNumber is invalid.
		/// Returns Server Error if there went something wrong with saving the candidate.</returns>
		[HttpPost("pauseLevel")]
		async public Task<ActionResult> StopLevel([FromHeader(Name="Authorization")] string sessionID, [FromBody]object levelNumber)
		{
            if(!int.TryParse(levelNumber.ToString(), out int level))
            {
                return BadRequest();
            }
			CandidateEntity candidate = await _repo.GetCandidate(sessionID);
			if(candidate == null || !candidate.IsStarted()){
				return BadRequest();
			}
			LevelSession levelSession = candidate.GetLevelSession(level);
			if(levelSession == null)
			{
				return BadRequest();
			}
			levelSession.Stop();
			await _repo.SaveCandidate(candidate);
			CandidateEntity foundCandidate = await _repo.GetCandidate(sessionID);

			if(!foundCandidate.GetLevelSession(level).InProgress)
			{
				return Ok();
			} else {
				return new StatusCodeResult(500);
			}
		}
        /// <summary>
        /// Returns if a level is solved.
        /// </summary>
        /// <returns>
		/// Returns Bad Request if the ID is invalid or the levelNumber is invalid.</returns>
		[HttpGet("levelIsSolved/{levelNumber}")]
		async public Task<ActionResult<bool>> IsSolved([FromHeader(Name="Authorization")] string sessionID, string levelNumber)
		{
            if(!int.TryParse(levelNumber.ToString(), out int level))
            {
                return BadRequest();
            }
			CandidateEntity candidate = await _repo.GetCandidate(sessionID);
			if(candidate == null || !candidate.IsStarted()){
				return BadRequest();
			}
			LevelSession levelSession = candidate.GetLevelSession(level);
			if(levelSession == null)
			{
				return BadRequest();
			}
			return levelSession.Solved;
		}
	}
}
