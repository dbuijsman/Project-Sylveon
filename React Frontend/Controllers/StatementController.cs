using System.Text.Json;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BackEnd;

namespace React_Frontend.Controllers
{
	[ApiController]
	[Route("api/statement")]
	public class StatementController : Controller
	{
		private IRepository _repo;
		public StatementController(IRepository repo = null) : base()
		{
			_repo = repo == null ? new MongoDataBase() : repo;
		}
        /// <summary>
        /// Adds a solution from a candidate to the database.
        /// </summary>
        /// <returns> Returns a list of all occuring states.
		/// Returns Bad Request if the ID is invalid, the levelNumber is invalid, the statementTreeJson can not be converted or the converted array is empty.
        /// Returns Gone if there is no time left (i.e. time is up or session is over).
		/// Returns Server Error if there went something wrong with saving the candidate.</returns>
		[HttpPost("{levelNumber}")]
		async public Task<ActionResult<string>> PostStatements([FromHeader(Name="Authorization")] string sessionID, int levelNumber, [FromBody] JsonElement statementTreeJson)
		{
			if(!StatementParser.TryParseStatementTreeJson(statementTreeJson, out IEnumerable<Statement> statementsEnumarble))
			{
				return BadRequest();
			}
			Statement[] statements = statementsEnumarble.ToArray();
			return await SaveAttempt(sessionID, levelNumber, statements);
		}

		
		async public Task<ActionResult<string>> SaveAttempt([FromHeader(Name="Authorization")] string sessionID, int levelNumber, [FromBody] Statement[] statements)
		{
            CandidateEntity candidate = await _repo.GetCandidate(sessionID);
			if(candidate == null){
				return BadRequest();
			}
            if(!candidate.HasTimeLeft())
            {
				GameSessionController gameSessionController = new GameSessionController(_repo);
				LevelSessionController levelSessionController = new LevelSessionController(_repo);
				await levelSessionController.StopLevel(sessionID, levelNumber);
				await gameSessionController.EndSession(sessionID);
                return new StatusCodeResult(410);
            }
			LevelSession levelSession = candidate.GetLevelSession(levelNumber);
			if(levelSession == null || !levelSession.InProgress || statements == null || statements.Length == 0)
			{
				return BadRequest();
			}
			int attempts = levelSession.Solutions.Count();
			LevelSolution solution = levelSession.Attempt(statements);
			await _repo.SaveCandidate(candidate);
			CandidateEntity foundCandidate = await _repo.GetCandidate(candidate.ID);
			if(foundCandidate.GetLevelSession(levelNumber).Solutions.Count() == attempts +1)
			{
				return JSON.Serialize(solution);
			} else
			{
				return BadRequest();
			}
		
		}
	}
}
