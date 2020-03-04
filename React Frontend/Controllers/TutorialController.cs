using System.Text.Json;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using BackEnd;

namespace React_Frontend.Controllers
{
	[ApiController]
	[Route("api/tutorial")]
	public class TutorialController : Controller
	{
		[HttpGet("retrieveLevel")]
		public ActionResult<string> GetTutorialLevel()
		{
			string sessionID = Request.Headers["Authorization"];
			if (sessionID=="tutorialSylveon")
			{
				return JSON.Serialize(Api.GetTutorialLevel());
			}
			else
			{
				return Unauthorized();
			}
		}

		[HttpPost("submitSolution")]
		public ActionResult<string> PostStatementsTutorial(int levelId, [FromBody] JsonElement statementTreeJson)
		{
			string sessionID = Request.Headers["Authorization"];
			if (sessionID=="tutorialSylveon")
			{
				if(StatementParser.TryParseStatementTreeJson(statementTreeJson, out IEnumerable<Statement> statements))
				{
                	return JSON.Serialize(Api.SubmitSolutionTutorial(statements.ToArray()));
				} else 
				{
					return BadRequest();
				}
			}
			else
			{
				return Unauthorized();
			}
		}
	}
}
