using System;
using System.Collections.Generic;
using PimIntegration.Tasks.VismaGlobal.Dto;

namespace PimIntegration.Tasks.Database
{
	public interface IPriceUpdateQuery
	{
		IList<ArticleForPriceAndStockUpdate> GetPriceUpdatesSince(DateTime lastQuery); 
	}
}