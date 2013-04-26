using Moq;
using NUnit.Framework;
using PimIntegration.Tasks;
using PimIntegration.Tasks.Setup;

namespace UnitTest
{
	[TestFixture]
    public class GetNewProductsTaskTests
    {
		private GetNewProductsTask _task;
		private Mock<IStateRepository> _stateRepository;

		[SetUp]
		public void SetUp()
		{
			var settings = new TaskSettings
			{
				MaximumNumberOfRetries = 5,
				MillisecondsBetweenRetries = 1000
			};

			_stateRepository = new Mock<IStateRepository>();

			_task = new GetNewProductsTask(settings, _stateRepository.Object);
		}

		[Test]
		public void Should_get_timestamp_for_last_request_when_executing_task()
		{
			// Arrange
			// Act
			_task.Execute(); 

			// Assert
			_stateRepository.Verify(repo => repo.GetTimeStampOfLastRequestForNewProducts());
		}
    }
}
