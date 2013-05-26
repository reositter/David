using PimIntegration.Tasks.Database.Interfaces;
using StructureMap;

namespace PimIntegration.Host.Modules
{
	public class DashboardModule : Nancy.NancyModule
	{
		public DashboardModule()
		{
			Get["/recentrequests/{limit}"] = parameters =>
			{
				var repo = ObjectFactory.Container.GetInstance<IPimRequestLogRepository>();
				var lostMessages = repo.GetRecentRequests(parameters.limit);

				return View["partial/RequestLog.cshtml", lostMessages];
			};
		}
	}
}