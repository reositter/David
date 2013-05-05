using System;
using System.Threading;
using Nancy.Hosting.Self;
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
		private Timer _publishStockBalanceUpdatesTimer;
		private Timer _publishPriceUpdatesTimer;
		private IContainer _container;

		private NancyHost _nancyHost;

		public Service()
		{
			XmlConfigurator.Configure();

		}

		public void Start()
	    {
			if(_nancyHost == null)
				_nancyHost = new NancyHost(new Uri("http://localhost:4000"));

			Log.ForCurrent.Info("Starting Nancy UI host");
			_nancyHost.Start();

			Log.ForCurrent.Info("Starting PIM Integration Service");

			_container = PimIntegrationSetup.BootstrapEverything(PimIntegrationSettings.AppSettings);

			_getNewProductsTimer = new Timer(GetNewProducts, null, Timeout.Infinite, Timeout.Infinite);
		    //_getNewProductsTimer.Change(0, Timeout.Infinite);

			_publishStockBalanceUpdatesTimer = new Timer(PublishStockBalanceUpdates, null, Timeout.Infinite, Timeout.Infinite);
			//_publishStockBalanceUpdatesTimer.Change(0, Timeout.Infinite);

			_publishPriceUpdatesTimer = new Timer(PublishPriceUpdates, null, Timeout.Infinite, Timeout.Infinite);
			//_publishPriceUpdatesTimer.Change(0, Timeout.Infinite);
	    }

	    public void Stop()
	    {
			Log.ForCurrent.Info("Stopping Nancy UI host");
			_nancyHost.Stop();

			Log.ForCurrent.Info("Stopping PIM Integration Service");

			_getNewProductsTimer.Dispose();
		    _publishStockBalanceUpdatesTimer.Dispose();
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

		private void PublishStockBalanceUpdates(Object state)
		{
			try
			{
				Log.ForCurrent.Debug("Starting Publishing Stock Balance Updates Task.");
				_container.GetInstance<IPublishStockBalanceUpdatesTask>().Execute();
				Log.ForCurrent.Debug("Finished Publishing Stock Balance Updates Task.");

			}
			catch (Exception ex)
			{
				Log.ForCurrent.Error(ex.Message);
				Log.ForCurrent.Error(ex.StackTrace);
				throw;
			}

			_publishStockBalanceUpdatesTimer.Change(PimIntegrationSettings.IntervalInSecondsForPublishStockBalanceUpdates, Timeout.Infinite);
		}

		private void PublishPriceUpdates(Object state)
		{
			try
			{
				Log.ForCurrent.Debug("Starting Publishing Price Updates Task.");
				_container.GetInstance<IPublishPriceUpdatesTask>().Execute();
				Log.ForCurrent.Debug("Finished Publishing Price Updates Task.");

			}
			catch (Exception ex)
			{
				Log.ForCurrent.Error(ex.Message);
				Log.ForCurrent.Error(ex.StackTrace);
				throw;
			}

			_publishStockBalanceUpdatesTimer.Change(PimIntegrationSettings.IntervalInSecondsForPublishPriceUpdates, Timeout.Infinite);
		}
	}
}
