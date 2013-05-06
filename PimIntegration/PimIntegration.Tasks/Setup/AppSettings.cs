using System.Collections.Generic;

namespace PimIntegration.Tasks.Setup
{
	public class AppSettings : ITaskSettings, IVismaSettings
	{
		public AppSettings()
		{
			Markets = new List<Market>();
		}

		public bool TrialMode { get; set; }
		public int NancyUiPort { get; set; }
		public string SqliteConnectionString { get; set; }
		public int MaximumNumberOfRetries { get; set; }
		public int MillisecondsBetweenRetries { get; set; }
		public string TimeStampFormat { get; set; }

		public string VismaClientName { get; set; }
		public string VismaBapiKey { get; set; }
		public string VismaUserName { get; set; }
		public string VismaPassword { get; set; }
		public int VismaPostingTemplateNo { get; set; }
		public int VismaPriceCalcMethodsNo { get; set; }
		public int VismaStockProfileNo { get; set; }
		public string VismaDbSchema { get; set; }
		public string VismaDbConnectionString { get; set; }
		public IList<Market> Markets { get; private set; }
	}

	public interface IVismaSettings
	{
		string VismaClientName { get; }
		string VismaBapiKey { get; }
		string VismaUserName { get; }
		string VismaPassword { get; }
		int VismaPostingTemplateNo { get; }
		int VismaPriceCalcMethodsNo { get; }
		int VismaStockProfileNo { get; }
		string VismaDbSchema { get; }
		string VismaDbConnectionString { get; }
	}

	public interface ITaskSettings
	{
		string SqliteConnectionString { get; set; }
		int MaximumNumberOfRetries { get; }
		int MillisecondsBetweenRetries { get; }
		string TimeStampFormat { get; }
		IList<Market> Markets { get; }
	}

	public class Market
	{
		public string MarketKey { get; private set; }
		public int VendorId { get; private set; }
		public int VismaCustomerNoForPriceCalculation { get; private set; }

		public Market(string marketKey, int vendorId, int vismaCustomerNoForPriceCalculation)
		{
			MarketKey = marketKey;
			VendorId = vendorId;
			VismaCustomerNoForPriceCalculation = vismaCustomerNoForPriceCalculation;
		}
	}
}
