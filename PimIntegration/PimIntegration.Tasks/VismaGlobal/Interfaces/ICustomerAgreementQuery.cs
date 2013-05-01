using System.Collections.Generic;
using PimIntegration.Tasks.Database.Dto;

namespace PimIntegration.Tasks.VismaGlobal.Interfaces
{
	public interface ICustomerAgreementQuery
	{
		decimal GetPrice(int customerNo, string articleNo);
		void PopulateNewPrice(int customerNo, IList<ArticleForPriceUpdate> articlesForPriceUpdate);
	}
}