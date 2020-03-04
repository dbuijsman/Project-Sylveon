using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BackEnd;

namespace React_Frontend.Controllers
{
    [ApiController]
    [Route("api/candidate")]
    public class CandidateController : Controller
    {
        [HttpGet("get")]
        async public Task<ActionResult<string>> getCandidate()
        {
            CandidateEntity candidate = await Api.GetCandidate();
            if (candidate == null)
            {
                return NotFound();
            }
            else
            {
                return JSON.Serialize(candidate);
            }

        }
        [HttpGet("{id}")]
        async public Task<ActionResult<string>> getCandidateName(string id)
        {
            CandidateEntity candidate = await Api.GetCandidate(id);
            if (candidate == null)
            {
                return NotFound();
            }
            else
            {
                return JSON.Serialize(candidate);
            }

        }
        [HttpGet("getUnstartedCandidates"), Authorize]
        async public Task<IEnumerable<CandidateEntity>> GetAllUnstartedCandidates()
        {
            return await Api.GetAllUnstartedCandidate();
        }

        [HttpGet("sessionIDValidation")]
        async public Task<bool> IsSessionValid()
        {
            string sessionID = Request.Headers["Authorization"];
            return await Api.IsUnstarted(sessionID);
        }
        [HttpGet("isStarted")]
        async public Task<bool> isStarted()
        {
            string sessionID = Request.Headers["Authorization"];
            return await Api.IsStarted(sessionID);
        }
        [HttpGet("hasNotYetStarted")]
        async public Task<bool> IsEligible()
        {
            string sessionID = Request.Headers["Authorization"];
            return await Api.HasCandidateNotYetStarted(sessionID);
        }
        [HttpGet("isActive")]
        async public Task<bool> IsActive()
        {
            string sessionID = Request.Headers["Authorization"];
            return await Api.IsCandidateStillActive(sessionID);
        }
    }
}
