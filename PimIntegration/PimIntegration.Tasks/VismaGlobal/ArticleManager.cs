using PimIntegration.Tasks.PIMServiceEndpoint;
using PimIntegration.Tasks.Setup;
using RG_SRVLib.Interop;

namespace PimIntegration.Tasks.VismaGlobal
{
	public class ArticleManager : IArticleManager 
	{
		private readonly GlobalServerComponent _vgConnection;
		private readonly BusinessComponentNavigate _articleComponent;
		private readonly IVismaSettings _settings;

		public ArticleManager(GlobalServerComponent vgConnection, IVismaSettings settings)
		{
			_vgConnection = vgConnection;
			_articleComponent = _vgConnection.GetBusinessComponent(GLOBAL_Components.BC_Article);
			_settings = settings;
		}

		~ArticleManager()
		{
			System.Runtime.InteropServices.Marshal.ReleaseComObject(_articleComponent);
		}
		public string CreateArticle(ProductQueryResponseItem article)
		{
			_articleComponent.bcInitData();
			_articleComponent.bcSetInitialValues();

			//_articleComponent.bcSetValueFromStr((int)Article_Properties.ART_ArticleNo, articleNo);
			_articleComponent.bcUpdateStr((int)Article_Properties.ART_Name, article.Model);
			_articleComponent.bcUpdateStr(ZUsrFields.ArticleZUsrPimSku, article.SKU);
			_articleComponent.bcUpdateInt((int) Article_Properties.ART_ExtraCostUnitIINo, article.BrandId);
			_articleComponent.bcUpdateStr((int)Article_Properties.ART_EANNo, article.EAN);
			_articleComponent.bcUpdateStr(ZUsrFields.ArticleZUsrLuthmanKortTextDan, string.Empty);
			_articleComponent.bcUpdateStr(ZUsrFields.ArticleZUsrLuthmanKortTextSwe, string.Empty);
			_articleComponent.bcUpdateStr(ZUsrFields.ArticleZUsrLuthmanKortTextNor, string.Empty);
			_articleComponent.bcUpdateStr((int)Article_Properties.ART_SupplArtNo, string.Empty);
			_articleComponent.bcUpdateInt((int)Article_Properties.ART_PostingTemplateNo, _settings.VismaPostingTemplateNo); // Konteringsmall
			_articleComponent.bcUpdateInt((int)Article_Properties.ART_PriceCalcMethodsNo, _settings.VismaPriceCalcMethodsNo); // Prisprofil
			_articleComponent.bcUpdateInt((int)Article_Properties.ART_StockProfileNo, _settings.VismaStockProfileNo); // Lagerprofil

			var errCode = _articleComponent.bcAddNew();

			if (errCode != 0)
			{
				_articleComponent.bcCancelRecord();
				Log.ForCurrent.ErrorFormat("Attempt to create article failed. SKU: {0} Code {1} - {2}", article.SKU, errCode, _articleComponent.bcGetMessageText(errCode));
			}

			return string.Empty;
		}
	}
}