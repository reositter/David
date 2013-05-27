using System;
using PimIntegration.Tasks.Database.Interfaces;
using PimIntegration.Tasks.PimApi.Interfaces;
using PimIntegration.Tasks.Setup;

namespace PimIntegration.Tasks
{
	public interface IPublishStockBalanceUpdatesTask
	{
		object Execute(DateTime? timeOfLastQueryForStockBalanceUpdatesOverride = null);
	}

	public class PublishStockBalanceUpdatesTask : IPublishStockBalanceUpdatesTask
	{
		private readonly ITaskSettings _settings;
		private readonly ILastCallsRepository _lastCallsRepository;
		private readonly IStockBalanceQuery _stockBalanceQuery;
		private readonly IPimCommandService _pimCommandService;

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
		}

		public object Execute(DateTime? timeOfLastQueryForStockBalanceUpdatesOverride = null)
		{
			var timeOfLastQueryForStockBalanceUpdates = timeOfLastQueryForStockBalanceUpdatesOverride ?? _lastCallsRepository.GetTimeOfLastQueryForStockBalanceUpdates();
			var timeOfThisQuery = DateTime.Now;
			var stockBalanceUpdates = _stockBalanceQuery.GetStockBalanceUpdatesSince(timeOfLastQueryForStockBalanceUpdates);

			foreach (var market in _settings.Markets)
			{
				_pimCommandService.PublishStockBalanceUpdates(market.MarketKey, stockBalanceUpdates);
			}
			
			_lastCallsRepository.UpdateTimeOfLastQueryForStockBalanceUpdates(timeOfThisQuery);

			return new
			{
				TimeOfLastQueryForStockBalanceUpdates = timeOfLastQueryForStockBalanceUpdates,
				StockBalanceUpdates = stockBalanceUpdates
			};
		}
	}
}
