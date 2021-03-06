﻿using System.Collections.Generic;
using PimIntegration.Tasks.PIMServiceEndpoint;
using PimIntegration.Tasks.Setup;
using PimIntegration.Tasks.VismaGlobal.Dto;

namespace PimIntegration.Tasks
{
	public interface IMapper
	{
		IList<ArticleForCreate> MapPimProductsToVismaArticles(IEnumerable<ProductQueryResponseItem> pimProducts);
	}

	public class Mapper : IMapper
	{
		private readonly IVismaSettings _settings;

		public Mapper(IVismaSettings settings)
		{
			_settings = settings;
		}

		public IList<ArticleForCreate> MapPimProductsToVismaArticles(IEnumerable<ProductQueryResponseItem> pimProducts)
		{
			var list = new List<ArticleForCreate>();

			foreach (var pimProduct in pimProducts)
			{
				var article = new ArticleForCreate
				{
					PimSku = pimProduct.SKU,
					Name = pimProduct.FullModel,
					PostingTemplateNo = _settings.VismaPostingTemplateNo,
					PriceCalcMethodsNo = _settings.VismaPriceCalcMethodsNo,
					StockProfileNo = _settings.VismaStockProfileNo
				};

				if (pimProduct.Markets != null)
				{
					foreach (var market in pimProduct.Markets)
					{
						switch (market.Market)
						{
							case "4Sound.dk":
								article.ShortDescriptionDen = market.ShortDescription;
								break;
							case "4Sound.no":
								article.ShortDescriptionNor = market.ShortDescription;
								break;
							case "4Sound.se":
								article.ShortDescriptionSwe = market.ShortDescription;
								break;
						}
					}	
				}

				list.Add(article);
			}

			return list;
		}
	}
}