using System;
using System.Threading;
using PimIntegration.Tasks.PIMServiceEndpoint;
using PimIntegration.Tasks.Setup;

namespace PimIntegration.Tasks.Queries
{
	public class PimQueryService : IPimQueryService
	{
		private readonly ITaskSettings _settings;

		public PimQueryService(ITaskSettings settings)
		{
			_settings = settings;
		}

		public ProductQueryResponseItem[] GetNewProductsSince(DateTime lastRequest)
		{
			// Create client for requesting new products
			var client = new QueueOf_ProductQueryRequest_ProductQueryResponseClient();

			// Create filter for request
			var queryItem = new ProductQueryRequestItem
			{
				// Get all products created since last request
				CreatedOn = lastRequest,
				ProductGroupId = 10,
				BrandId = 10
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

			return null;
		}
	}
}
