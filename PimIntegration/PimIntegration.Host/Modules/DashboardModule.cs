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

				var response = (Response) item;
				response.ContentType = "application/json";

				return response;
			};

			Get["/requestlog/{id}/responseitem"] = parameters =>
			{
				var repo = ObjectFactory.Container.GetInstance<IPimRequestLogRepository>();
				var item = repo.GetResponseItemAsJson(parameters.id);

				var response = (Response)item;
				response.ContentType = "application/json";

				return response;
			};
		}
	}
}