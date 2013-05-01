using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using PimIntegration.Tasks;
using PimIntegration.Tasks.Database.Interfaces;
using PimIntegration.Tasks.PIMServiceEndpoint;
using PimIntegration.Tasks.PimApi;
using PimIntegration.Tasks.VismaGlobal;
using PimIntegration.Tasks.VismaGlobal.Dto;
using PimIntegration.Tasks.VismaGlobal.Interfaces;

namespace PimIntegration.Test.UnitTests
{
	[TestFixture]
    public class GetNewProductsTaskTests : TestBase
    {
		private IGetNewProductsTask _task;
		private Mock<ILastCallsRepository> _stateRepository;
		private Mock<IPimQueryService> _pimQueryService;
		private Mock<IPimCommandService> _pimCommandService;
		private Mock<IArticleManager> _articleManager;
		private DateTime _timeOfLastRequest;

		[SetUp]
		public void SetUp()
		{
			_stateRepository = new Mock<ILastCallsRepository>();
			_pimQueryService = new Mock<IPimQueryService>();
			_pimCommandService = new Mock<IPimCommandService>();
			_articleManager = new Mock<IArticleManager>();
			_timeOfLastRequest = DateTime.Now.AddHours(-2);

			_stateRepository.Setup(repo => repo.GetTimeOfLastRequestForNewProducts()).Returns(_timeOfLastRequest);
			_task = new GetNewProductsTask(_stateRepository.Object, _pimQueryService.Object, _pimCommandService.Object, _articleManager.Object);
		}

		[Test]
		public void Should_get_time_of_last_request_from_db_when_ctor_is_called()
		{
			// Arrange
			var repo = new Mock<ILastCallsRepository>();

			// Act
			_task = new GetNewProductsTask(repo.Object, _pimQueryService.Object, _pimCommandService.Object, _articleManager.Object);

			// Assert
			repo.Verify(x => x.GetTimeOfLastRequestForNewProducts());
		}

		[Test]
		public void Should_query_for_new_products()
		{
			// Arrange
			_pimQueryService.Setup(service => service.GetNewProductsSince(_timeOfLastRequest)).Returns(new ProductQueryResponseItem[0]);

			// Act
			_task.Execute();

			// Assert
			_pimQueryService.Verify(service => service.GetNewProductsSince(_timeOfLastRequest));
		}

		[Test]
		public void Should_create_articles_in_visma_when_pim_response_contains_new_products()
		{
			// Arrange
			var newProducts = new[]
			{
				new ProductQueryResponseItem {SKU = "PIM00001"},
				new ProductQueryResponseItem {SKU = "PIM00002"},
				new ProductQueryResponseItem {SKU = "PIM00003"}
			};

			_pimQueryService.Setup(service => service.GetNewProductsSince(_timeOfLastRequest)).Returns(newProducts);

			// Act
			_task.Execute();

			// Assert
			_articleManager.Verify(am => am.CreateArticle(It.IsAny<ProductQueryResponseItem>()), Times.Exactly(newProducts.Length));
		}

		[Test]
		public void Should_not_create_articles_in_visma_when_pim_response_is_empty()
		{
			// Arrange
			_pimQueryService.Setup(service => service.GetNewProductsSince(_timeOfLastRequest)).Returns(new ProductQueryResponseItem[0]);

			// Act
			_task.Execute();

			// Assert
			_articleManager.Verify(am => am.CreateArticle(It.IsAny<ProductQueryResponseItem>()), Times.Never());
		}

		[Test]
		public void Should_report_visma_article_numbers_to_pim_when_articles_are_created()
		{
			// Arrange
			var newProducts = new[]
			{
				new ProductQueryResponseItem {SKU = "PIM00001"},
				new ProductQueryResponseItem {SKU = "PIM00002"},
				new ProductQueryResponseItem {SKU = "PIM00003"}
			};

			_pimQueryService.Setup(service => service.GetNewProductsSince(_timeOfLastRequest)).Returns(newProducts);

			// Act
			_task.Execute();

			// Assert
			_pimCommandService.Verify(service => service.ReportVismaProductNumbers(It.IsAny<List<ArticleForGetNewProductsScenario>>()), Times.Once());
		}

		[Test]
		public void Should_update_time_of_last_request_when_request_is_successful()
		{
			// Arrange
			_pimQueryService.Setup(service => service.GetNewProductsSince(_timeOfLastRequest)).Returns(new ProductQueryResponseItem[0]);

			// Act
			_task.Execute();

			// Assert
			_stateRepository.Verify(repo => repo.UpdateTimeOfLastRequestForNewProducts(It.IsAny<DateTime>()));
		}

		[Test]
		public void Should_not_update_time_of_last_request_when_there_is_no_response()
		{
			// Arrange
			_pimQueryService.Setup(service => service.GetNewProductsSince(_timeOfLastRequest)).Returns((ProductQueryResponseItem[])null);

			// Act
			_task.Execute();

			// Assert
			_stateRepository.Verify(repo => repo.UpdateTimeOfLastRequestForNewProducts(It.IsAny<DateTime>()), Times.Never());
		}
    }
}
