using System.Collections.Generic;
using PimIntegration.Tasks.VismaGlobal.Dto;
using PimIntegration.Tasks.VismaGlobal.Interfaces;
using RG_SRVLib.Interop;

namespace PimIntegration.Tasks.VismaGlobal
{
	public class ArticleManager : IArticleManager
	{
		private readonly IVismaConnection _vismaConnection;
		private string _colZUsrPimSku;

		public ArticleManager(IVismaConnection vismaConnection)
		{
			_vismaConnection = vismaConnection;
		}

		public IList<CreatedArticle> CreateArticles(IList<ArticleForCreate> articles)
		{
			var list = new List<CreatedArticle>();
			var connection = _vismaConnection.Open();
			var articleComponent = connection.GetBusinessComponent(GLOBAL_Components.BC_Article);

			_colZUsrPimSku = articleComponent.bcGetTableObjectName(ZUsrFields.ArticleZUsrPimSku);

			foreach (var article in articles)
			{
				if (ArticleExists(article.PimSku, articleComponent))
				{
					Log.ForCurrent.InfoFormat("Article with PIM SKU '{0}' already exists and will not be created.", article.PimSku);
					continue;
				}

				var articleNo = CreateArticle(article, articleComponent);
				if (string.IsNullOrEmpty(articleNo))
					list.Add(new CreatedArticle(articleNo, article.PimSku));
			}

			System.Runtime.InteropServices.Marshal.ReleaseComObject(articleComponent);
			return list;
		}

		private static string CreateArticle(ArticleForCreate article, IBisComNavigate articleComponent)
		{
			var articleNo = string.Empty;
			articleComponent.bcInitData();
			articleComponent.bcSetInitialValues();

			articleComponent.bcNewRecord();
			articleNo = articleComponent.bcGetStr((int)Article_Properties.ART_ArticleNo);
			articleComponent.bcUpdateStr((int)Article_Properties.ART_Name, article.Name);
			articleComponent.bcUpdateStr(ZUsrFields.ArticleZUsrPimSku, article.PimSku);
			articleComponent.bcUpdateStr(ZUsrFields.ArticleZUsrLuthmanKortTextDen, article.ShortDescriptionDen);
			articleComponent.bcUpdateStr(ZUsrFields.ArticleZUsrLuthmanKortTextNor, article.ShortDescriptionNor);
			articleComponent.bcUpdateStr(ZUsrFields.ArticleZUsrLuthmanKortTextSwe, article.ShortDescriptionSwe);

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

		private bool ArticleExists(string pimSku, IBisComNavigate articleComponent)
		{
			articleComponent.bcSetFilterRequeryStr(string.Format("{0} = '{1}'", _colZUsrPimSku, pimSku));
			articleComponent.bcFetchFirst(0);

			return articleComponent.bcGetNoOfRecords() > 0;
		}
	}
}