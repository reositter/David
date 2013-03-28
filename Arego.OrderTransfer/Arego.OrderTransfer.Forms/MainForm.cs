using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Windows.Forms;
using Arego.OrderTransfer.Process;
using Arego.OrderTransfer.Process.Dto;

namespace Arego.OrderTransfer.Forms
{
    public partial class MainForm : Form
    {
        private readonly string _sourceClientName;
        private readonly string _destinationClientName;
	    private readonly string _bapiKey;
        private readonly int _chainNo;
	    private readonly int _loanReturnNo;
	    private readonly int _postingTemplate;
	    private readonly int _priceCalcMethodsNo;
	    private readonly int _stockProfileNo;

	    private int _attemptsToCreateArticle;
	    private int _failedAttemptsToCreateArticle;

	    private IList<TransferItem> _transferItems; 

	    public MainForm()
        {
            InitializeComponent();

            _sourceClientName = ConfigurationManager.AppSettings["SourceClientName"].Trim();
            _destinationClientName = ConfigurationManager.AppSettings["DestinationClientName"].Trim();
		    _bapiKey = ConfigurationManager.AppSettings["BapiKey"];
		    _chainNo = Convert.ToInt32(ConfigurationManager.AppSettings["ChainNo"].Trim());
		    _loanReturnNo = Convert.ToInt32(ConfigurationManager.AppSettings["LoanReturnNo"].Trim());

		    _postingTemplate = Convert.ToInt32(ConfigurationManager.AppSettings["PostingTemplateNo"].Trim());
		    _priceCalcMethodsNo = Convert.ToInt32(ConfigurationManager.AppSettings["PriceCalcMethodsNo"].Trim());
		    _stockProfileNo = Convert.ToInt32(ConfigurationManager.AppSettings["StockProfileNo"].Trim());

		    txtSourceClientName.Text = _sourceClientName;
		    txtDestinationClientName.Text = _destinationClientName;
		    txtBapiKey.Text = _bapiKey;
		    txtChainNo.Text = _chainNo.ToString(CultureInfo.InvariantCulture);

			ConnectToClients();
        }

	    private void ConnectToClients()
	    {
		    var sourceLoginCode = VgConnections.ConnectToSourceClient(_sourceClientName, _bapiKey);
			if (sourceLoginCode != 0)
				MessageBox.Show(string.Format("Login mot {0} misslyckades. {1}{2}", _sourceClientName, Environment.NewLine, VgConnections.TranslateLoginCodeToMessage(sourceLoginCode)));

			var destinationLoginCode = VgConnections.ConnectToDestinationClient(_destinationClientName, txtBapiKey.Text);
			if (destinationLoginCode != 0)
				MessageBox.Show(string.Format("Login mot {0} misslyckades. {1}{2}", _destinationClientName, Environment.NewLine, VgConnections.TranslateLoginCodeToMessage(destinationLoginCode)));

		    btnGetInvoicesFromSourceClient.Enabled = (sourceLoginCode + destinationLoginCode) == 0;

			btnGetInvoicesFromSourceClient.Text = string.Format("Last ned fakturaer fra {0}", VgConnections.GetNameOfSourceClient());
			btnTransferArticlesToDestination.Text = string.Format("Overfør artikkler til {0}", VgConnections.GetNameOfDestinationClient());
		    btnCreateCustomerOrders.Text = string.Format("Lag salgsordre i {0}", VgConnections.GetNameOfDestinationClient());
	    }

		private void btnGetInvoicesFromSourceClient_Click(object sender, EventArgs e)
		{
			Cursor = Cursors.WaitCursor;

			_transferItems = new InvoiceQuery(VgConnections.SourceConnection).GetInvoices(_chainNo, _loanReturnNo);
			lstLog.Items.Add(string.Format("{0} ordre lest fra {1}", _transferItems.Count, VgConnections.GetNameOfSourceClient()));

			DisplayTransferItems(_transferItems);
			
			btnGetInvoicesFromSourceClient.Enabled = _transferItems.Count == 0;
			btnTransferArticlesToDestination.Enabled = _transferItems.Count > 0;

			Cursor = Cursors.Default;
		}

