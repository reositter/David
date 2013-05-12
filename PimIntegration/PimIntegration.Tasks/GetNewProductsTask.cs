using System;
using System.Collections.Generic;
using PimIntegration.Tasks.Database.Interfaces;
using PimIntegration.Tasks.PIMServiceEndpoint;
using PimIntegration.Tasks.PimApi;
using PimIntegration.Tasks.Setup;
using PimIntegration.Tasks.VismaGlobal.Dto;
using PimIntegration.Tasks.VismaGlobal.Interfaces;

namespace PimIntegration.Tasks
{
	public interface IGetNewProductsTask
	{
		void Execute();
	}

	public class GetNewProductsTask : IGetNewProductsTask
	{
		private readonly ITaskSettings _settings;
		private readonly ILastCallsRepository _lastCallsRepository;
		private readonly IPimQueryService _pimQueryService;
		private readonly IPimCommandService _pimCommandService;
		private readonly IArticleManager _articleManager;
		private DateTime _timeOfLastRequest;

		public GetNewProductsTask(
			ITaskSettings settings,
			ILastCallsRepository lastCallsRepository, 
			IPimQueryService pimQueryService,
			IPimCommandService pimCommandService,
			IArticleManager articleManager)
		{
			_settings = settings;
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

			_lastCallsRepository.UpdateTimeOfLastRequestForNewProducts(timeOfThisRequest);
			_timeOfLastRequest = timeOfThisRequest;

			if (newProducts == null || newProducts.Length == 0) return;

			var createdArticles = _articleManager.CreateArticles(MapPimProductToVismaArticle(newProducts));

			foreach (var market in _settings.Markets)
			{
				_pimCommandService.ReportVismaProductNumbers(market.MarketKey, market.VendorId, createdArticles);
			}
		}

		private static IList<ArticleForCreate> MapPimProductToVismaArticle(IEnumerable<ProductQueryResponseItem> pimProducts)
		{
			var list = new List<ArticleForCreate>();

			foreach (var pimProduct in pimProducts)
			{
				var article = new ArticleForCreate
				{
					Name = pimProduct.MasterModel,
					PimSku = pimProduct.SKU
				};

				foreach (var market in pimProduct.Markets)
				{
					switch (market.Market)
					{
						case "4Sound.dk":
							article.ShortDescriptionDen = market.Description;
							break;
						case "4Sound.no":
							article.ShortDescriptionNor = market.Description;
							break;
						case "4Sound.se":
							article.Name = market.DisplayName,
							article.ShortDescriptionSwe = market.Description;
							break;
					}
				}

				list.Add(article);
			}

			return list;
		}
	}
}