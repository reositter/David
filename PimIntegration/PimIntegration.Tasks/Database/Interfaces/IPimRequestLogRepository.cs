using System;
using System.Collections.Generic;
using PimIntegration.Tasks.Database.Dto;

namespace PimIntegration.Tasks.Database.Interfaces
{
	public interface IPimRequestLogRepository
	{
		IEnumerable<PimRequestLogItem> GetRecentRequests(int maximumNumberOfItems);
		string GetRequestItemAsJson(int requestLogId);
		string GetResponseItemAsJson(int requestLogId);
		void LogEnqueuedRequest(EnqueuedRequest pimRequestLogItem);
		void UpdateRequestWithResponseData(int messageId, DateTime? dequeuedAt, int numberOfFailedAttemptsToDequeue, object responseItem);
	}
}