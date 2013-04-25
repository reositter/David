using PimIntegration.Service;
using Topshelf;

namespace PimIntegration.Host
{
	public class Program
	{
		static void Main(string[] args)
		{
			const string AppName = "PIM Integration Service";
			
			HostFactory.Run(x =>
			{
				x.Service<Service>(s =>
					{
						s.ConstructUsing(name => new Service());
						s.WhenStarted(pis => pis.Start());
						s.WhenStopped(pis => pis.Stop());
					});
				x.RunAsLocalSystem();

				x.SetDescription(AppName);
				x.SetDisplayName(AppName);
				x.SetServiceName("PimIntegrationService");
			});
		}
	}
}
