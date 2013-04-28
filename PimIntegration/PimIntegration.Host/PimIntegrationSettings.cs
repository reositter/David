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
		public static readonly AppSettings AppSettings;

		static PimIntegrationSettings()
		{
			Log.ForCurrent.Info("Reading config settings");
			IntervalInSecondsForGetNewProducts = Convert.ToInt32(ConfigurationManager.AppSettings["IntervalInSecondsForGetNewProducts"]) * 1000;
			IntervalInSecondsForPublishProductUpdates = Convert.ToInt32(ConfigurationManager.AppSettings["IntervalInSecondsForPublishProductUpdates"]) * 1000;

			AppSettings = new AppSettings
			{
				MaximumNumberOfRetries = Convert.ToInt32(ConfigurationManager.AppSettings["MaximumNumberOfRetries"]),
				MillisecondsBetweenRetries = Convert.ToInt32(ConfigurationManager.AppSettings["MillisecondsBetweenRetries"]),
				TimeStampFormat = ConfigurationManager.AppSettings["TimeStampFormat"],
				SqliteConnectionString = ConfigurationManager.ConnectionStrings["SQLite"].ConnectionString,
				VismaClientName = ConfigurationManager.AppSettings["VismaClientName"],
				VismaBapiKey = ConfigurationManager.AppSettings["VismaBapiKey"],
				VismaUserName = ConfigurationManager.AppSettings["VismaUserName"],
				VismaPassword = ConfigurationManager.AppSettings["VismaPassword"],
				VismaPostingTemplateNo = Convert.ToInt32(ConfigurationManager.AppSettings["VismaPostingTemplateNo"].Trim()),
				VismaPriceCalcMethodsNo = Convert.ToInt32(ConfigurationManager.AppSettings["VismaPriceCalcMethodsNo"].Trim()),
				VismaStockProfileNo = Convert.ToInt32(ConfigurationManager.AppSettings["VismaStockProfileNo"].Trim()),
				VismaDbSchema = ConfigurationManager.AppSettings["VismaDbSchema"],
				VismaDbConnectionString = ConfigurationManager.ConnectionStrings["VismaDb"].ConnectionString
			};

			Log.ForCurrent.InfoFormat("IntervalInSecondsForGetNewProducts converted to ms: {0}", IntervalInSecondsForGetNewProducts);
			Log.ForCurrent.InfoFormat("IntervalInSecondsForPublishProductUpdates converted to ms: {0}", IntervalInSecondsForPublishProductUpdates);
			Log.ForCurrent.InfoFormat("MaximumNumberOfRetries: {0}", AppSettings.MaximumNumberOfRetries);
			Log.ForCurrent.InfoFormat("MillisecondsBetweenRetries: {0}", AppSettings.MillisecondsBetweenRetries);
			Log.ForCurrent.InfoFormat("TimeStampFormat: {0}", AppSettings.TimeStampFormat);
			Log.ForCurrent.InfoFormat("VismaClientName: {0}", AppSettings.VismaClientName);
		}
	}
}