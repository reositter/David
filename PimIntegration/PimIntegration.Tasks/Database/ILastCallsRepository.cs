using System;

namespace PimIntegration.Tasks.Database
{
	public interface ILastCallsRepository
	{
		DateTime GetTimeOfLastRequestForNewProducts();
		void UpdateTimesOfLastRequestForNewProducts(DateTime timeOfRequest);
		DateTime GetTimeOfLastQueryForStockBalanceUpdates();
		void UpdateTimeOfLastQueryForStockBalanceUpdates(DateTime publishedTime);
	}
}