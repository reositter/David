using System.Collections.Generic;

namespace Arego.OrderTransfer.Process.Dto
{
    public class TransferItem
    {
        public int InvoiceNo { get; set; }
		public string OrderNo { get; set; }
        public int CustomerNo { get; set; }
        public string CustomerName { get; set; }
        public IList<TransferItemLine> Lines { get; set; }
		public string OrderNoInDestinationClient { get; set; }

	    public TransferItem()
        {
            Lines = new List<TransferItemLine>();
        }
    }
}