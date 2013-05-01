using System;
using System.Configuration;
using NUnit.Framework;
using PimIntegration.Tasks.Database;
using PimIntegration.Tasks.Database.Interfaces;
using PimIntegration.Tasks.Setup;

namespace PimIntegration.Test.IntegrationTests
{
	[TestFixture]
	public class LastCallsRepositoryTests : TestBase
	{
		private ILastCallsRepository _repository;
		private ITaskSettings _settings;

		[SetUp]
		public void SetUp()
		{
			_settings = CreateTaskSettings(10);
			_settings.SqliteConnectionString = ConfigurationManager.ConnectionStrings["SQLite"].ConnectionString;
			_repository = new LastCallsRepository(_settings);
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

			// Act
			_repository.UpdateTimeOfLastRequestForNewProducts(now);
			var timestamp = _repository.GetTimeOfLastRequestForNewProducts();

			// Assert
			Assert.That(timestamp.ToString(_settings.TimeStampFormat), Is.EqualTo(now.ToString(_settings.TimeStampFormat)));
		}
	}
}
