using System;
using System.Collections.Generic;
using System.Linq;
using PimIntegration.Tasks.Database.Dto;
using PimIntegration.Tasks.Database.Interfaces;
using PimIntegration.Tasks.PimApi;
using PimIntegration.Tasks.PimApi.Interfaces;
using PimIntegration.Tasks.Setup;
using PimIntegration.Tasks.VismaGlobal.Dto;
using PimIntegration.Tasks.VismaGlobal.Interfaces;

namespace PimIntegration.Tasks
{
	public interface IPublishPriceUpdatesTask
	{
		void Execute();
	}

	public class PublishPriceUpdatesTask : IPublishPriceUpdatesTask
	{
		private readonly ITaskSettings _settings;
		private readonly ILastCallsRepository _lastCallsRepository;
		private readonly IPriceUpdateQuery _priceUpdateQuery;
		private readonly ICustomerAgreementQuery _customerAgreementQuery;
		private readonly IPimCommandService _pimCommandService;
		private DateTime _timeOfLastQueryForPriceUpdates;

		public PublishPriceUpdatesTask(
			ITaskSettings settings,
			ILastCallsRepository lastCallsRepository, 
			IPriceUpdateQuery priceUpdateQuery,
			ICustomerAgreementQuery customerAgreementQuery,
			IPimCommandService pimCommandService)
		{
			_settings = settings;
			_lastCallsRepository = lastCallsRepository;
			_priceUpdateQuery = priceUpdateQuery;
			_pimCommandService = pimCommandService;
			_customerAgreementQuery = customerAgreementQuery;

			_timeOfLastQueryForPriceUpdates = lastCallsRepository.GetTimeOfLastQueryForPriceUpdates();
		}

		public void Execute()
		{
			var timeOfThisQuery = DateTime.Now;
			var articlesForPriceUpdate = _priceUpdateQuery.GetArticlesForPriceUpdate(_timeOfLastQueryForPriceUpdates);

			foreach (var market in _settings.Markets)
			{
				_customerAgreementQuery.PopulateNewPrice(market.VismaCustomerNoForPriceCalculation, articlesForPriceUpdate);
				var articlesWithPriceUpdates = MapToArticleForPriceAndStockUpdate(articlesForPriceUpdate);
				_pimCommandService.PublishPriceUpdates(market.MarketKey, articlesWithPriceUpdates);
			}

			_lastCallsRepository.UpdateTimeOfLastQueryForPriceUpdates(timeOfThisQuery);
			_timeOfLastQueryForPriceUpdates = timeOfThisQuery;			
		}

		private static IEnumerable<ArticleForPriceAndStockUpdate> MapToArticleForPriceAndStockUpdate(IEnumerable<ArticleForPriceUpdate> source)
		{
			return from s in source
					select new ArticleForPriceAndStockUpdate
					{
						ArticleNo = s.ArticleNo,
						PimSku = s.PimSku,
						Price = s.NewPrice
					};
		}
	}
}