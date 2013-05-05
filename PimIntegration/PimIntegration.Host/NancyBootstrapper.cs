using Nancy;
using Nancy.Conventions;

namespace PimIntegration.Host
{
	public class NancyBootstrapper : DefaultNancyBootstrapper
	{
		protected override void ConfigureConventions(NancyConventions conventions)
		{
			conventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("css", @"Content/css"));
			conventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("js", @"Content/js"));
			conventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("img", @"Content/img"));
			base.ConfigureConventions(conventions);
		}	
	}
}