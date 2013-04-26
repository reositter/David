using System.Data.SQLite;
using PimIntegration.Tasks.Database;
using StructureMap;

namespace PimIntegration.Tasks.Setup
{
	public class PimIntegrationSetup
	{
		public static IContainer InitializeEverything(TaskSettings settings)
		{
			var container = ObjectFactory.Container;

			EnsureDbTablesExists(settings.DbConnectionString);
			PrepareIocContainer(container, settings);

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

		private static void EnsureDbTablesExists(string dbConnectionString)
		{
			using (var conn = new SQLiteConnection(dbConnectionString))
			{
				conn.Open();
				var query = @"CREATE TABLE IF NOT EXISTS PimConversationState (ID INTEGER PRIMARY KEY AUTOINCREMENT, MethodName TEXT, TimeStampForLastRequest TEXT)";
				using (var cmd = new SQLiteCommand(query, conn))
				{
					cmd.ExecuteNonQuery();
				}
			}
		}
	}
}
