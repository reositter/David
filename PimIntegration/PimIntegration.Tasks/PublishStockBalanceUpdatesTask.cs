using System;
using PimIntegration.Tasks.Database;

namespace PimIntegration.Tasks
{
	public class PublishStockBalanceUpdatesTask : IPublishProductUpdatesTask
	{
		private readonly ILastCallsRepository _lastCallsRepository;
		private DateTime _timeOfLastPublishedStockBalanceUpdates;

		public PublishStockBalanceUpdatesTask(ILastCallsRepository lastCallsRepository)
		{
			_lastCallsRepository = lastCallsRepository;
			_timeOfLastPublishedStockBalanceUpdates = _lastCallsRepository.GetTimeOfLastPublishedStockBalanceUpdates();
		}

		public void Execute()
		{
			var timeOfThisPublishing = DateTime.Now;
		}
	}

	public interface IPublishProductUpdatesTask
	{
		void Execute();
	}
}
