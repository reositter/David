using System;
using System.Data.SQLite;
using PimIntegration.Exceptions;
using PimIntegration.Tasks.Setup;

namespace PimIntegration.Tasks.Database
{
	public class PimApiConversationStateRepository : IPimApiConversationStateRepository
	{
		private readonly string _connectionString;
		private readonly ITaskSettings _settings;
		private const string TableName = "PimApiConversationState";

		public PimApiConversationStateRepository(ConnectionStringWrapper connectionStringWrapper, ITaskSettings settings)
		{
			_connectionString = connectionStringWrapper.ConnectionString;
			_settings = settings;
		}

		public DateTime GetTimeStampOfLastRequestForNewProducts()
		{
			DateTime timeStamp;

			using (var conn = new SQLiteConnection(_connectionString))
			{
				conn.Open();
				var query = string.Format("SELECT TimeOfLastRequest FROM {0} WHERE MethodName = 'GetProductByGroupAndBrand';", TableName);

				using (var cmd = new SQLiteCommand(query, conn))
				{
					using (var reader = cmd.ExecuteReader())
					{
						if (reader.Read())
							timeStamp = Convert.ToDateTime(reader["TimeOfLastRequest"]);
						else
							throw new PimIntegrationDbException(string.Format("Failed to get 'TimeOfLastRequest' from table '{0}'", TableName));
					}
				}
			}

			return timeStamp;
		}

		public void UpdateTimeStampOfLastRequestForNewProducts(DateTime timeOfRequest)
		{
			UpdateTimeStampOfLastRequest("GetProductByGroupAndBrand", timeOfRequest);
		}

		public void EnsureExistensAndInitializeTable()
		{
			using (var conn = new SQLiteConnection(_connectionString))
			{
				conn.Open();
				var query = @"CREATE TABLE IF NOT EXISTS PimApiConversationState (ID INTEGER PRIMARY KEY AUTOINCREMENT, MethodName TEXT, TimeOfLastRequest TEXT)";
				using (var cmd = new SQLiteCommand(query, conn))
				{
					var result = cmd.ExecuteNonQuery();
				}

				try
				{
					GetTimeStampOfLastRequestForNewProducts();
				}
				catch (PimIntegrationDbException pide)
				{
					// This should only happen the first time
					Log.ForCurrent.Error(pide.Message);

					Log.ForCurrent.InfoFormat("Initializing the {0} table.", TableName);
					query = string.Format(
						"INSERT INTO {0}(MethodName, TimeOfLastRequest) VALUES('GetProductByGroupAndBrand', '{1}');", 
						TableName, 
						DateTime.Now.ToString(_settings.TimeStampFormat));

					using (var cmd = new SQLiteCommand(query, conn))
					{
						cmd.ExecuteNonQuery();
					}
				}
			}
		}

		private void UpdateTimeStampOfLastRequest(string methodName, DateTime timeOfRequest)
		{
			var timeStamp = timeOfRequest.ToString(_settings.TimeStampFormat);
			using (var conn = new SQLiteConnection(_connectionString))
			{
				conn.Open();
				var query = string.Format("UPDATE {0} SET TimeOfLastRequest = '{1}' WHERE MethodName = '{2}';", TableName, timeStamp, methodName);

				using (var cmd = new SQLiteCommand(query, conn))
				{
					cmd.ExecuteNonQuery();
				}
			}
		}
	}
}