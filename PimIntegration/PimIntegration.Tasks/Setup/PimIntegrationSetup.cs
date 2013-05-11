using PimIntegration.Tasks.VismaGlobal;
using StructureMap;

namespace PimIntegration.Tasks.Setup
{
	public static class PimIntegrationSetup
	{
		public static IContainer BootstrapEverything(AppSettings settings)
		{
			var container = ObjectFactory.Container;

			PrepareIocContainer(container, settings);
			InitializeCustomVismaFields(container.GetInstance<IVismaConnection>());

			return container;
		}

		private static void PrepareIocContainer(IContainer container, AppSettings settings)
		{
			var vismaConnection = new VismaConnection(settings.VismaClientName, settings.VismaUserName, settings.VismaPassword, settings.VismaBapiKey);

			container.Configure(cnfg =>
			{
				// Using a specific instance makes the VismaConnection a singleton in practise
				cnfg.For<IVismaConnection>().Use(vismaConnection);
				cnfg.For<ITaskSettings>().Use(settings);
				cnfg.For<IVismaSettings>().Use(settings);

				cnfg.Scan(scan =>
				{
					scan.TheCallingAssembly();
					// Exclude IVismaConnection in case it gains a ctor without params in the future.
					scan.ExcludeType<IVismaConnection>();
					scan.WithDefaultConventions();
				});
			}); 
		}

		private static void InitializeCustomVismaFields(IVismaConnection vismaConnection)
		{
			ZUsrFields.Initialize(vismaConnection.Open());
		}
	}
}
