using System;
using NUnit.Framework;
using PimIntegration.Tasks.Database;
using PimIntegration.Tasks.Setup;

namespace PimIntegration.Test.IntegrationTests
{
	[TestFixture]
	public class DbQueriesTests : TestBase
	{
		private AppSettings _settings;

		[SetUp]
		public void SetUp()
		{
			_settings = GetSettingsFromAppConfigForUnitTests();
		}

		[Test]
		public void Should_get_articles_for_price_update()
		{
			// Arrange
			var dbQuery = new PriceUpdateQuery(_settings);

			// Act
			var result = dbQuery.GetArticlesForPriceUpdate(DateTime.Now.AddHours(-2));

			// Assert
			Assert.That(result, Is.Not.Null);
		}
	}
}