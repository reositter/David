using System;
using System.Collections.Generic;

namespace PimIntegration.Tasks.Queries
{
	public interface IPimQueries
	{
		T GetResponse<T>(object ticketId);
		IList<string> GetNewProductsSince(DateTime lastRequest);
	}

	public class PimQueries : IPimQueries {
		public T GetResponse<T>(object ticketId)
		{
			throw new NotImplementedException();
		}

		public IList<string> GetNewProductsSince(DateTime lastRequest)
		{
			throw new NotImplementedException();
		}
	}
}
