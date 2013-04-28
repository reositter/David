namespace PimIntegration.Tasks.Setup
{
	public class AppSettings : ITaskSettings, IVismaSettings
	{
		public int MaximumNumberOfRetries { get; set; }
		public int MillisecondsBetweenRetries { get; set; }
		public string TimeStampFormat { get; set; }

		public string DbConnectionString { get; set; }
		public string VismaClientName { get; set; }
		public string VismaBapiKey { get; set; }
		public string VismaUserName { get; set; }
		public string VismaPassword { get; set; }
	}

	public interface IVismaSettings
	{
		string VismaClientName { get; }
		string VismaBapiKey { get; }
		string VismaUserName { get; }
		string VismaPassword { get; }
	}

	public interface ITaskSettings
	{
		int MaximumNumberOfRetries { get; }
		int MillisecondsBetweenRetries { get; }
		string TimeStampFormat { get; }
	}
}
