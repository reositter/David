namespace PimIntegration.Tasks.Setup
{
	public class TaskSettings
	{
		public int MaximumNumberOfRetries { get; set; }
		public int MillisecondsBetweenRetries { get; set; }
		public string DbConnectionString { get; set; }
	}
}
