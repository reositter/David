using System;
using PimIntegration.Tasks.PIMServiceEndpoint;

namespace PimIntegration.Tasks.PimApi
{
	public interface IPimQueryService
	{
		ProductQueryResponseItem[] GetNewProductsSince(DateTime lastRequest);
	}
}