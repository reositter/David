using System;

namespace PimIntegration.Tasks.Database
{
	public interface IPimApiConversationStateRepository
	{
		DateTime GetTimeStampOfLastRequestForNewProducts();
		void UpdateTimeStampOfLastRequestForNewProducts(DateTime timeOfRequest);
		void EnsureExistensAndInitializeTable();
	}
}