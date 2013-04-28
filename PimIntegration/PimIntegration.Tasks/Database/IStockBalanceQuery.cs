using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using PimIntegration.Tasks.Setup;
using PimIntegration.Tasks.VismaGlobal.Dto;

namespace PimIntegration.Tasks.Database
{
	public interface IStockBalanceQuery
	{
		IList<StockBalance> GetStockBalanceUpdatesSince(DateTime lastQuery);
	}

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
				var query = string.Format("SELECT * FROM {0}.StockSurveyTotalsView;", _settings.VismaDbSchema);
				using (var cmd = new SqlCommand(query, conn))
				{
					//cmd.CommandType = CommandType.StoredProcedure;
					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							list.Add(new StockBalance
							{
								ArticleNo = (string)reader["ArticleNo"]
							});
						}
					}
				}
			}

			return list;
		}
	}
}