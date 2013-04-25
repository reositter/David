using System;
using System.Threading;
using PimIntegration.Service;
using log4net.Config;
using Timer = System.Threading.Timer;

namespace PimIntegration.Host
{
	public class Service
	{
		private Timer _getNewProductsTimer;
		private Timer _publishProductUpdatesTimer;

		public Service()
		{
			XmlConfigurator.Configure();
	    }

	    public void Start()
	    {
			Log.ForCurrent.Info("Starting PIM Integration Service");
			_getNewProductsTimer = new Timer(GetNewProducts, null, Timeout.Infinite, Timeout.Infinite);
		    _getNewProductsTimer.Change(0, Timeout.Infinite);

			_publishProductUpdatesTimer = new Timer(PublishProductUpdates, null, Timeout.Infinite, Timeout.Infinite);
			_publishProductUpdatesTimer.Change(0, Timeout.Infinite);
	    }

	    public void Stop()
	    {
			Log.ForCurrent.Info("Stopping PIM Integration Service");

			_getNewProductsTimer.Dispose();
		    _publishProductUpdatesTimer.Dispose();
	    }

		private void GetNewProducts(Object state)
		{
			Log.ForCurrent.Info(string.Format("Getting new products. {0}", DateTime.Now));

			_getNewProductsTimer.Change(AppSettings.IntervalInSecondsForGetNewProducts, Timeout.Infinite);
		}

		private void PublishProductUpdates(Object state)
		{
			Log.ForCurrent.Info(string.Format("Publishing product updates. {0}", DateTime.Now));

			_publishProductUpdatesTimer.Change(AppSettings.IntervalInSecondsForPublishProductUpdates, Timeout.Infinite);
		}
	}
}
