using PimIntegration.Tasks.PIMServiceEndpoint;

namespace PimIntegration.Tasks.PimApi.Interfaces
{
	public interface IProductQueryDequeuer
	{
		ProductQueryResponseItem[] DequeueProductQueryResponse(int messageId);
		ProductQueryResponseItem[] DequeueProductQueryResponseArray(int messageId);
	}
}