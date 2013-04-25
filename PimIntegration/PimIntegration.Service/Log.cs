using System;
using System.Diagnostics;
using log4net;

namespace PimIntegration.Tasks
{
	public class Log
	{
		public static ILog For(Type type)
		{
			return LogManager.GetLogger(type);
		}

		public static ILog For(object itemThatRequiresLoggingServices)
		{
			return LogManager.GetLogger(itemThatRequiresLoggingServices.GetType());
		}

		public static ILog ForCurrent
		{
			get
			{
				return LogManager.GetLogger(new StackFrame(1).GetMethod().ReflectedType);
			}
		}
	}
}
