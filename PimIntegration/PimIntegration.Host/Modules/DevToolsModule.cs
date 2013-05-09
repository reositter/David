using System;
using Nancy;
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

			Post["/products"] = o =>
			{
				Log.ForCurrent.Info("POST /products");

				var pimQueryService = ObjectFactory.Container.GetInstance<IPimQueryService>();
				var products = pimQueryService.GetNewProductsSinceDummy(DateTime.Now.AddHours(-4));

				var response = (Response) products.Length.ToString();
				response.ContentType = "applicatin/json";
				return response;
			};
		}
	}
}