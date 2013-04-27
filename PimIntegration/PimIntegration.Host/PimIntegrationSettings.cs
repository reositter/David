using System;
using System.Configuration;
using PimIntegration.Tasks;
using PimIntegration.Tasks.Setup;

namespace PimIntegration.Host
{
	internal static class PimIntegrationSettings
	{
		public static readonly int IntervalInSecondsForGetNewProducts;
		public static readonly int IntervalInSecondsForPublishProductUpdates;
		public static TaskSettings TaskSettings;

		static PimIntegrationSettings()
		{
			Log.ForCurrent.Info("Reading config settings");
			IntervalInSecondsForGetNewProducts = Convert.ToInt32(ConfigurationManager.AppSettings["IntervalInSecondsForGetNewProducts"]) * 1000;
			IntervalInSecondsForPublishProductUpdates = Convert.ToInt32(ConfigurationManager.AppSettings["IntervalInSecondsForPublishProductUpdates"]) * 1000;

			TaskSettings = new TaskSettings
			{
				MaximumNumberOfRetries = Convert.ToInt32(ConfigurationManager.AppSettings["MaximumNumberOfRetries"]),
				MillisecondsBetweenRetries = Convert.ToInt32(ConfigurationManager.AppSettings["MillisecondsBetweenRetries"]),
				TimeStampFormat = ConfigurationManager.AppSettings["TimeStampFormat"],
				DbConnectionString = ConfigurationManager.ConnectionStrings["SQLite"].ConnectionString
			};

			Log.ForCurrent.InfoFormat("IntervalInSecondsForGetNewProducts converted to ms: {0}", IntervalInSecondsForGetNewProducts);
			Log.ForCurrent.InfoFormat("IntervalInSecondsForPublishProductUpdates converted to ms: {0}", IntervalInSecondsForPublishProductUpdates);
			Log.ForCurrent.InfoFormat("MaximumNumberOfRetries: {0}", TaskSettings.MaximumNumberOfRetries);
			Log.ForCurrent.InfoFormat("MillisecondsBetweenRetries: {0}", TaskSettings.MillisecondsBetweenRetries);
			Log.ForCurrent.InfoFormat("TimeStampFormat: {0}", TaskSettings.TimeStampFormat);
		}
	}
}