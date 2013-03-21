using RG_SRVLib.Interop;

namespace Arego.OrderTransfer.Process
{
    public class ArticleManager
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
			ReleaseComponent();
	    }

	    public bool ArticleExists(string articleNo)
        {
			_articleComponent.bcSetValueFromStr((int) Article_Properties.ART_ArticleNo, articleNo);
	        _articleComponent.bcFetchEqual();

	        return _articleComponent.bcGetNoOfRecords() > 0;
        }

		public bool CreateArticle(string articleNo, string name, int priceCalcMethodsNo, int postingTemplate, int stockProfileNo)
	    {
		    _articleComponent.bcInitData();
		    _articleComponent.bcSetInitialValues();

		    _articleComponent.bcSetValueFromStr((int)Article_Properties.ART_ArticleNo, articleNo);
		    _articleComponent.bcUpdateStr((int)Article_Properties.ART_Name, name);
			// Konteringsmall
			_articleComponent.bcUpdateInt((int)Article_Properties.ART_PostingTemplateNo, postingTemplate);
			// Prisprofil
			_articleComponent.bcUpdateInt((int)Article_Properties.ART_PriceCalcMethodsNo, priceCalcMethodsNo);
			// Lagerprofil
			_articleComponent.bcUpdateInt((int)Article_Properties.ART_StockProfileNo, stockProfileNo);

			var errCode = _articleComponent.bcAddNew();

		    if (errCode != 0)
		    {
			    _articleComponent.bcCancelRecord();
				LogFileWriter.WriteLine(string.Format("Attempt to create article '{0} - {1}' failed. Code {2} - {3}", articleNo, name, errCode, _articleComponent.bcGetMessageText(errCode)));
			    return false;
		    }

		    return true;
	    }

	    private void ReleaseComponent()
	    {
		    System.Runtime.InteropServices.Marshal.ReleaseComObject(_articleComponent);
	    }
    }
}