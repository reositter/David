using System;
using System.Collections.Generic;
using PimIntegration.Tasks.Database.Interfaces;
using PimIntegration.Tasks.PIMServiceEndpoint;
using PimIntegration.Tasks.PimApi;
using PimIntegration.Tasks.VismaGlobal.Dto;
using PimIntegration.Tasks.VismaGlobal.Interfaces;

namespace PimIntegration.Tasks
{
	public class GetNewProductsTask : IGetNewProductsTask
	{
		private readonly ILastCallsRepository _lastCallsRepository;
		private readonly IPimQueryService _pimQueryService;
		private readonly IPimCommandService _pimCommandService;
		private readonly IArticleManager _articleManager;
		private DateTime _timeOfLastRequest;

		public GetNewProductsTask(
			ILastCallsRepository lastCallsRepository, 
			IPimQueryService pimQueryService,
			IPimCommandService pimCommandService,
			IArticleManager articleManager)
		{
			_lastCallsRepository = lastCallsRepository;
			_pimQueryService = pimQueryService;
			_pimCommandService = pimCommandService;
			_articleManager = articleManager;
			_timeOfLastRequest = _lastCallsRepository.GetTimeOfLastRequestForNewProducts();
		}

		public void Execute()
		{
			var	timeOfThisRequest = DateTime.Now;
			var newProducts = _pimQueryService.GetNewProductsSince(_timeOfLastRequest);

			if (newProducts == null) return;

			var createdArticles = new List<ArticleForGetNewProductsScenario>();
			foreach (var product in newProducts)
			{
				var articleNo = _articleManager.CreateArticle(MapPimProductToVismaArticle(product));
				createdArticles.Add(new ArticleForGetNewProductsScenario(product, articleNo));
			}

			if (createdArticles.Count > 0)
				_pimCommandService.ReportVismaProductNumbers(createdArticles);

			_lastCallsRepository.UpdateTimeOfLastRequestForNewProducts(timeOfThisRequest);
			_timeOfLastRequest = timeOfThisRequest;
		}

		private ArticleForCreate MapPimProductToVismaArticle(ProductQueryResponseItem pimProduct)
		{
			return new ArticleForCreate
			{
				Name = pimProduct.Brand,
				PimSku = pimProduct.SKU
			};
		}
	}

	public interface IGetNewProductsTask
	{
		void Execute();
	}
}