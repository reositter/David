using Moq;
using NUnit.Framework;
using PimIntegration.Tasks.Database.Interfaces;
using PimIntegration.Tasks.PimApi;

namespace PimIntegration.Test.IntegrationTests
{
	[TestFixture]
	public class PimProductQueryRequestEnqueuerTests : TestBase
	{
		[Test]
		public void Should_be_able_to_make_remote_call()
		{
			// Arrange
			var settings = GetSettingsFromAppConfigForUnitTests();
			var repo = new Mock<IPimRequestLogRepository>();

			// Act
			var enqueuer = new ProductQueryEnqueuer(repo.Object);
			
			var products = enqueuer.EnqueueProductQueryRequest(PrimaryAction.GetProductByDateDummy, SecondaryAction.None, null);

			// Assert

		}
	}
}