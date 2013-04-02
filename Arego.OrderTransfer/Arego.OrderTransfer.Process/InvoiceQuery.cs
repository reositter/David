using System;
using System.Collections.Generic;
using Arego.OrderTransfer.Process.Dto;
using RG_SRVLib.Interop;

namespace Arego.OrderTransfer.Process
{
    public class InvoiceQuery
    {
        private readonly GlobalServerComponent _vgConnection;
        private BusinessComponentNavigate _invoiceComp;
        private BusinessComponentNavigate _lineComp;
        private string _colClpInvoiceNo;

	    public InvoiceQuery(GlobalServerComponent vgConnection)
        {
            _vgConnection = vgConnection;
        }

        public IList<TransferItem> GetInvoices(int chainNo, int loanReturnNo)
        {
            InitializeComponents();

			var where = CreateFilter(chainNo, loanReturnNo);
			LogFileWriter.WriteLine(string.Format("Setting filter before fetching invoices: '{0}'", where));
            _invoiceComp.bcSetFilterRequeryStr(where);

	        var start = DateTime.Now;
            var errCode = _invoiceComp.bcFetchFirst(0);
			LogFileWriter.WriteLine(string.Format("Time spent fetching invoices: {0}", DateTime.Now.Subtract(start)));
			LogFileWriter.WriteLine(string.Format("bcGetNoOfRecords = {0}", _invoiceComp.bcGetNoOfRecords()));

            if (errCode != 0)
            {
                LogErrorMessage(errCode);
                return new List<TransferItem>();
            }

            var list = new List<TransferItem>();

            do
            {
                var item = new TransferItem
                    {
                        InvoiceNo = _invoiceComp.bcGetInt((int)CustomerOrderCopy_Properties.COP_InvoiceNo),
						OrderNo = _invoiceComp.bcGetStr((int)CustomerOrderCopy_Properties.COP_OrderCopyNo),
                        CustomerNo = _invoiceComp.bcGetInt((int)CustomerOrderCopy_Properties.COP_CustomerNo),
                        CustomerName = _invoiceComp.bcGetStr((int)CustomerOrderCopy_Properties.COP_DeliveryCustomerName),
						CustomerContactNameForInvoice = _invoiceComp.bcGetStr((int)CustomerOrderCopy_Properties.COP_NameContactNoInvoice)
                    };

	            start = DateTime.Now;
                item.Lines = GetInvoiceLines(item.InvoiceNo);
				LogFileWriter.WriteLine(string.Format("Time spent fetching lines for invoice {0}: {1}", item.InvoiceNo, DateTime.Now.Subtract(start)));

				if (item.Lines.Count > 0)
					list.Add(item);
            } while (_invoiceComp.bcFetchNext(0) == 0);

            return list;
        }

        private IList<TransferItemLine> GetInvoiceLines(int invoiceNo)
        {
            var list = new List<TransferItemLine>();

	        _lineComp.bcSetFilterRequeryStr(string.Format("{0} = {1}", _colClpInvoiceNo, invoiceNo));
	        _lineComp.bcFetchFirst(0);

	        do
	        {
		        var line = new TransferItemLine
			        {
						ArticleNo = _lineComp.bcGetStr((int) CustomerOrderLineCopy_Properties.CLP_ArticleNo),
						ArticleName = _lineComp.bcGetStr((int) CustomerOrderLineCopy_Properties.CLP_Name),
						Price = (decimal)_lineComp.bcGetDouble((int) CustomerOrderLineCopy_Properties.CLP_ExchangeSalesPrice),
						DiscountInPercent1 = (decimal)_lineComp.bcGetDouble((int) CustomerOrderLineCopy_Properties.CLP_DiscountI),
						DiscountInPercent2 = (decimal)_lineComp.bcGetDouble((int) CustomerOrderLineCopy_Properties.CLP_DiscountII),
						Quantity = (decimal)_lineComp.bcGetDouble((int)CustomerOrderLineCopy_Properties.CLP_Invoiced)
			        };

				if (line.Quantity != 0)
					list.Add(line);
	        } while (_lineComp.bcFetchNext(0) == 0);

	        return list;
        }

		private void InitializeComponents()
		{
			_invoiceComp = _vgConnection.GetBusinessComponent(GLOBAL_Components.BC_CustomerOrderCopy);
			_lineComp = _vgConnection.GetBusinessComponent(GLOBAL_Components.BC_CustomerOrderLineCopy);

			_colClpInvoiceNo = _lineComp.bcGetTableObjectName((int)CustomerOrderLineCopy_Properties.CLP_InvoiceNo);
		}

	    private string CreateFilter(int chainNo, int loanReturnNo)
	    {
			var colChainNo = _invoiceComp.bcGetTableObjectName((int)CustomerOrderCopy_Properties.COP_ChainNo);
			var colLoanReturnNo = _invoiceComp.bcGetTableObjectName((int)CustomerOrderCopy_Properties.COP_LoanReturnNo);
			var colInvoiceNo = _invoiceComp.bcGetTableObjectName((int)CustomerOrderCopy_Properties.COP_InvoiceNo);

			return string.Format("{0} = {1} AND NOT {2} = {3} AND {4} <> 0", colChainNo, chainNo, colLoanReturnNo, loanReturnNo, colInvoiceNo);
	    }

	    private void LogErrorMessage(int errCode)
        {
            if (errCode == 4)
                LogFileWriter.WriteLine("Did not find any invoices to transfer.");
            else if (errCode > 99)
                LogFileWriter.WriteLine(string.Format("Failed to get invoices. Code {0} - {1}", errCode, _invoiceComp.bcGetMessageText(errCode)));

        }
    }
}