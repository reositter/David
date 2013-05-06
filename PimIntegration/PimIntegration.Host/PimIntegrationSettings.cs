using System;
using System.Configuration;
using PimIntegration.Tasks;
using PimIntegration.Tasks.Setup;

namespace PimIntegration.Host
{
	internal static class PimIntegrationSettings
	{
		public static readonly int IntervalInSecondsForGetNewProducts;
		public static readonly int IntervalInSecondsForPublishStockBalanceUpdates;
		public static readonly int IntervalInSecondsForPublishPriceUpdates;
		public static readonly AppSettings AppSettings;

		static PimIntegrationSettings()
		{
			Log.ForCurrent.Info("Reading config settings");
			IntervalInSecondsForGetNewProducts = Convert.ToInt32(ConfigurationManager.AppSettings["IntervalInSecondsForGetNewProducts"]) * 1000;
			IntervalInSecondsForPublishStockBalanceUpdates = Convert.ToInt32(ConfigurationManager.AppSettings["IntervalInSecondsForPublishStockBalanceUpdates"]) * 1000;
			IntervalInSecondsForPublishPriceUpdates = Convert.ToInt32(ConfigurationManager.AppSettings["IntervalInSecondsForPublishPriceUpdates"]) * 1000;

			AppSettings = new AppSettings
			{
				TrialMode = Convert.ToBoolean(ConfigurationManager.AppSettings["TrialMode"]),
				NancyUiPort = Convert.ToInt32(ConfigurationManager.AppSettings["NancyUiPort"]),
				MaximumNumberOfRetries = Convert.ToInt32(ConfigurationManager.AppSettings["MaximumNumberOfRetries"]),
				MillisecondsBetweenRetries = Convert.ToInt32(ConfigurationManager.AppSettings["MillisecondsBetweenRetries"]),
				TimeStampFormat = ConfigurationManager.AppSettings["TimeStampFormat"],
				SqliteConnectionString = ConfigurationManager.ConnectionStrings["SQLite"].ConnectionString,
				VismaClientName = ConfigurationManager.AppSettings["VismaClientName"],
				VismaBapiKey = ConfigurationManager.AppSettings["VismaBapiKey"],
				VismaUserName = ConfigurationManager.AppSettings["VismaUserName"],
				VismaPassword = ConfigurationManager.AppSettings["VismaPassword"],
				VismaPostingTemplateNo = Convert.ToInt32(ConfigurationManager.AppSettings["VismaPostingTemplateNo"]),
				VismaPriceCalcMethodsNo = Convert.ToInt32(ConfigurationManager.AppSettings["VismaPriceCalcMethodsNo"]),
				VismaStockProfileNo = Convert.ToInt32(ConfigurationManager.AppSettings["VismaStockProfileNo"]),
				VismaDbSchema = ConfigurationManager.AppSettings["VismaDbSchema"],
				VismaDbConnectionString = ConfigurationManager.ConnectionStrings["VismaDb"].ConnectionString
			};

			var marketSettingsDenmark = new Market(
				ConfigurationManager.AppSettings["MarketKeyDenmark"],
				Convert.ToInt32(ConfigurationManager.AppSettings["VendorIdDenmark"]),
				Convert.ToInt32(ConfigurationManager.AppSettings["CustomerNoDenmark"]));

			var marketSettingsNorway = new Market(
				ConfigurationManager.AppSettings["MarketKeyNorway"],
				Convert.ToInt32(ConfigurationManager.AppSettings["VendorIdNorway"]),
				Convert.ToInt32(ConfigurationManager.AppSettings["CustomerNoNorway"]));

			var marketSettingsSweden = new Market(
				ConfigurationManager.AppSettings["MarketKeySweden"],
				Convert.ToInt32(ConfigurationManager.AppSettings["VendorIdSweden"]),
				Convert.ToInt32(ConfigurationManager.AppSettings["CustomerNoSweden"]));

			AppSettings.Markets.Add(marketSettingsDenmark);
			AppSettings.Markets.Add(marketSettingsNorway);
			AppSettings.Markets.Add(marketSettingsSweden);

			Log.ForCurrent.InfoFormat("IntervalInSecondsForGetNewProducts converted to ms: {0}", IntervalInSecondsForGetNewProducts);
			Log.ForCurrent.InfoFormat("IntervalInSecondsForPublishStockBalanceUpdates converted to ms: {0}", IntervalInSecondsForPublishStockBalanceUpdates);
			Log.ForCurrent.InfoFormat("IntervalInSecondsForPublishPriceUpdates converted to ms: {0}", IntervalInSecondsForPublishPriceUpdates);
			Log.ForCurrent.InfoFormat("MaximumNumberOfRetries: {0}", AppSettings.MaximumNumberOfRetries);
			Log.ForCurrent.InfoFormat("MillisecondsBetweenRetries: {0}", AppSettings.MillisecondsBetweenRetries);
			Log.ForCurrent.InfoFormat("TimeStampFormat: {0}", AppSettings.TimeStampFormat);
			Log.ForCurrent.InfoFormat("VismaClientName: {0}", AppSettings.VismaClientName);
		}
	}
}