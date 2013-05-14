using System;
using Nancy;
using PimIntegration.Tasks;
using PimIntegration.Tasks.Database.Interfaces;
using PimIntegration.Tasks.PimApi.Interfaces;
using PimIntegration.Tasks.VismaGlobal.Interfaces;
using StructureMap;

namespace PimIntegration.Host.Modules
{
	public class DevToolsModule : NancyModule
	{
		public DevToolsModule()
		{
			Get["/devtools"] = o =>
			{
				o.Title = "Dev Tools";
				return View["devtools.cshtml", o];
			};

			Get["/devtools/form/getproductbydatedummy"] = o => View["partial/GetProductByDateDummy.cshtml", o];
			Get["/products/new/getproductbydatedummy"] = parameters =>
			{
				var pimQueryService = ObjectFactory.Container.GetInstance<IPimQueryService>();
				var products = pimQueryService.GetNewProductsSinceDummy();

				return Response.AsJson(products);
			};

			Get["/devtools/form/getproductbydate"] = o => View["partial/GetProductByDate.cshtml", o];
			Get["/products/new/getproductbydate"] = parameters =>
			{
				Log.ForCurrent.InfoFormat("{0} {1}", Request.Method, Request.Path);

				var now = DateTime.Now;
				var timestamp = new DateTime(now.Year, now.Month, now.Day, Request.Query.Hour, Request.Query.Minute, Request.Query.Second);

				Log.ForCurrent.InfoFormat("Timestamp {0}", timestamp.ToString(PimIntegrationSettings.AppSettings.TimeStampFormat));

				var pimQueryService = ObjectFactory.Container.GetInstance<IPimQueryService>();
				var products = pimQueryService.GetNewProductsSince(timestamp);

				return Response.AsJson(products);
			};

			Get["/devtools/form/getnewproductstask"] = o => View["partial/GetNewProductsTask.cshtml", o];
			Post["/products/getnewproductstask"] = parameters =>
			{
				var now = DateTime.Now;
				var timestamp = new DateTime(now.Year, now.Month, now.Day, Request.Form.Hour, Request.Form.Minute, Request.Form.Second);

				// Emulate GetNewProductsTask.Execute()
				var pimQueryService = ObjectFactory.Container.GetInstance<IPimQueryService>();
				var newProducts = pimQueryService.GetNewProductsSince(timestamp);

				if (newProducts == null || newProducts.Length == 0)
				{
					return Response.AsJson(new {});
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

			Get["/devtools/form/forstockbalanceupdate"] = o => View["partial/GetArticlesForStockBalanceUpdate.cshtml", o];
			Get["/products/forstockbalanceupdate"] = o =>
			{
				var now = DateTime.Now;
				var timestamp = new DateTime(now.Year, now.Month, now.Day, Request.Query.Hour, Request.Query.Minute, Request.Query.Second);
				var dbQuery = ObjectFactory.Container.GetInstance<IStockBalanceQuery>();
				var result = dbQuery.GetStockBalanceUpdatesSince(timestamp);

				return Response.AsJson(result);
			};

			Get["/devtools/form/publishstockbalanceupdatestask"] = o => View["partial/PublishStockBalanceUpdatesTask.cshtml", o];
			Post["/products/publishstockbalanceupdatestask"] = parameters =>
			{
				var now = DateTime.Now;
				var timestamp = new DateTime(now.Year, now.Month, now.Day, Request.Form.Hour, Request.Form.Minute, Request.Form.Second);

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

			Get["/devtools/form/forpriceupdate"] = o => View["partial/GetArticlesForPriceUpdate.cshtml", o];
			Get["products/forpriceupdate"] = o =>
			{
				var dbQuery = ObjectFactory.Container.GetInstance<IPriceUpdateQuery>();
				var customerAgreementQuery = ObjectFactory.Container.GetInstance<ICustomerAgreementQuery>();
				var articlesForPriceUpdate = dbQuery.GetArticlesForPriceUpdate(DateTime.Now.AddHours(-4));
				var settings = PimIntegrationSettings.AppSettings;

				customerAgreementQuery.PopulateNewPrice(settings.Markets[0].VismaCustomerNoForPriceCalculation, articlesForPriceUpdate);

				return Response.AsJson(articlesForPriceUpdate);
			};
		}
	}
}