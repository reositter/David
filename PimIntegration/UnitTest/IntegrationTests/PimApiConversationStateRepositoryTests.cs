using NUnit.Framework;
using PimIntegration.Tasks.Database;

namespace PimIntegration.Test.IntegrationTests
{
	[TestFixture]
	public class PimApiConversationStateRepositoryTests : TestBase
	{
		private ConnectionStringWrapper _connectionStringWrapper;

		[SetUp]
		public void SetUp()
		{
			_connectionStringWrapper = new ConnectionStringWrapper("Data Source=Database/PimIntegrationDb.s3db");
		}

		[Ignore]
		[Test]
		public void Should_get_timestamp_for_last_request_for_new_products()
		{
			// Arrange
			var repo = new PimApiConversationStateRepository(_connectionStringWrapper, CreateTaskSettings(1000));

			// Act
			repo.EnsureExistensAndInitializeTable();
			var lastRequest = repo.GetTimeStampOfLastRequestForNewProducts();

			// Assert
			Assert.That(lastRequest, Is.Not.Null);
		}
	}
}
