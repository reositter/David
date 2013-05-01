using System;
using System.Collections.Generic;
using PimIntegration.Tasks.Database.Dto;

namespace PimIntegration.Tasks.Database.Interfaces
{
	public interface IPriceUpdateQuery
	{
		IList<ArticleForPriceUpdate> GetArticlesForPriceUpdate(DateTime lastQuery); 
	}
}