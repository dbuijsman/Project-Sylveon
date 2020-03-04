using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BackEnd
{
	public partial class Api
	{
		async public static Task<bool> AddCandidate(string name)
		{
			return await Database.AddNewCandidate(name) != null;
		}
    }
}