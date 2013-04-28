using NUnit.Framework;
using PimIntegration.Tasks.VismaGlobal;

namespace PimIntegration.Test.IntegrationTests
{
	[TestFixture]
	public class VismaGlobalTests
	{
		[SetUp]
		public void SetUp()
		{

		}

		[Test]
		public void Should_connect_to_visma()
		{
			// Arrange

			// Act
			var loginCode = VismaConnection.Open("Luthman AB", string.Empty, string.Empty, "59618988851856124");

			// Assert
			Assert.That(loginCode, Is.EqualTo(0));
		}
	}
}