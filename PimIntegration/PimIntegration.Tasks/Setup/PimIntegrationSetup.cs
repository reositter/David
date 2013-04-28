using System;
using PimIntegration.Tasks.Database;
using PimIntegration.Tasks.VismaGlobal;
using StructureMap;

namespace PimIntegration.Tasks.Setup
{
	public class PimIntegrationSetup
	{
		public static IContainer BootstrapEverything(AppSettings settings)
		{
			var container = ObjectFactory.Container;

			ConnectToVismaGlobal(settings);
			PrepareIocContainer(container, settings);

			return container;
		}

		private static void ConnectToVismaGlobal(IVismaSettings vismaSettings)
		{
			var loginCode = VismaConnection.Open(vismaSettings.VismaClientName, vismaSettings.VismaUserName, vismaSettings.VismaPassword, vismaSettings.VismaBapiKey);
			if (loginCode == 0)
			{
				Log.ForCurrent.InfoFormat("Connected to Visma Global - {0}", vismaSettings.VismaClientName);
				ZUsrFields.Initialize(VismaConnection.Connection);				
			}
			else
			{
				Log.ForCurrent.ErrorFormat("Failed to open connection to Visma Global. ErrorCode: {0}", loginCode);
				throw new ApplicationException("Failed to open connection to Visma Global");				
			}
		}

		private static void PrepareIocContainer(IContainer container, AppSettings settings)
		{
			container.Configure(cnfg =>
			{	
				cnfg.ForConcreteType<ConnectionStringWrapper>().Configure.Ctor<string>().Is(settings.DbConnectionString);
				cnfg.For<ITaskSettings>().Use(settings);
				cnfg.For<IVismaSettings>().Use(settings);

				cnfg.Scan(scan =>
				{
					scan.TheCallingAssembly();
					scan.WithDefaultConventions();
				});
			}); 
		}
	}
}
