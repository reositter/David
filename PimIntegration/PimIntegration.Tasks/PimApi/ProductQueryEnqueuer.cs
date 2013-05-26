using System;
using Newtonsoft.Json;
using PimIntegration.Tasks.Database.Dto;
using PimIntegration.Tasks.Database.Interfaces;
using PimIntegration.Tasks.PIMServiceEndpoint;
using PimIntegration.Tasks.PimApi.Interfaces;

namespace PimIntegration.Tasks.PimApi
{
	public class ProductQueryEnqueuer : IProductQueryEnqueuer
	{
		private readonly IPimRequestLogRepository _pimRequestLogRepository;

		public ProductQueryEnqueuer(IPimRequestLogRepository pimRequestLogRepository)
		{
			_pimRequestLogRepository = pimRequestLogRepository;
		}

		public int EnqueueProductQueryRequest(string primaryAction, string secondaryAction, ProductQueryRequestItem queryItem)
		{
			Log.ForCurrent.DebugFormat("Enqueueing ProductQueryRequest. Primary action: {0}. Secondary action: {1}", primaryAction, secondaryAction);
			var messageId = new QueueOf_ProductQueryRequest_ProductQueryResponseClient().EnqueueMessage(queryItem, primaryAction, secondaryAction);

			LogRequest(messageId, primaryAction, secondaryAction, queryItem);

			return messageId;
		}

		public int EnqueueProductQueryRequestArray(string primaryAction, string secondaryAction, ProductQueryRequestItem[] queryItems)
		{
			Log.ForCurrent.DebugFormat("Enqueueing ProductQueryRequestArray. Primary action: {0}. Secondary action: {1}", primaryAction, secondaryAction);
			var messageId = new QueueOf_ProductQueryRequestArray_ProductQueryResponseClient().EnqueueMessage(primaryAction, queryItems, secondaryAction);

			LogRequest(messageId, primaryAction, secondaryAction, queryItems);

			return messageId;
		}

		private void LogRequest(int messageId, string primaryAction, string secondaryAction, object queryItem)
		{
			_pimRequestLogRepository.LogEnqueuedRequest(new EnqueuedRequest
			{
				MessageId = messageId,
				EnqueuedAt = DateTime.Now,
				PrimaryAction = primaryAction,
				SecondaryAction = secondaryAction,
				RequestItem = JsonConvert.SerializeObject(queryItem)
			});
		}
	}
}