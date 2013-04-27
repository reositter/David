using System;

namespace PimIntegration.Exceptions
{
	public class PimIntegrationDbException : ApplicationException
	{
		public PimIntegrationDbException() { }

		public PimIntegrationDbException(string message) : base(message)  { }
	}
}