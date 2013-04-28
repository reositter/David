using System;
using System.Collections.Generic;
using PimIntegration.Tasks.Database;
using PimIntegration.Tasks.PimApi;
using PimIntegration.Tasks.VismaGlobal;

namespace PimIntegration.Tasks
{
	public class GetNewProductsTask
	{
		private readonly IPimApiConversationStateRepository _pimApiConversationStateRepository;
		private readonly IPimQueryService _pimQueryService;
		private readonly IPimCommandService _pimCommandService;
		private readonly IArticleManager _articleManager;
		private DateTime _timeOfLastRequest;

		public GetNewProductsTask(
			IPimApiConversationStateRepository pimApiConversationStateRepository, 
			IPimQueryService pimQueryService,
			IPimCommandService pimCommandService,
			IArticleManager articleManager)
		{
			_pimApiConversationStateRepository = pimApiConversationStateRepository;
			_pimQueryService = pimQueryService;
			_pimCommandService = pimCommandService;
			_articleManager = articleManager;
			_timeOfLastRequest = _pimApiConversationStateRepository.GetTimeStampOfLastRequestForNewProducts();
		}

		public void Execute()
		{
			var	timeOfThisRequest = DateTime.Now;
			var newProducts = _pimQueryService.GetNewProductsSince(_timeOfLastRequest);

			if (newProducts == null) return;

			// Create products in Visma Global
			var createdArticles = new List<ArticleForGetNewProductsScenario>();
			foreach (var product in newProducts)
			{
				var articleNo = _articleManager.CreateArticle(product);
				createdArticles.Add(new ArticleForGetNewProductsScenario(product, articleNo));
			}

			// Report local id back to PIM
			_pimCommandService.ReportVismaProductNumbers(createdArticles);

			// Update time stamp for last call
			_pimApiConversationStateRepository.UpdateTimeStampOfLastRequestForNewProducts(timeOfThisRequest);
			_timeOfLastRequest = timeOfThisRequest;
		}
	}
}