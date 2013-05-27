using System;
using System.Linq;
using PimIntegration.Tasks.Database.Dto;
using PimIntegration.Tasks.Database.Interfaces;
using PimIntegration.Tasks.PimApi.Interfaces;
using PimIntegration.Tasks.Setup;
using PimIntegration.Tasks.VismaGlobal.Dto;
using PimIntegration.Tasks.VismaGlobal.Interfaces;

namespace PimIntegration.Tasks
{
	public interface IPublishPriceUpdatesTask
	{
		void Execute();
	}

	public class PublishPriceUpdatesTask : IPublishPriceUpdatesTask
	{
		private readonly ITaskSettings _settings;
		private readonly ILastCallsRepository _lastCallsRepository;
		private readonly IPriceUpdateQuery _priceUpdateQuery;
		private readonly ICustomerAgreementQuery _customerAgreementQuery;
		private readonly IPimCommandService _pimCommandService;
		private DateTime _timeOfLastQueryForPriceUpdates;

		public PublishPriceUpdatesTask(
			ITaskSettings settings,
			ILastCallsRepository lastCallsRepository, 
			IPriceUpdateQuery priceUpdateQuery,
			ICustomerAgreementQuery customerAgreementQuery,
			IPimCommandService pimCommandService)
		{
			_settings = settings;
			_lastCallsRepository = lastCallsRepository;
			_priceUpdateQuery = priceUpdateQuery;
			_pimCommandService = pimCommandService;
			_customerAgreementQuery = customerAgreementQuery;

			_timeOfLastQueryForPriceUpdates = lastCallsRepository.GetTimeOfLastQueryForPriceUpdates();
		}

		public void Execute()
		{
			var timeOfThisQuery = DateTime.Now;
			var articlesForPriceUpdate = _priceUpdateQuery.GetArticlesForPriceUpdate(_timeOfLastQueryForPriceUpdates);

			foreach (var article in articlesForPriceUpdate)
			{
				switch (article.Market)
				{
					case "DK":
						var dkmarket = _settings.Markets.First(m => m.MarketKey == "4sound.dk");
						article.NewPrice = _customerAgreementQuery.GetPrice(dkmarket.VendorId, article.ArticleNo);
						_pimCommandService.PublishPriceUpdate(dkmarket.MarketKey, MapToArticleForPriceAndStockUpdate(article));
						break;
					case "NO":
						var nomarket = _settings.Markets.First(m => m.MarketKey == "4sound.no");
						article.NewPrice = _customerAgreementQuery.GetPrice(nomarket.VendorId, article.ArticleNo);
						_pimCommandService.PublishPriceUpdate(nomarket.MarketKey, MapToArticleForPriceAndStockUpdate(article));
						break;
					case "SE":
						var semarket = _settings.Markets.First(m => m.MarketKey == "4sound.se");
						article.NewPrice = _customerAgreementQuery.GetPrice(semarket.VendorId, article.ArticleNo);
						_pimCommandService.PublishPriceUpdate(semarket.MarketKey, MapToArticleForPriceAndStockUpdate(article));
						break;
				}
			}

			_lastCallsRepository.UpdateTimeOfLastQueryForPriceUpdates(timeOfThisQuery);
			_timeOfLastQueryForPriceUpdates = timeOfThisQuery;			
		}

		private static ArticleForPriceAndStockUpdate MapToArticleForPriceAndStockUpdate(ArticleForPriceUpdate source)
		{
			return  new ArticleForPriceAndStockUpdate
			{
				ArticleNo = source.ArticleNo,
				PimSku = source.PimSku,
				Price = source.NewPrice
			};
		}
	}
}