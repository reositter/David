using PimIntegration.Tasks;

namespace PimIntegration.Host.Modules
{
	public class DashboardModule : Nancy.NancyModule
	{
		public DashboardModule()
		{
			Get["/"] = parameters =>
			{
				dynamic model = new
				{
					Title = "Dashboard",
					Parameters = parameters
				};
				return View["dashboard.cshtml", model];
			};

			Post["/trial"] = o =>
			{
				Log.ForCurrent.Debug("POST to /trial");
				return 200;
			};
		}
	}
}