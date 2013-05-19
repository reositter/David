﻿using System.Collections.Generic;
using PimIntegration.Tasks.Database.Dto;
using PimIntegration.Tasks.PIMServiceEndpoint;
using PimIntegration.Tasks.PimApi.Dto;
using PimIntegration.Tasks.Setup;
using PimIntegration.Tasks.VismaGlobal.Dto;

namespace PimIntegration.Tasks
{
	public interface IMapper
	{
		IList<ArticleForCreate> MapPimProductsToVismaArticles(IEnumerable<ProductQueryResponseItem> pimProducts);
		PimMessageResult MapMessageResultToPimMessageResult(MessageResult msg);
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
					// just in case there is no swedish market
					Name = pimProduct.MasterModel,
					PimSku = pimProduct.SKU,
					PostingTemplateNo = _settings.VismaPostingTemplateNo,
					PriceCalcMethodsNo = _settings.VismaPriceCalcMethodsNo,
					StockProfileNo = _settings.VismaStockProfileNo
				};

				foreach (var market in pimProduct.Markets)
				{
					switch (market.Market)
					{
						case "4Sound.dk":
							article.ShortDescriptionDen = market.Description;
							break;
						case "4Sound.no":
							article.ShortDescriptionNor = market.Description;
							break;
						case "4Sound.se":
							// override name
							article.Name = market.DisplayName;
							article.ShortDescriptionSwe = market.Description;
							break;
					}
				}

				list.Add(article);
			}

			return list;
		}

		public PimMessageResult MapMessageResultToPimMessageResult(MessageResult msg)
		{
			return new PimMessageResult
			{
				MessageId = msg.MessageId,
				PrimaryAction = msg.PrimaryAction,
				SecondaryAction = msg.SecondaryAction,
				EnqueuedAt = msg.EnqueuedAt,
				DequeuedAt = msg.DequeuedAt,
				NumberOfFailedAttemptsToDequeue = msg.NumberOfFailedAttemptsToDequeue,
				Status = msg.Status,
				ErrorDetails = msg.ErrorDetails
			};
		}
	}
}