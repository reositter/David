using System;
using System.Threading;
using PimIntegration.Tasks.Database.Interfaces;
using PimIntegration.Tasks.PIMServiceEndpoint;
using PimIntegration.Tasks.PimApi.Interfaces;
using PimIntegration.Tasks.Setup;

namespace PimIntegration.Tasks.PimApi
{
	public class ProductQueryDequeuer : IProductQueryDequeuer 
	{
		private readonly ITaskSettings _settings;
		private readonly IPimRequestLogRepository _pimRequestLogRepository;

		public ProductQueryDequeuer(ITaskSettings settings, IPimRequestLogRepository pimRequestLogRepository)
		{
			_settings = settings;
			_pimRequestLogRepository = pimRequestLogRepository;
		}

		public ProductQueryResponseItem[] DequeueProductQueryRequest(int messageId)
		{
			ProductQueryResponseItem[] products = null;
			var client = new QueueOf_ProductQueryRequest_ProductQueryResponseClient();

			DateTime? dequeuedAt = null;
			var numberOfFailedAttempts = 0;

			Log.ForCurrent.DebugFormat("Dequeueing ProductQueryRequest. MessageId: {0}", messageId);
			for (var i = 0; i < _settings.MaximumNumberOfRetries; i++)
			{
				Thread.Sleep(_settings.MillisecondsBetweenRetries);
				products = client.DequeueMessage(messageId);

				if (products != null)
				{
					dequeuedAt = DateTime.Now;
					break;					
				}

				numberOfFailedAttempts++;
			}

			_pimRequestLogRepository.UpdateRequest(messageId, dequeuedAt, numberOfFailedAttempts);

			return products;
		}

		public ProductQueryResponseItem[] DequeueProductQueryRequestArray(int messageId)
		{
			ProductQueryResponseItem[] products = null;
			var client = new QueueOf_ProductQueryRequestArray_ProductQueryResponseClient();

			DateTime? dequeuedAt = null;
			var numberOfFailedAttempts = 0;

			Log.ForCurrent.DebugFormat("Dequeueing ProductQueryRequestArray. MessageId: {0}", messageId);
			for (var i = 0; i < _settings.MaximumNumberOfRetries; i++)
			{
				Thread.Sleep(_settings.MillisecondsBetweenRetries);
				products = client.DequeueMessage(messageId);

				if (products != null)
				{
					dequeuedAt = DateTime.Now;
					break;
				}

				numberOfFailedAttempts++;
			}

			_pimRequestLogRepository.UpdateRequest(messageId, dequeuedAt, numberOfFailedAttempts);

			return products;
		}
	}
}