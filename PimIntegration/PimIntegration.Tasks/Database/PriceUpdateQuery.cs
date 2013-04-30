using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using PimIntegration.Tasks.Setup;
using PimIntegration.Tasks.VismaGlobal.Dto;

namespace PimIntegration.Tasks.Database
{
	public class PriceUpdateQuery : IPriceUpdateQuery 
	{
		private readonly IVismaSettings _settings;

		public PriceUpdateQuery(IVismaSettings settings)
		{
			_settings = settings;			
		}

		public IList<ArticleForPriceAndStockUpdate> GetPriceUpdatesSince(DateTime lastQuery)
		{
			var list = new List<ArticleForPriceAndStockUpdate>();

			using (var conn = new SqlConnection(_settings.VismaDbConnectionString))
			{
				conn.Open();

				using (var cmd = new SqlCommand(string.Format("[{0}].[SP_GetPriceUpdatesForPimIntegration]", _settings.VismaDbSchema), conn))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddWithValue("@Since", lastQuery);

					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							list.Add(new ArticleForPriceAndStockUpdate
							{
								ArticleNo = (string)reader["ArticleNo"],
								PimSku = (string)reader["PimSku"],
								StockBalance = Convert.ToDecimal(reader["ArticleForPriceAndStockUpdate"])
							});
						}
					}
				}
			}

			return list;
		}
	}
}