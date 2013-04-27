using System;
using PimIntegration.Tasks.Database;
using PimIntegration.Tasks.Queries;

namespace PimIntegration.Tasks
{
	public class GetNewProductsTask
	{
		private readonly IPimConversationStateRepository _pimConversationStateRepository;
		private readonly IPimQueryService _pimQueryService;

		public GetNewProductsTask(IPimConversationStateRepository pimConversationStateRepository, IPimQueryService pimQueryService)
		{
			_pimConversationStateRepository = pimConversationStateRepository;
			_pimQueryService = pimQueryService;
		}

		public void Execute()
		{
			var timeOfLastRequest = _pimConversationStateRepository.GetTimeStampOfLastRequestForNewProducts();
			var	timeOfThisRequest = DateTime.Now;

			var newProducts = _pimQueryService.GetNewProductsSince(timeOfLastRequest);

			// Create products in Visma Global

			// Report local id back to PIM

			// Update time stamp for last call to
			_pimConversationStateRepository.UpdateTimeStampOfLastRequestForNewProducts(timeOfThisRequest);
		}
	}
}