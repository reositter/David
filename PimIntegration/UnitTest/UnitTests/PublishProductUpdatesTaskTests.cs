using System;
using Moq;
using NUnit.Framework;
using PimIntegration.Tasks;
using PimIntegration.Tasks.Database;
using PimIntegration.Tasks.VismaGlobal;

namespace PimIntegration.Test.UnitTests
{
	public class PublishProductUpdatesTaskTests
	{
		private  IPublishProductUpdatesTask _task;
		private Mock<ILastCallsRepository> _stateRepository;
		private Mock<IStockBalanceQuery> _stockBalanceQuery;
		private DateTime _timeOfLastQuery;

		[SetUp]
		public void SetUp()
		{
			_stateRepository = new Mock<ILastCallsRepository>();
			_stockBalanceQuery = new Mock<IStockBalanceQuery>();
			_timeOfLastQuery = DateTime.Now.AddHours(-2);

			_stateRepository.Setup(repo => repo.GetTimeOfLastQueryForStockBalanceUpdates()).Returns(_timeOfLastQuery);
			_task = new PublishStockBalanceUpdatesTask(_stateRepository.Object, _stockBalanceQuery.Object);
		}

		[Test]
		public void Should_get_time_of_last_publish_from_db_when_ctor_is_called()
		{
			// Arrange

			// Act
			_task.Execute();

			// Assert
			_stateRepository.Verify(x => x.GetTimeOfLastQueryForStockBalanceUpdates(), Times.Once());
		}

		[Test]
		public void Should_query_for_updated_stock_balances()
		{
			// Arrange

			// Act
			_task.Execute();

			// Assert
			_stockBalanceQuery.Verify(x => x.GetStockBalanceUpdatesSince(_timeOfLastQuery), Times.Once());
		}

		[Test]
		public void Should_()
		{
			// Arrange

			// Act

			// Assert

		}
	}
}