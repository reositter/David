using PimIntegration.Exceptions;
using PimIntegration.Tasks.VismaGlobal.Interfaces;
using RG_SRVLib.Interop;

namespace PimIntegration.Tasks.VismaGlobal
{
	public class CustomerAgreementQuery : VismaConnection, ICustomerAgreementQuery
	{
		private ArticleServerComponent _articleServerComponent;
		private BusinessComponentNavigate _customerComponent;
		private string _articleNoColumnName;

		public void Initialize()
		{
			_articleServerComponent = (ArticleServerComponent)Connection.bcBusinessComponent[(int)GLOBAL_Components.BC_Article];
			_articleServerComponent.bcEstablishData();
			_articleServerComponent.bcBindData();

			_articleNoColumnName = _articleServerComponent.bcGetTableObjectName((int)Article_Properties.ART_ArticleNo);

			_customerComponent = Connection.GetBusinessComponent(GLOBAL_Components.BC_Customer);
		}

		public decimal GetPrice(int customerNo, string articleNo)
		{
			var customerDiscountSettings = GetCustomerDiscountSettings(customerNo);
			_articleServerComponent.bcCalcAgreedPricesForCustomer(
				customerNo, 
				customerDiscountSettings.ChainNo,
				customerDiscountSettings.CustomerGroupNo,
				customerDiscountSettings.PriceListNo);

			var where = string.Format("{0} = '{1}'", _articleNoColumnName, articleNo);

			_articleServerComponent.bcSetFilterRequeryStr(where);
			var fetchCode = _articleServerComponent.bcFetchFirst(0);

			if (fetchCode != 0)
			{
				throw new PimIntegrationVismaObjectNotFoundException(string.Format("ArticleNo = {0} Code = {1}", articleNo, fetchCode));
			}

			// (decimal)_articleServerComponent.bcGetDouble((int)Article_Properties.ART_Price1);
			return (decimal)_articleServerComponent.bcGetDouble((int)Static_Properties.IDST_AgreedPrice);
		}

		private CustomerDiscountSettings GetCustomerDiscountSettings(int customerNo)
		{
			// TODO: Clear component
			_customerComponent.bcSetInt((int)Customer_Properties.CUS_CustomerNo, customerNo);
			var fetchCode = _customerComponent.bcFetchEqual();

			if (fetchCode != 0)
				throw new PimIntegrationVismaObjectNotFoundException(string.Format("CustomerNo = {0} Code = {1}", customerNo, fetchCode));

			return new CustomerDiscountSettings
			{
				ChainNo = _customerComponent.bcGetInt((int)Customer_Properties.CUS_ChainNo),
				PriceListNo = _customerComponent.bcGetInt((int)Customer_Properties.CUS_PriceListNo),
				CustomerGroupNo = _customerComponent.bcGetInt((int)Customer_Properties.CUS_CustomerGrpNo)
			};
		}

		public void Dispose()
		{
			if (_articleServerComponent != null)
				System.Runtime.InteropServices.Marshal.ReleaseComObject(_articleServerComponent);

			if (_articleServerComponent != null)
				System.Runtime.InteropServices.Marshal.ReleaseComObject(_customerComponent);
		}
	}

	internal class CustomerDiscountSettings
	{
		public int ChainNo { get; set; }
		public int PriceListNo { get; set; }
		public int CustomerGroupNo { get; set; }
	}
}