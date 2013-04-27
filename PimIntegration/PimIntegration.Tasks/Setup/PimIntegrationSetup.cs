using System;
using System.Data.SQLite;
using PimIntegration.Exceptions;
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
			EnsureStateOfDatabase(settings.DbConnectionString, container, settings);

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

		private static void EnsureStateOfDatabase(string dbConnectionString, IContainer container, ITaskSettings settings)
		{
			using (var conn = new SQLiteConnection(dbConnectionString))
			{
				conn.Open();
				var query = @"CREATE TABLE IF NOT EXISTS PimApiConversationState (ID INTEGER PRIMARY KEY AUTOINCREMENT, MethodName TEXT, TimeStampForLastRequest TEXT)";
				using (var cmd = new SQLiteCommand(query, conn))
				{
					cmd.ExecuteNonQuery();
				}

				var repo = container.GetInstance<IPimConversationStateRepository>();

				try
				{
					repo.GetTimeStampOfLastRequestForNewProducts();
				}
				catch (PimIntegrationDbException pide)
				{
					// This should only happen the first time
					Log.ForCurrent.Error(pide.Message);
					repo.UpdateTimeStampOfLastRequestForNewProducts(DateTime.Now);
				}
			}
		}
	}
}
