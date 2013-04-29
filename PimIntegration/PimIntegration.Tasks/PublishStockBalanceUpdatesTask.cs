using System;
using PimIntegration.Tasks.Database;
using PimIntegration.Tasks.PimApi;

namespace PimIntegration.Tasks
{
	public class PublishStockBalanceUpdatesTask : IPublishStockBalanceUpdatesTask
	{
		private readonly ILastCallsRepository _lastCallsRepository;
		private readonly IStockBalanceQuery _stockBalanceQuery;
		private readonly IPimCommandService _pimCommandService;
		private DateTime _timeOfLastQueryForStockBalanceUpdates;

		public PublishStockBalanceUpdatesTask(
			ILastCallsRepository lastCallsRepository, 
			IStockBalanceQuery stockBalanceQuery, 
			IPimCommandService pimCommandService)
		{
			_lastCallsRepository = lastCallsRepository;
			_stockBalanceQuery = stockBalanceQuery;
			_pimCommandService = pimCommandService;
			_timeOfLastQueryForStockBalanceUpdates = _lastCallsRepository.GetTimeOfLastQueryForStockBalanceUpdates();
		}

		public void Execute()
		{
			var timeOfThisQuery = DateTime.Now;
			var stockBalanceUpdates = _stockBalanceQuery.GetStockBalanceUpdatesSince(_timeOfLastQueryForStockBalanceUpdates);

			if (_pimCommandService.PublishStockBalanceUpdates(stockBalanceUpdates))
			{
				_lastCallsRepository.UpdateTimeOfLastQueryForStockBalanceUpdates(timeOfThisQuery);
				_timeOfLastQueryForStockBalanceUpdates = timeOfThisQuery;
			}
		}
	}

	public interface IPublishStockBalanceUpdatesTask
	{
		void Execute();
	}
}
