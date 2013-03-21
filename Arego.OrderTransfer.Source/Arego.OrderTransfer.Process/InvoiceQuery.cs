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
        private string _colInvoiceNo;
        private string _colChainNo;
	    private string _colLoanReturnNo;
        private string _copInvoiceNo;
      

	    public InvoiceQuery(GlobalServerComponent vgConnection)
        {
            _vgConnection = vgConnection;
        }

        public IList<TransferItem> GetInvoices(int chainNo, int loanReturnNo)
        {
            InitializeComponents();

            var where = string.Format("{0} = {1} AND NOT {2} = {3} AND {4} > 0", _colChainNo, chainNo, _colLoanReturnNo, loanReturnNo, _copInvoiceNo);
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
                        CustomerNo = _invoiceComp.bcGetInt((int)CustomerOrderCopy_Properties.COP_CustomerNo),
                        //CustomerName = _invoiceComp.bcGetStr((int)CustomerOrderCopy_Properties.COP_CustomerName),
                        CustomerName = _invoiceComp.bcGetStr((int)CustomerOrderCopy_Properties.COP_DeliveryCustomerName),
                    };

	            start = DateTime.Now;
                item.Lines = GetInvoiceLines(item.InvoiceNo);
				LogFileWriter.WriteLine(string.Format("Time spent fetching lines for invoice {0}: {1}", item.InvoiceNo, DateTime.Now.Subtract(start)));

                list.Add(item);
            } while (_invoiceComp.bcFetchNext(0) == 0);

            return list;
        }

        private void InitializeComponents()
        {
            _invoiceComp = _vgConnection.GetBusinessComponent(GLOBAL_Components.BC_CustomerOrderCopy);
            _lineComp = _vgConnection.GetBusinessComponent(GLOBAL_Components.BC_CustomerOrderLineCopy);

            // Get column names
            _colChainNo = _invoiceComp.bcGetTableObjectName((int)CustomerOrderCopy_Properties.COP_ChainNo);
	        _colLoanReturnNo = _invoiceComp.bcGetTableObjectName((int) CustomerOrderCopy_Properties.COP_LoanReturnNo);
            _colInvoiceNo = _lineComp.bcGetTableObjectName((int)CustomerOrderLineCopy_Properties.CLP_InvoiceNo);
            _copInvoiceNo = _invoiceComp.bcGetTableObjectName((int)CustomerOrderCopy_Properties.COP_InvoiceNo);
        }

        private IList<TransferItemLine> GetInvoiceLines(int invoiceNo)
        {
            var list = new List<TransferItemLine>();

	        _lineComp.bcSetFilterRequeryStr(string.Format("{0} = {1}", _colInvoiceNo, invoiceNo));
	        _lineComp.bcFetchFirst(0);

	        do
	        {
		        var line = new TransferItemLine
			        {
						ArticleNo = _lineComp.bcGetStr((int) CustomerOrderLineCopy_Properties.CLP_ArticleNo),
						ArticleName = _lineComp.bcGetStr((int) CustomerOrderLineCopy_Properties.CLP_Name),
						NetPrice = (decimal)_lineComp.bcGetDouble((int) CustomerOrderLineCopy_Properties.CLP_NetPrice),
						DiscountInPercent = (decimal)_lineComp.bcGetDouble((int) CustomerOrderLineCopy_Properties.CLP_DiscountI),
						Quantity = (decimal)_lineComp.bcGetDouble((int)CustomerOrderLineCopy_Properties.CLP_Quantity)
			        };

				list.Add(line);
	        } while (_lineComp.bcFetchNext(0) == 0);

	        return list;
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