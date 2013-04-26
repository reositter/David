using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using PimIntegration.Tasks.Setup;

namespace UnitTest
{
	[TestFixture]
	public class PimIntegrationSetupTests
	{
		[Test]
		public void Should_ensure_that_tables_exists()
		{
			// Arrange
			var settings = new TaskSettings
			{
				DbConnectionString = @"Data Source=C:\4 Uppdrag\Arego\dev\PimIntegration\PimIntegration.Tasks\Database\PimIntegrationDb.s3db"
			};

			// Act
			PimIntegrationSetup.InitializeEverything(settings);

			// Assert

		}
	}
}
