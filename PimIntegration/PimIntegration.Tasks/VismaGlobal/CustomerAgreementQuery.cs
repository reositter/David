using System.Collections.Generic;
using PimIntegration.Exceptions;
using PimIntegration.Tasks.Database.Dto;
using PimIntegration.Tasks.VismaGlobal.Interfaces;
using RG_SRVLib.Interop;

namespace PimIntegration.Tasks.VismaGlobal
{
	public class CustomerAgreementQuery : VismaConnection, ICustomerAgreementQuery
	{
		private ArticleServerComponent _articleServerComponent;
		private BusinessComponentNavigate _customerComponent;
		private string _articleNoColumnName;

		public decimal GetPrice(int customerNo, string articleNo)
		{
			Initialize();
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

			var agreedPrice = (decimal)_articleServerComponent.bcGetDouble((int)Static_Properties.IDST_AgreedPrice);
			Dispose();

			return agreedPrice;
		}

		public void PopulateNewPrice(int customerNo, IList<ArticleForPriceUpdate> articlesForPriceUpdate)
		{
			Initialize();
			foreach (var article in articlesForPriceUpdate)
			{
				var customerDiscountSettings = GetCustomerDiscountSettings(customerNo);
				_articleServerComponent.bcCalcAgreedPricesForCustomer(
					customerNo,
					customerDiscountSettings.ChainNo,
					customerDiscountSettings.CustomerGroupNo,
					customerDiscountSettings.PriceListNo);

				var where = string.Format("{0} = '{1}'", _articleNoColumnName, article.ArticleNo);
				_articleServerComponent.bcSetFilterRequeryStr(where);
				var fetchCode = _articleServerComponent.bcFetchFirst(0);

				if (fetchCode != 0)
				{
					throw new PimIntegrationVismaObjectNotFoundException(string.Format("ArticleNo = {0} Code = {1}", article.ArticleNo, fetchCode));
				}

				// TODO: Get valutapris
				article.NewPrice = (decimal)_articleServerComponent.bcGetDouble((int)Static_Properties.IDST_AgreedPrice);
			}
			Dispose();
		}

		private void Dispose()
		{
			if (_articleServerComponent != null)
				System.Runtime.InteropServices.Marshal.ReleaseComObject(_articleServerComponent);

			if (_articleServerComponent != null)
				System.Runtime.InteropServices.Marshal.ReleaseComObject(_customerComponent);
		}

		private void Initialize()
		{
			_articleServerComponent = (ArticleServerComponent)Connection.bcBusinessComponent[(int)GLOBAL_Components.BC_Article];
			_articleServerComponent.bcEstablishData();
			_articleServerComponent.bcBindData();

			_articleNoColumnName = _articleServerComponent.bcGetTableObjectName((int)Article_Properties.ART_ArticleNo);

			_customerComponent = Connection.GetBusinessComponent(GLOBAL_Components.BC_Customer);
		}

		private CustomerDiscountSettings GetCustomerDiscountSettings(int customerNo)
		{
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
	}

	internal class CustomerDiscountSettings
	{
		public int ChainNo { get; set; }
		public int PriceListNo { get; set; }
		public int CustomerGroupNo { get; set; }
	}
}