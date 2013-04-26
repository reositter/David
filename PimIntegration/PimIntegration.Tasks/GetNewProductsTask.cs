using System;
using System.Threading;
using PimIntegration.Tasks.Database;
using PimIntegration.Tasks.PIMServiceEndpoint;
using PimIntegration.Tasks.Setup;

namespace PimIntegration.Tasks
{
	public class GetNewProductsTask
	{
		private readonly ITaskSettings _settings;
		private readonly IPimConversationStateRepository _pimConversationStateRepository;

		public GetNewProductsTask(ITaskSettings settings, IPimConversationStateRepository pimConversationStateRepository)
		{
			_pimConversationStateRepository = pimConversationStateRepository;
		}

		public void Execute()
		{
			var timeStampOfLastRequest = _pimConversationStateRepository.GetTimeStampOfLastRequestForNewProducts() ?? DateTime.Now;

			// Create client for requesting new products
			var client = new QueueOf_ProductQueryRequest_ProductQueryResponseClient();

			// Create filter for request
			var queryItem = new ProductQueryRequestItem
			{
				// Get all products created since last request
				CreatedOn = timeStampOfLastRequest
			};
			
			// Send the request
			var messageId = client.EnqueueMessage(queryItem, "GetProductByGroupAndBrand", "MarketAll");
			ProductQueryResponseItem[] response;

			for (var i = 0; i < _settings.MaximumNumberOfRetries; i++)
			{
				response = client.DequeueMessage(messageId);

				if (response != null) 
					break;

				Thread.Sleep(_settings.MillisecondsBetweenRetries);
			}

			// Create products in Visma Global
		}
	}
}