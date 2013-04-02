namespace Arego.OrderTransfer.Process.Dto
{
    public class TransferItemLine
    {
        public string ArticleNo { get; set; }
        public string ArticleName { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountInPercent1 { get; set; }
        public decimal DiscountInPercent2 { get; set; }
		public decimal Quantity { get; set; }
    }
}