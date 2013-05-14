using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using PimIntegration.Tasks;
using PimIntegration.Tasks.Database.Dto;
using PimIntegration.Tasks.Database.Interfaces;
using PimIntegration.Tasks.PimApi.Interfaces;
using PimIntegration.Tasks.Setup;

namespace PimIntegration.Test.UnitTests
{
	public class PublishStockBalanceUpdatesTaskTests : TestBase
	{
		private IPublishStockBalanceUpdatesTask _task;
		private ITaskSettings _settings;
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

			_settings = GetSettingsFromAppConfigForUnitTests();

			_stateRepository.Setup(repo => repo.GetTimeOfLastQueryForStockBalanceUpdates()).Returns(_timeOfLastQuery);
			_task = new PublishStockBalanceUpdatesTask(_settings, _stateRepository.Object, _stockBalanceQuery.Object, _pimCommandService.Object);
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
		public void Should_publish_stock_balance_updates_for_each_market_when_updates_exists()
		{
			// Arrange

			// Act
			_task.Execute();

			// Assert
			_pimCommandService.Verify(x => x.PublishStockBalanceUpdates(_settings.Markets[0].MarketKey, It.IsAny<IEnumerable<ArticleForStockBalanceUpdate>>()), Times.Once());
			_pimCommandService.Verify(x => x.PublishStockBalanceUpdates(_settings.Markets[1].MarketKey, It.IsAny<IEnumerable<ArticleForStockBalanceUpdate>>()), Times.Once());
			_pimCommandService.Verify(x => x.PublishStockBalanceUpdates(_settings.Markets[2].MarketKey, It.IsAny<IEnumerable<ArticleForStockBalanceUpdate>>()), Times.Once());
		}

		[Test]
		public void Should_update_time_of_last_request_when_executing_task()
		{
			// Arrange
			// Act
			_task.Execute();

			// Assert
			_stateRepository.Verify(repo => repo.UpdateTimeOfLastQueryForStockBalanceUpdates(It.IsAny<DateTime>()));
		}
	}
}