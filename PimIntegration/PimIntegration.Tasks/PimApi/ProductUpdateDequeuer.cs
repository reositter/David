using System;
using System.Threading;
using PimIntegration.Tasks.Database.Interfaces;
using PimIntegration.Tasks.PIMServiceEndpoint;
using PimIntegration.Tasks.PimApi.Interfaces;
using PimIntegration.Tasks.Setup;

namespace PimIntegration.Tasks.PimApi
{
	public class ProductUpdateDequeuer : IProductUpdateDequeuer 
	{
		private readonly ITaskSettings _settings;
		private readonly IPimRequestLogRepository _pimRequestLogRepository;

		public ProductUpdateDequeuer(ITaskSettings settings, IPimRequestLogRepository pimRequestLogRepository)
		{
			_settings = settings;
			_pimRequestLogRepository = pimRequestLogRepository;
		}

		public ProductUpdateResponseItem[] DequeueProductUpdateResponse(int messageId)
		{
			ProductUpdateResponseItem[] products = null;
			var client = new QueueOf_ProductUpdateRequest_ProductUpdateResponseClient();

			DateTime? dequeuedAt = null;
			var numberOfFailedAttempts = 0;

			Log.ForCurrent.DebugFormat("Dequeueing ProductUpdateResponse. MessageId: {0}", messageId);
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

		public ProductUpdateResponseItem[] DequeueProductUpdateResponseArray(int messageId)
		{
			ProductUpdateResponseItem[] products = null;
			var client = new QueueOf_ProductUpdateRequestArray_ProductUpdateResponseClient();

			DateTime? dequeuedAt = null;
			var numberOfFailedAttempts = 0;

			Log.ForCurrent.DebugFormat("Dequeueing ProductUpdateResponseArray. MessageId: {0}", messageId);
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