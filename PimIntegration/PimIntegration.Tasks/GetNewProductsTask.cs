using System;
using PimIntegration.Tasks.Database.Interfaces;
using PimIntegration.Tasks.PimApi.Interfaces;
using PimIntegration.Tasks.Setup;
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
		private readonly IMapper _mapper;
		private DateTime _timeOfLastRequest;

		public GetNewProductsTask(
			ITaskSettings settings,
			ILastCallsRepository lastCallsRepository, 
			IPimQueryService pimQueryService,
			IPimCommandService pimCommandService,
			IArticleManager articleManager, 
			IMapper mapper)
		{
			_settings = settings;
			_lastCallsRepository = lastCallsRepository;
			_pimQueryService = pimQueryService;
			_pimCommandService = pimCommandService;
			_articleManager = articleManager;
			_mapper = mapper;
			_timeOfLastRequest = _lastCallsRepository.GetTimeOfLastRequestForNewProducts();
		}

		public void Execute()
		{
			var	timeOfThisRequest = DateTime.Now;
			var newProducts = _pimQueryService.GetNewProductsSince(_timeOfLastRequest);

			_lastCallsRepository.UpdateTimeOfLastRequestForNewProducts(timeOfThisRequest);
			_timeOfLastRequest = timeOfThisRequest;

			if (newProducts == null || newProducts.Length == 0) return;

			var createdArticles = _articleManager.CreateArticles(_mapper.MapPimProductsToVismaArticles(newProducts));

			if (createdArticles.Count == 0) return;

			foreach (var market in _settings.Markets)
			{
				_pimCommandService.ReportVismaProductNumbers(market.MarketKey, market.VendorId, createdArticles);
			}
		}
	}
}