using System;
using PimIntegration.Tasks.PIMServiceEndpoint;

namespace PimIntegration.Tasks.Queries
{
	public interface IPimQueryService
	{
		ProductQueryResponseItem[] GetNewProductsSince(DateTime lastRequest);
	}
}