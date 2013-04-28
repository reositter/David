using System.Collections.Generic;
using System.Linq;
using System.Threading;
using PimIntegration.Tasks.PIMServiceEndpoint;
using PimIntegration.Tasks.Setup;
using PimIntegration.Tasks.VismaGlobal;

namespace PimIntegration.Tasks.PimApi
{
	public interface IPimCommandService
	{
		void ReportVismaProductNumbers(IEnumerable<ArticleForGetNewProductsScenario> newProducts);
	}

	public class PimCommandService : IPimCommandService 
	{
		private readonly ITaskSettings _settings;

		public PimCommandService(ITaskSettings settings)
		{
			_settings = settings;
		}

		public void ReportVismaProductNumbers(IEnumerable<ArticleForGetNewProductsScenario> newProducts)
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
				if (result != null) break;
				Thread.Sleep(_settings.MillisecondsBetweenRetries);
			}
		}
	}
}