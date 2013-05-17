using Nancy;

namespace PimIntegration.Host.Modules
{
	public class MainMenuModule : NancyModule
	{
		public MainMenuModule()
		{
			Get["/"] = _ =>
			{
				_.Title = "Dashboard";
				return View["dashboard.cshtml", _];
			};

			Get["/trial"] = _ =>
			{
				_.Title = "Trial";
				return View["/trial.cshtml", _];
			};
		}
	}
}