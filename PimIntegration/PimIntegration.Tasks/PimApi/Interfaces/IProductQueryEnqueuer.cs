using PimIntegration.Tasks.PIMServiceEndpoint;

namespace PimIntegration.Tasks.PimApi.Interfaces
{
	public interface IProductQueryEnqueuer
	{
		int EnqueueProductQueryRequest(string primaryAction, string secondaryAction, ProductQueryRequestItem queryItem);
		int EnqueueProductQueryRequestArray(string primaryAction, string secondaryAction, ProductQueryRequestItem[] queryItem);
	}
}