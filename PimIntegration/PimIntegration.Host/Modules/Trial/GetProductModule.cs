using Nancy;
using PimIntegration.Tasks;
using PimIntegration.Tasks.PimApi.Interfaces;
using PimIntegration.Tasks.VismaGlobal.Interfaces;
using StructureMap;

namespace PimIntegration.Host.Modules.Trial
{
	public class GetProductModule : NancyModule
	{
		public GetProductModule()
		{
			Get["/trial/manual/form/getproduct"] = o =>
			{
				return View["partial/GetProduct.cshtml", new { VismaClientName = PimIntegrationSettings.AppSettings.VismaClientName }];
			};

			Post["/trial/manual/getproduct"] = parameters =>
			{
				var sku = Request.Form.Sku;
				var pimQueryService = ObjectFactory.Container.GetInstance<IPimQueryService>();
				var products = pimQueryService.GetProductBySku(sku);

				if (products == null)
				{
					return Response.AsText("PIM did not return a product").WithStatusCode(404);
				}

				// Create Visma article
				var articleManager = ObjectFactory.Container.GetInstance<IArticleManager>();
				var mapper = ObjectFactory.Container.GetInstance<IMapper>();

				var createdArticles = articleManager.CreateArticles(mapper.MapPimProductsToVismaArticles(products));

				// Report Visma articleNo
				var pimCommandService = ObjectFactory.Container.GetInstance<IPimCommandService>();

				foreach (var market in PimIntegrationSettings.AppSettings.Markets)
				{
					pimCommandService.ReportVismaArticleNumbers(market.MarketKey, market.VendorId, createdArticles);
				}

				return Response.AsJson(new
				{
					CreatedArticles = createdArticles
				});
			};
		}
	}
}