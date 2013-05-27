using System;
using Newtonsoft.Json;
using PimIntegration.Tasks.Database.Dto;
using PimIntegration.Tasks.Database.Interfaces;
using PimIntegration.Tasks.PIMServiceEndpoint;
using PimIntegration.Tasks.PimApi.Interfaces;

namespace PimIntegration.Tasks.PimApi
{
	public class ProductUpdateEnqueuer : IProductUpdateEnqueuer 
	{
		private readonly IPimRequestLogRepository _pimRequestLogRepository;

		public ProductUpdateEnqueuer(IPimRequestLogRepository pimRequestLogRepository)
		{
			_pimRequestLogRepository = pimRequestLogRepository;
		}

		public int EnqueueProductUpdateRequest(string primaryAction, string secondaryAction, ProductUpdateRequestItem queryItem)
		{
			Log.ForCurrent.DebugFormat("Enqueueing ProductUpdateRequest. Primary action: {0}. Secondary action: {1}", primaryAction, secondaryAction);
			var messageId = new QueueOf_ProductUpdateRequest_ProductUpdateResponseClient().EnqueueMessage(primaryAction, secondaryAction, queryItem);

			LogRequest(messageId, primaryAction, secondaryAction, queryItem);

			return messageId;
		}

		public int EnqueueProductUpdateRequestArray(string primaryAction, string secondaryAction, ProductUpdateRequestItem[] queryItem)
		{
			Log.ForCurrent.DebugFormat("Enqueueing ProductUpdateRequestArray. Primary action: {0}. Secondary action: {1}", primaryAction, secondaryAction);
			var messageId = new QueueOf_ProductUpdateRequestArray_ProductUpdateResponseClient().EnqueueMessage(primaryAction, secondaryAction, queryItem);

			LogRequest(messageId, primaryAction, secondaryAction, queryItem);

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