using System;
using System.Configuration;
using NUnit.Framework;
using PimIntegration.Tasks;
using PimIntegration.Tasks.Database.Interfaces;
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
		public void Should_have_valid_ioc_configuration()
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

			// Act
			var query = container.GetInstance<IStockBalanceQuery>();

			// Assert
			Assert.That(query, Is.Not.Null);
		}

		[Test]
		public void Should_be_able_to_query_for_stock_balance_updates()
		{
			// Arrange
			var container = PimIntegrationSetup.BootstrapEverything(_settings);
			var query = container.GetInstance<IStockBalanceQuery>();

			// Act
			var result = query.GetStockBalanceUpdatesSince(DateTime.Now.AddHours(-1));

			// Assert
			Assert.That(result, Is.Not.Null);
		}
	}
}
