using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace PimIntegration.Tasks.Setup
{
	public class PimIntegrationSetup
	{
		public static void InitializeEverything(TaskSettings settings)
		{
			EnsureDbTablesExists(settings.DbConnectionString);
		}

		private static void EnsureDbTablesExists(string dbConnectionString)
		{
			var conn = new SQLiteConnection(dbConnectionString);
			conn.Open();

			var cmd = new SQLiteCommand("CREATE TABLE IF NOT EXISTS PimConversationState (ID INTEGER PRIMARY KEY AUTOINCREMENT, MethodName TEXT, TimeStampForLastRequest TEXT)", conn);
			cmd.ExecuteNonQuery();

			conn.Close();
		}
	}
}
