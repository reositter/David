using RG_SRVLib.Interop;

namespace Arego.OrderTransfer.Process
{
	public class CustomerQuery
	{
		private readonly GlobalServerComponent _vgConnection;
		private readonly BusinessComponentNavigate _customerComponent;
		private readonly string _colCustomerNo;

		public CustomerQuery(GlobalServerComponent vgConnection)
		{
			_vgConnection = vgConnection;
			_customerComponent = _vgConnection.GetBusinessComponent(GLOBAL_Components.BC_Customer);
			_colCustomerNo = _customerComponent.bcGetTableObjectName((int)Customer_Properties.CUS_CustomerNo);
		}

		public bool CustomerExists(int customerNo)
		{
			_customerComponent.bcSetFilterRequeryStr(string.Format("{0} = {1}", _colCustomerNo, customerNo));
			_customerComponent.bcFetchFirst(0);

			return _customerComponent.bcGetNoOfRecords() > 0;
		}
	}
}