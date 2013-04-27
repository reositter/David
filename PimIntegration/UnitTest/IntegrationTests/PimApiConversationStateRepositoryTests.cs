using System;
using NUnit.Framework;
using PimIntegration.Tasks.Database;
using PimIntegration.Tasks.Setup;

namespace PimIntegration.Test.IntegrationTests
{
	[Ignore("Only used during development")]
	[TestFixture]
	public class PimApiConversationStateRepositoryTests : TestBase
	{
		private IPimApiConversationStateRepository _repository;
		private ITaskSettings _settings;

		[SetUp]
		public void SetUp()
		{
			var connectionStringWrapper = new ConnectionStringWrapper("Data Source=Database/PimIntegrationDb.s3db");

			_settings = CreateTaskSettings(10);
			_repository = new PimApiConversationStateRepository(connectionStringWrapper, _settings);

			_repository.EnsureExistensAndInitializeTable();
		}

		[Test]
		public void Should_get_time_of_last_request_for_new_products()
		{
			// Arrange

			// Act
			var lastRequest = _repository.GetTimeStampOfLastRequestForNewProducts();

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
			_repository.UpdateTimeStampOfLastRequestForNewProducts(now);
			var timestamp = _repository.GetTimeStampOfLastRequestForNewProducts();

			// Assert
			Assert.That(timestamp, Is.EqualTo(expected));
		}
	}
}
