﻿using System;
using System.Runtime.CompilerServices;
using NUnit.Framework;
using PimIntegration.Tasks;
using PimIntegration.Tasks.Database.Interfaces;
using PimIntegration.Tasks.Setup;
using PimIntegration.Tasks.VismaGlobal;

namespace PimIntegration.Test.IntegrationTests
{
	[TestFixture]
	public class PimIntegrationSetupTests : TestBase
	{
		private AppSettings _settings;

		[SetUp]
		public void SetUp()
		{
			_settings = GetSettingsFromAppConfigForUnitTests();
		}

		[Test]
		public void Should_have_valid_ioc_configuration()
		{
			// Arrange
			// Act
			var container = PimIntegrationSetup.BootstrapEverything(_settings);

			// Assert
			container.AssertConfigurationIsValid();
		}

		[Test]
		public void Should_be_able_to_resolve_get_new_products_task()
		{
			// Arrange
			var container = PimIntegrationSetup.BootstrapEverything(_settings);

			// Act
			var task = container.GetInstance<IGetNewProductsTask>();

			// Assert
			Assert.That(task, Is.Not.Null);
		}

		[Test]
		public void Should_be_able_to_connect_to_visma_db()
		{
			// Arrange
			var container = PimIntegrationSetup.BootstrapEverything(_settings);

			// Act
			var query = container.GetInstance<IStockBalanceQuery>();

			// Assert
			Assert.That(query, Is.Not.Null);
		}

		[Test]
		public void Should_be_able_to_query_for_stock_balance_updates()
		{
			// Arrange
			var container = PimIntegrationSetup.BootstrapEverything(_settings);
			var query = container.GetInstance<IStockBalanceQuery>();

			// Act
			var result = query.GetStockBalanceUpdatesSince(DateTime.Now.AddHours(-1));

			// Assert
			Assert.That(result, Is.Not.Null);
		}

		[Test]
		public void Should_always_use_same_instance_of_IVismaConnection()
		{
			// Arrange
			var container = PimIntegrationSetup.BootstrapEverything(_settings);

			// Act
			var conn1 = container.GetInstance<IVismaConnection>();
			var conn2 = container.GetInstance<IVismaConnection>();

			// Assert
			Assert.That(RuntimeHelpers.GetHashCode(conn1), Is.EqualTo(RuntimeHelpers.GetHashCode(conn2)));
		}
	}
}
