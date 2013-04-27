namespace PimIntegration.Tasks.Setup
{
	public class TaskSettings : ITaskSettings
	{
		public int MaximumNumberOfRetries { get; set; }
		public int MillisecondsBetweenRetries { get; set; }
		public string TimeStampFormat { get; set; }
		public string DbConnectionString { get; set; }
	}

	public interface ITaskSettings
	{
		int MaximumNumberOfRetries { get; }
		int MillisecondsBetweenRetries { get; }
		string TimeStampFormat { get; }
	}
}
