using System;
using NUnit.Framework;
using PimIntegration.Tasks.Queries;
using PimIntegration.Tasks.Setup;

namespace UnitTest.IntegrationTests
{
	[TestFixture]
	public class PimQueryServiceTests
	{
		[SetUp]
		public void SetUp()
		{
			var settings = new TaskSettings
			{
				MaximumNumberOfRetries = 5,
				MillisecondsBetweenRetries = 1000
			};
		}

		[Test]
		public void Should_()
		{
			// Arrange

			// Act
			new PimQueryService(createTaskSettings(1000)).GetNewProductsSince(DateTime.Now.AddDays(-1));

			// Assert

		}

		private ITaskSettings createTaskSettings(int millisecondsBetweenRetries, int maximumNumberOfRetries = 5)
		{
			return new TaskSettings
			{
				MaximumNumberOfRetries = maximumNumberOfRetries,
				MillisecondsBetweenRetries = millisecondsBetweenRetries
			};
		}
	}
}