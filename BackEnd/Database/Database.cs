using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Entities;
using MongoDB.Driver;

namespace BackEnd
{
	public partial class Database
	{

		private static DB MongoDB = new DB(MongoClientSettings.FromConnectionString(
		"mongodb+srv://sylveon-client:development@sylveon-xf66r.azure.mongodb.net/test?retryWrites=true&w=majority"),
		"sylveon");


		async internal static Task<string> AddNewCandidate(string name)
		{
			CandidateEntity candidate = new CandidateEntity(name);
			await MongoDB.SaveAsync(candidate);
			return candidate.ID;
		}
		async internal static Task<CandidateEntity> GetCandidate()
		{
			List<CandidateEntity> unstartedCandidates = await MongoDB.Find<CandidateEntity>()
				.Match(candidate => candidate.started == new DateTime())
				.Limit(1)
				.ExecuteAsync();
			return unstartedCandidates.Count != 0 ? unstartedCandidates.First() : null;
		}

		async internal static Task<CandidateEntity> GetCandidate(string ID)
		{
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
		async internal static Task<IEnumerable<CandidateEntity>> GetAllUnstartedCandidate()
		{
			return (await MongoDB.Find<CandidateEntity>()
			.ManyAsync(a => a.started == new DateTime())).ToList();
		}

		async internal static Task<bool> HasCandidateNotYetStarted(string ID)
		{
			CandidateEntity candidate = await GetCandidate(ID);
			// Checks if candidate exists && candidate is not started && candidate is not finished.
			return candidate != null && !candidate.IsStarted() && !candidate.IsFinished();
		}

		async internal static Task<bool> IsCandidateStillActive(string ID)
		{
			CandidateEntity candidate = await GetCandidate(ID);
			// Checks if candidate exists && candidate is started && candidate is not finished.
			return candidate != null && candidate.IsStarted() && !candidate.IsFinished();
		}
		[Obsolete]
		/// This function checks if a candidate is started instead of unstarted. This method is the same as HasCandidateNotYetStarted
		async internal static Task<bool> IsUnstarted(string ID)
		{
			CandidateEntity candidate = await GetCandidate(ID);
			// Checks if candidate exists && candidate is started
			return candidate != null && !candidate.IsStarted();
		}
	}
}