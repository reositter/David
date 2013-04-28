using PimIntegration.Tasks.PIMServiceEndpoint;
using RG_SRVLib.Interop;

namespace PimIntegration.Tasks.VismaGlobal
{
	public class ArticleManager : IArticleManager 
	{
		private readonly GlobalServerComponent _vgConnection;
		private readonly BusinessComponentNavigate _articleComponent;

		public ArticleManager(GlobalServerComponent vgConnection)
		{
			_vgConnection = vgConnection;
			_articleComponent = _vgConnection.GetBusinessComponent(GLOBAL_Components.BC_Article);
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

			// Konteringsmall
			//_articleComponent.bcUpdateInt((int)Article_Properties.ART_PostingTemplateNo, postingTemplate);
			// Prisprofil
			//_articleComponent.bcUpdateInt((int)Article_Properties.ART_PriceCalcMethodsNo, priceCalcMethodsNo);
			// Lagerprofil
			//_articleComponent.bcUpdateInt((int)Article_Properties.ART_StockProfileNo, stockProfileNo);

			var errCode = _articleComponent.bcAddNew();

			if (errCode != 0)
			{
				_articleComponent.bcCancelRecord();
				//Log.ForCurrent.ErrorFormat(("Attempt to create article '{0} - {1}' failed. Code {2} - {3}", articleNo, name, errCode, _articleComponent.bcGetMessageText(errCode)));
			}

			return string.Empty;
		}
	}
}