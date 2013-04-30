namespace PimIntegration.Tasks.VismaGlobal.Dto
{
	public class ArticleForPriceAndStockUpdate
	{
		public string ArticleNo { get; set; }
		public string PimSku { get; set; }
		public decimal StockBalance { get; set; }
		public decimal Price { get; set; }
	}
}