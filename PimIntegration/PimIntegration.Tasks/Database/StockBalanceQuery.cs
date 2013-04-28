using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using PimIntegration.Tasks.Setup;
using PimIntegration.Tasks.VismaGlobal.Dto;

namespace PimIntegration.Tasks.Database
{
	public class StockBalanceQuery : IStockBalanceQuery
	{
		private readonly IVismaSettings _settings;

		public StockBalanceQuery(IVismaSettings settings)
		{
			_settings = settings;
		}

		public IList<StockBalance> GetStockBalanceUpdatesSince(DateTime lastQuery)
		{
			var list = new List<StockBalance>();

			using (var conn = new SqlConnection(_settings.VismaDbConnectionString))
			{
				conn.Open();

				using (var cmd = new SqlCommand(string.Format("[{0}].[SP_GetStockBalanceForPimIntegration]", _settings.VismaDbSchema), conn))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddWithValue("@TimeOfLastQuery", lastQuery);

					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							list.Add(new StockBalance
							{
								ArticleNo = (string)reader["ArticleNo"],
								Quantity = Convert.ToDecimal(reader["StockBalance"])
							});
						}
					}
				}
			}

			return list;
		}
	}
}