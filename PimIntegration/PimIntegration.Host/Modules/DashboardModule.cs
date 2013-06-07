using Nancy;
using PimIntegration.Tasks.Database.Interfaces;
using StructureMap;

namespace PimIntegration.Host.Modules
{
	public class DashboardModule : NancyModule
	{
		public DashboardModule()
		{
			Get["/recentrequests/{limit}"] = parameters =>
			{
				var repo = ObjectFactory.Container.GetInstance<IPimRequestLogRepository>();
				var recentRequests = repo.GetRecentRequests(parameters.limit);

				return View["partial/RequestLog.cshtml", recentRequests];
			};

			Get["/requestlog/{id}/requestitem"] = parameters =>
			{
				var repo = ObjectFactory.Container.GetInstance<IPimRequestLogRepository>();
				var item = repo.GetRequestItemAsJson(parameters.id);

				return Response.AsText((string)item).WithContentType("application/json");
			};

			Get["/requestlog/{id}/responseitem"] = parameters =>
			{
				var repo = ObjectFactory.Container.GetInstance<IPimRequestLogRepository>();
				var item = repo.GetResponseItemAsJson(parameters.id);

				return Response.AsText((string)item).WithContentType("application/json");
			};
		}
	}
}