	    private void DisplayTransferItems(IEnumerable<TransferItem> items)
	    {
			lstTransferItems.Items.Clear();

		    foreach (var item in items)
		    {
				var subitems = new[]
				    {
					    item.InvoiceNo.ToString(CultureInfo.InvariantCulture),
					    item.CustomerNo.ToString(CultureInfo.InvariantCulture),
					    item.CustomerName,
						item.OrderNoInDestinationClient
				    };

			    lstTransferItems.Items.Add(new ListViewItem(subitems) {Tag = item});
		    }
	    }

	    private void DisplayLines(IEnumerable<TransferItemLine> lines)
	    {
			lstLines.Items.Clear();

		    foreach (var line in lines)
		    {
			    var subitems = new[]
				    {
					    line.ArticleNo,
						line.ArticleName,
						line.Price.ToString("n2"),
						line.DiscountInPercent.ToString(CultureInfo.InvariantCulture) + " %",
						line.Quantity.ToString("n2")
				    };

			    lstLines.Items.Add(new ListViewItem(subitems));
		    }
	    }

	    private void lstTransferItems_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
	    {
		    DisplayLines(((TransferItem)e.Item.Tag).Lines);
	    }

		private void btnTransferArticlesToDestination_Click(object sender, EventArgs e)
		{
			Cursor = Cursors.WaitCursor;
			btnTransferArticlesToDestination.Enabled = false;

			var articleManager = new ArticleManager(VgConnections.DestinationConnection);
			_attemptsToCreateArticle = 0;
			_failedAttemptsToCreateArticle = 0;

			foreach (var item in _transferItems)
			{
				EnsureAllArticlesExists(item.Lines, articleManager);
			}

			lstLog.Items.Add(string.Format("{0} av {1} artikler opprettet", (_attemptsToCreateArticle - _failedAttemptsToCreateArticle), _attemptsToCreateArticle));

			btnCreateCustomerOrders.Enabled = _failedAttemptsToCreateArticle == 0;
			Cursor = Cursors.Default;
		}

		private void EnsureAllArticlesExists(IEnumerable<TransferItemLine> lines, ArticleManager articleManager)
	    {
		    foreach (var line in lines)
		    {
                if (string.IsNullOrWhiteSpace(line.ArticleNo) || articleManager.ArticleExists(line.ArticleNo))
					continue;

                _attemptsToCreateArticle++;

                if (!articleManager.CreateArticle(line.ArticleNo, line.ArticleName, _priceCalcMethodsNo, _postingTemplate, _stockProfileNo))
                {
                    _failedAttemptsToCreateArticle++;
                    lstLog.Items.Add(string.Format("Kunne ikke opprette artikkel '{0} - {1}'", line.ArticleNo, line.ArticleName));
                }
		    }
	    }

		private void btnCreateCustomerOrders_Click(object sender, EventArgs e)
		{
			Cursor = Cursors.WaitCursor;
			btnCreateCustomerOrders.Enabled = false;

			var salesOrderManager = new SalesOrderManager(VgConnections.DestinationConnection);
			var customerQuery = new CustomerQuery(VgConnections.DestinationConnection);
			var invoiceManager = new InvoiceManager(VgConnections.SourceConnection);

			var failedCustomerOrders = 0;

			foreach (var item in _transferItems)
			{
				if (customerQuery.CustomerExists(item.CustomerNo))
				{
					item.OrderNoInDestinationClient = salesOrderManager.CreateSalesOrderFromInvoice(item);

					if (string.IsNullOrEmpty(item.OrderNoInDestinationClient))
						failedCustomerOrders++;						
					else
						invoiceManager.MarkInvoiceAsTransferred(item.InvoiceNo);
				}
				else
				{
					failedCustomerOrders++;
					lstLog.Items.Add(string.Format("Kunne ikke opprette ordre før fakura {0}. Kunden ({1}) ikke funnet'", item.InvoiceNo, item.CustomerNo));
				}
			}

			lstLog.Items.Add(string.Format("{0} av {1} ordre opprettet", (_transferItems.Count - failedCustomerOrders), _transferItems.Count));

			DisplayTransferItems(_transferItems);
			Cursor = Cursors.Default;
		}
    }
}
