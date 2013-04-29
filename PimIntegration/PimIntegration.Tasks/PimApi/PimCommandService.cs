using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using PimIntegration.Tasks.PIMServiceEndpoint;
using PimIntegration.Tasks.Setup;
using PimIntegration.Tasks.VismaGlobal.Dto;

namespace PimIntegration.Tasks.PimApi
{
	public class PimCommandService : IPimCommandService 
	{
		private readonly ITaskSettings _settings;

		public PimCommandService(ITaskSettings settings)
		{
			_settings = settings;
		}

		public bool ReportVismaProductNumbers(IEnumerable<ArticleForGetNewProductsScenario> newProducts)
		{
			var client = new QueueOf_ProductUpdateRequestArray_ProductUpdateResponseClient();

			var messageId = client.EnqueueMessage("UpdateProductBySKU", "IndentificationDetails", newProducts.Select(newProduct => new ProductUpdateRequestItem
			{
				SKU = newProduct.ResponseItem.SKU, 
				MarketName = string.Empty, 
				VendorId = default(int), 
				ProductCodeVendor = newProduct.VismaArticleNo, 
				EAN = newProduct.ResponseItem.EAN
			}).ToArray());

			for (var i = 0; i < _settings.MaximumNumberOfRetries; i++)
			{
				var result = client.DequeueMessage(messageId);
				if (result != null) return true;
				Thread.Sleep(_settings.MillisecondsBetweenRetries);
			}

			Log.ForCurrent.ErrorFormat("ReportVismaProductNumbers: No response found for message ID '{0}'", messageId);

			return false;
		}

		public bool PublishStockBalanceUpdates(IEnumerable<ArticleForPriceAndStockUpdate> articlesWithStockUpdates)
		{
			if (articlesWithStockUpdates.Count() == 0) 
				return true;

			var client = new QueueOf_ProductUpdateRequestArray_ProductUpdateResponseClient();

			var messageId = client.EnqueueMessage("UpdateProductBySKU", "PriceAndStock", articlesWithStockUpdates.Select(article => new ProductUpdateRequestItem
			{
				SKU = article.PimSku,
				MarketName = string.Empty, // TODO: Fix it
				Stock = Convert.ToInt32(article.StockBalance)

			}).ToArray());

			for (var i = 0; i < _settings.MaximumNumberOfRetries; i++)
			{
				var result = client.DequeueMessage(messageId);
				if (result != null) return true;
				Thread.Sleep(_settings.MillisecondsBetweenRetries);
			}

			Log.ForCurrent.ErrorFormat("PublishStockBalanceUpdates: No response found for message ID '{0}'", messageId);

			return false;
		}

		public bool PublishPriceUpdates(IEnumerable<ArticleForPriceAndStockUpdate> articlesWithPriceUpdates)
		{
			if (articlesWithPriceUpdates.Count() == 0)
				return true;

			var client = new QueueOf_ProductUpdateRequestArray_ProductUpdateResponseClient();

			var messageId = client.EnqueueMessage("UpdateProductBySKU", "PriceAndStock", articlesWithPriceUpdates.Select(article => new ProductUpdateRequestItem
			{
				SKU = article.PimSku,
				MarketName = string.Empty, // TODO: Fix it
				Price = article.Price

			}).ToArray());

			for (var i = 0; i < _settings.MaximumNumberOfRetries; i++)
			{
				var result = client.DequeueMessage(messageId);
				if (result != null) return true;
				Thread.Sleep(_settings.MillisecondsBetweenRetries);
			}

			Log.ForCurrent.ErrorFormat("PublishPriceUpdates: No response found for message ID '{0}'", messageId);

			return false;
		}
	}
}