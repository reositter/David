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

			Get["/devtools/method/newproducts"] = o => View["partial/GetNewProductsSince.cshtml", o];

			Get["/products/new"] = parameters =>
			{
				Log.ForCurrent.Info("GET /products/new");
				Log.ForCurrent.InfoFormat("Hour = {0} Minute = {1}", Request.Query.Hour, Request.Query.Minute);

				var pimQueryService = ObjectFactory.Container.GetInstance<IPimQueryService>();
				var products = pimQueryService.GetNewProductsSinceDummy(DateTime.Now.AddHours(-4));

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