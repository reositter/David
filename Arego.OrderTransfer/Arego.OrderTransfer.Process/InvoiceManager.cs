using RG_SRVLib.Interop;

namespace Arego.OrderTransfer.Process
{
	public class InvoiceManager
	{
		private readonly BusinessComponentNavigate _invoiceComp;
		private readonly string _colInvoiceNo;

		public InvoiceManager(GlobalServerComponent vgConnection)
		{
			_invoiceComp = vgConnection.GetBusinessComponent(GLOBAL_Components.BC_CustomerOrderCopy);
			_colInvoiceNo = _invoiceComp.bcGetTableObjectName((int)CustomerOrderCopy_Properties.COP_InvoiceNo);
		}

		~InvoiceManager()
		{
			System.Runtime.InteropServices.Marshal.ReleaseComObject(_invoiceComp);
		}

		public void MarkInvoiceAsTransferred(int invoiceNo)
		{
			_invoiceComp.bcSetFilterRequeryStr(string.Format("{0} = {1}", _colInvoiceNo, invoiceNo));
			_invoiceComp.bcUpdateInt((int)CustomerOrderCopy_Properties.COP_LoanReturnNo, 1);
			_invoiceComp.bcSaveRecord();
		}
	}
}