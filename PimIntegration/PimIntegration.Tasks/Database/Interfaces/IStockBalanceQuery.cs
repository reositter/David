using System;
using System.Collections.Generic;
using PimIntegration.Tasks.Database.Dto;

namespace PimIntegration.Tasks.Database.Interfaces
{
	public interface IStockBalanceQuery
	{
		IList<ArticleForStockBalanceUpdate> GetStockBalanceUpdatesSince(DateTime lastQuery);
	}
}