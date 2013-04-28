using System;
using System.Collections.Generic;
using PimIntegration.Tasks.VismaGlobal.Dto;

namespace PimIntegration.Tasks.Database
{
	public interface IStockBalanceQuery
	{
		IList<ArticleForPriceAndStockUpdate> GetStockBalanceUpdatesSince(DateTime lastQuery);
	}
}