using Moq;
using NUnit.Framework;
using PimIntegration.Tasks.Database.Interfaces;
using PimIntegration.Tasks.PimApi;

namespace PimIntegration.Test.IntegrationTests
{
	[TestFixture]
	public class PimQueryServiceTests : TestBase
	{
		[Test]
		public void Should_get_new_dummy_product()
		{
			// Arrange
			var repo = new Mock<IPimRequestLogRepository>();
			var enqueuer = new ProductQueryEnqueuer(repo.Object);
			var dequeuer = new ProductQueryDequeuer(GetSettingsFromAppConfigForUnitTests(), repo.Object);

			// Act
			var products = new PimQueryService(enqueuer, dequeuer).GetNewProductsSinceDummy();

			// Assert
			Assert.That(products, Is.Not.Null);
		}

		[Test]
		public void Should_get_dummy_product_by_sku()
		{
			// Arrange
			var repo = new Mock<IPimRequestLogRepository>();
			var enqueuer = new ProductQueryEnqueuer(repo.Object);
			var dequeuer = new ProductQueryDequeuer(GetSettingsFromAppConfigForUnitTests(), repo.Object);

			// Act
			var products = new PimQueryService(enqueuer, dequeuer).GetProductBySkuDummy();

			// Assert
			Assert.That(products, Is.Not.Null);
		}
	}
}