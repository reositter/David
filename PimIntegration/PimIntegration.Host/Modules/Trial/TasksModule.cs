using System;
using Nancy;
using PimIntegration.Tasks;
using StructureMap;

namespace PimIntegration.Host.Modules.Trial
{
	public class TasksModule : NancyModule
	{
		public TasksModule() : base("/trial/task")
		{
			Get["/form/getnewproducts"] = o =>
			{
				dynamic model = new
				{
					Title = "GetNewProducts",
					ActionUrl = "/trial/task/getnewproducts",
					Method = "POST"
				};
				return View["partial/Since.cshtml", model];
			};

			Post["/getnewproducts"] = parameters =>
			{
				var timestamp = Convert.ToDateTime(Request.Form.Timestamp.ToString());

				var task = ObjectFactory.Container.GetInstance<IGetNewProductsTask>();
				var result = task.Execute(timestamp);

				return Response.AsJson(new { Result = result });
			};

			Get["/form/publishstockbalanceupdates"] = o =>
			{
				dynamic model = new
				{
					Title = "PublishStockBalanceUpdates",
					ActionUrl = "/trial/task/publishstockbalanceupdates",
					Method = "POST"
				};
				return View["partial/Since.cshtml", model];
			};

			Post["/publishstockbalanceupdates"] = parameters =>
			{
				var timestamp = Convert.ToDateTime(Request.Form.Timestamp.ToString());

				var task = ObjectFactory.Container.GetInstance<IPublishStockBalanceUpdatesTask>();
				var result = task.Execute(timestamp);

				return Response.AsJson(new { Result = result });
			};
		}
	}
}