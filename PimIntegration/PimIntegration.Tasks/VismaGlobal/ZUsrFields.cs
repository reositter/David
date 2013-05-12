using PimIntegration.Exceptions;
using RG_SRVLib.Interop;

namespace PimIntegration.Tasks.VismaGlobal
{
	public static class ZUsrFields
	{
		public static int ArticleZUsrPimSku;
		public static int ArticleZUsrLuthmanKortTextDen;
		public static int ArticleZUsrLuthmanKortTextSwe;
		public static int ArticleZUsrLuthmanKortTextNor;

		public static void Initialize(GlobalServerComponent vismaConnection)
		{
			object dictionaryObj; //Används som inparameter i bcGetSrvDictioanry då den bara tar Object.

			//Anv dictionary-objektet för att dynamiskt hämta objektid:n för egendefinerade fält.
			vismaConnection.bcGetSrvDictionary(out dictionaryObj); //dictionaryObj får en referens
			var dictComp = (ISrvDictionary)dictionaryObj;

			//Hämta objekt-id:n för egendefinierade fält.
			ArticleZUsrPimSku = dictComp.bcGetObjectIDFromName("ZUsrPimSku", (int)GLOBAL_Components.BC_Article);
			ArticleZUsrLuthmanKortTextDen = dictComp.bcGetObjectIDFromName("ZUsrLuthmanKortTextDAN", (int)GLOBAL_Components.BC_Article);
			ArticleZUsrLuthmanKortTextSwe = dictComp.bcGetObjectIDFromName("ZUsrLuthmanKortTextSWE", (int)GLOBAL_Components.BC_Article);
			ArticleZUsrLuthmanKortTextNor = dictComp.bcGetObjectIDFromName("ZUsrLuthmanKortTextNOR", (int)GLOBAL_Components.BC_Article);

			if (ArticleZUsrPimSku == 0
				|| ArticleZUsrLuthmanKortTextDen == 0
				|| ArticleZUsrLuthmanKortTextSwe == 0
				|| ArticleZUsrLuthmanKortTextNor == 0)
			{
				Log.ForCurrent.InfoFormat("ZUsrPimSku = {0}", ArticleZUsrPimSku);
				Log.ForCurrent.InfoFormat("ZUsrLuthmanKortTextDan = {0}", ArticleZUsrLuthmanKortTextDen);
				Log.ForCurrent.InfoFormat("ZUsrLuthmanKortTextSwe = {0}", ArticleZUsrLuthmanKortTextSwe);
				Log.ForCurrent.InfoFormat("ZUsrLuthmanKortTextNor = {0}", ArticleZUsrLuthmanKortTextNor);
				throw new PimIntegrationConfigurationException("Failed to initalize ZUsrFields. Check log file for clues.");
			}

			System.Runtime.InteropServices.Marshal.ReleaseComObject(dictComp);
			System.Runtime.InteropServices.Marshal.ReleaseComObject(dictionaryObj);
		}
	}
}