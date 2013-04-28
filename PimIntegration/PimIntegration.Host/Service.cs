using System;
using System.Threading;
using PimIntegration.Tasks;
using PimIntegration.Tasks.Setup;
using StructureMap;
using log4net.Config;
using Timer = System.Threading.Timer;

namespace PimIntegration.Host
{
	public class Service
	{
		private Timer _getNewProductsTimer;
		private Timer _publishProductUpdatesTimer;
		private IContainer _container;

		public Service()
		{
			XmlConfigurator.Configure();
		}

		public void Start()
	    {
			Log.ForCurrent.Info("Starting PIM Integration Service");

			_container = PimIntegrationSetup.BootstrapEverything(PimIntegrationSettings.AppSettings);

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
			Log.ForCurrent.Debug("Getting new products.");

			try
			{
				_container.GetInstance<IGetNewProductsTask>().Execute();
			}
			catch (Exception ex)
			{
				Log.ForCurrent.Error(ex.Message);
				Log.ForCurrent.Error(ex.StackTrace);
				throw;
			}

			_getNewProductsTimer.Change(PimIntegrationSettings.IntervalInSecondsForGetNewProducts, Timeout.Infinite);
		}

		private void PublishProductUpdates(Object state)
		{
			Log.ForCurrent.Debug("Publishing product updates.");

			try
			{
				_container.GetInstance<IPublishStockBalanceUpdatesTask>().Execute();
			}
			catch (Exception ex)
			{
				Log.ForCurrent.Error(ex.Message);
				Log.ForCurrent.Error(ex.StackTrace);
				throw;
			}

			_publishProductUpdatesTimer.Change(PimIntegrationSettings.IntervalInSecondsForPublishProductUpdates, Timeout.Infinite);
		}
	}
}
