using System;
using Nancy;
using PimIntegration.Tasks;
using PimIntegration.Tasks.Database.Interfaces;
using PimIntegration.Tasks.PimApi;
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
			Get["/devtools/form/getproductbydate"] = o => View["partial/GetProductByDate.cshtml", o];
			Get["/devtools/form/getnewproductstask"] = o => View["partial/GetNewProductsTask.cshtml", o];

			Get["/products/new/getproductbydatedummy"] = parameters =>
			{
				var pimQueryService = ObjectFactory.Container.GetInstance<IPimQueryService>();
				var products = pimQueryService.GetNewProductsSinceDummy();

				return Response.AsJson(products);
			};

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

			Post["/products/getnewproductstask"] = parameters =>
			{
				Log.ForCurrent.InfoFormat("{0} {1}", Request.Method, Request.Path);

				var now = DateTime.Now;
				var timestamp = new DateTime(now.Year, now.Month, now.Day, Request.Query.Hour, Request.Query.Minute, Request.Query.Second);

				Log.ForCurrent.InfoFormat("Timestamp {0}", timestamp.ToString(PimIntegrationSettings.AppSettings.TimeStampFormat));

				var pimQueryService = ObjectFactory.Container.GetInstance<IPimQueryService>();
				var products = pimQueryService.GetNewProductsSince(timestamp);

				return Response.AsJson(products);
			};

			Get["/products/forpriceupdate"] = o =>
			{
				var dbQuery = ObjectFactory.Container.GetInstance<IPriceUpdateQuery>();
				var result = dbQuery.GetArticlesForPriceUpdate(DateTime.Now.AddHours(-4));

				return Response.AsJson(result);
			};

			Get["products/forpriceupdatewithagreedprice"] = o =>
			{
				var dbQuery = ObjectFactory.Container.GetInstance<IPriceUpdateQuery>();
				var customerAgreementQuery = ObjectFactory.Container.GetInstance<ICustomerAgreementQuery>();
				var articlesForPriceUpdate = dbQuery.GetArticlesForPriceUpdate(DateTime.Now.AddHours(-4));
				var settings = PimIntegrationSettings.AppSettings;

				customerAgreementQuery.PopulateNewPrice(settings.Markets[0].VismaCustomerNoForPriceCalculation, articlesForPriceUpdate);

				return Response.AsJson(articlesForPriceUpdate);
			};

			Get["products/forstockbalanceupdate"] = o =>
			{
				var dbQuery = ObjectFactory.Container.GetInstance<IStockBalanceQuery>();
				var result = dbQuery.GetStockBalanceUpdatesSince(DateTime.Now.AddHours(-4));

				return Response.AsJson(result);
			};
		}
	}
}