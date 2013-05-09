using System;
using Nancy;
using Newtonsoft.Json;
using PimIntegration.Tasks;
using PimIntegration.Tasks.PimApi;
using StructureMap;

namespace PimIntegration.Host.Modules
{
	public class DevToolsModule : Nancy.NancyModule
	{
		public DevToolsModule()
		{
			Get["/devtools"] = o =>
			{
				o.Title = "Dev Tools";
				return View["devtools.cshtml", o];
			};

			Get["/products/new"] = o =>
			{
				Log.ForCurrent.Info("GET /products/new");

				var pimQueryService = ObjectFactory.Container.GetInstance<IPimQueryService>();
				var products = pimQueryService.GetNewProductsSinceDummy(DateTime.Now.AddHours(-4));

				var response = (Response) (products != null ? JsonConvert.SerializeObject(products) : "null");
				response.ContentType = "applicatin/json";
				return response;
			};
		}
	}
}