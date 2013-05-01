using PimIntegration.Tasks.PIMServiceEndpoint;

namespace PimIntegration.Tasks.VismaGlobal.Interfaces
{
	public interface IArticleManager
	{
		string CreateArticle(ProductQueryResponseItem article);
	}
}