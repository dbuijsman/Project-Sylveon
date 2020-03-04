using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BackEnd
{
	/// <summary>
	/// Currently this is a mock for keeping data in memory without any proper persistence, but this should talk to the database, once we have one
	/// </summary>
	public class Repository
	{

		/// <summary>
		/// This method should validate the credentials of HR. This will later be implemented!!!!!!
		/// </summary>
		/// <returns></returns>
		internal static bool ValidateUser(string username, string hashedPassword)
		{
			return true;
		}
	}
}
