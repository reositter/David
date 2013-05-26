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

			Get["/form/dequeueproductqueryresponse"] = _ =>
			{
				dynamic model = new {
					Title = "Dequeue Message",
					ActionUrl = "trial/pim/productqueryresponse/",
					Method = "GET"
				};
				
				return View["partial/DequeueResponse.cshtml", model];
			};
			Get["/productqueryresponse/{messageid}"] = parameters =>
			{
				var pimQueryService = ObjectFactory.Container.GetInstance<IPimQueryService>();
				var products = pimQueryService.DequeueProductQueryResponseWithoutRetries(parameters.messageid);

				return Response.AsJson(new
				{
					Products = products
				});
			};
		}
	}
}
