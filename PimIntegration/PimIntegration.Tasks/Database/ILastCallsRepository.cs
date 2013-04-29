using System;

namespace PimIntegration.Tasks.Database
{
	public interface ILastCallsRepository
	{
		DateTime GetTimeOfLastRequestForNewProducts();
		void UpdateTimeOfLastRequestForNewProducts(DateTime timeOfRequest);
		DateTime GetTimeOfLastQueryForStockBalanceUpdates();
		void UpdateTimeOfLastQueryForStockBalanceUpdates(DateTime publishedTime);
		DateTime GetTimeOfLastQueryForPriceUpdates();
		void UpdateTimeOfLastQueryForPriceUpdates(DateTime publishedTime);
	}
}