using System;
using System.Configuration;
using PimIntegration.Service;

namespace PimIntegration.Host
{
	internal static class AppSettings
	{
		public static readonly int IntervalInSecondsForGetNewProducts;
		public static readonly int IntervalInSecondsForPublishProductUpdates;

		static AppSettings()
		{
			Log.ForCurrent.Info("Initalizing config settings");
			IntervalInSecondsForGetNewProducts = Convert.ToInt32(ConfigurationManager.AppSettings["IntervalInSecondsForGetNewProducts"]) * 1000;
			IntervalInSecondsForPublishProductUpdates = Convert.ToInt32(ConfigurationManager.AppSettings["IntervalInSecondsForPublishProductUpdates"]) * 1000;
		}
	}
}