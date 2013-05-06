using PimIntegration.Tasks.VismaGlobal.Dto;

namespace PimIntegration.Tasks.VismaGlobal.Interfaces
{
	public interface IArticleManager
	{
		string CreateArticle(ArticleForCreate article);
	}
}