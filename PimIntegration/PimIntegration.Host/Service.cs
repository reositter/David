using System;
using System.Configuration;
using log4net;
using log4net.Config;
using Timer = System.Threading.Timer;

namespace PimIntegration.Host
{
	public class Service
	{
		private Timer _getNewProductsTimer;
		private readonly ILog _log;
		private bool _getNewProductsInProgress;

		public Service()
		{
			_log = LogManager.GetLogger(this.GetType());
			XmlConfigurator.Configure();
	    }

	    public void Start()
	    {
			_log.Info("Reading App Settings...");
		    var settings = GetAppSettings();
			_log.Info(string.Format("ActionIntervalInSeconds set to '{0}'", settings.IntervalInSecondsForGetNewProducts));

			_getNewProductsTimer = new Timer(GetNewProducts, null, 0, settings.IntervalInSecondsForGetNewProducts);
	    }

	    public void Stop()
	    {
			_getNewProductsTimer.Dispose();
	    }

		public void GetNewProducts(Object state)
		{
			_log.Info(string.Format("It's {0} and everyting is dead calm.", DateTime.Now));

			if (!_getNewProductsInProgress)
			{
				_getNewProductsInProgress = true;
				_log.Info("Do Task");
				_getNewProductsInProgress = false;
			}
			else
				_log.Info("in progress");
		}

		private AppSettings GetAppSettings()
		{
			var settings = new AppSettings();
			
			settings.IntervalInSecondsForGetNewProducts = Convert.ToInt32(ConfigurationManager.AppSettings["ActionIntervalInSeconds"]) * 1000;

			return settings;
		}
	}
}
