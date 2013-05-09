namespace PimIntegration.Tasks.Database.Dto
{
	public class ArticleForStockBalanceUpdate
	{
		public string ArticleNo { get; set; }
		public string PimSku { get; set; }
		public decimal StockBalance { get; set; } 
	}
}