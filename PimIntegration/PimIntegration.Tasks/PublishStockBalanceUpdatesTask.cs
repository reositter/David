using System;
using PimIntegration.Tasks.Database;
using PimIntegration.Tasks.VismaGlobal;

namespace PimIntegration.Tasks
{
	public class PublishStockBalanceUpdatesTask : IPublishProductUpdatesTask
	{
		private readonly ILastCallsRepository _lastCallsRepository;
		private DateTime _timeOfLastQueryForStockBalanceUpdates;
		private IStockBalanceQuery _stockBalanceQuery;

		public PublishStockBalanceUpdatesTask(ILastCallsRepository lastCallsRepository, IStockBalanceQuery stockBalanceQuery)
		{
			_lastCallsRepository = lastCallsRepository;
			_stockBalanceQuery = stockBalanceQuery;
			_timeOfLastQueryForStockBalanceUpdates = _lastCallsRepository.GetTimeOfLastQueryForStockBalanceUpdates();
		}

		public void Execute()
		{
			var timeOfThisPublishing = DateTime.Now;
			var stockBalanceUpdates = _stockBalanceQuery.GetStockBalanceUpdatesSince(_timeOfLastQueryForStockBalanceUpdates);

			_lastCallsRepository.UpdateTimeOfLastQueryForStockBalanceUpdates(timeOfThisPublishing);
			_timeOfLastQueryForStockBalanceUpdates = timeOfThisPublishing;
		}
	}

	public interface IPublishProductUpdatesTask
	{
		void Execute();
	}
}
