using PimIntegration.Tasks.Database;
using StructureMap;

namespace PimIntegration.Tasks.Setup
{
	public class PimIntegrationSetup
	{
		public static IContainer BootstrapEverything(TaskSettings settings)
		{
			var container = ObjectFactory.Container;

			PrepareIocContainer(container, settings);
			EnsureStateOfDatabase(container);

			return container;
		}

		private static void PrepareIocContainer(IContainer container, TaskSettings settings)
		{
			container.Configure(cnfg =>
			{	
				cnfg.ForConcreteType<ConnectionStringWrapper>().Configure.Ctor<string>().Is(settings.DbConnectionString);

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
