using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AB
{
    public partial class MainMenu : Form
    {
        public MainMenu()
        {
            InitializeComponent();
        }
        private void MainMenu_Load(object sender, EventArgs e)
        {

        }

        public void showForm(Form form)
        {
            form.TopLevel = false;
            panelchildform.Controls.Add(form);
            form.BringToFront();
            form.Show();
        }

        private void pendingOrdersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PendingOrder pendingOrder = new PendingOrder();
            showForm(pendingOrder);
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to logout?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                Login.jsonResult = null;
                this.Dispose();
                Login login = new Login();
                login.ShowDialog();
            }
        }

        private void usersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Users users = new Users();
            showForm(users);
        }

        private void MainMenu_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to logout?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.No)
            {
                e.Cancel = true;
            }
            else
            {
                Login.jsonResult = null;
                this.Dispose();
                Login login = new Login();
                login.ShowDialog();
            }
        }

        private void advancePaymentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AdvancePayment advancePayment = new AdvancePayment();
            showForm(advancePayment);
        }

        private void inventoryToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Inventory inventory = new Inventory();
            showForm(inventory);
        }

        private void itemRequestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ItemRequest itemRequest = new ItemRequest();
            showForm(itemRequest);
        }

        private void transferTransactionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Transfer transfer = new Transfer();
            transfer.Text = "Transfer Transactions";
            showForm(transfer);
        }

        private void cashTransactionReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CashTransactionReport cashTransactionReport = new CashTransactionReport();
            showForm(cashTransactionReport);
        }

        private void salesReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SalesReport salesReport = new SalesReport();
            showForm(salesReport);
        }

        private void receivedTransactionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Transfer transfer = new Transfer();
            transfer.Text = "Received Transactions";
            showForm(transfer);
        }

        private void branchesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Branches branch = new Branches();
            showForm(branch);
        }

        private void customersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Customers customers = new Customers();
            showForm(customers);
        }

        private void inventoryCountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            printableReports("Final Count Report");
        }

        public void printableReports(string reportType)
        {
            EnterDate enterDate = new EnterDate(reportType);
            enterDate.ShowDialog();
        }

        private void inventorySummaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            printableReports("Final Report");
        }

        private void salesTransactionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SalesTransactions salesTransactions = new SalesTransactions();
            showForm(salesTransactions);
        }

        private void pulloutTransactionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Transfer transfer = new Transfer();
            transfer.Text = "Pullout Transactions";
            showForm(transfer);
        }

        private void adjustmentInToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AdjustmentIn adjustmentIn = new AdjustmentIn("in");
            showForm(adjustmentIn);
        }

        private void adjustmentOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AdjustmentIn adjustmentIn = new AdjustmentIn("out");
            showForm(adjustmentIn);
        }

        private void objectTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ObjectType adjustmentIn = new ObjectType();
            showForm(adjustmentIn);
        }
    }
}
