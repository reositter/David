using System;
using System.Collections.Generic;
using PimIntegration.Tasks.Database.Dto;

namespace PimIntegration.Tasks.Database.Interfaces
{
	public interface IPimRequestLogRepository
	{
		IEnumerable<PimRequestLogItem> GetRecentRequests(int maximumNumberOfItems);
		void LogEnqueuedRequest(EnqueuedRequest pimRequestLogItem);
		void UpdateRequest(int messageId, DateTime? dequeuedAt, int numberOfFailedAttemptsToDequeue);
	}
}