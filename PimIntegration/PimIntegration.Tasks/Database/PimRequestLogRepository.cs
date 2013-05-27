using System;
using System.Collections.Generic;
using System.Data.SQLite;
using PimIntegration.Tasks.Database.Dto;
using PimIntegration.Tasks.Database.Interfaces;
using PimIntegration.Tasks.Setup;

namespace PimIntegration.Tasks.Database
{
	public class PimRequestLogRepository : IPimRequestLogRepository 
	{
		private readonly string _connectionString;
		private const string TableName = "PimRequestLog";

		public PimRequestLogRepository(ITaskSettings settings)
		{
			_connectionString = settings.SqliteConnectionString;
			EnsureTableExists();
		}

		private void EnsureTableExists()
		{
			using (var conn = new SQLiteConnection(_connectionString))
			{
				conn.Open();
				var query = string.Format("CREATE TABLE IF NOT EXISTS {0} ("
										+ "ID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, "
										+ "MessageId INTEGER NOT NULL, "
										+ "PrimaryAction TEXT NOT NULL, "
										+ "SecondaryAction TEXT NOT NULL, "
										+ "RequestItem TEXT, "
										+ "EnqueuedAt TIMESTAMP NOT NULL, "
										+ "DequeuedAt TIMESTAMP, "
										+ "NumberOfFailedAttemptsToDequeue INTEGER"
										+");", TableName);
				using (var cmd = new SQLiteCommand(query, conn))
				{
					cmd.ExecuteNonQuery();
				}
			}
		}

		public IEnumerable<PimRequestLogItem> GetRecentRequests(int maximumNumberOfItems)
		{
			var list = new List<PimRequestLogItem>();

			using (var conn = new SQLiteConnection(_connectionString))
			{
				conn.Open();
				var query = string.Format("SELECT * FROM {0} ORDER BY Id DESC LIMIT @MaximumNumberOfItems;", TableName);

				using (var cmd = new SQLiteCommand(query, conn))
				{
					cmd.Parameters.AddWithValue("@MaximumNumberOfItems", maximumNumberOfItems);
					using (var r = cmd.ExecuteReader())
					{
						while (r.Read())
						{
							list.Add(new PimRequestLogItem
							{
								Id = Convert.ToInt32(r["Id"]),
								MessageId = Convert.ToInt32(r["MessageId"]),
								PrimaryAction = Convert.ToString(r["PrimaryAction"]),
								SecondaryAction = Convert.ToString(r["SecondaryAction"]),
								EnqueuedAt = Convert.ToDateTime(r["EnqueuedAt"]),
								DequeuedAt = r.IsDBNull(r.GetOrdinal("DequeuedAt")) ? default(DateTime?) : Convert.ToDateTime(r["DequeuedAt"]),
								NumberOfFailedAttemptsToDequeue = r.IsDBNull(r.GetOrdinal("NumberOfFailedAttemptsToDequeue")) ? default(int?) : Convert.ToInt32(r["NumberOfFailedAttemptsToDequeue"])
							});
						}
					}
				}
			}

			return list;
		}

		public string GetRequestItem(int requestLogId)
		{
			using (var conn = new SQLiteConnection(_connectionString))
			{
				conn.Open();
				using (var cmd = new SQLiteCommand(string.Format("SELECT RequestItem FROM {0} WHERE Id = @Id", TableName), conn))
				{
					cmd.Parameters.AddWithValue("@Id", requestLogId);
					using (var r = cmd.ExecuteReader())
					{
						if (r.Read())
							return Convert.ToString(r["RequestItem"]);
					}
				}
			}

			return string.Empty;
		}

		public void LogEnqueuedRequest(EnqueuedRequest enqueuedRequest)
		{
			var query = string.Format(
				"INSERT INTO {0}(MessageId, PrimaryAction, SecondaryAction, RequestItem, EnqueuedAt) " +
				"VALUES(@MessageId, @PrimaryAction, @SecondaryAction, @RequestItem, @EnqueuedAt);", TableName);

			using (var conn = new SQLiteConnection(_connectionString))
			{
				conn.Open();
				using (var cmd = new SQLiteCommand(query, conn))
				{
					cmd.Parameters.AddWithValue("@MessageId", enqueuedRequest.MessageId);
					cmd.Parameters.AddWithValue("@PrimaryAction", enqueuedRequest.PrimaryAction);
					cmd.Parameters.AddWithValue("@SecondaryAction", enqueuedRequest.SecondaryAction);
					cmd.Parameters.AddWithValue("@RequestItem", enqueuedRequest.RequestItem);
					cmd.Parameters.AddWithValue("@EnqueuedAt", enqueuedRequest.EnqueuedAt);

					cmd.ExecuteNonQuery();
				}
			}
		}

		public void UpdateRequest(int messageId, DateTime? dequeuedAt, int numberOfFailedAttemptsToDequeue)
		{
			var query = string.Format(
				"UPDATE {0} " +
				"SET DequeuedAt = @DequeuedAt, NumberOfFailedAttemptsToDequeue = @NumberOfFailedAttemptsToDequeue " +
				"WHERE MessageId = @MessageId;",
				TableName);

			using (var conn = new SQLiteConnection(_connectionString))
			{
				conn.Open();
				using (var cmd = new SQLiteCommand(query, conn))
				{
					cmd.Parameters.AddWithValue("@MessageId", messageId);
					cmd.Parameters.AddWithValue("@DequeuedAt", dequeuedAt.HasValue ? dequeuedAt.Value : (object)DBNull.Value);
					cmd.Parameters.AddWithValue("@NumberOfFailedAttemptsToDequeue", numberOfFailedAttemptsToDequeue);

					cmd.ExecuteNonQuery();
				}
			}
		}
	}
}