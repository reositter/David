using System;
using NUnit.Framework;
using PimIntegration.Tasks.PimApi;

namespace PimIntegration.Test.IntegrationTests
{
	[TestFixture]
	public class PimQueryServiceTests : TestBase
	{
		[SetUp]
		public void SetUp()
		{

		}

		[Test]
		public void Should_()
		{
			// Arrange

			// Act
			new PimQueryService(CreateTaskSettings(1000)).GetNewProductsSince(DateTime.Now.AddDays(-1));

			// Assert

		}
	}
}