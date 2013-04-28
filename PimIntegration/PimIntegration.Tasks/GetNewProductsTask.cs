using System;
using System.Collections.Generic;
using PimIntegration.Tasks.Database;
using PimIntegration.Tasks.Queries;
using PimIntegration.Tasks.VismaGlobal;

namespace PimIntegration.Tasks
{
	public class GetNewProductsTask
	{
		private readonly IPimQueryService _pimQueryService;
		private readonly IPimApiConversationStateRepository _pimApiConversationStateRepository;
		private IArticleManager _articleManager;
		private DateTime _timeOfLastRequest;

		public GetNewProductsTask(
			IPimApiConversationStateRepository pimApiConversationStateRepository, 
			IPimQueryService pimQueryService, 
			IArticleManager articleManager)
		{
			_pimQueryService = pimQueryService;
			_articleManager = articleManager;
			_pimApiConversationStateRepository = pimApiConversationStateRepository;
			_timeOfLastRequest = _pimApiConversationStateRepository.GetTimeStampOfLastRequestForNewProducts();
		}

		public void Execute()
		{
			var	timeOfThisRequest = DateTime.Now;
			var newProducts = _pimQueryService.GetNewProductsSince(_timeOfLastRequest);

			if (newProducts == null) return;

			// Create products in Visma Global
			var articleNumberInVisma = new List<string>();
			foreach (var product in newProducts)
			{
				var articleNo = _articleManager.CreateArticle(product);
				articleNumberInVisma.Add(articleNo);
			}

			// Report local id back to PIM

			// Update time stamp for last call
			_pimApiConversationStateRepository.UpdateTimeStampOfLastRequestForNewProducts(timeOfThisRequest);
			_timeOfLastRequest = timeOfThisRequest;
		}
	}
}