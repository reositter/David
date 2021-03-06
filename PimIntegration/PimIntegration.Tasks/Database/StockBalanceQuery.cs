using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using PimIntegration.Tasks.Database.Dto;
using PimIntegration.Tasks.Database.Interfaces;
using PimIntegration.Tasks.Setup;

namespace PimIntegration.Tasks.Database
{
	public class StockBalanceQuery : IStockBalanceQuery
	{
		private readonly IVismaSettings _settings;

		public StockBalanceQuery(IVismaSettings settings)
		{
			_settings = settings;
		}

		public IList<ArticleForStockBalanceUpdate> GetStockBalanceUpdatesSince(DateTime lastQuery)
		{
			var list = new List<ArticleForStockBalanceUpdate>();

			using (var conn = new SqlConnection(_settings.VismaDbConnectionString))
			{
				conn.Open();

				using (var cmd = new SqlCommand(string.Format("[{0}].[SP_PimIntegration_GetStockBalanceUpdates]", _settings.VismaDbSchema), conn))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddWithValue("@Since", lastQuery);

					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							list.Add(new ArticleForStockBalanceUpdate
							{
								ArticleNo = (string)reader["ArticleNo"],
								PimSku = reader["ZUsrPimSku"] != DBNull.Value ? (string)reader["ZUsrPimSku"] : string.Empty,
								StockBalance = Convert.ToDecimal(reader["StockBalance"])
							});
						}
					}
				}
			}

			return list;
		}
	}
}