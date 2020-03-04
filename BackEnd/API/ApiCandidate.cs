using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BackEnd
{
	public partial class Api
	{
		async public static Task<CandidateEntity> GetCandidate()
		{
			return await Database.GetCandidate();
		}
		async public static Task<CandidateEntity> GetCandidate(string ID)
		{
			return await Database.GetCandidate(ID);
		}
		async public static Task<bool> IsUnstarted(string ID)
		{
			return await Database.IsUnstarted(ID);
		}
		async public static Task<IEnumerable<CandidateEntity>> GetAllUnstartedCandidate()
		{
			return await Database.GetAllUnstartedCandidate();
		}
		/// Checks property of candidate
		
		///<summary>
		/// Returns true if candidate has yet to start a session.
		///</summary>
		///<param name="ID"></param>
		///<returns>Bool</returns>
		async public static Task<bool> HasCandidateNotYetStarted(string ID)
		{
			return await Database.HasCandidateNotYetStarted(ID);
		}
		async public static Task<bool> IsStarted(string ID)
		{
			CandidateEntity candidate = await Database.GetCandidate(ID);
			return candidate != null && candidate.IsStarted();
		}
		///<summary>
		/// Returns true if candidate still has an active session available.
		///</summary>
		///<param name="ID"></param>
		///<returns>Bool</returns>
		async public static Task<bool> IsCandidateStillActive(string ID)
		{
			return await Database.IsCandidateStillActive(ID);
		}
    }
}