namespace AB
{
    partial class MainMenu
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainMenu));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.masterDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.itemsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.usersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.branchesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.warehouseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.customersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.objectTypeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inventoryTransactionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.itemRequestToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.receivedTransactionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.transferTransactionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pulloutTransactionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.salesTransactionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.adjustmentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.adjustmentInToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.adjustmentOutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.orderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.advancePaymentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pendingOrdersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inventoryToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.salesReportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cashTransactionReportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.printableReportsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inventoryCountToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inventorySummaryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panelchildform = new System.Windows.Forms.Panel();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem,
            this.masterDataToolStripMenuItem,
            this.inventoryTransactionToolStripMenuItem,
            this.orderToolStripMenuItem,
            this.reportToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(700, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.logoutToolStripMenuItem});
            this.settingsToolStripMenuItem.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // logoutToolStripMenuItem
            // 
            this.logoutToolStripMenuItem.Name = "logoutToolStripMenuItem";
            this.logoutToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.logoutToolStripMenuItem.Text = "Logout";
            this.logoutToolStripMenuItem.Click += new System.EventHandler(this.logoutToolStripMenuItem_Click);
            // 
            // masterDataToolStripMenuItem
            // 
            this.masterDataToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.itemsToolStripMenuItem,
            this.usersToolStripMenuItem,
            this.branchesToolStripMenuItem,
            this.warehouseToolStripMenuItem,
            this.customersToolStripMenuItem,
            this.objectTypeToolStripMenuItem});
            this.masterDataToolStripMenuItem.Name = "masterDataToolStripMenuItem";
            this.masterDataToolStripMenuItem.Size = new System.Drawing.Size(82, 20);
            this.masterDataToolStripMenuItem.Text = "Master Data";
            // 
            // itemsToolStripMenuItem
            // 
            this.itemsToolStripMenuItem.Name = "itemsToolStripMenuItem";
            this.itemsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.itemsToolStripMenuItem.Text = "Items";
            this.itemsToolStripMenuItem.Click += new System.EventHandler(this.itemsToolStripMenuItem_Click);
            // 
            // usersToolStripMenuItem
            // 
            this.usersToolStripMenuItem.Name = "usersToolStripMenuItem";
            this.usersToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.usersToolStripMenuItem.Text = "Users";
            this.usersToolStripMenuItem.Click += new System.EventHandler(this.usersToolStripMenuItem_Click);
            // 
            // branchesToolStripMenuItem
            // 
            this.branchesToolStripMenuItem.Name = "branchesToolStripMenuItem";
            this.branchesToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.branchesToolStripMenuItem.Text = "Branches";
            this.branchesToolStripMenuItem.Click += new System.EventHandler(this.branchesToolStripMenuItem_Click);
            // 
            // warehouseToolStripMenuItem
            // 
            this.warehouseToolStripMenuItem.Name = "warehouseToolStripMenuItem";
            this.warehouseToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.warehouseToolStripMenuItem.Text = "Warehouse";
            this.warehouseToolStripMenuItem.Click += new System.EventHandler(this.warehouseToolStripMenuItem_Click);
            // 
            // customersToolStripMenuItem
            // 
            this.customersToolStripMenuItem.Name = "customersToolStripMenuItem";
            this.customersToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.customersToolStripMenuItem.Text = "Customers";
            this.customersToolStripMenuItem.Click += new System.EventHandler(this.customersToolStripMenuItem_Click);
            // 
            // objectTypeToolStripMenuItem
            // 
            this.objectTypeToolStripMenuItem.Name = "objectTypeToolStripMenuItem";
            this.objectTypeToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.objectTypeToolStripMenuItem.Text = "Object Type";
            this.objectTypeToolStripMenuItem.Click += new System.EventHandler(this.objectTypeToolStripMenuItem_Click);
            // 
            // inventoryTransactionToolStripMenuItem
            // 
            this.inventoryTransactionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.itemRequestToolStripMenuItem,
            this.receivedTransactionsToolStripMenuItem,
            this.transferTransactionsToolStripMenuItem,
            this.pulloutTransactionsToolStripMenuItem,
            this.salesTransactionsToolStripMenuItem,
            this.adjustmentsToolStripMenuItem});
            this.inventoryTransactionToolStripMenuItem.Name = "inventoryTransactionToolStripMenuItem";
            this.inventoryTransactionToolStripMenuItem.Size = new System.Drawing.Size(138, 20);
            this.inventoryTransactionToolStripMenuItem.Text = "Inventory Transactions";
            // 
            // itemRequestToolStripMenuItem
            // 
            this.itemRequestToolStripMenuItem.Name = "itemRequestToolStripMenuItem";
            this.itemRequestToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.itemRequestToolStripMenuItem.Text = "Item Request";
            this.itemRequestToolStripMenuItem.Click += new System.EventHandler(this.itemRequestToolStripMenuItem_Click);
            // 
            // receivedTransactionsToolStripMenuItem
            // 
            this.receivedTransactionsToolStripMenuItem.Name = "receivedTransactionsToolStripMenuItem";
            this.receivedTransactionsToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.receivedTransactionsToolStripMenuItem.Text = "Received Transactions";
            this.receivedTransactionsToolStripMenuItem.Click += new System.EventHandler(this.receivedTransactionsToolStripMenuItem_Click);
            // 
            // transferTransactionsToolStripMenuItem
            // 
            this.transferTransactionsToolStripMenuItem.Name = "transferTransactionsToolStripMenuItem";
            this.transferTransactionsToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.transferTransactionsToolStripMenuItem.Text = "Transfer Transactions";
            this.transferTransactionsToolStripMenuItem.Click += new System.EventHandler(this.transferTransactionsToolStripMenuItem_Click);
            // 
            // pulloutTransactionsToolStripMenuItem
            // 
            this.pulloutTransactionsToolStripMenuItem.Name = "pulloutTransactionsToolStripMenuItem";
            this.pulloutTransactionsToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.pulloutTransactionsToolStripMenuItem.Text = "Pullout Transactions";
            this.pulloutTransactionsToolStripMenuItem.Click += new System.EventHandler(this.pulloutTransactionsToolStripMenuItem_Click);
            // 
            // salesTransactionsToolStripMenuItem
            // 
            this.salesTransactionsToolStripMenuItem.Name = "salesTransactionsToolStripMenuItem";
            this.salesTransactionsToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.salesTransactionsToolStripMenuItem.Text = "Sales Transactions";
            this.salesTransactionsToolStripMenuItem.Click += new System.EventHandler(this.salesTransactionsToolStripMenuItem_Click);
            // 
            // adjustmentsToolStripMenuItem
            // 
            this.adjustmentsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.adjustmentInToolStripMenuItem1,
            this.adjustmentOutToolStripMenuItem});
            this.adjustmentsToolStripMenuItem.Name = "adjustmentsToolStripMenuItem";
            this.adjustmentsToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.adjustmentsToolStripMenuItem.Text = "Adjustments";
            // 
            // adjustmentInToolStripMenuItem1
            // 
            this.adjustmentInToolStripMenuItem1.Name = "adjustmentInToolStripMenuItem1";
            this.adjustmentInToolStripMenuItem1.Size = new System.Drawing.Size(159, 22);
            this.adjustmentInToolStripMenuItem1.Text = "Adjustment In";
            this.adjustmentInToolStripMenuItem1.Click += new System.EventHandler(this.adjustmentInToolStripMenuItem1_Click);
            // 
            // adjustmentOutToolStripMenuItem
            // 
            this.adjustmentOutToolStripMenuItem.Name = "adjustmentOutToolStripMenuItem";
            this.adjustmentOutToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.adjustmentOutToolStripMenuItem.Text = "Adjustment Out";
            this.adjustmentOutToolStripMenuItem.Click += new System.EventHandler(this.adjustmentOutToolStripMenuItem_Click);
            // 
            // orderToolStripMenuItem
            // 
            this.orderToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.advancePaymentToolStripMenuItem,
            this.pendingOrdersToolStripMenuItem});
            this.orderToolStripMenuItem.Name = "orderToolStripMenuItem";
            this.orderToolStripMenuItem.Size = new System.Drawing.Size(45, 20);
            this.orderToolStripMenuItem.Text = "Sales";
            // 
            // advancePaymentToolStripMenuItem
            // 
            this.advancePaymentToolStripMenuItem.Name = "advancePaymentToolStripMenuItem";
            this.advancePaymentToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.advancePaymentToolStripMenuItem.Text = "Deposit";
            this.advancePaymentToolStripMenuItem.Click += new System.EventHandler(this.advancePaymentToolStripMenuItem_Click);
            // 
            // pendingOrdersToolStripMenuItem
            // 
            this.pendingOrdersToolStripMenuItem.Name = "pendingOrdersToolStripMenuItem";
            this.pendingOrdersToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.pendingOrdersToolStripMenuItem.Text = "Sales";
            this.pendingOrdersToolStripMenuItem.Click += new System.EventHandler(this.pendingOrdersToolStripMenuItem_Click);
            // 
            // reportToolStripMenuItem
            // 
            this.reportToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.inventoryToolStripMenuItem1,
            this.salesReportToolStripMenuItem,
            this.cashTransactionReportToolStripMenuItem,
            this.printableReportsToolStripMenuItem});
            this.reportToolStripMenuItem.Name = "reportToolStripMenuItem";
            this.reportToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.reportToolStripMenuItem.Text = "Reports";
            // 
            // inventoryToolStripMenuItem1
            // 
            this.inventoryToolStripMenuItem1.Name = "inventoryToolStripMenuItem1";
            this.inventoryToolStripMenuItem1.Size = new System.Drawing.Size(202, 22);
            this.inventoryToolStripMenuItem1.Text = "Inventory";
            this.inventoryToolStripMenuItem1.Click += new System.EventHandler(this.inventoryToolStripMenuItem1_Click);
            // 
            // salesReportToolStripMenuItem
            // 
            this.salesReportToolStripMenuItem.Name = "salesReportToolStripMenuItem";
            this.salesReportToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
            this.salesReportToolStripMenuItem.Text = "Sales Report";
            this.salesReportToolStripMenuItem.Click += new System.EventHandler(this.salesReportToolStripMenuItem_Click);
            // 
            // cashTransactionReportToolStripMenuItem
            // 
            this.cashTransactionReportToolStripMenuItem.Name = "cashTransactionReportToolStripMenuItem";
            this.cashTransactionReportToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
            this.cashTransactionReportToolStripMenuItem.Text = "Cash Transaction Report";
            this.cashTransactionReportToolStripMenuItem.Click += new System.EventHandler(this.cashTransactionReportToolStripMenuItem_Click);
            // 
            // printableReportsToolStripMenuItem
            // 
            this.printableReportsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.inventoryCountToolStripMenuItem,
            this.inventorySummaryToolStripMenuItem});
            this.printableReportsToolStripMenuItem.Name = "printableReportsToolStripMenuItem";
            this.printableReportsToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
            this.printableReportsToolStripMenuItem.Text = "Printable Reports";
            // 
            // inventoryCountToolStripMenuItem
            // 
            this.inventoryCountToolStripMenuItem.Name = "inventoryCountToolStripMenuItem";
            this.inventoryCountToolStripMenuItem.Size = new System.Drawing.Size(217, 22);
            this.inventoryCountToolStripMenuItem.Text = "Inventory Count Summary";
            this.inventoryCountToolStripMenuItem.Click += new System.EventHandler(this.inventoryCountToolStripMenuItem_Click);
            // 
            // inventorySummaryToolStripMenuItem
            // 
            this.inventorySummaryToolStripMenuItem.Name = "inventorySummaryToolStripMenuItem";
            this.inventorySummaryToolStripMenuItem.Size = new System.Drawing.Size(217, 22);
            this.inventorySummaryToolStripMenuItem.Text = "Sales and Inventory Report ";
            this.inventorySummaryToolStripMenuItem.Click += new System.EventHandler(this.inventorySummaryToolStripMenuItem_Click);
            // 
            // panelchildform
            // 
            this.panelchildform.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelchildform.BackColor = System.Drawing.Color.White;
            this.panelchildform.Location = new System.Drawing.Point(12, 27);
            this.panelchildform.Name = "panelchildform";
            this.panelchildform.Size = new System.Drawing.Size(676, 399);
            this.panelchildform.TabIndex = 1;
            // 
            // MainMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(700, 429);
            this.Controls.Add(this.panelchildform);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainMenu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Atlantic Bakery";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainMenu_FormClosing);
            this.Load += new System.EventHandler(this.MainMenu_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem logoutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem orderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pendingOrdersToolStripMenuItem;
        private System.Windows.Forms.Panel panelchildform;
        private System.Windows.Forms.ToolStripMenuItem masterDataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem itemsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem usersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem advancePaymentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem inventoryToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem salesReportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem inventoryTransactionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem itemRequestToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem receivedTransactionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem transferTransactionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem salesTransactionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cashTransactionReportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem branchesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem customersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem printableReportsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem inventoryCountToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem inventorySummaryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pulloutTransactionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem adjustmentsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem adjustmentInToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem adjustmentOutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem objectTypeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem warehouseToolStripMenuItem;
    }
}