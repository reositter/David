using System;
using System.Collections.Generic;
using System.Data.SQLite;
using PimIntegration.Tasks.Database.Dto;
using PimIntegration.Tasks.Setup;

namespace PimIntegration.Tasks.Database.Interfaces
{
	public interface IPimMessageResultRepository
	{
		IList<PimMessageResult> GetMessagesWithoutResponse(int maximumNumberOfItems);
		void SaveMessageResult(PimMessageResult pimMessageResult);
	}

	public class PimMessageResultRepository : IPimMessageResultRepository 
	{
		private readonly string _connectionString;
		private readonly ITaskSettings _settings;
		private const string TableName = "PimMessageResult";

		public PimMessageResultRepository(ITaskSettings settings)
		{
			_connectionString = settings.SqliteConnectionString;
			_settings = settings;
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
					+ "Status INTEGER NOT NULL"
					+");", TableName);
				using (var cmd = new SQLiteCommand(query, conn))
				{
					cmd.ExecuteNonQuery();
				}
			}
		}

		public IList<PimMessageResult> GetMessagesWithoutResponse(int maximumNumberOfItems)
		{
			var list = new List<PimMessageResult>();

			using (var conn = new SQLiteConnection(_connectionString))
			{
				conn.Open();
				var query = string.Format("SELECT * FROM {0} WHERE Status = 404 ORDER BY Id DESC LIMIT @MaximumNumberOfItems;", TableName);

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
								Status = Convert.ToInt32(r["Status"])
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
				"INSERT INTO {0}(MessageId, PrimaryAction, SecondaryAction, EnqueuedAt, DequeuedAt, NumberOfFailedAttemptsToDequeue, Status) "
				+ "VALUES(@MessageId, @PrimaryAction, @SecondaryAction, @EnqueuedAt, @DequeuedAt, @NumberOfFailedAttemptsToDequeue, @Status);", TableName);
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
					cmd.Parameters.AddWithValue("@Status", msg.Status);

					cmd.ExecuteNonQuery();
				}
			}
		}
	}
}