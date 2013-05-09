using System.Collections.Generic;
using PimIntegration.Tasks.Database.Dto;
using PimIntegration.Tasks.VismaGlobal.Dto;

namespace PimIntegration.Tasks.PimApi
{
	public interface IPimCommandService
	{
		bool ReportVismaProductNumbers(string marketKey, int vendorId, IEnumerable<CreatedArticle> newArticles);
		bool PublishStockBalanceUpdates(string marketKey, IEnumerable<ArticleForStockBalanceUpdate> articlesWithStockUpdates);
		bool PublishPriceUpdates(string marketKey, IEnumerable<ArticleForPriceAndStockUpdate> articlesWithPriceUpdates);
	}
}