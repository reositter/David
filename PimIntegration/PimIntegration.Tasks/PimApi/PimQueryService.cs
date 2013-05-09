using System;
using System.Threading;
using PimIntegration.Tasks.PIMServiceEndpoint;
using PimIntegration.Tasks.Setup;

namespace PimIntegration.Tasks.PimApi
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
			var client = new QueueOf_ProductQueryRequest_ProductQueryResponseClient();

			var queryItem = new ProductQueryRequestItem
			{
				CreatedOn = lastRequest,
				ProductGroupId = 10,
				BrandId = 10
			};

			var messageId = client.EnqueueMessage(queryItem, "GetProductByDate", "MarketAll");
			ProductQueryResponseItem[] products = null;

			for (var i = 0; i < _settings.MaximumNumberOfRetries; i++)
			{
				products = client.DequeueMessage(messageId);
				if (products != null) break;
				Thread.Sleep(_settings.MillisecondsBetweenRetries);
			}

			return products;
		}

		public ProductQueryResponseItem[] GetNewProductsSinceDummy(DateTime lastRequest)
		{
			var client = new QueueOf_ProductQueryRequest_ProductQueryResponseClient();

			var messageId = client.EnqueueMessage(null, "GetProductByDateDummy", string.Empty);
			ProductQueryResponseItem[] products = null;

			for (var i = 0; i < _settings.MaximumNumberOfRetries; i++)
			{
				products = client.DequeueMessage(messageId);
				if (products != null) break;
				Thread.Sleep(_settings.MillisecondsBetweenRetries);
			}

			return products;
		}
	}
}
