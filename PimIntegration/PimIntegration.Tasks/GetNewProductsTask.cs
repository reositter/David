using System;
using PimIntegration.Tasks.Database;
using PimIntegration.Tasks.Queries;

namespace PimIntegration.Tasks
{
	public class GetNewProductsTask
	{
		private readonly IPimQueryService _pimQueryService;
		private readonly IPimApiConversationStateRepository _pimApiConversationStateRepository;
		private DateTime _timeOfLastRequest;

		public GetNewProductsTask(IPimApiConversationStateRepository pimApiConversationStateRepository, IPimQueryService pimQueryService)
		{
			_pimQueryService = pimQueryService;
			_pimApiConversationStateRepository = pimApiConversationStateRepository;
			_timeOfLastRequest = _pimApiConversationStateRepository.GetTimeStampOfLastRequestForNewProducts();
		}

		public void Execute()
		{
			var	timeOfThisRequest = DateTime.Now;
			var newProducts = _pimQueryService.GetNewProductsSince(_timeOfLastRequest);

			if (newProducts == null) return;

			// Create products in Visma Global

			// Report local id back to PIM

			// Update time stamp for last call
			_pimApiConversationStateRepository.UpdateTimeStampOfLastRequestForNewProducts(timeOfThisRequest);
			_timeOfLastRequest = timeOfThisRequest;
		}
	}
}