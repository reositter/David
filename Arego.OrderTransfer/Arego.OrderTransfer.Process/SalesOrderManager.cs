using System;
using Arego.OrderTransfer.Process.Dto;
using RG_SRVLib.Interop;

namespace Arego.OrderTransfer.Process
{
    public class SalesOrderManager
    {
        private readonly GlobalServerComponent _vgConnection;
	    private readonly CustomerQuery _customerQuery;
	    private readonly PostageCalculationParameters _postageCalculationParameters;

        public SalesOrderManager(GlobalServerComponent vgConnection, CustomerQuery customerQuery, PostageCalculationParameters postageCalculationParameters)
        {
	        _vgConnection = vgConnection;
	        _customerQuery = customerQuery;
			_postageCalculationParameters = postageCalculationParameters;
        }

	    public string CreateSalesOrderFromInvoice(TransferItem transferItem)
	    {
			var salesOrderComp = _vgConnection.GetSalesOrderServerComponent();
			salesOrderComp.bcSetDefaultValuesActive(1);

		    salesOrderComp.bcInitNewOrder(string.Empty);
			transferItem.OrderNoInDestinationClient = salesOrderComp.bcGetStr((int)CustomerOrder_Properties.COR_OrderNo);

			salesOrderComp.bcUpdateInt((int)CustomerOrder_Properties.COR_CustomerNo, transferItem.CustomerNo);
		    salesOrderComp.bcUpdateStr((int) CustomerOrder_Properties.COR_CustomerPurchaseNo, transferItem.OrderNo);
		    if (!string.IsNullOrWhiteSpace(transferItem.CustomerContactNameForInvoice))
			    salesOrderComp.bcUpdateStr((int) CustomerOrder_Properties.COR_NameContactNoInvoice, transferItem.CustomerContactNameForInvoice);

		    var allLinesCreated = true;
			int errCode;

		    foreach (var line in transferItem.Lines)
		    {
			    var lineIsValid = true;
				var artCode = salesOrderComp.bcUpdateStr((int)CustomerOrderLine_Properties.COL_ArticleNo, line.ArticleNo);
			    salesOrderComp.bcUpdateStr((int) CustomerOrderLine_Properties.COL_Name, line.ArticleName);
				var priceCode = salesOrderComp.bcUpdateDouble((int)CustomerOrderLine_Properties.COL_NetPrice, (double)line.Price);

				if (line.DiscountInPercent1 > 0)
					salesOrderComp.bcUpdateDouble((int)CustomerOrderLine_Properties.COL_DiscountI, (double)line.DiscountInPercent1);

				var qtyCode = salesOrderComp.bcUpdateDouble((int)CustomerOrderLine_Properties.COL_Quantity, (double)line.Quantity);

				// Setting DiscountII after Quantity because that's how it's done by default in VG.
				if (line.DiscountInPercent2 > 0)
					salesOrderComp.bcUpdateDouble((int)CustomerOrderLine_Properties.COL_DiscountII, (double)line.DiscountInPercent2);

				#region Validera raden.
				if (artCode > 0 && artCode != 258 && artCode != 30202)
				{
					// 258 får man vid registrering av artikelstrukturer.
					// 30202 = Utgår. Ur hjälptexten: 'Du får registrera order på en artikel med status "Utgår" så länge som den disponibla behållningen är större än 0'
					lineIsValid = false;
					LogFileWriter.WriteLine(string.Format("ArticleNo '{0}' caused exception({1}): {2}", line.ArticleNo, artCode, salesOrderComp.bcGetMessageText(artCode)));
				}
				else if (priceCode > 0)
				{
					lineIsValid = false;
					LogFileWriter.WriteLine(string.Format("NetPrice '{0}' caused exception({1}): {2}", line.Price, priceCode, salesOrderComp.bcGetMessageText(priceCode)));
				}
				else if (qtyCode > 0 && qtyCode != 11944) // 11944 får man vid registrering av artikelstrukturer.
				{
					lineIsValid = false;
					LogFileWriter.WriteLine(string.Format("Quantity '{0}' caused exception({1}): {2}", line.Quantity, qtyCode, salesOrderComp.bcGetMessageText(qtyCode)));
				}
				#endregion

				#region Skapa felrapport eller spara raden.
				if (lineIsValid)
				{
					errCode = salesOrderComp.bcChangeLine(salesOrderComp.bcGetNoOfLines() + 1);

					if (errCode > 0)
					{
						LogFileWriter.WriteLine(string.Format("bcChangeLine caused exception: {0}", salesOrderComp.bcGetMessageText(errCode)));
						allLinesCreated = false;
						break;
					}
				}
				else
				{
					allLinesCreated = false;
					salesOrderComp.bcDeleteLine(salesOrderComp.bcGetNoOfLines());
				}
				#endregion
		    }

			#region Kontrollera att alla orderrader skapats.
			if (allLinesCreated)
			{
				if (_customerQuery.CustomerShouldPayPostage(transferItem.CustomerNo))
				{
					var orderValue = (decimal)salesOrderComp.bcGetDouble((int) CustomerOrder_Properties.COR_TotalAmount);
					orderValue = orderValue - CalculateValueOfLinesThatShouldBeExcluded(transferItem);
 
					// Due to rounding (errors) order value can actually be less than zero if all lines ar excluded.
					orderValue = Math.Max(orderValue, 0);

					var postage = Math.Round((double)orderValue * (_postageCalculationParameters.PostagePercentage / 100));
					salesOrderComp.bcUpdateDouble((int)CustomerOrder_Properties.COR_Postage, postage);
				}

				errCode = salesOrderComp.bcSaveAndFinish();

				if (errCode > 0)
					LogFileWriter.WriteLine("bcSaveAndFinish failed with message: " + salesOrderComp.bcGetMessageText(errCode));
			}
			else
			{
				LogFileWriter.WriteLine(string.Format("At least one orderline failed. Calling CancelAndFinish for invoice {0}.", transferItem.InvoiceNo));

				salesOrderComp.bcCancelAndFinish();
				transferItem.OrderNoInDestinationClient = string.Empty;
			}
			#endregion

		    return transferItem.OrderNoInDestinationClient;
	    }

	    private decimal CalculateValueOfLinesThatShouldBeExcluded(TransferItem transferItem)
	    {
		    var value = 0m;

		    foreach (var line in transferItem.Lines)
		    {
			    if (OrderlineShouldBeExcludedWhenCalculatingPostage(line))
				    value += line.GetTotalValue();
		    }
		    return value;
	    }

	    private bool OrderlineShouldBeExcludedWhenCalculatingPostage(TransferItemLine line)
	    {
		    if (line.StockProfileNo == _postageCalculationParameters.ExcludeLinesWithStockProfileNo)
			    return true;

		    return _postageCalculationParameters.ExcludeLinesWithWareHouseNo.Contains(line.WareHouseNo);
	    }
    }
}