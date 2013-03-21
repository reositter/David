using RG_SRVLib.Interop;

namespace Arego.OrderTransfer.Process
{
	public class InvoiceManager
	{
		private readonly GlobalServerComponent _vgConnection;
		private BusinessComponentNavigate _invoiceComp;
		private string _colInvoiceNo;

		public InvoiceManager(GlobalServerComponent vgConnection)
		{
			_vgConnection = vgConnection;
			_invoiceComp = vgConnection.GetBusinessComponent(GLOBAL_Components.BC_CustomerOrderCopy);
			_colInvoiceNo = _invoiceComp.bcGetTableObjectName((int)CustomerOrderCopy_Properties.COP_InvoiceNo);
		}

		public void MarkInvoiceAsTransferred(int invoiceNo)
		{
			_invoiceComp.bcSetFilterRequeryStr(string.Format("{0} = {1}", _colInvoiceNo, invoiceNo));
			var errCode = _invoiceComp.bcFetchFirst(1);
			errCode = _invoiceComp.bcUpdateInt((int)CustomerOrderCopy_Properties.COP_LoanReturnNo, 1);
			errCode = _invoiceComp.bcSaveRecord();
		}
	}
}