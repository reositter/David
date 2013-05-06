using System.Collections.Generic;
using PimIntegration.Tasks.VismaGlobal.Dto;

namespace PimIntegration.Tasks.PimApi
{
	public interface IPimCommandService
	{
		bool ReportVismaProductNumbers(IEnumerable<CreatedArticle> newArticles);
		bool PublishStockBalanceUpdates(string marketKey, IEnumerable<ArticleForPriceAndStockUpdate> articlesWithStockUpdates);
		bool PublishPriceUpdates(string marketKey, IEnumerable<ArticleForPriceAndStockUpdate> articlesWithPriceUpdates);
	}
}