using System;
using System.Configuration;
using NUnit.Framework;
using PimIntegration.Tasks;
using PimIntegration.Tasks.Database;
using PimIntegration.Tasks.Setup;

namespace PimIntegration.Test.IntegrationTests
{
	[TestFixture]
	public class PimIntegrationSetupTests
	{
		private AppSettings _settings;

		[SetUp]
		public void SetUp()
		{
			_settings = new AppSettings
			{
				SqliteConnectionString = @"Data Source=C:\4 Uppdrag\Arego\dev\PimIntegration\PimIntegration.Tasks\Database\PimIntegrationDb.s3db",
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
		}

		[Test]
		public void Should_ensure_that_tables_exists()
		{
			// Arrange

			// Act
			var container = PimIntegrationSetup.BootstrapEverything(_settings);

			// Assert
			container.AssertConfigurationIsValid();
		}

		[Test]
		public void Should_be_able_to_resolve_get_new_products_task()
		{
			// Arrange
			var container = PimIntegrationSetup.BootstrapEverything(_settings);

			// Act
			var task = container.GetInstance<IGetNewProductsTask>();

			// Assert
			Assert.That(task, Is.Not.Null);
		}

		[Test]
		public void Should_be_able_to_connect_to_visma_db()
		{
			// Arrange
			var container = PimIntegrationSetup.BootstrapEverything(_settings);
			var query = container.GetInstance<IStockBalanceQuery>();

			// Act
			query.GetStockBalanceUpdatesSince(DateTime.Now.AddHours(-1));

			// Assert
			Assert.That(query, Is.Not.Null);
		}
	}
}
