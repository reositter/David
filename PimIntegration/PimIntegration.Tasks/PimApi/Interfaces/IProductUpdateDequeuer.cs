using PimIntegration.Tasks.PIMServiceEndpoint;

namespace PimIntegration.Tasks.PimApi.Interfaces
{
	public interface IProductUpdateDequeuer
	{
		ProductUpdateResponseItem[] DequeueProductUpdateResponse(int messageId);
		ProductUpdateResponseItem[] DequeueProductUpdateResponseArray(int messageId);
	}
}