using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using PimIntegration.Tasks;
using PimIntegration.Tasks.Database.Interfaces;
using PimIntegration.Tasks.PIMServiceEndpoint;
using PimIntegration.Tasks.PimApi.Interfaces;
using PimIntegration.Tasks.Setup;
using PimIntegration.Tasks.VismaGlobal.Dto;
using PimIntegration.Tasks.VismaGlobal.Interfaces;

namespace PimIntegration.Test.UnitTests
{
	[TestFixture]
    public class GetNewProductsTaskTests : TestBase
    {
		private IGetNewProductsTask _task;
		private AppSettings _settings;
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

			_settings = GetSettingsFromAppConfigForUnitTests();

			_stateRepository.Setup(repo => repo.GetTimeOfLastRequestForNewProducts()).Returns(_timeOfLastRequest);
			_task = new GetNewProductsTask(
				_settings, 
				_stateRepository.Object, 
				_pimQueryService.Object, 
				_pimCommandService.Object, 
				_articleManager.Object,
				new Mapper(_settings));
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
				new ProductQueryResponseItem {SKU = "PIM00001", Markets = new ProductQueryResponseMarketItem[0]},
				new ProductQueryResponseItem {SKU = "PIM00002", Markets = new ProductQueryResponseMarketItem[0]},
				new ProductQueryResponseItem {SKU = "PIM00003", Markets = new ProductQueryResponseMarketItem[0]}
			};

			_pimQueryService.Setup(service => service.GetNewProductsSince(_timeOfLastRequest)).Returns(newProducts);
			_articleManager.Setup(x => x.CreateArticles(It.IsAny<IList<ArticleForCreate>>())).Returns(new List<CreatedArticle>());

			// Act
			_task.Execute();

			// Assert
			_articleManager.Verify(am => am.CreateArticles(It.IsAny<IList<ArticleForCreate>>()), Times.Once());
		}

		[Test]
		public void Should_not_create_articles_in_visma_when_pim_response_is_empty()
		{
			// Arrange
			_pimQueryService.Setup(service => service.GetNewProductsSince(_timeOfLastRequest)).Returns(new ProductQueryResponseItem[0]);

			// Act
			_task.Execute();

			// Assert
			_articleManager.Verify(am => am.CreateArticles(It.IsAny<IList<ArticleForCreate>>()), Times.Never());
		}

		[Test]
		public void Should_report_visma_product_numbers_to_each_market_when_articles_are_created()
		{
			// Arrange
			var newProducts = new[]
			{
				new ProductQueryResponseItem { 
					SKU = "PIM00001", 
					Markets = new [] { 
						new ProductQueryResponseMarketItem { Market = "4Sound.dk", Description = "Dejlig" },
						new ProductQueryResponseMarketItem { Market = "4Sound.no", Description = "Den e go" },
						new ProductQueryResponseMarketItem { Market = "4Sound.se", Description = "Bra", DisplayName = "Flying V"}
					}
				},
				new ProductQueryResponseItem {SKU = "PIM00002", Markets = new ProductQueryResponseMarketItem[0]},
				new ProductQueryResponseItem {SKU = "PIM00003", Markets = new ProductQueryResponseMarketItem[0]}
			};
			var createdArticles = new List<CreatedArticle>
			{
				new CreatedArticle("111", "PIM00001"),
				new CreatedArticle("112", "PIM00002"),
				new CreatedArticle("113", "PIM00003")
			};

			_pimQueryService.Setup(service => service.GetNewProductsSince(_timeOfLastRequest)).Returns(newProducts);
			_articleManager.Setup(x => x.CreateArticles(It.IsAny<IList<ArticleForCreate>>())).Returns(createdArticles);

			// Act
			_task.Execute();

			// Assert
			_pimCommandService.Verify(service => service.ReportVismaProductNumbers(_settings.Markets[0].MarketKey, _settings.Markets[0].VendorId, It.IsAny<List<CreatedArticle>>()), Times.Once());
			_pimCommandService.Verify(service => service.ReportVismaProductNumbers(_settings.Markets[1].MarketKey, _settings.Markets[1].VendorId, It.IsAny<List<CreatedArticle>>()), Times.Once());
			_pimCommandService.Verify(service => service.ReportVismaProductNumbers(_settings.Markets[2].MarketKey, _settings.Markets[2].VendorId, It.IsAny<List<CreatedArticle>>()), Times.Once());
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
    }
}
