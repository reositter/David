using System;
using System.Configuration;
using NUnit.Framework;
using PimIntegration.Tasks;
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
				DbConnectionString = @"Data Source=C:\4 Uppdrag\Arego\dev\PimIntegration\PimIntegration.Tasks\Database\PimIntegrationDb.s3db",
				VismaClientName = ConfigurationManager.AppSettings["VismaClientName"],
				VismaBapiKey = ConfigurationManager.AppSettings["VismaBapiKey"],
				VismaUserName = ConfigurationManager.AppSettings["VismaUserName"],
				VismaPassword = ConfigurationManager.AppSettings["VismaPassword"],
				VismaPostingTemplateNo = Convert.ToInt32(ConfigurationManager.AppSettings["VismaPostingTemplateNo"].Trim()),
				VismaPriceCalcMethodsNo = Convert.ToInt32(ConfigurationManager.AppSettings["VismaPriceCalcMethodsNo"].Trim()),
				VismaStockProfileNo = Convert.ToInt32(ConfigurationManager.AppSettings["VismaStockProfileNo"].Trim())
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
	}
}
