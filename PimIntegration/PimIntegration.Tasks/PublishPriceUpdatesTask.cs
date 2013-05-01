using System;
using PimIntegration.Tasks.Database;
using PimIntegration.Tasks.Database.Interfaces;
using PimIntegration.Tasks.PimApi;
using PimIntegration.Tasks.VismaGlobal;

namespace PimIntegration.Tasks
{
	public class PublishPriceUpdatesTask : IPublishPriceUpdatesTask
	{
		private readonly ILastCallsRepository _lastCallsRepository;
		private readonly IPriceUpdateQuery _priceUpdateQuery;
		private readonly ICustomerAgreementQuery _customerAgreementQuery;
		private readonly IPimCommandService _pimCommandService;
		private DateTime _timeOfLastQueryForPriceUpdates;

		public PublishPriceUpdatesTask(
			ILastCallsRepository lastCallsRepository, 
			IPriceUpdateQuery priceUpdateQuery,
			ICustomerAgreementQuery customerAgreementQuery,
			IPimCommandService pimCommandService)
		{
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

			// Get price for each article and market
			foreach (var article in articlesForPriceUpdate)
			{
				_customerAgreementQuery.GetPrice(1000, article.ArticleNo);
			}

			// Update each market

			//if (_pimCommandService.PublishPriceUpdates(articlesForPriceUpdate))
			//{
			//	_lastCallsRepository.UpdateTimeOfLastQueryForPriceUpdates(timeOfThisQuery);
			//	_timeOfLastQueryForPriceUpdates = timeOfThisQuery;
			//}
		}
	}

	public interface IPublishPriceUpdatesTask
	{
		void Execute();
	}
}