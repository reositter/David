namespace PimIntegration.Tasks.Database.Dto
{
	public class ArticleForPriceUpdate
	{
		public string ArticleNo { get; private set; }
		public string PimSku { get; private set; }
		public string Market { get; set; }
		public decimal NewPrice { get; set; }

		public ArticleForPriceUpdate(string articleNo, string pimSku, string market)
		{
			ArticleNo = articleNo;
			PimSku = pimSku;
			Market = market;
		}
	}
}