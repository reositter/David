namespace PimIntegration.Tasks.VismaGlobal.Dto
{
	public class CreatedArticle
	{
		public string ArticleNo { get; private set; }
		public string PimSku { get; private set; }

		public CreatedArticle(string articleNo, string pimSku)
		{
			ArticleNo = articleNo;
			PimSku = pimSku;
		}
	}
}