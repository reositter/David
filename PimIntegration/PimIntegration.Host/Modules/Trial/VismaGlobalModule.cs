using System;
using Nancy;
using PimIntegration.Tasks.Database.Interfaces;
using PimIntegration.Tasks.VismaGlobal.Interfaces;
using StructureMap;

namespace PimIntegration.Host.Modules.Trial
{
	public class VismaGlobalModule : NancyModule
	{
		public VismaGlobalModule() : base("/trial/visma")
		{
			Get["/form/getarticlesforstockbalanceupdate"] = o =>
			{
				dynamic model = new
				{
					Title = "Get Articles for Stock Balance Update",
					ActionUrl = "/trial/visma/getarticlesforstockbalanceupdate",
					Method = "GET"
				};
				return View["partial/Since.cshtml", model];
			};

			Get["/getarticlesforstockbalanceupdate"] = o =>
			{
				var timestamp = Convert.ToDateTime(Request.Query.Timestamp.ToString());
				var dbQuery = ObjectFactory.Container.GetInstance<IStockBalanceQuery>();
				var stockBalanceUpdates = dbQuery.GetStockBalanceUpdatesSince(timestamp);

				return Response.AsJson(new
				{
					StockBalanceUpdates = stockBalanceUpdates
				});
			};

			Get["/form/getarticlesforpriceupdate"] = o =>
			{
				dynamic model = new
				{
					Title = "Get Articles for Price Update",
					ActionUrl = "/trial/visma/getarticlesforpriceupdate",
					Method = "GET"
				};
				return View["partial/Since.cshtml", model];
			};

			Get["/getarticlesforpriceupdate"] = o =>
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
		}
	}
}