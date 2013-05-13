using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using PimIntegration.Tasks.Database.Dto;
using PimIntegration.Tasks.Database.Interfaces;
using PimIntegration.Tasks.PIMServiceEndpoint;
using PimIntegration.Tasks.PimApi.Dto;
using PimIntegration.Tasks.PimApi.Interfaces;
using PimIntegration.Tasks.Setup;
using PimIntegration.Tasks.VismaGlobal.Dto;

namespace PimIntegration.Tasks.PimApi
{
	public class PimCommandService : IPimCommandService 
	{
		private readonly ITaskSettings _settings;
		private readonly IPimMessageResultRepository _pimMessageResultRepository;

		public PimCommandService(ITaskSettings settings, IPimMessageResultRepository pimMessageResultRepository)
		{
			_settings = settings;
			_pimMessageResultRepository = pimMessageResultRepository;
		}

		public void ReportVismaProductNumbers(string marketKey, int vendorId, IEnumerable<CreatedArticle> newProducts)
		{
			var client = new QueueOf_ProductUpdateRequestArray_ProductUpdateResponseClient();
			var msg = new MessageResult(PrimaryAction.UpdateProductBySku, SecondaryAction.IndentificationDetails);
			var productUpdates = newProducts.Select(newProduct => new ProductUpdateRequestItem
			{
				SKU = newProduct.PimSku,
				MarketName = marketKey,
				VendorId = vendorId,
				ProductCodeVendor = newProduct.ArticleNo
			}).ToArray();

			var messageId = client.EnqueueMessage(msg.PrimaryAction, msg.SecondaryAction, productUpdates);
			
			msg.Status = MessageStatus.Enqueued;
			msg.MessageId = messageId;
			msg.EnqueuedAt = DateTime.Now;

			DequeueMessage(msg, client);

			_pimMessageResultRepository.SaveMessageResult(MapToDbDto(msg));
		}

		private static PimMessageResult MapToDbDto(MessageResult msg)
		{
			return new PimMessageResult
			{
				MessageId = msg.MessageId,
				PrimaryAction = msg.PrimaryAction,
				SecondaryAction = msg.SecondaryAction,
				EnqueuedAt = msg.EnqueuedAt,
				DequeuedAt = msg.DequeuedAt,
				Status = (int)msg.Status
			};
		}

		private void DequeueMessage(MessageResult msg, QueueOf_ProductUpdateRequestArray_ProductUpdateResponseClient client)
		{
			for (var i = 0; i < _settings.MaximumNumberOfRetries; i++)
			{
				Thread.Sleep(_settings.MillisecondsBetweenRetries);
				var result = client.DequeueMessage(msg.MessageId);

				if (result != null)
				{
					msg.DequeuedAt = DateTime.Now;
					msg.Status = MessageStatus.Completed;
					break;
				}

				msg.Status = MessageStatus.NoResponseFound;
				msg.NumberOfFailedAttemptsToDequeue++;
			}
		}

		public void PublishStockBalanceUpdates(string marketKey, IEnumerable<ArticleForStockBalanceUpdate> stockBalanceUpdates)
		{
			if (stockBalanceUpdates.Count() == 0) 
				return;

			var client = new QueueOf_ProductUpdateRequestArray_ProductUpdateResponseClient();
			var msg = new MessageResult(PrimaryAction.UpdateProductBySku, SecondaryAction.PriceAndStock);
			var productUpdates = stockBalanceUpdates.Select(article => new ProductUpdateRequestItem
			{
				SKU = article.PimSku,
				MarketName = marketKey,
				Stock = Convert.ToInt32(article.StockBalance)
			}).ToArray();

			var messageId = client.EnqueueMessage(msg.PrimaryAction, msg.SecondaryAction, productUpdates);

			msg.Status = MessageStatus.Enqueued;
			msg.MessageId = messageId;
			msg.EnqueuedAt = DateTime.Now;

			DequeueMessage(msg, client);

			_pimMessageResultRepository.SaveMessageResult(MapToDbDto(msg));
		}

		public void PublishPriceUpdates(string marketKey, IEnumerable<ArticleForPriceAndStockUpdate> articlesWithPriceUpdates)
		{
			if (articlesWithPriceUpdates.Count() == 0)
				return;

			var client = new QueueOf_ProductUpdateRequestArray_ProductUpdateResponseClient();

			var messageId = client.EnqueueMessage("UpdateProductBySKU", "PriceAndStock", articlesWithPriceUpdates.Select(article => new ProductUpdateRequestItem
			{
				SKU = article.PimSku,
				MarketName = marketKey,
				Price = article.Price

			}).ToArray());

			for (var i = 0; i < _settings.MaximumNumberOfRetries; i++)
			{
				Thread.Sleep(_settings.MillisecondsBetweenRetries);
				var result = client.DequeueMessage(messageId);
				if (result != null) return;
			}

			Log.ForCurrent.ErrorFormat("PublishPriceUpdates: No response found for message ID '{0}'", messageId);
		}
	}
}