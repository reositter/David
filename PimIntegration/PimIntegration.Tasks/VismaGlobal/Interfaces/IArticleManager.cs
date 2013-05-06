using System.Collections.Generic;
using PimIntegration.Tasks.VismaGlobal.Dto;

namespace PimIntegration.Tasks.VismaGlobal.Interfaces
{
	public interface IArticleManager
	{
		IList<CreatedArticle> CreateArticles(IList<ArticleForCreate> articles);
	}
}