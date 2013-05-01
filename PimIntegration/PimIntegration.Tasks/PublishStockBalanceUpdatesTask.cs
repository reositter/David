using System;
using PimIntegration.Tasks.Database.Interfaces;
using PimIntegration.Tasks.PimApi;
using PimIntegration.Tasks.Setup;

namespace PimIntegration.Tasks
{
	public class PublishStockBalanceUpdatesTask : IPublishStockBalanceUpdatesTask
	{
		private readonly ITaskSettings _settings;
		private readonly ILastCallsRepository _lastCallsRepository;
		private readonly IStockBalanceQuery _stockBalanceQuery;
		private readonly IPimCommandService _pimCommandService;
		private DateTime _timeOfLastQueryForStockBalanceUpdates;

		public PublishStockBalanceUpdatesTask(
			ITaskSettings settings,
			ILastCallsRepository lastCallsRepository, 
			IStockBalanceQuery stockBalanceQuery, 
			IPimCommandService pimCommandService)
		{
			_settings = settings;
			_lastCallsRepository = lastCallsRepository;
			_stockBalanceQuery = stockBalanceQuery;
			_pimCommandService = pimCommandService;
			_timeOfLastQueryForStockBalanceUpdates = _lastCallsRepository.GetTimeOfLastQueryForStockBalanceUpdates();
		}

		public void Execute()
		{
			var timeOfThisQuery = DateTime.Now;
			var stockBalanceUpdates = _stockBalanceQuery.GetStockBalanceUpdatesSince(_timeOfLastQueryForStockBalanceUpdates);

			foreach (var market in _settings.Markets)
			{
				_pimCommandService.PublishStockBalanceUpdates(market.MarketKey, stockBalanceUpdates);
			}
			
			_lastCallsRepository.UpdateTimeOfLastQueryForStockBalanceUpdates(timeOfThisQuery);
			_timeOfLastQueryForStockBalanceUpdates = timeOfThisQuery;
		}
	}

	public interface IPublishStockBalanceUpdatesTask
	{
		void Execute();
	}
}
