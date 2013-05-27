using PimIntegration.Tasks.PIMServiceEndpoint;

namespace PimIntegration.Tasks.PimApi.Interfaces
{
	public interface IProductUpdateEnqueuer
	{
		int EnqueueProductUpdateRequest(string primaryAction, string secondaryAction, ProductUpdateRequestItem queryItem);
		int EnqueueProductUpdateRequestArray(string primaryAction, string secondaryAction, ProductUpdateRequestItem[] queryItem);
	}
}