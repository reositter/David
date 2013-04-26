using System;
using PimIntegration.Tasks.Database;
using PimIntegration.Tasks.PIMServiceEndpoint;
using PimIntegration.Tasks.Setup;

namespace PimIntegration.Tasks
{
	public class GetNewProductsTask
	{
		private readonly IPimConversationStateRepository _pimConversationStateRepository;

		public GetNewProductsTask(TaskSettings settings, IPimConversationStateRepository pimConversationStateRepository)
		{
			_pimConversationStateRepository = pimConversationStateRepository;
		}

		public void Execute()
		{
			var lastRequest = _pimConversationStateRepository.GetTimeStampOfLastRequestForNewProducts();

			var request = new PIMServiceEndpoint.ProductQueryRequest
			{
				Item = new ProductQueryRequestItem() {CreatedOn = DateTime.Now.AddHours(-7)}
			};
		}
	}
}