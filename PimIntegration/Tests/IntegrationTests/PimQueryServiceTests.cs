using System;
using Moq;
using NUnit.Framework;
using PimIntegration.Tasks;
using PimIntegration.Tasks.Database.Interfaces;
using PimIntegration.Tasks.PimApi;

namespace PimIntegration.Test.IntegrationTests
{
	[TestFixture]
	public class PimQueryServiceTests : TestBase
	{
		[Test]
		public void Should_be_able_to_make_remote_call_for_new_products()
		{
			// Arrange
			var settings = GetSettingsFromAppConfigForUnitTests();
			var repo = new Mock<IPimMessageResultRepository>();

			// Act
			var task = new PimQueryService(CreateTaskSettings(1000), repo.Object, new Mapper(settings));
			
			var products = task.GetNewProductsSince(DateTime.Now.AddDays(-1));

			// Assert

		}
	}
}