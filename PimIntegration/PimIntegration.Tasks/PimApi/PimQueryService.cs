using System;
using System.Threading;
using PimIntegration.Tasks.Database.Interfaces;
using PimIntegration.Tasks.PIMServiceEndpoint;
using PimIntegration.Tasks.PimApi.Dto;
using PimIntegration.Tasks.PimApi.Interfaces;
using PimIntegration.Tasks.Setup;

namespace PimIntegration.Tasks.PimApi
{
	public class PimQueryService : IPimQueryService
	{
		private readonly ITaskSettings _settings;
		private readonly IPimMessageResultRepository _pimMessageResultRepository;
		private readonly IMapper _mapper;

		public PimQueryService(
			ITaskSettings settings, 
			IPimMessageResultRepository pimMessageResultRepository, 
			IMapper mapper)
		{
			_settings = settings;
			_pimMessageResultRepository = pimMessageResultRepository;
			_mapper = mapper;
		}

		public ProductQueryResponseItem[] GetNewProductsSince(DateTime lastRequest)
		{
			var client = new QueueOf_ProductQueryRequest_ProductQueryResponseClient();
			var msg = new MessageResult(PrimaryAction.GetProductByDate, SecondaryAction.None);

			var queryItem = new ProductQueryRequestItem
			{
				CreatedOn = lastRequest
			};

			Log.ForCurrent.DebugFormat("Calling PIM API. Primary action: GetProductByDate. Secondary action: None. CreatedOn: {0}", lastRequest.ToString(_settings.TimeStampFormat));
			var messageId = client.EnqueueMessage(queryItem, msg.PrimaryAction, msg.SecondaryAction);

			msg.MessageId = messageId;
			msg.EnqueuedAt = DateTime.Now;

			ProductQueryResponseItem[] products = null;

			for (var i = 0; i < _settings.MaximumNumberOfRetries; i++)
			{
				Thread.Sleep(_settings.MillisecondsBetweenRetries);
				products = client.DequeueMessage(messageId);

				if (products != null)
				{
					msg.DequeuedAt = DateTime.Now;
					break;
				}

				msg.NumberOfFailedAttemptsToDequeue++;
			}

			_pimMessageResultRepository.SaveMessageResult(_mapper.MapMessageResultToPimMessageResult(msg));

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

		public ProductQueryResponseItem[] DequeueProductQueryResponse(int messageId)
		{
			var client = new QueueOf_ProductQueryRequest_ProductQueryResponseClient();

			return client.DequeueMessage(messageId);
		}

		public ProductQueryResponseItem GetProductBySku(string sku)
		{
			var client = new QueueOf_ProductQueryRequest_ProductQueryResponseClient();
			var msg = new MessageResult(PrimaryAction.GetProductBySku, SecondaryAction.MarketAll);

			var queryItem = new ProductQueryRequestItem
			{
				SKU = sku
			};

			var messageId = client.EnqueueMessage(queryItem, msg.PrimaryAction, msg.SecondaryAction);

			msg.MessageId = messageId;
			msg.EnqueuedAt = DateTime.Now;

			ProductQueryResponseItem[] responseItems = null;

			for (var i = 0; i < _settings.MaximumNumberOfRetries; i++)
			{
				Thread.Sleep(_settings.MillisecondsBetweenRetries);
				responseItems = client.DequeueMessage(msg.MessageId);

				if (responseItems != null)
				{
					msg.DequeuedAt = DateTime.Now;
					break;
				}

				msg.NumberOfFailedAttemptsToDequeue++;
			}

			_pimMessageResultRepository.SaveMessageResult(_mapper.MapMessageResultToPimMessageResult(msg));

			return responseItems != null ? responseItems[0] : null;
		}
	}
}
