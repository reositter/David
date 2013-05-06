using PimIntegration.Tasks.Setup;
using PimIntegration.Tasks.VismaGlobal.Dto;
using PimIntegration.Tasks.VismaGlobal.Interfaces;
using RG_SRVLib.Interop;

namespace PimIntegration.Tasks.VismaGlobal
{
	public class ArticleManager : VismaConnection, IArticleManager 
	{
		private readonly BusinessComponentNavigate _articleComponent;
		private readonly IVismaSettings _settings;

		public ArticleManager(IVismaSettings settings)
		{
			_articleComponent = Connection.GetBusinessComponent(GLOBAL_Components.BC_Article);
			_settings = settings;
		}

		~ArticleManager()
		{
			System.Runtime.InteropServices.Marshal.ReleaseComObject(_articleComponent);
		}

		public string CreateArticle(ArticleForCreate article)
		{
			var articleNo = string.Empty;
			_articleComponent.bcInitData();
			_articleComponent.bcSetInitialValues();

			_articleComponent.bcNewRecord();
			articleNo = _articleComponent.bcGetStr((int)Article_Properties.ART_ArticleNo);
			_articleComponent.bcUpdateStr((int)Article_Properties.ART_Name, article.Name);
			_articleComponent.bcUpdateStr(ZUsrFields.ArticleZUsrPimSku, article.PimSku);
			//_articleComponent.bcUpdateInt((int) Article_Properties.ART_ExtraCostUnitIINo, article.BrandId);
			//_articleComponent.bcUpdateStr((int)Article_Properties.ART_EANNo, article.EAN);
			_articleComponent.bcUpdateStr(ZUsrFields.ArticleZUsrLuthmanKortTextDan, string.Empty);
			_articleComponent.bcUpdateStr(ZUsrFields.ArticleZUsrLuthmanKortTextSwe, string.Empty);
			_articleComponent.bcUpdateStr(ZUsrFields.ArticleZUsrLuthmanKortTextNor, string.Empty);
			_articleComponent.bcUpdateStr((int)Article_Properties.ART_SupplArtNo, string.Empty);

			if (article.PostingTemplateNo.HasValue)
				_articleComponent.bcUpdateInt((int)Article_Properties.ART_PostingTemplateNo, article.PostingTemplateNo.Value); // Konteringsmall

			if (article.PriceCalcMethodsNo.HasValue)
				_articleComponent.bcUpdateInt((int)Article_Properties.ART_PriceCalcMethodsNo, article.PriceCalcMethodsNo.Value); // Prisprofil

			if (article.StockProfileNo.HasValue)
				_articleComponent.bcUpdateInt((int)Article_Properties.ART_StockProfileNo, article.StockProfileNo.Value); // Lagerprofil

			var errCode = _articleComponent.bcSaveRecord();

			if (errCode != 0)
			{
				_articleComponent.bcCancelRecord();
				Log.ForCurrent.ErrorFormat("Attempt to create article failed. SKU: {0} Code {1} - {2}", article.PimSku, errCode, _articleComponent.bcGetMessageText(errCode));
			}

			return articleNo;
		}
	}
}