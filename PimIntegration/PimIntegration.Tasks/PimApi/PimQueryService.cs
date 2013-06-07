using System;
using PimIntegration.Tasks.PIMServiceEndpoint;
using PimIntegration.Tasks.PimApi.Interfaces;

namespace PimIntegration.Tasks.PimApi
{
	public class PimQueryService : IPimQueryService
	{
		private readonly IProductQueryEnqueuer _productQueryEnqueuer;
		private readonly IProductQueryDequeuer _productQueryDequeuer;

		public PimQueryService(
			IProductQueryEnqueuer productQueryEnqueuer,
			IProductQueryDequeuer productQueryDequeuer)
		{
			_productQueryEnqueuer = productQueryEnqueuer;
			_productQueryDequeuer = productQueryDequeuer;
		}

		public ProductQueryResponseItem[] GetNewProductsSince(DateTime lastRequest)
		{
			var queryItem = new ProductQueryRequestItem { CreatedOn = lastRequest };
			var messageId = _productQueryEnqueuer.EnqueueProductQueryRequest(PrimaryAction.GetProductByDate, SecondaryAction.None, queryItem);

			return _productQueryDequeuer.DequeueProductQueryResponse(messageId);
		}

		public ProductQueryResponseItem[] GetNewProductsSinceDummy()
		{
			var messageId = _productQueryEnqueuer.EnqueueProductQueryRequest(PrimaryAction.GetProductByDateDummy, SecondaryAction.None, null);
			return _productQueryDequeuer.DequeueProductQueryResponse(messageId);
		}

		public ProductQueryResponseItem[] DequeueProductQueryResponseWithoutRetries(int messageId)
		{
			return new QueueOf_ProductQueryRequest_ProductQueryResponseClient().DequeueMessage(messageId);
		}

		public ProductQueryResponseItem[] DequeueProductQueryArrayResponseWithoutRetries(int messageId)
		{
			return new QueueOf_ProductQueryRequestArray_ProductQueryResponseClient().DequeueMessage(messageId);
		}

		public ProductQueryResponseItem[] GetProductBySku(string sku)
		{
			var queryItem = new ProductQueryRequestItem { SKU = sku };
			var messageId = _productQueryEnqueuer.EnqueueProductQueryRequest(PrimaryAction.GetProductBySku, SecondaryAction.MarketAll, queryItem);
			var responseItems = _productQueryDequeuer.DequeueProductQueryResponse(messageId);

			return responseItems;
		}
	}
}
