using NUnit.Framework;
using Arego.OrderTransfer.Process;

namespace Arego.OrderTransfer.Tests
{
	[TestFixture]
    public class ExtensionMethodsTests
    {
		[Test]
		public void Should_()
		{
			// Arrange
			const string s = "2,8,16";

			// Act
			var ints = s.ToListOfIntegers();

			// Assert
			Assert.That(ints.Count, Is.EqualTo(3));
			Assert.That(ints[0], Is.EqualTo(2));
		}
    }
}
