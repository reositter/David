using PimIntegration.Tasks.Setup;

namespace PimIntegration.Test
{
	public class TestBase
	{
		protected ITaskSettings CreateTaskSettings(int millisecondsBetweenRetries, int maximumNumberOfRetries = 5)
		{
			return new TaskSettings
			{
				MaximumNumberOfRetries = maximumNumberOfRetries,
				MillisecondsBetweenRetries = millisecondsBetweenRetries,
				TimeStampFormat = "yyyy-MM-dd HH:mm:ss.sss"
			};
		} 
	}
}