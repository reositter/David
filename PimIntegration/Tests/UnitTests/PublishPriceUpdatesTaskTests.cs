using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using PimIntegration.Tasks;
using PimIntegration.Tasks.Database;
using PimIntegration.Tasks.Database.Dto;
using PimIntegration.Tasks.Database.Interfaces;
using PimIntegration.Tasks.PimApi;
using PimIntegration.Tasks.VismaGlobal;
using PimIntegration.Tasks.VismaGlobal.Dto;

namespace PimIntegration.Test.UnitTests
{
	public class PublishPriceUpdatesTaskTests
	{
		private IPublishPriceUpdatesTask _task;
		private Mock<ILastCallsRepository> _stateRepository;
		private Mock<IPriceUpdateQuery> _priceUpdateQuery;
		private Mock<ICustomerAgreementQuery> _customerAgreementQuery;
		private Mock<IPimCommandService> _pimCommandService;
		private DateTime _timeOfLastQuery;

		[SetUp]
		public void SetUp()
		{
			_stateRepository = new Mock<ILastCallsRepository>();
			_priceUpdateQuery = new Mock<IPriceUpdateQuery>();
			_customerAgreementQuery = new Mock<ICustomerAgreementQuery>();
			_pimCommandService = new Mock<IPimCommandService>();
			_timeOfLastQuery = DateTime.Now.AddHours(-2);

			_stateRepository.Setup(repo => repo.GetTimeOfLastQueryForPriceUpdates()).Returns(_timeOfLastQuery);
			_task = new PublishPriceUpdatesTask(_stateRepository.Object, _priceUpdateQuery.Object, _customerAgreementQuery.Object, _pimCommandService.Object);
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
		public void Should_get_articles_for_price_update()
		{
			// Arrange

			// Act
			_task.Execute();

			// Assert
			_priceUpdateQuery.Verify(x => x.GetArticlesForPriceUpdate(_timeOfLastQuery), Times.Once());
		}

		[Test]
		public void Should_get_current_price_each_article()
		{
			// Arrange
			var article1 = new ArticleForPriceUpdate("181", string.Empty);
			var article2 = new ArticleForPriceUpdate("100", string.Empty);
			var articlesForPriceUpdates = new List<ArticleForPriceUpdate>
			{
				article1,
				article2
			};
			_priceUpdateQuery.Setup(x => x.GetArticlesForPriceUpdate(_timeOfLastQuery)).Returns(articlesForPriceUpdates);

			// Act
			_task.Execute();

			// Assert
			_customerAgreementQuery.Verify(x => x.GetPrice(1000, article1.ArticleNo), Times.Once());
			_customerAgreementQuery.Verify(x => x.GetPrice(1000, article2.ArticleNo), Times.Once());
		}

		[Ignore("WIP")]
		[Test]
		public void Should_publish_price_updates_when_updates_exists()
		{
			// Arrange

			// Act
			_task.Execute();

			// Assert
			_pimCommandService.Verify(x => x.PublishPriceUpdates(It.IsAny<IEnumerable<ArticleForPriceAndStockUpdate>>()), Times.Once());
		}

		[Ignore("WIP")]
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
