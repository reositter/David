using System;
using PimIntegration.Tasks.Database;
using PimIntegration.Tasks.VismaGlobal;
using RG_SRVLib.Interop;
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
			EnsureStateOfDatabase(container);

			return container;
		}

		private static void ConnectToVismaGlobal(IVismaSettings vismaSettings)
		{
			var loginCode = VgConnection.Open(vismaSettings.VismaClientName, vismaSettings.VismaBapiKey);
			if (loginCode == 0)
			{
				Log.ForCurrent.InfoFormat("Established connection to {0}", vismaSettings.VismaClientName);
				ZUsrFields.Initialize(VgConnection.Connection);				
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
				cnfg.For<GlobalServerComponent>().Use(VgConnection.Connection);

				cnfg.Scan(scan =>
				{
					scan.TheCallingAssembly();
					scan.WithDefaultConventions();
				});
			}); 
		}

		private static void EnsureStateOfDatabase(IContainer container)
		{
			container.GetInstance<IPimApiConversationStateRepository>().EnsureExistensAndInitializeTable();
		}
	}
}
