using System;
using System.Linq;
using NUnit.Framework;
using PimIntegration.Tasks.Database;
using PimIntegration.Tasks.Database.Dto;
using PimIntegration.Tasks.Database.Interfaces;
using PimIntegration.Tasks.Setup;

namespace PimIntegration.Test.IntegrationTests
{
	[TestFixture]
	public class PimMessageResultRepositoryTests : TestBase
	{
		private IPimMessageResultRepository _repository;
		private AppSettings _settings;

		[SetUp]
		public void SetUp()
		{
			_settings = GetSettingsFromAppConfigForUnitTests();
			_repository = new PimMessageResultRepository(_settings);
		}

		[Test]
		public void Should_save_pim_message_result()
		{
			// Arrange
			var messageResult = new PimMessageResult
			{
				MessageId = 19,
				PrimaryAction = "TestPrimary",
				SecondaryAction = "TestSecondary",
				EnqueuedAt = DateTime.Now,
				DequeuedAt = null,
				NumberOfFailedAttemptsToDequeue = 5,
				Status = 404
			};

			// Act
			_repository.SaveMessageResult(messageResult);

			// Assert
			var lastMessages = _repository.GetMessagesWithoutResponse(2);
			Assert.That(lastMessages.Any(msg => msg.MessageId == 19), Is.True);
		}
	}
}