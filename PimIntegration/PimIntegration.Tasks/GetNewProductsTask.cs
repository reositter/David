using System;
using PimIntegration.Tasks.PIMServiceEndpoint;
using PimIntegration.Tasks.Setup;

namespace PimIntegration.Tasks
{
	public class GetNewProductsTask
	{
		private readonly IStateRepository _stateRepository;

		public GetNewProductsTask(TaskSettings settings, IStateRepository stateRepository)
		{
			_stateRepository = stateRepository;
		}

		public void Execute()
		{
			var lastRequest = _stateRepository.GetTimeStampOfLastRequestForNewProducts();

			var request = new PIMServiceEndpoint.ProductQueryRequest
			{
				Item = new ProductQueryRequestItem() {CreatedOn = DateTime.Now.AddHours(-7)}
			};
		}
	}

	public interface IStateRepository
	{
		DateTime GetTimeStampOfLastRequestForNewProducts();
	}
}
