namespace PimIntegration.Tasks.VismaGlobal.Dto
{
	public class ArticleForCreate
	{
		public string Name { get; set; }
		public string PimSku { get; set; }
		public string ShortDescriptionDen { get; set; }
		public string ShortDescriptionNor { get; set; }
		public string ShortDescriptionSwe { get; set; }
		public int? PostingTemplateNo { get; set; }
		public int? PriceCalcMethodsNo { get; set; }
		public int? StockProfileNo { get; set; }
	}
}