using Arego.OrderTransfer.Process.Dto;
using NUnit.Framework;

namespace Arego.OrderTransfer.Tests
{
	[TestFixture]
	public class PostageCalculationTests
	{
		[Test]
		public void Should_return_correct_total_value_when_transfer_item_line_has_no_discount()
		{
			// Arrange
			var line = new TransferItemLine
			{
				Price = 1000,
				Quantity = 3
			};
			const decimal expected = 3000;

			// Act
			var actual = line.GetTotalValue();

			// Assert
			Assert.That(actual, Is.EqualTo(expected));
		}

		[Test]
		public void Should_return_correct_total_value_when_transfer_item_line_has_discount1()
		{
			// Arrange
			var line = new TransferItemLine
			{
				Price = 1000,
				Quantity = 3,
				DiscountInPercent1 = 10.0m
			};
			const decimal expected = 2700;

			// Act
			var actual = line.GetTotalValue();

			// Assert
			Assert.That(actual, Is.EqualTo(expected));
		}

		[Test]
		public void Should_return_correct_total_value_when_transfer_item_line_has_discount2()
		{
			// Arrange
			var line = new TransferItemLine
			{
				Price = 1000,
				Quantity = 3,
				DiscountInPercent2 = 10.0m
			};
			const decimal expected = 2700;

			// Act
			var actual = line.GetTotalValue();

			// Assert
			Assert.That(actual, Is.EqualTo(expected));
		}

		[Test]
		public void Should_return_correct_total_value_when_transfer_item_line_has_both_discount1_and_discount2()
		{
			// Arrange
			var line = new TransferItemLine
			{
				Price = 1000,
				Quantity = 3,
				DiscountInPercent1 = 10.0m,
				DiscountInPercent2 = 10.0m
			};

			// 
			const decimal expected = 2430;

			// Act
			var actual = line.GetTotalValue();

			// Assert
			Assert.That(actual, Is.EqualTo(expected));
		}
	}
}