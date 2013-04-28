using Moq;
using NUnit.Framework;
using PimIntegration.Tasks;
using PimIntegration.Tasks.Database;

namespace PimIntegration.Test.UnitTests
{
	public class PublishProductUpdatesTaskTests
	{
		private  IPublishProductUpdatesTask _task;
		private Mock<ILastCallsRepository> _stateRepository;

		[SetUp]
		public void SetUp()
		{
			_stateRepository = new Mock<ILastCallsRepository>();

			_task = new PublishStockBalanceUpdatesTask(_stateRepository.Object);
		}

		[Test]
		public void Should_get_time_of_last_publish_from_db_when_ctor_is_called()
		{
			// Arrange

			// Act
			_task.Execute();

			// Assert
			_stateRepository.Verify(x => x.GetTimeOfLastPublishedStockBalanceUpdates(), Times.Once());
		}
	}
}