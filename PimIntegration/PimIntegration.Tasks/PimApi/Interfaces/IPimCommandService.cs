using System.Collections.Generic;
using PimIntegration.Tasks.Database.Dto;
using PimIntegration.Tasks.VismaGlobal.Dto;

namespace PimIntegration.Tasks.PimApi.Interfaces
{
	public interface IPimCommandService
	{
		void ReportVismaProductNumbers(string marketKey, int vendorId, IEnumerable<CreatedArticle> newArticles);
		void PublishStockBalanceUpdates(string marketKey, IEnumerable<ArticleForStockBalanceUpdate> articlesWithStockUpdates);
		void PublishPriceUpdates(string marketKey, IEnumerable<ArticleForPriceAndStockUpdate> articlesWithPriceUpdates);
	}
}