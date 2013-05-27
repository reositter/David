using System;
using Nancy;
using PimIntegration.Tasks;
using PimIntegration.Tasks.Database.Interfaces;
using PimIntegration.Tasks.PimApi.Interfaces;
using PimIntegration.Tasks.VismaGlobal.Interfaces;
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
					ActionUrl = "/trial/task/getnewproductsx",
					Method = "POST"
				};
				return View["partial/Since.cshtml", model];
			};

			Post["/getnewproductsx"] = parameters =>
			{
				var timestamp = Convert.ToDateTime(Request.Form.Timestamp.ToString());

				var task = ObjectFactory.Container.GetInstance<IGetNewProductsTask>();
				var result = task.Execute(timestamp);

				return Response.AsJson(new
				{
					Result = result
				});
			};

			Post["/getnewproducts"] = parameters =>
			{
				var timestamp = Convert.ToDateTime(Request.Form.Timestamp.ToString());

				// Emulate GetNewProductsTask.Execute()
				var pimQueryService = ObjectFactory.Container.GetInstance<IPimQueryService>();
				var newProducts = pimQueryService.GetNewProductsSince(timestamp);

				if (newProducts == null || newProducts.Length == 0)
				{
					return Response.AsJson(new { });
				}

				var articlesForCreate = ObjectFactory.Container.GetInstance<IMapper>().MapPimProductsToVismaArticles(newProducts);
				var createdArticles = ObjectFactory.Container.GetInstance<IArticleManager>().CreateArticles(articlesForCreate);
				var pimCommandService = ObjectFactory.Container.GetInstance<IPimCommandService>();

				foreach (var market in PimIntegrationSettings.AppSettings.Markets)
				{
					pimCommandService.ReportVismaProductNumbers(market.MarketKey, market.VendorId, createdArticles);
				}

				return Response.AsJson(new
				{
					NewProductsFromPim = newProducts,
					ArticlesBeforeCreate = articlesForCreate,
					CreatedArticles = createdArticles
				});
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

				// Emulate PublishStockBalanceUpdatesTask.Execute()
				var articlesForUpdate = ObjectFactory.Container.GetInstance<IStockBalanceQuery>().GetStockBalanceUpdatesSince(timestamp);
				var pimCommandService = ObjectFactory.Container.GetInstance<IPimCommandService>();

				foreach (var market in PimIntegrationSettings.AppSettings.Markets)
				{
					pimCommandService.PublishStockBalanceUpdates(market.MarketKey, articlesForUpdate);
				}

				return Response.AsJson(new
				{
					ArticlesWithStockBalanceUpdates = articlesForUpdate
				});
			};
		}
	}
}