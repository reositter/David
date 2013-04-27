using System;
using System.Data.SQLite;
using PimIntegration.Exceptions;
using PimIntegration.Tasks.Setup;

namespace PimIntegration.Tasks.Database
{
	public class PimConversationStateRepository : IPimConversationStateRepository
	{
		private readonly string _connectionString;
		private ITaskSettings _settings;
		private const string TableName = "PimApiConversationState";

		public PimConversationStateRepository(ConnectionStringWrapper connectionStringWrapper, ITaskSettings settings)
		{
			_connectionString = connectionStringWrapper.ConnectionString;
		}

		public DateTime GetTimeStampOfLastRequestForNewProducts()
		{
			DateTime timeStamp;

			using (var conn = new SQLiteConnection(_connectionString))
			{
				conn.Open();
				const string query = @"SELECT TimeStampForLastRequest FROM PimConversationState WHERE MethodName = 'GetProductByGroupAndBrand'";

				using (var cmd = new SQLiteCommand(query, conn))
				{
					using (var reader = cmd.ExecuteReader())
					{
						if (reader.Read())
							timeStamp = Convert.ToDateTime(reader["TimeStampForLastRequest"]);
						else
							throw new PimIntegrationDbException("Failed to get 'TimeStampForLastRequest' from table 'PimConversationState'");
					}
				}
			}

			return timeStamp;
		}

		public void UpdateTimeStampOfLastRequestForNewProducts(DateTime timeOfRequest)
		{
			UpdateTimeStampOfLastRequest("GetProductByGroupAndBrand", timeOfRequest);
		}

		private void UpdateTimeStampOfLastRequest(string methodName, DateTime timeOfRequest)
		{
			var timeStamp = timeOfRequest.ToString(_settings.TimeStampFormat);
			using (var conn = new SQLiteConnection(_connectionString))
			{
				conn.Open();
				var query = string.Format("UPDATE {0} SET TimeStampForLastRequest = '{1}' WHERE MethodName = '{2}'", TableName, timeStamp, methodName);

				using (var cmd = new SQLiteCommand(query, conn))
				{
					cmd.ExecuteNonQuery();
				}
			}
		}
	}

	public interface IPimConversationStateRepository
	{
		DateTime GetTimeStampOfLastRequestForNewProducts();
		void UpdateTimeStampOfLastRequestForNewProducts(DateTime timeOfRequest);
	}
}