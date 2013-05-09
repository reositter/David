using System.Collections.Generic;
using NUnit.Framework;
using PimIntegration.Tasks.Database.Dto;
using PimIntegration.Tasks.VismaGlobal;
using PimIntegration.Tasks.VismaGlobal.Dto;

namespace PimIntegration.Test.IntegrationTests
{
	[TestFixture]
	public class VismaGlobalTests
	{
		[Test]
		public void Should_connect_to_visma()
		{
			// Arrange

			// Act
			var loginCode = VismaConnection.Open("Luthman AB", string.Empty, string.Empty, "59618988851856124");

			// Assert
			Assert.That(loginCode, Is.EqualTo(0));
		}

		[Test]
		public void Should_get_agreed_prices_from_visma_for_different_customers()
		{
			// Arrange
			VismaConnection.Open("Luthman AB", string.Empty, string.Empty, "59618988851856124");
			var query = new CustomerAgreementQuery();
			const string articleNo = "100";

			// Act
			var agreedPriceForSwe = query.GetPrice(11801, articleNo);
			var agreedPriceForNor = query.GetPrice(10648, articleNo);
			var agreedPriceForDen = query.GetPrice(10048, articleNo);
			
			// Assert
			Assert.That(agreedPriceForSwe, Is.Not.Null);
			Assert.That(agreedPriceForNor, Is.Not.Null);
			Assert.That(agreedPriceForDen, Is.Not.Null);
			Assert.That(agreedPriceForSwe, Is.Not.EqualTo(agreedPriceForNor));
		}

		[Test]
		public void Should_get_prices_of_all_articles_for_price_updates()
		{
			// Arrange
			VismaConnection.Open("Luthman AB", string.Empty, string.Empty, "59618988851856124");
			var query = new CustomerAgreementQuery();
			var articlesForPriceUpdate = new List<ArticleForPriceUpdate>
			{
				new ArticleForPriceUpdate("181", string.Empty),
				new ArticleForPriceUpdate("110", string.Empty)
			};

			// Act
			query.PopulateNewPrice(10001, articlesForPriceUpdate);

			// Assert
			Assert.That(articlesForPriceUpdate[0].NewPrice, Is.Not.EqualTo(0));
			Assert.That(articlesForPriceUpdate[1].NewPrice, Is.Not.EqualTo(0));
		}

		[Test]
		public void Should_create_new_article_that_gets_an_automatically_assigned_article_no()
		{
			// Arrange
			VismaConnection.Open("Luthman AB", string.Empty, string.Empty, "59618988851856124");
			ZUsrFields.Initialize(VismaConnection.Connection);
			var articleManager = new ArticleManager();

			var articles = new List<ArticleForCreate>
			{
				new ArticleForCreate { Name = "PIM Integration Test", PimSku = "PIM48", PostingTemplateNo = 1, PriceCalcMethodsNo = 1, StockProfileNo = 1},
				new ArticleForCreate { Name = "PIM Integration Test", PimSku = "PIM49", PostingTemplateNo = 1, PriceCalcMethodsNo = 1, StockProfileNo = 1}
			};

			// Act
			var createdArticles = articleManager.CreateArticles(articles);

			// Assert
			Assert.That(createdArticles.Count, Is.EqualTo(2));
		}
	}
}