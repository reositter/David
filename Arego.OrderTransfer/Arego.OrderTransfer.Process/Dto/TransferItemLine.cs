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
		public int StockProfileNo { get; set; }
	    public int WareHouseNo { get; set; }

	    public decimal GetTotalValue()
	    {
		    var totalAmount = (Price * Quantity);

		    if (DiscountInPercent1 != 0)
			    totalAmount = totalAmount * (1 - (DiscountInPercent1/100));

			if (DiscountInPercent2 != 0)
				totalAmount = totalAmount * (1 - (DiscountInPercent2 / 100));

		    return totalAmount;
	    }
    }
}