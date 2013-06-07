using System;
using System.Linq;
using NUnit.Framework;
using PimIntegration.Tasks.Database;
using PimIntegration.Tasks.Database.Dto;
using PimIntegration.Tasks.Database.Interfaces;
using PimIntegration.Tasks.PIMServiceEndpoint;
using PimIntegration.Tasks.Setup;

namespace PimIntegration.Test.IntegrationTests
{
	[TestFixture]
	public class PimRequestLogRepositoryTests : TestBase
	{
		private IPimRequestLogRepository _repository;
		private AppSettings _settings;
		private int _enqueuedRequestMessageId;

		[SetUp]
		public void SetUp()
		{
			_settings = GetSettingsFromAppConfigForUnitTests();
			_repository = new PimRequestLogRepository(_settings);
			_enqueuedRequestMessageId = 19;
		}

		[Test]
		public void Should_log_enqueued_pim_request()
		{
			// Arrange
			var messageResult = new EnqueuedRequest
			{
				MessageId = _enqueuedRequestMessageId,
				PrimaryAction = "TestPrimary",
				SecondaryAction = "TestSecondary",
				RequestItem = new ProductQueryRequestItem{ CreatedOn = DateTime.Now },
				EnqueuedAt = DateTime.Now
			};

			// Act
			_repository.LogEnqueuedRequest(messageResult);

			// Assert
			var lastMessages = _repository.GetRecentRequests(2);
			Assert.That(lastMessages.Any(msg => msg.MessageId == _enqueuedRequestMessageId), Is.True);
		}

		[Test]
		public void Should_update_request()
		{
			// Arrange

			// Act
			_repository.UpdateRequestWithResponseData(_enqueuedRequestMessageId, DateTime.Now, 3, null);

			// Assert
			var lastMessages = _repository.GetRecentRequests(2);
			Assert.That(lastMessages.Any(msg => msg.MessageId == _enqueuedRequestMessageId && msg.NumberOfFailedAttemptsToDequeue == 3), Is.True);
		}

		[Test]
		public void Should_retrieve_response_item()
		{
			// Arrange

			// Act
			var responseItemAsJson = _repository.GetResponseItemAsJson(1);

			// Assert
			Assert.That(responseItemAsJson, Is.Not.Null);
		}
	}
}