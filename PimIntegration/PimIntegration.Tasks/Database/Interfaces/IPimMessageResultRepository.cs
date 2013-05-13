using System.Collections.Generic;
using PimIntegration.Tasks.Database.Dto;

namespace PimIntegration.Tasks.Database.Interfaces
{
	public interface IPimMessageResultRepository
	{
		IEnumerable<PimMessageResult> GetMessagesWithoutResponse(int maximumNumberOfItems);
		void SaveMessageResult(PimMessageResult pimMessageResult);
	}
}