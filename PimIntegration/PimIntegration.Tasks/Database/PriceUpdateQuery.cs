using System;
using System.Collections.Generic;
using PimIntegration.Tasks.VismaGlobal.Dto;

namespace PimIntegration.Tasks.Database
{
	public class PriceUpdateQuery : IPriceUpdateQuery 
	{
		public IList<ArticleForPriceAndStockUpdate> GetPriceUpdatesSince(DateTime lastQuery)
		{
			throw new NotImplementedException();
		}
	}
}