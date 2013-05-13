using System;
using System.Configuration;
using PimIntegration.Tasks.Setup;

namespace PimIntegration.Test
{
	public class TestBase
	{
		protected static ITaskSettings CreateTaskSettings(int millisecondsBetweenRetries, int maximumNumberOfRetries = 5)
		{
			return new AppSettings
			{
				MaximumNumberOfRetries = maximumNumberOfRetries,
				MillisecondsBetweenRetries = millisecondsBetweenRetries,
				TimeStampFormat = "yyyy-MM-dd HH:mm:ss.fff"
			};
		}

		protected static AppSettings GetSettingsFromAppConfigForUnitTests()
		{
			var settings = new AppSettings
			{
				SqliteConnectionString = @"Data Source=C:\4 Uppdrag\Arego\dev\PimIntegration\PimIntegration.Tasks\Database\PimIntegrationDb.s3db",
				MaximumNumberOfRetries = Convert.ToInt32(ConfigurationManager.AppSettings["MaximumNumberOfRetries"]),
				MillisecondsBetweenRetries = Convert.ToInt32(ConfigurationManager.AppSettings["MillisecondsBetweenRetries"]),
				TimeStampFormat = ConfigurationManager.AppSettings["TimeStampFormat"],
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

			settings.Markets.Add(marketSettingsDenmark);
			settings.Markets.Add(marketSettingsNorway);
			settings.Markets.Add(marketSettingsSweden);

			return settings;
		}
	}
}