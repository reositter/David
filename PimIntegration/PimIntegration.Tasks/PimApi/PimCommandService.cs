using System;
using System.Collections.Generic;
using System.Linq;
using PimIntegration.Tasks.Database.Dto;
using PimIntegration.Tasks.PIMServiceEndpoint;
using PimIntegration.Tasks.PimApi.Interfaces;
using PimIntegration.Tasks.VismaGlobal.Dto;

namespace PimIntegration.Tasks.PimApi
{
	public class PimCommandService : IPimCommandService 
	{
		private readonly IProductUpdateEnqueuer _productUpdateEnqueuer;
		private readonly IProductUpdateDequeuer _productUpdateDequeuer;

		public PimCommandService(
			IProductUpdateEnqueuer productUpdateEnqueuer, 
			IProductUpdateDequeuer productUpdateDequeuer)
		{
			_productUpdateEnqueuer = productUpdateEnqueuer;
			_productUpdateDequeuer = productUpdateDequeuer;
		}

		public void ReportVismaArticleNumbers(string marketKey, int vendorId, IEnumerable<CreatedArticle> newProducts)
		{
			var productUpdates = newProducts.Select(newProduct => new ProductUpdateRequestItem
			{
				SKU = newProduct.PimSku,
				MarketName = marketKey,
				VendorId = vendorId,
				ProductCodeVendor = newProduct.ArticleNo
			}).ToArray();

			var messageId = _productUpdateEnqueuer.EnqueueProductUpdateRequestArray(PrimaryAction.UpdateProductBySku, SecondaryAction.IdentificationDetails, productUpdates);
			_productUpdateDequeuer.DequeueProductUpdateResponseArray(messageId);
		}

		public void PublishStockBalanceUpdates(string marketKey, IEnumerable<ArticleForStockBalanceUpdate> stockBalanceUpdates)
		{
			if (!stockBalanceUpdates.Any()) 
				return;

			var productUpdates = stockBalanceUpdates.Select(article => new ProductUpdateRequestItem
			{
				SKU = article.PimSku,
				MarketName = marketKey,
				Stock = Convert.ToInt32(article.StockBalance)
			}).ToArray();

			var messageId = _productUpdateEnqueuer.EnqueueProductUpdateRequestArray(PrimaryAction.UpdateProductBySku, SecondaryAction.PriceAndStock, productUpdates);
			_productUpdateDequeuer.DequeueProductUpdateResponseArray(messageId);
		}

		public void PublishPriceUpdates(string marketKey, IEnumerable<ArticleForPriceAndStockUpdate> articlesWithPriceUpdates)
		{
			if (!articlesWithPriceUpdates.Any())
				return;

			var productUpdates = articlesWithPriceUpdates.Select(article => new ProductUpdateRequestItem
			{
				SKU = article.PimSku,
				MarketName = marketKey,
				Price = article.Price

			}).ToArray();

			var messageId = _productUpdateEnqueuer.EnqueueProductUpdateRequestArray(PrimaryAction.UpdateProductBySku, SecondaryAction.PriceAndStock, productUpdates);
			_productUpdateDequeuer.DequeueProductUpdateResponseArray(messageId);
		}

		public void PublishPriceUpdate(string marketKey, ArticleForPriceAndStockUpdate articleWithPriceUpdates)
		{
			if (articleWithPriceUpdates == null)
				return;

			var productUpdates = new ProductUpdateRequestItem
			{
				SKU = articleWithPriceUpdates.PimSku,
				MarketName = marketKey,
				Price = articleWithPriceUpdates.Price
			};

			var messageId = _productUpdateEnqueuer.EnqueueProductUpdateRequest(PrimaryAction.UpdateProductBySku, SecondaryAction.PriceAndStock, productUpdates);
			_productUpdateDequeuer.DequeueProductUpdateResponse(messageId);
		}

		public ProductUpdateResponseItem[] DequeueProductUpdateResponseWithoutRetries(int messageId)
		{
			return new QueueOf_ProductUpdateRequest_ProductUpdateResponseClient().DequeueMessage(messageId);
		}

		public ProductUpdateResponseItem[] DequeueProductUpdateArrayResponseWithoutRetries(int messageId)
		{
			return new QueueOf_ProductUpdateRequestArray_ProductUpdateResponseClient().DequeueMessage(messageId);
		}
	}
}