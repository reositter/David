using System.Collections.Generic;
using PimIntegration.Tasks.VismaGlobal.Dto;

namespace PimIntegration.Tasks.PimApi
{
	public interface IPimCommandService
	{
		bool ReportVismaProductNumbers(IEnumerable<ArticleForGetNewProductsScenario> newProducts);
		bool PublishStockBalanceUpdates(IEnumerable<ArticleForPriceAndStockUpdate> articlesWithUpdates);
	}
}