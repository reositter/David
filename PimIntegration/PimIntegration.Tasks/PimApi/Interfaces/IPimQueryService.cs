using System;
using PimIntegration.Tasks.PIMServiceEndpoint;

namespace PimIntegration.Tasks.PimApi.Interfaces
{
	public interface IPimQueryService
	{
		ProductQueryResponseItem[] GetNewProductsSince(DateTime lastRequest);
		ProductQueryResponseItem[] GetNewProductsSinceDummy();
		ProductQueryResponseItem GetProductBySku(string sku);
	}
}