using System;

namespace PimIntegration.Exceptions
{
	public class PimIntegrationVismaObjectNotFoundException : ApplicationException
	{
		public PimIntegrationVismaObjectNotFoundException(){
			
		}

		public PimIntegrationVismaObjectNotFoundException(string message) : base(message) { }
	}
}