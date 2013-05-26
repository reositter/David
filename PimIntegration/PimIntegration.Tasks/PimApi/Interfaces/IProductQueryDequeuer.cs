using PimIntegration.Tasks.PIMServiceEndpoint;

namespace PimIntegration.Tasks.PimApi.Interfaces
{
	public interface IProductQueryDequeuer
	{
		ProductQueryResponseItem[] DequeueProductQueryRequest(int messageId);
		ProductQueryResponseItem[] DequeueProductQueryRequestArray(int messageId);
	}
}