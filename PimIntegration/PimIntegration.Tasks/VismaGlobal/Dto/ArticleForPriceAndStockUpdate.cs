namespace PimIntegration.Tasks.VismaGlobal.Dto
{
	public class ArticleForPriceAndStockUpdate
	{
		public string PimSku { get; set; }
		public string Market { get; set; }
		public decimal StockBalance { get; set; }
	}
}