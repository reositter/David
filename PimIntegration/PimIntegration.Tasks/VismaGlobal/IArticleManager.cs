using PimIntegration.Tasks.PIMServiceEndpoint;
using PimIntegration.Tasks.Setup;

namespace PimIntegration.Tasks.VismaGlobal
{
	public interface IArticleManager
	{
		string CreateArticle(ProductQueryResponseItem article);
	}
}