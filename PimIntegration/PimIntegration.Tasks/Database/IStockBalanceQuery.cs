using System;
using System.Collections.Generic;
using PimIntegration.Tasks.VismaGlobal.Dto;

namespace PimIntegration.Tasks.Database
{
	public interface IStockBalanceQuery
	{
		IList<StockBalance> GetStockBalanceUpdatesSince(DateTime lastQuery);
	}
}