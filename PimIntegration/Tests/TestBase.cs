using PimIntegration.Tasks.Setup;

namespace PimIntegration.Test
{
	public class TestBase
	{
		protected ITaskSettings CreateTaskSettings(int millisecondsBetweenRetries, int maximumNumberOfRetries = 5)
		{
			return new AppSettings
			{
				MaximumNumberOfRetries = maximumNumberOfRetries,
				MillisecondsBetweenRetries = millisecondsBetweenRetries,
				TimeStampFormat = "yyyy-MM-dd HH:mm:ss.fff"
			};
		} 
	}
}