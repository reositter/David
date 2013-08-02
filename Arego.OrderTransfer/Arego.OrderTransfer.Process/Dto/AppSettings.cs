using System.Collections.Generic;

namespace Arego.OrderTransfer.Process.Dto
{
	public class AppSettings
	{
		public PostageCalculationParameters PostageCalculationParameters { get; set; } 
	}

	public class PostageCalculationParameters
	{
		public double PostagePercentage { get; set; }
		public int ExcludeLinesWithStockProfileNo { get; set; }
		public IList<int> ExcludeLinesWithWareHouseNo { get; set; }
	}
}