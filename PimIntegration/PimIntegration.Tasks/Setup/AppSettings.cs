namespace PimIntegration.Tasks.Setup
{
	public class AppSettings : ITaskSettings, IVismaSettings
	{
		public int MaximumNumberOfRetries { get; set; }
		public int MillisecondsBetweenRetries { get; set; }
		public string TimeStampFormat { get; set; }

		public string SqliteConnectionString { get; set; }

		public string VismaClientName { get; set; }
		public string VismaBapiKey { get; set; }
		public string VismaUserName { get; set; }
		public string VismaPassword { get; set; }
		public int VismaPostingTemplateNo { get; set; }
		public int VismaPriceCalcMethodsNo { get; set; }
		public int VismaStockProfileNo { get; set; }
		public string VismaDbSchema { get; set; }
		public string VismaDbConnectionString { get; set; }
		public int CustomerNoDenmark { get; set; }
		public int CustomerNoNorway { get; set; }
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
		int CustomerNoDenmark { get; }
		int CustomerNoNorway { get; }
	}

	public interface ITaskSettings
	{
		string SqliteConnectionString { get; set; }
		int MaximumNumberOfRetries { get; }
		int MillisecondsBetweenRetries { get; }
		string TimeStampFormat { get; }
	}
}
