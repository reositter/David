using RG_SRVLib.Interop;

namespace PimIntegration.Tasks.VismaGlobal
{
	public static class ZUsrFields
	{
		public static int ArticleZUsrPimSku;
		public static int ArticleZUsrLuthmanKortTextDan;
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
			ArticleZUsrLuthmanKortTextDan = dictComp.bcGetObjectIDFromName("ZUsrLuthmanKortTextDAN", (int)GLOBAL_Components.BC_Article);
			ArticleZUsrLuthmanKortTextSwe = dictComp.bcGetObjectIDFromName("ZUsrLuthmanKortTextSWE", (int)GLOBAL_Components.BC_Article);
			ArticleZUsrLuthmanKortTextNor = dictComp.bcGetObjectIDFromName("ZUsrLuthmanKortTextNOR", (int)GLOBAL_Components.BC_Article);

			System.Runtime.InteropServices.Marshal.ReleaseComObject(dictComp);
			System.Runtime.InteropServices.Marshal.ReleaseComObject(dictionaryObj);
		}
	}
}