using System;

namespace PimIntegration.Tasks.VismaGlobal.Interfaces
{
	public interface ICustomerAgreementQuery : IDisposable
	{
		void Initialize();
		decimal GetPrice(int customerNo, string articleNo);
	}
}