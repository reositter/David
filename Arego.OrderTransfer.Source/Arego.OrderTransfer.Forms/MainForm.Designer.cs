namespace Arego.OrderTransfer.Forms
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.txtDestinationClientName = new System.Windows.Forms.TextBox();
			this.txtSourceClientName = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.txtChainNo = new System.Windows.Forms.TextBox();
			this.txtBapiKey = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.btnGetInvoicesFromSourceClient = new System.Windows.Forms.Button();
			this.lstTransferItems = new System.Windows.Forms.ListView();
			this.colInvoiceNo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colCustomerNo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colCustomerName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colOrderNo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.label5 = new System.Windows.Forms.Label();
			this.lstLines = new System.Windows.Forms.ListView();
			this.colArticleNo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colArticleName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colNetPrice = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colDiscountI = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colQty = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.btnTransferArticlesToDestination = new System.Windows.Forms.Button();
			this.lstLog = new System.Windows.Forms.ListBox();
			this.btnCreateCustomerOrders = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.txtDestinationClientName);
			this.groupBox1.Controls.Add(this.txtSourceClientName);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.txtChainNo);
			this.groupBox1.Controls.Add(this.txtBapiKey);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Location = new System.Drawing.Point(8, 8);
			this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
			this.groupBox1.Size = new System.Drawing.Size(769, 122);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Global innstillinger";
			// 
			// txtDestinationClientName
			// 
			this.txtDestinationClientName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtDestinationClientName.Enabled = false;
			this.txtDestinationClientName.Location = new System.Drawing.Point(91, 67);
			this.txtDestinationClientName.Margin = new System.Windows.Forms.Padding(2);
			this.txtDestinationClientName.Name = "txtDestinationClientName";
			this.txtDestinationClientName.Size = new System.Drawing.Size(674, 20);
			this.txtDestinationClientName.TabIndex = 4;
			// 
			// txtSourceClientName
			// 
			this.txtSourceClientName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtSourceClientName.Enabled = false;
			this.txtSourceClientName.Location = new System.Drawing.Point(91, 43);
			this.txtSourceClientName.Margin = new System.Windows.Forms.Padding(2);
			this.txtSourceClientName.Name = "txtSourceClientName";
			this.txtSourceClientName.Size = new System.Drawing.Size(674, 20);
			this.txtSourceClientName.TabIndex = 3;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(4, 94);
			this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(37, 13);
			this.label4.TabIndex = 2;
			this.label4.Text = "Kjede:";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(4, 70);
			this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(66, 13);
			this.label3.TabIndex = 2;
			this.label3.Text = "Norsk klient:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(4, 45);
			this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(74, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "Svensk klient:";
			// 
			// txtChainNo
			// 
			this.txtChainNo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtChainNo.Enabled = false;
			this.txtChainNo.Location = new System.Drawing.Point(91, 91);
			this.txtChainNo.Margin = new System.Windows.Forms.Padding(2);
			this.txtChainNo.Name = "txtChainNo";
			this.txtChainNo.Size = new System.Drawing.Size(675, 20);
			this.txtChainNo.TabIndex = 1;
			// 
			// txtBapiKey
			// 
			this.txtBapiKey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtBapiKey.Enabled = false;
			this.txtBapiKey.Location = new System.Drawing.Point(91, 19);
			this.txtBapiKey.Margin = new System.Windows.Forms.Padding(2);
			this.txtBapiKey.Name = "txtBapiKey";
			this.txtBapiKey.Size = new System.Drawing.Size(674, 20);
			this.txtBapiKey.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(4, 21);
			this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(34, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "BAPI:";
			// 
			// btnGetInvoicesFromSourceClient
			// 
			this.btnGetInvoicesFromSourceClient.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.btnGetInvoicesFromSourceClient.Enabled = false;
			this.btnGetInvoicesFromSourceClient.Location = new System.Drawing.Point(12, 135);
			this.btnGetInvoicesFromSourceClient.Name = "btnGetInvoicesFromSourceClient";
			this.btnGetInvoicesFromSourceClient.Size = new System.Drawing.Size(761, 23);
			this.btnGetInvoicesFromSourceClient.TabIndex = 2;
			this.btnGetInvoicesFromSourceClient.Text = "Last ned fakturaer";
			this.btnGetInvoicesFromSourceClient.UseVisualStyleBackColor = true;
			this.btnGetInvoicesFromSourceClient.Click += new System.EventHandler(this.btnGetInvoicesFromSourceClient_Click);
			// 
			// lstTransferItems
			// 
			this.lstTransferItems.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lstTransferItems.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colInvoiceNo,
            this.colCustomerNo,
            this.colCustomerName,
            this.colOrderNo});
			this.lstTransferItems.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lstTransferItems.FullRowSelect = true;
			this.lstTransferItems.GridLines = true;
			this.lstTransferItems.Location = new System.Drawing.Point(12, 169);
			this.lstTransferItems.MultiSelect = false;
			this.lstTransferItems.Name = "lstTransferItems";
			this.lstTransferItems.Size = new System.Drawing.Size(761, 166);
			this.lstTransferItems.TabIndex = 5;
			this.lstTransferItems.UseCompatibleStateImageBehavior = false;
			this.lstTransferItems.View = System.Windows.Forms.View.Details;
			this.lstTransferItems.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.lstTransferItems_ItemSelectionChanged);
			// 
			// colInvoiceNo
			// 
			this.colInvoiceNo.Text = "Fakturanr";
			this.colInvoiceNo.Width = 74;
			// 
			// colCustomerNo
			// 
			this.colCustomerNo.Text = "Kundenr";
			this.colCustomerNo.Width = 74;
			// 
			// colCustomerName
			// 
			this.colCustomerName.Text = "Navn";
			this.colCustomerName.Width = 240;
			// 
			// colOrderNo
			// 
			this.colOrderNo.Text = "Ordrenr";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(12, 338);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(32, 13);
			this.label5.TabIndex = 6;
			this.label5.Text = "Linjer";
			// 
			// lstLines
			// 
			this.lstLines.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lstLines.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colArticleNo,
            this.colArticleName,
            this.colNetPrice,
            this.colDiscountI,
            this.colQty});
			this.lstLines.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lstLines.GridLines = true;
			this.lstLines.Location = new System.Drawing.Point(12, 354);
			this.lstLines.MultiSelect = false;
			this.lstLines.Name = "lstLines";
			this.lstLines.Size = new System.Drawing.Size(761, 97);
			this.lstLines.TabIndex = 7;
			this.lstLines.UseCompatibleStateImageBehavior = false;
			this.lstLines.View = System.Windows.Forms.View.Details;
			// 
			// colArticleNo
			// 
			this.colArticleNo.Text = "Artikkelnr";
			this.colArticleNo.Width = 100;
			// 
			// colArticleName
			// 
			this.colArticleName.Text = "Artikkelnavn";
			this.colArticleName.Width = 200;
			// 
			// colNetPrice
			// 
			this.colNetPrice.Text = "Nto. pris";
			this.colNetPrice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// colDiscountI
			// 
			this.colDiscountI.Text = "Rab. I";
			this.colDiscountI.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// colQty
			// 
			this.colQty.Text = "Antall";
			this.colQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// btnTransferArticlesToDestination
			// 
			this.btnTransferArticlesToDestination.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.btnTransferArticlesToDestination.Enabled = false;
			this.btnTransferArticlesToDestination.Location = new System.Drawing.Point(12, 457);
			this.btnTransferArticlesToDestination.Name = "btnTransferArticlesToDestination";
			this.btnTransferArticlesToDestination.Size = new System.Drawing.Size(761, 23);
			this.btnTransferArticlesToDestination.TabIndex = 8;
			this.btnTransferArticlesToDestination.Text = "Overfør artikkler";
			this.btnTransferArticlesToDestination.UseVisualStyleBackColor = true;
			this.btnTransferArticlesToDestination.Click += new System.EventHandler(this.btnTransferArticlesToDestination_Click);
			// 
			// lstLog
			// 
			this.lstLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lstLog.FormattingEnabled = true;
			this.lstLog.Location = new System.Drawing.Point(12, 527);
			this.lstLog.Name = "lstLog";
			this.lstLog.Size = new System.Drawing.Size(761, 95);
			this.lstLog.TabIndex = 9;
			// 
			// btnCreateCustomerOrders
			// 
			this.btnCreateCustomerOrders.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCreateCustomerOrders.Enabled = false;
			this.btnCreateCustomerOrders.Location = new System.Drawing.Point(12, 486);
			this.btnCreateCustomerOrders.Name = "btnCreateCustomerOrders";
			this.btnCreateCustomerOrders.Size = new System.Drawing.Size(761, 23);
			this.btnCreateCustomerOrders.TabIndex = 10;
			this.btnCreateCustomerOrders.Text = "Lag salgsordre";
			this.btnCreateCustomerOrders.UseVisualStyleBackColor = true;
			this.btnCreateCustomerOrders.Click += new System.EventHandler(this.btnCreateCustomerOrders_Click);
			// 
			// MainForm
			// 
			this.AcceptButton = this.btnGetInvoicesFromSourceClient;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(785, 634);
			this.Controls.Add(this.btnCreateCustomerOrders);
			this.Controls.Add(this.lstLog);
			this.Controls.Add(this.btnTransferArticlesToDestination);
			this.Controls.Add(this.lstLines);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.lstTransferItems);
			this.Controls.Add(this.btnGetInvoicesFromSourceClient);
			this.Controls.Add(this.groupBox1);
			this.Margin = new System.Windows.Forms.Padding(2);
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = " Order Transfer";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtChainNo;
        private System.Windows.Forms.TextBox txtBapiKey;
        private System.Windows.Forms.Button btnGetInvoicesFromSourceClient;
        private System.Windows.Forms.TextBox txtDestinationClientName;
		private System.Windows.Forms.TextBox txtSourceClientName;
		private System.Windows.Forms.ListView lstTransferItems;
		private System.Windows.Forms.ColumnHeader colInvoiceNo;
		private System.Windows.Forms.ColumnHeader colCustomerNo;
		private System.Windows.Forms.ColumnHeader colCustomerName;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.ListView lstLines;
		private System.Windows.Forms.ColumnHeader colArticleNo;
		private System.Windows.Forms.ColumnHeader colArticleName;
		private System.Windows.Forms.ColumnHeader colNetPrice;
		private System.Windows.Forms.ColumnHeader colDiscountI;
		private System.Windows.Forms.ColumnHeader colQty;
		private System.Windows.Forms.Button btnTransferArticlesToDestination;
		private System.Windows.Forms.ListBox lstLog;
		private System.Windows.Forms.Button btnCreateCustomerOrders;
		private System.Windows.Forms.ColumnHeader colOrderNo;
    }
}

