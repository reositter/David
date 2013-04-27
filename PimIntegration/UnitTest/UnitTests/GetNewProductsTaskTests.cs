using System;
using Moq;
using NUnit.Framework;
using PimIntegration.Tasks;
using PimIntegration.Tasks.Database;
using PimIntegration.Tasks.PIMServiceEndpoint;
using PimIntegration.Tasks.Queries;

namespace PimIntegration.Test.UnitTests
{
	[TestFixture]
    public class GetNewProductsTaskTests : TestBase
    {
		private GetNewProductsTask _task;
		private Mock<IPimApiConversationStateRepository> _stateRepository;
		private Mock<IPimQueryService> _pimQueryService;

		[SetUp]
		public void SetUp()
		{
			_stateRepository = new Mock<IPimApiConversationStateRepository>();
			_pimQueryService = new Mock<IPimQueryService>();

			_task = new GetNewProductsTask(_stateRepository.Object, _pimQueryService.Object);
		}

		[Test]
		public void Should_get_timestamp_for_last_request_when_executing_task()
		{
			// Arrange
			_pimQueryService.Setup(service => service.GetNewProductsSince(It.IsAny<DateTime>())).Returns(new ProductQueryResponseItem[0]);

			// Act
			_task.Execute(); 

			// Assert
			_stateRepository.Verify(repo => repo.GetTimeStampOfLastRequestForNewProducts());
		}

		[Test]
		public void Should_query_for_new_products()
		{
			// Arrange
			var lastRequest = DateTime.Now;

			_stateRepository.Setup(repo => repo.GetTimeStampOfLastRequestForNewProducts()).Returns(lastRequest);
			_pimQueryService.Setup(service => service.GetNewProductsSince(lastRequest)).Returns(new ProductQueryResponseItem[0]);

			// Act
			_task.Execute();

			// Assert
			_pimQueryService.Verify(service => service.GetNewProductsSince(lastRequest));
		}
    }
}
