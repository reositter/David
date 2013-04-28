using NUnit.Framework;
using PimIntegration.Tasks.Setup;

namespace PimIntegration.Test.IntegrationTests
{
	[TestFixture]
	public class PimIntegrationSetupTests
	{
		[Test]
		[Ignore]
		public void Should_ensure_that_tables_exists()
		{
			// Arrange
			var settings = new AppSettings
			{
				DbConnectionString = @"Data Source=C:\4 Uppdrag\Arego\dev\PimIntegration\PimIntegration.Tasks\Database\PimIntegrationDb.s3db"
			};

			// Act
			PimIntegrationSetup.BootstrapEverything(settings);

			// Assert

		}
	}
}
