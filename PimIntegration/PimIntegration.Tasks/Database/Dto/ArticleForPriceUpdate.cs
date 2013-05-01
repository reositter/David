namespace PimIntegration.Tasks.Database.Dto
{
	public class ArticleForPriceUpdate
	{
		public string ArticleNo { get; private set; }
		public string PimSku { get; private set; }

		public ArticleForPriceUpdate(string articleNo, string pimSku)
		{
			ArticleNo = articleNo;
			PimSku = pimSku;
		}
	}
}