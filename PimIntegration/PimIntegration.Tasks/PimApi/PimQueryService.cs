using System;
using System.Threading;
using PimIntegration.Tasks.PIMServiceEndpoint;
using PimIntegration.Tasks.PimApi.Interfaces;
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
				CreatedOn = lastRequest
			};

			Log.ForCurrent.DebugFormat("Calling PIM API. Primary action: GetProductByDate. Secondary action: None. CreatedOn: {0}", lastRequest.ToString(_settings.TimeStampFormat));
			var messageId = client.EnqueueMessage(queryItem, "GetProductByDate", "None");
			Log.ForCurrent.DebugFormat("MessageId for GetProductByDate reguest: {0}", messageId);
			ProductQueryResponseItem[] products = null;

			var failedAttempts = 0;
			for (var i = 0; i < _settings.MaximumNumberOfRetries; i++)
			{
				Thread.Sleep(_settings.MillisecondsBetweenRetries);
				products = client.DequeueMessage(messageId);

				if (products != null) break;

				failedAttempts++;
			}

			Log.ForCurrent.DebugFormat("Dequeued message {0}. {1} failed attempts.", messageId, failedAttempts);

			return products;
		}

		public ProductQueryResponseItem[] GetNewProductsSinceDummy()
		{
			var client = new QueueOf_ProductQueryRequest_ProductQueryResponseClient();

			var messageId = client.EnqueueMessage(null, "GetProductByDateDummy", "None");
			Log.ForCurrent.DebugFormat("Message ID = {0}", messageId);
			ProductQueryResponseItem[] products = null;

			for (var i = 0; i < _settings.MaximumNumberOfRetries; i++)
			{
				Thread.Sleep(_settings.MillisecondsBetweenRetries);
				products = client.DequeueMessage(messageId);
				if (products != null) break;
			}

			return products;
		}
	}
}
