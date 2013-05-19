using System;
using System.Collections.Generic;
using System.Data.SQLite;
using PimIntegration.Tasks.Database.Dto;
using PimIntegration.Tasks.Database.Interfaces;
using PimIntegration.Tasks.Setup;

namespace PimIntegration.Tasks.Database
{
	public class PimMessageResultRepository : IPimMessageResultRepository 
	{
		private readonly string _connectionString;
		private const string TableName = "PimMessageResult";

		public PimMessageResultRepository(ITaskSettings settings)
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
										+ "EnqueuedAt TIMESTAMP NOT NULL, "
										+ "DequeuedAt TIMESTAMP, "
										+ "NumberOfFailedAttemptsToDequeue INTEGER NOT NULL, "
										+ "Status INTEGER, "
										+ "ErrorDetails TEXT"
										+");", TableName);
				using (var cmd = new SQLiteCommand(query, conn))
				{
					cmd.ExecuteNonQuery();
				}
			}
		}

		public IEnumerable<PimMessageResult> GetRecentMessages(int maximumNumberOfItems)
		{
			var list = new List<PimMessageResult>();

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
							list.Add(new PimMessageResult
							{
								MessageId = Convert.ToInt32(r["MessageId"]),
								PrimaryAction = Convert.ToString(r["PrimaryAction"]),
								SecondaryAction = Convert.ToString(r["SecondaryAction"]),
								EnqueuedAt = Convert.ToDateTime(r["EnqueuedAt"]),
								DequeuedAt = r.IsDBNull(r.GetOrdinal("DequeuedAt")) ? default(DateTime?) : Convert.ToDateTime(r["DequeuedAt"]),
								NumberOfFailedAttemptsToDequeue = Convert.ToInt32(r["NumberOfFailedAttemptsToDequeue"]),
								Status = r.IsDBNull(r.GetOrdinal("Status")) ? default(int?) : Convert.ToInt32(r["Status"]),
								ErrorDetails = r.IsDBNull(r.GetOrdinal("ErrorDetails")) ? string.Empty : Convert.ToString(r["ErrorDetails"])
							});
						}
					}
				}
			}

			return list;
		}

		public void SaveMessageResult(PimMessageResult msg)
		{
			var query = string.Format(
				"INSERT INTO {0}(MessageId, PrimaryAction, SecondaryAction, EnqueuedAt, DequeuedAt, NumberOfFailedAttemptsToDequeue, Status, ErrorDetails) "
				+ "VALUES(@MessageId, @PrimaryAction, @SecondaryAction, @EnqueuedAt, @DequeuedAt, @NumberOfFailedAttemptsToDequeue, @Status, @ErrorDetails);", TableName);
			using (var conn = new SQLiteConnection(_connectionString))
			{
				conn.Open();
				using (var cmd = new SQLiteCommand(query, conn))
				{
					cmd.Parameters.AddWithValue("@MessageId", msg.MessageId);
					cmd.Parameters.AddWithValue("@PrimaryAction", msg.PrimaryAction);
					cmd.Parameters.AddWithValue("@SecondaryAction", msg.SecondaryAction);
					cmd.Parameters.AddWithValue("@EnqueuedAt", msg.EnqueuedAt);
					cmd.Parameters.AddWithValue("@DequeuedAt", msg.DequeuedAt.HasValue ? msg.DequeuedAt.Value : (object)DBNull.Value);
					cmd.Parameters.AddWithValue("@NumberOfFailedAttemptsToDequeue", msg.NumberOfFailedAttemptsToDequeue);
					cmd.Parameters.AddWithValue("@Status", msg.Status.HasValue ? msg.Status.Value : (object)DBNull.Value);
					cmd.Parameters.AddWithValue("@ErrorDetails", msg.ErrorDetails);

					cmd.ExecuteNonQuery();
				}
			}
		}
	}
}