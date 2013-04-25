using System;
using System.Configuration;
using PimIntegration.Tasks;

namespace PimIntegration.Host
{
	internal static class PimIntegrationSettings
	{
		public static readonly int IntervalInSecondsForGetNewProducts;
		public static readonly int IntervalInSecondsForPublishProductUpdates;
		public static TaskSettings TaskSettings;

		static PimIntegrationSettings()
		{
			Log.ForCurrent.Info("Initalizing config settings");
			IntervalInSecondsForGetNewProducts = Convert.ToInt32(ConfigurationManager.AppSettings["IntervalInSecondsForGetNewProducts"]) * 1000;
			IntervalInSecondsForPublishProductUpdates = Convert.ToInt32(ConfigurationManager.AppSettings["IntervalInSecondsForPublishProductUpdates"]) * 1000;

			TaskSettings = new TaskSettings
			{
				MaximumNumberOfRetries = Convert.ToInt32(ConfigurationManager.AppSettings["MaximumNumberOfRetries"]),
				MillisecondsBetweenRetries = Convert.ToInt32(ConfigurationManager.AppSettings["MillisecondsBetweenRetries"])
			};
		}
	}
}