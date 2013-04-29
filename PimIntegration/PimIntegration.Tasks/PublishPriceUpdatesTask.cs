using System;
using PimIntegration.Tasks.Database;
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
			var priceUpdates = _priceUpdateQuery.GetPriceUpdatesSince(_timeOfLastQueryForPriceUpdates);

			// Populate DKK and NOK

			if (_pimCommandService.PublishPriceUpdates(priceUpdates))
			{
				_lastCallsRepository.UpdateTimeOfLastQueryForPriceUpdates(timeOfThisQuery);
				_timeOfLastQueryForPriceUpdates = timeOfThisQuery;
			}
		}
	}

	public interface IPublishPriceUpdatesTask
	{
		void Execute();
	}
}