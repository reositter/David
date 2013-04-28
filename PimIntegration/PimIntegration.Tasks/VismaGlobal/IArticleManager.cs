using PimIntegration.Tasks.PIMServiceEndpoint;

namespace PimIntegration.Tasks.VismaGlobal
{
	public interface IArticleManager
	{
		string CreateArticle(ProductQueryResponseItem article);
	}
}