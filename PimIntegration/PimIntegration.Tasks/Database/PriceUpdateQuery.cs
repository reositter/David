using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using PimIntegration.Tasks.Database.Dto;
using PimIntegration.Tasks.Database.Interfaces;
using PimIntegration.Tasks.Setup;

namespace PimIntegration.Tasks.Database
{
	public class PriceUpdateQuery : IPriceUpdateQuery 
	{
		private readonly IVismaSettings _settings;

		public PriceUpdateQuery(IVismaSettings settings)
		{
			_settings = settings;			
		}

		public IList<ArticleForPriceUpdate> GetArticlesForPriceUpdate(DateTime lastQuery)
		{
			var list = new List<ArticleForPriceUpdate>();

			using (var conn = new SqlConnection(_settings.VismaDbConnectionString))
			{
				conn.Open();

				using (var cmd = new SqlCommand(string.Format("[{0}].[SP_PimIntegration_GetArticlesForPriceUpdate]", _settings.VismaDbSchema), conn))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddWithValue("@Since", lastQuery);

					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							list.Add(new ArticleForPriceUpdate
							{
								ArticleNo = (string)reader["ArticleNo"],
								PimSku = (string)reader["PimSku"]
							});
						}
					}
				}
			}

			return list;
		}
	}
}