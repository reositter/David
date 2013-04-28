using System;
using NUnit.Framework;
using PimIntegration.Tasks.Database;
using PimIntegration.Tasks.Setup;

namespace PimIntegration.Test.IntegrationTests
{
	//[Ignore("Only used during development")]
	[TestFixture]
	public class LastCallsRepositoryTests : TestBase
	{
		private ILastCallsRepository _repository;
		private ITaskSettings _settings;

		[SetUp]
		public void SetUp()
		{
			var connectionStringWrapper = new ConnectionStringWrapper("Data Source=Database/PimIntegrationDb.s3db");

			_settings = CreateTaskSettings(10);
			_repository = new LastCallsRepository(connectionStringWrapper, _settings);
		}

		[Test]
		public void Should_get_time_of_last_request_for_new_products()
		{
			// Arrange

			// Act
			var lastRequest = _repository.GetTimeOfLastRequestForNewProducts();

			// Assert
			Assert.That(lastRequest, Is.Not.Null);
		}

		[Test]
		public void Should_update_timestamp_of_last_request_for_new_products()
		{
			// Arrange
			var now = DateTime.Now;
			var expected = now.ToString(_settings.TimeStampFormat);

			// Act
			_repository.UpdateTimesOfLastRequestForNewProducts(now);
			var timestamp = _repository.GetTimeOfLastRequestForNewProducts();

			// Assert
			Assert.That(timestamp, Is.EqualTo(expected));
		}
	}
}
