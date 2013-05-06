using System.Collections.Generic;
using PimIntegration.Tasks.VismaGlobal.Dto;
using PimIntegration.Tasks.VismaGlobal.Interfaces;
using RG_SRVLib.Interop;

namespace PimIntegration.Tasks.VismaGlobal
{
	public class ArticleManager : VismaConnection, IArticleManager 
	{
		public IList<CreatedArticle> CreateArticles(IList<ArticleForCreate> articles)
		{
			var list = new List<CreatedArticle>();
			var articleComponent = Connection.GetBusinessComponent(GLOBAL_Components.BC_Article);

			foreach (var article in articles)
			{
				var articleNo = CreateArticle(article, articleComponent);
				if (string.IsNullOrEmpty(articleNo))
					list.Add(new CreatedArticle(articleNo, article.PimSku));
			}

			System.Runtime.InteropServices.Marshal.ReleaseComObject(articleComponent);
			return list;
		}

		private string CreateArticle(ArticleForCreate article, IBisComNavigate articleComponent)
		{
			var articleNo = string.Empty;
			articleComponent.bcInitData();
			articleComponent.bcSetInitialValues();

			articleComponent.bcNewRecord();
			articleNo = articleComponent.bcGetStr((int)Article_Properties.ART_ArticleNo);
			articleComponent.bcUpdateStr((int)Article_Properties.ART_Name, article.Name);
			articleComponent.bcUpdateStr(ZUsrFields.ArticleZUsrPimSku, article.PimSku);
			//articleComponent.bcUpdateInt((int) Article_Properties.ART_ExtraCostUnitIINo, article.BrandId);
			//articleComponent.bcUpdateStr((int)Article_Properties.ART_EANNo, article.EAN);
			articleComponent.bcUpdateStr(ZUsrFields.ArticleZUsrLuthmanKortTextDan, string.Empty);
			articleComponent.bcUpdateStr(ZUsrFields.ArticleZUsrLuthmanKortTextSwe, string.Empty);
			articleComponent.bcUpdateStr(ZUsrFields.ArticleZUsrLuthmanKortTextNor, string.Empty);
			articleComponent.bcUpdateStr((int)Article_Properties.ART_SupplArtNo, string.Empty);

			if (article.PostingTemplateNo.HasValue)
				articleComponent.bcUpdateInt((int)Article_Properties.ART_PostingTemplateNo, article.PostingTemplateNo.Value); // Konteringsmall

			if (article.PriceCalcMethodsNo.HasValue)
				articleComponent.bcUpdateInt((int)Article_Properties.ART_PriceCalcMethodsNo, article.PriceCalcMethodsNo.Value); // Prisprofil

			if (article.StockProfileNo.HasValue)
				articleComponent.bcUpdateInt((int)Article_Properties.ART_StockProfileNo, article.StockProfileNo.Value); // Lagerprofil

			var errCode = articleComponent.bcSaveRecord();

			if (errCode != 0)
			{
				articleComponent.bcCancelRecord();
				Log.ForCurrent.ErrorFormat("Attempt to create article failed. SKU: {0} Code {1} - {2}", article.PimSku, errCode, articleComponent.bcGetMessageText(errCode));
			}

			return articleNo;
		}
	}
}