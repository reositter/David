using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using PimIntegration.Tasks;
using PimIntegration.Tasks.Database;
using PimIntegration.Tasks.PimApi;
using PimIntegration.Tasks.VismaGlobal.Dto;

namespace PimIntegration.Test.UnitTests
{
	public class PublishProductUpdatesTaskTests
	{
		private IPublishStockBalanceUpdatesTask _task;
		private Mock<ILastCallsRepository> _stateRepository;
		private Mock<IStockBalanceQuery> _stockBalanceQuery;
		private Mock<IPimCommandService> _pimCommandService;
		private DateTime _timeOfLastQuery;

		[SetUp]
		public void SetUp()
		{
			_stateRepository = new Mock<ILastCallsRepository>();
			_stockBalanceQuery = new Mock<IStockBalanceQuery>();
			_pimCommandService = new Mock<IPimCommandService>();
			_timeOfLastQuery = DateTime.Now.AddHours(-2);

			_stateRepository.Setup(repo => repo.GetTimeOfLastQueryForStockBalanceUpdates()).Returns(_timeOfLastQuery);
			_task = new PublishStockBalanceUpdatesTask(_stateRepository.Object, _stockBalanceQuery.Object, _pimCommandService.Object);
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
		public void Should_publish_stock_balance_updates_when_updates_exists()
		{
			// Arrange

			// Act
			_task.Execute();

			// Assert
			_pimCommandService.Verify(x => x.PublishStockBalanceUpdates(It.IsAny<IEnumerable<ArticleForPriceAndStockUpdate>>()), Times.Once());
		}
	}
}