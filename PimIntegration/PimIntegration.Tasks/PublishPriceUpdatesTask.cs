using System;
using PimIntegration.Tasks.Database;
using PimIntegration.Tasks.Database.Interfaces;
using PimIntegration.Tasks.PimApi;

namespace PimIntegration.Tasks
{
	public class PublishPriceUpdatesTask : IPublishPriceUpdatesTask
	{
		private readonly ILastCallsRepository _lastCallsRepository;
		private readonly IPriceUpdateQuery _priceUpdateQuery;
		private readonly IPimCommandService _pimCommandService;
		private DateTime _timeOfLastQueryForPriceUpdates;

		public PublishPriceUpdatesTask(
			ILastCallsRepository lastCallsRepository, 
			IPriceUpdateQuery priceUpdateQuery, 
			IPimCommandService pimCommandService)
		{
			_lastCallsRepository = lastCallsRepository;
			_priceUpdateQuery = priceUpdateQuery;
			_pimCommandService = pimCommandService;

			_timeOfLastQueryForPriceUpdates = lastCallsRepository.GetTimeOfLastQueryForPriceUpdates();
		}

		public void Execute()
		{
			var timeOfThisQuery = DateTime.Now;
			var articlesForPriceUpdate = _priceUpdateQuery.GetArticlesForPriceUpdate(_timeOfLastQueryForPriceUpdates);

			// Get prices for each market

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