using NUnit.Framework;
using PimIntegration.Tasks.Setup;
using StructureMap;

namespace UnitTest.IntegrationTests
{
	[TestFixture]
	public class PimIntegrationSetupTests
	{
		[Test]
		[Ignore]
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
