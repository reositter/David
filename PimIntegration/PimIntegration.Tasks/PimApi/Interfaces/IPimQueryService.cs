using System;
using PimIntegration.Tasks.PIMServiceEndpoint;

namespace PimIntegration.Tasks.PimApi.Interfaces
{
	public interface IPimQueryService
	{
		ProductQueryResponseItem[] GetNewProductsSince(DateTime lastRequest);
		ProductQueryResponseItem[] GetNewProductsSinceDummy();
		ProductQueryResponseItem[] DequeueProductQueryResponse(int messageId);
		ProductQueryResponseItem GetProductBySku(string sku);
	}
}