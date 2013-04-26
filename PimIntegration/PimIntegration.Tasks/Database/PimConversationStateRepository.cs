using System;
using System.Data.SQLite;

namespace PimIntegration.Tasks.Database
{
	public class PimConversationStateRepository : IPimConversationStateRepository
	{
		private SQLiteConnection _conn;

		public PimConversationStateRepository(ConnectionStringWrapper connectionStringWrapper)
		{
			_conn = new SQLiteConnection(connectionStringWrapper.ConnectionString);
		}

		public DateTime? GetTimeStampOfLastRequestForNewProducts()
		{
			DateTime? timeStamp = null;

			const string query = @"SELECT TimeStampForLastRequest FROM PimConversationState WHERE MethodName = 'GetProductByGroupAndBrand'";
			_conn.Open();

			using (var cmd = new SQLiteCommand(query, _conn))
			{
				using (var reader = cmd.ExecuteReader())
				{
					if (reader.Read())
						timeStamp = Convert.ToDateTime(reader["TimeStampForLastRequest"]);
				}
			}

			_conn.Close();

			return timeStamp;
		}

		public void SetTimeStampOfLastRequestForNewProducts(DateTime timeStamp)
		{
			throw new NotImplementedException();
		}
	}

	public interface IPimConversationStateRepository
	{
		DateTime? GetTimeStampOfLastRequestForNewProducts();
		void SetTimeStampOfLastRequestForNewProducts(DateTime timeStamp);
	}
}