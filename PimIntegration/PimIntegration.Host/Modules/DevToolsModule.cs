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

			Get["/devtools/form/getproductbydate"] = o =>
			{
				dynamic model = new
				{
					ActionUrl = "products/new/getproductbydate",
					Method = "GET"
				};
				return View["partial/Since.cshtml", model];
			};
			Get["/products/new/getproductbydate"] = parameters =>
			{
				Log.ForCurrent.InfoFormat("{0} {1}", Request.Method, Request.Path);

				var timestamp = Convert.ToDateTime(Request.Query.Timestamp.ToString());

				Log.ForCurrent.InfoFormat("Timestamp {0}", timestamp.ToString(PimIntegrationSettings.AppSettings.TimeStampFormat));

				var pimQueryService = ObjectFactory.Container.GetInstance<IPimQueryService>();
				var products = pimQueryService.GetNewProductsSince(timestamp);

				return Response.AsJson(new
				{
					NewProductsFromPim = products
				});
			};

			Get["/devtools/form/getnewproductstask"] = o =>
			{
				dynamic model = new
				{
					ActionUrl = "products/getnewproductstask",
					Method = "POST"
				};
				return View["partial/Since.cshtml", model];
			};
			Post["/products/getnewproductstask"] = parameters =>
			{
				var timestamp = Convert.ToDateTime(Request.Form.Timestamp.ToString());

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

			Get["/devtools/form/forstockbalanceupdate"] = o =>
			{
				dynamic model = new
				{
					ActionUrl = "products/forstockbalanceupdate",
					Method = "GET"
				};
				return View["partial/Since.cshtml", model];
			};
			Get["/products/forstockbalanceupdate"] = o =>
			{
				var timestamp = Convert.ToDateTime(Request.Query.Timestamp.ToString());
				var dbQuery = ObjectFactory.Container.GetInstance<IStockBalanceQuery>();
				var stockBalanceUpdates = dbQuery.GetStockBalanceUpdatesSince(timestamp);

				return Response.AsJson(new
				{
					StockBalanceUpdates = stockBalanceUpdates
				});
			};

			Get["/devtools/form/publishstockbalanceupdatestask"] = o =>
			{
				dynamic model = new
				{
					ActionUrl = "products/publishstockbalanceupdatestask",
					Method = "POST"
				};
				return View["partial/Since.cshtml", model];
			};
			Post["/products/publishstockbalanceupdatestask"] = parameters =>
			{
				var timestamp = Convert.ToDateTime(Request.Query.Timestamp.ToString());

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

			Get["/devtools/form/forpriceupdate"] = o =>
			{
				dynamic model = new
				{
					ActionUrl = "products/forpriceupdate",
					Method = "GET"
				};
				return View["partial/Since.cshtml", model];
			};
			Get["products/forpriceupdate"] = o =>
			{
				var timestamp = Convert.ToDateTime(Request.Query.Timestamp.ToString());

				var dbQuery = ObjectFactory.Container.GetInstance<IPriceUpdateQuery>();
				var customerAgreementQuery = ObjectFactory.Container.GetInstance<ICustomerAgreementQuery>();
				var priceUpdates = dbQuery.GetArticlesForPriceUpdate(timestamp);
				var settings = PimIntegrationSettings.AppSettings;

				customerAgreementQuery.PopulateNewPrice(settings.Markets[0].VismaCustomerNoForPriceCalculation, priceUpdates);

				return Response.AsJson(new
				{
					PriceUpdates = priceUpdates
				});
			};

			Get["/devtools/form/getproductbysku"] = _ => View["partial/GetProductBySku.cshtml", _];
			Get["/product/{sku}"] = parameters =>
			{
				var pimQueryService = ObjectFactory.Container.GetInstance<IPimQueryService>();
				var product = pimQueryService.GetProductBySku(parameters.sku);

				return Response.AsJson(new
				{
					Product = product
				});
			};
		}
	}
}