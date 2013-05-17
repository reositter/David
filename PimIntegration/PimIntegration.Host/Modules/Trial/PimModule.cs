using System;
using Nancy;
using PimIntegration.Tasks.PimApi.Interfaces;
using StructureMap;

namespace PimIntegration.Host.Modules.Trial
{
	public class PimModule : NancyModule
	{
		public PimModule() : base("/trial/pim")
		{
			Get["/form/getproductbydatedummy"] = o => View["partial/GetProductByDateDummy.cshtml", o];
			Get["/getproductbydatedummy"] = parameters =>
			{
				var pimQueryService = ObjectFactory.Container.GetInstance<IPimQueryService>();
				var products = pimQueryService.GetNewProductsSinceDummy();

				return Response.AsJson(products);
			};

			Get["/form/getproductbydate"] = o =>
			{
				dynamic model = new
				{
					Title = "GetProductByDate",
					ActionUrl = "trial/pim/getproductbydate",
					Method = "GET"
				};
				return View["partial/Since.cshtml", model];
			};

			Get["/getproductbydate"] = parameters =>
			{
				var timestamp = Convert.ToDateTime(Request.Query.Timestamp.ToString());

				var pimQueryService = ObjectFactory.Container.GetInstance<IPimQueryService>();
				var products = pimQueryService.GetNewProductsSince(timestamp);

				return Response.AsJson(new
				{
					NewProductsFromPim = products
				});
			};

			Get["/form/getproductbysku"] = _ => View["partial/GetProductBySku.cshtml", _];
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
