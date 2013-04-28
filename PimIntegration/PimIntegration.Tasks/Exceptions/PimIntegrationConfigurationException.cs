using System;

namespace PimIntegration.Exceptions
{
	public class PimIntegrationConfigurationException : ApplicationException
	{
		 public PimIntegrationConfigurationException() { }

		 public PimIntegrationConfigurationException(string message) : base(message) { }
	}
}