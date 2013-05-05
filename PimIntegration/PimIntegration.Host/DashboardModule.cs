namespace PimIntegration.Host
{
	public class DashboardModule : Nancy.NancyModule
	{
		public DashboardModule()
		{
			Get["/"] = parameters =>
			{
				return View["dashboard.cshtml", parameters];
			};
		}
	}
}