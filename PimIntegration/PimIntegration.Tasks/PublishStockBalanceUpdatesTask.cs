using System;
using PimIntegration.Tasks.Database;
using PimIntegration.Tasks.PimApi;

namespace PimIntegration.Tasks
{
	public class PublishStockBalanceUpdatesTask : IPublishProductUpdatesTask
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
			var timeOfThisPublishing = DateTime.Now;
			var stockBalanceUpdates = _stockBalanceQuery.GetStockBalanceUpdatesSince(_timeOfLastQueryForStockBalanceUpdates);

			if (_pimCommandService.PublishStockBalanceUpdates(stockBalanceUpdates))
			{
				_lastCallsRepository.UpdateTimeOfLastQueryForStockBalanceUpdates(timeOfThisPublishing);
				_timeOfLastQueryForStockBalanceUpdates = timeOfThisPublishing;
			}
		}
	}

	public interface IPublishProductUpdatesTask
	{
		void Execute();
	}
}
