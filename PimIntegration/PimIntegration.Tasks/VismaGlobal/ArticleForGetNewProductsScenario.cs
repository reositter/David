using PimIntegration.Tasks.PIMServiceEndpoint;

namespace PimIntegration.Tasks.VismaGlobal
{
	public class ArticleForGetNewProductsScenario
	{
		public ProductQueryResponseItem ResponseItem { get; private set; }
		public string VismaArticleNo { get; private set; }

		public ArticleForGetNewProductsScenario(ProductQueryResponseItem responseItem, string vismaArticleNo)
		{
			ResponseItem = responseItem;
			VismaArticleNo = vismaArticleNo;
		}
	}
}