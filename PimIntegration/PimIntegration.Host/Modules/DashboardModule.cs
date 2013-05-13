using PimIntegration.Tasks.Database.Interfaces;
using StructureMap;

namespace PimIntegration.Host.Modules
{
	public class DashboardModule : Nancy.NancyModule
	{
		public DashboardModule()
		{
			Get["/"] = parameters =>
			{
				dynamic model = new
				{
					Title = "Dashboard"
				};
				return View["dashboard.cshtml", model];
			};

			Get["/lostmessages/{limit}"] = parameters =>
			{
				var repo = ObjectFactory.Container.GetInstance<IPimMessageResultRepository>();
				var lostMessages = repo.GetMessagesWithoutResponse(parameters.limit);

				return View["partial/MessageResults.cshtml", lostMessages];
			};
		}
	}
}