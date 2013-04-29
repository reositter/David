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
	public class PublishPriceUpdatesTaskTests
	{
		private IPublishPriceUpdatesTask _task;
		private Mock<ILastCallsRepository> _stateRepository;
		private Mock<IPriceUpdateQuery> _priceUpdateQuery;
		private Mock<IPimCommandService> _pimCommandService;
		private DateTime _timeOfLastQuery;

		[SetUp]
		public void SetUp()
		{
			_stateRepository = new Mock<ILastCallsRepository>();
			_priceUpdateQuery = new Mock<IPriceUpdateQuery>();
			_pimCommandService = new Mock<IPimCommandService>();
			_timeOfLastQuery = DateTime.Now.AddHours(-2);

			_stateRepository.Setup(repo => repo.GetTimeOfLastQueryForPriceUpdates()).Returns(_timeOfLastQuery);
			_task = new PublishPriceUpdatesTask(_stateRepository.Object, _priceUpdateQuery.Object, _pimCommandService.Object);
		}

		[Test]
		public void Should_get_time_of_last_publish_from_db_when_ctor_is_called()
		{
			// Arrange

			// Act
			_task.Execute();

			// Assert
			_stateRepository.Verify(x => x.GetTimeOfLastQueryForPriceUpdates(), Times.Once());
		}

		[Test]
		public void Should_query_for_updated_prices()
		{
			// Arrange

			// Act
			_task.Execute();

			// Assert
			_priceUpdateQuery.Verify(x => x.GetPriceUpdatesSince(_timeOfLastQuery), Times.Once());
		}

		[Test]
		public void Should_publish_price_updates_when_updates_exists()
		{
			// Arrange

			// Act
			_task.Execute();

			// Assert
			_pimCommandService.Verify(x => x.PublishPriceUpdates(It.IsAny<IEnumerable<ArticleForPriceAndStockUpdate>>()), Times.Once());
		}

		[Test]
		public void Should_update_time_of_last_request_when_request_is_successful()
		{
			// Arrange
			_pimCommandService.Setup(service => service.PublishPriceUpdates(It.IsAny<IEnumerable<ArticleForPriceAndStockUpdate>>())).Returns(true);

			// Act
			_task.Execute();

			// Assert
			_stateRepository.Verify(repo => repo.UpdateTimeOfLastQueryForPriceUpdates(It.IsAny<DateTime>()));
		}
	}
}
