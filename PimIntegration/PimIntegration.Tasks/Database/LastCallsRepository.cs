using System;
using System.Data.SQLite;
using PimIntegration.Exceptions;
using PimIntegration.Tasks.Setup;

namespace PimIntegration.Tasks.Database
{
	public class LastCallsRepository : ILastCallsRepository
	{
		private readonly string _connectionString;
		private readonly ITaskSettings _settings;
		private const string TableName = "LastCalls";

		public LastCallsRepository(ConnectionStringWrapper connectionStringWrapper, ITaskSettings settings)
		{
			_connectionString = connectionStringWrapper.ConnectionString;
			_settings = settings;
			EnsureTableAndExpectedRowsExists();
		}

		public DateTime GetTimeOfLastRequestForNewProducts()
		{
			return GetTimeOfLastRequest("GetProductByGroupAndBrand");
		}

		public void UpdateTimesOfLastRequestForNewProducts(DateTime timeOfRequest)
		{
			UpdateTimeStampOfLastRequest("GetProductByGroupAndBrand", timeOfRequest);
		}

		public DateTime GetTimeOfLastPublishedStockBalanceUpdates()
		{
			return GetTimeOfLastRequest("PublishStockBalanceUpdates");
		}

		public void UpdateTimeOfLastPublishedStockBalanceUpdates(DateTime publishedTime)
		{
			UpdateTimeStampOfLastRequest("PublishStockBalanceUpdates", publishedTime);
		}

		private DateTime GetTimeOfLastRequest(string action)
		{
			DateTime timeStamp;

			using (var conn = new SQLiteConnection(_connectionString))
			{
				conn.Open();
				var query = string.Format("SELECT TimeOfLastRequest FROM {0} WHERE Action = '{1}';", TableName, action);

				using (var cmd = new SQLiteCommand(query, conn))
				{
					using (var reader = cmd.ExecuteReader())
					{
						if (reader.Read())
							timeStamp = Convert.ToDateTime(reader["TimeOfLastRequest"]);
						else
							throw new PimIntegrationDbException(string.Format("Failed to get 'TimeOfLastRequest' from table '{0}' for action '{1}'", TableName, action));
					}
				}
			}

			return timeStamp;
		}

		private void UpdateTimeStampOfLastRequest(string action, DateTime timeOfRequest)
		{
			var timeStamp = timeOfRequest.ToString(_settings.TimeStampFormat);
			using (var conn = new SQLiteConnection(_connectionString))
			{
				conn.Open();
				var query = string.Format("UPDATE {0} SET TimeOfLastRequest = '{1}' WHERE Action = '{2}';", TableName, timeStamp, action);

				using (var cmd = new SQLiteCommand(query, conn))
				{
					cmd.ExecuteNonQuery();
				}
			}
		}

		private void EnsureTableAndExpectedRowsExists()
		{
			using (var conn = new SQLiteConnection(_connectionString))
			{
				conn.Open();
				var query = string.Format("CREATE TABLE IF NOT EXISTS {0} (ID INTEGER PRIMARY KEY AUTOINCREMENT, Action TEXT, TimeOfLastRequest TEXT);", TableName);
				using (var cmd = new SQLiteCommand(query, conn))
				{
					cmd.ExecuteNonQuery();
				}

				try
				{
					GetTimeOfLastRequestForNewProducts();
				}
				catch (PimIntegrationDbException pide)
				{
					// This should only happen the very first time the app is run.
					Log.ForCurrent.Error(pide.Message);
					InsertInitialTimeOfLastRequestValue("GetProductByGroupAndBrand", DateTime.Now, conn);
				}

				try
				{
					GetTimeOfLastPublishedStockBalanceUpdates();
				}
				catch (PimIntegrationDbException pide)
				{
					// This should only happen the very first time the app is run.
					Log.ForCurrent.Error(pide.Message);
					InsertInitialTimeOfLastRequestValue("PublishStockBalanceUpdates", DateTime.Now, conn);
				}
			}
		}

		private void InsertInitialTimeOfLastRequestValue(string action, DateTime timestamp, SQLiteConnection conn)
		{
			Log.ForCurrent.InfoFormat("Setting initial value for '{0}' to '{1}'.", action, timestamp.ToString(_settings.TimeStampFormat));
			var query = string.Format("INSERT INTO {0}(Action, TimeOfLastRequest) VALUES('{1}', '{2}');", TableName, action, timestamp.ToString(_settings.TimeStampFormat));

			using (var cmd = new SQLiteCommand(query, conn))
			{
				cmd.ExecuteNonQuery();
			}
		}
	}
}