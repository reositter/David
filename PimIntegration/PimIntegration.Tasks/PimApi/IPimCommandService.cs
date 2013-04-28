using System.Collections.Generic;
using PimIntegration.Tasks.VismaGlobal;
using PimIntegration.Tasks.VismaGlobal.Dto;

namespace PimIntegration.Tasks.PimApi
{
	public interface IPimCommandService
	{
		void ReportVismaProductNumbers(IEnumerable<ArticleForGetNewProductsScenario> newProducts);
	}
}