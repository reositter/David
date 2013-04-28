using System;
using Moq;
using NUnit.Framework;
using PimIntegration.Tasks;
using PimIntegration.Tasks.Database;
using PimIntegration.Tasks.PIMServiceEndpoint;
using PimIntegration.Tasks.Queries;
using PimIntegration.Tasks.VismaGlobal;

namespace PimIntegration.Test.UnitTests
{
	[TestFixture]
    public class GetNewProductsTaskTests : TestBase
    {
		private GetNewProductsTask _task;
		private Mock<IPimApiConversationStateRepository> _stateRepository;
		private Mock<IPimQueryService> _pimQueryService;
		private Mock<IArticleManager> _articleManager;
		private DateTime _timeOfLastRequest;

		[SetUp]
		public void SetUp()
		{
			_stateRepository = new Mock<IPimApiConversationStateRepository>();
			_pimQueryService = new Mock<IPimQueryService>();
			_articleManager = new Mock<IArticleManager>();
			_timeOfLastRequest = DateTime.Now.AddHours(-2);

			_stateRepository.Setup(repo => repo.GetTimeStampOfLastRequestForNewProducts()).Returns(_timeOfLastRequest);
			_task = new GetNewProductsTask(_stateRepository.Object, _pimQueryService.Object, _articleManager.Object);
		}

		[Test]
		public void Should_get_time_of_last_request_from_db_when_ctor_is_called()
		{
			// Arrange
			var repo = new Mock<IPimApiConversationStateRepository>();

			// Act
			_task = new GetNewProductsTask(repo.Object, _pimQueryService.Object, _articleManager.Object);

			// Assert
			repo.Verify(x => x.GetTimeStampOfLastRequestForNewProducts());
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
		public void Should_update_time_of_last_request_when_request_is_successful()
		{
			// Arrange
			_pimQueryService.Setup(service => service.GetNewProductsSince(_timeOfLastRequest)).Returns(new ProductQueryResponseItem[0]);

			// Act
			_task.Execute();

			// Assert
			_stateRepository.Verify(repo => repo.UpdateTimeStampOfLastRequestForNewProducts(It.IsAny<DateTime>()));
		}

		[Test]
		public void Should_not_update_time_of_last_request_when_there_is_no_response()
		{
			// Arrange
			ProductQueryResponseItem[] nullResponse = null;
			_pimQueryService.Setup(service => service.GetNewProductsSince(_timeOfLastRequest)).Returns(nullResponse);

			// Act
			_task.Execute();

			// Assert
			_stateRepository.Verify(repo => repo.UpdateTimeStampOfLastRequestForNewProducts(It.IsAny<DateTime>()), Times.Never());
		}
    }
}
