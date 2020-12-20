using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AB.API_Class.Advance_Payment;
using Newtonsoft.Json.Linq;

namespace AB
{
    public partial class SelectAdvancePayment : Form
    {
        advancepayment_class advancepaymentc = new advancepayment_class();
        public SelectAdvancePayment()
        {
            InitializeComponent();
        }

        private void SelectAdvancePayment_Load(object sender, EventArgs e)
        {
            loadData();
        }

        public void loadData()
        {
            DataTable dtResponse = new DataTable();
            dtResponse = advancepaymentc.loadData("O", "In Deposit");
            dgv.Rows.Clear();
            if (dtResponse.Rows.Count > 0)
            {
                AutoCompleteStringCollection auto = new AutoCompleteStringCollection();
                foreach (DataRow r0w in dtResponse.Rows)
                {
                    double amount = Convert.ToDouble(r0w["amount"].ToString());
                    double balance = Convert.ToDouble(r0w["balance"].ToString());

                    auto.Add(r0w["reference"].ToString());
                    auto.Add(r0w["cust_code"].ToString());
                    auto.Add(r0w["remarks"].ToString());

                    if (!string.IsNullOrEmpty(txtSearch.Text.ToString().Trim()))
                    {
                        if (txtSearch.Text.ToString().Trim().ToLower().Contains(r0w["reference"].ToString().ToLower()))
                        {
                            dgv.Rows.Add(false, r0w["id"], r0w["cust_code"], Convert.ToDecimal(string.Format("{0:0.00}", amount)), Convert.ToDecimal(string.Format("{0:0.00}", balance)), r0w["reference"], r0w["sap_number"]);
                        }
                        else if (txtSearch.Text.ToString().Trim().ToLower().Contains(r0w["cust_code"].ToString().ToLower()))
                        {
                            dgv.Rows.Add(false, r0w["id"], r0w["cust_code"], Convert.ToDecimal(string.Format("{0:0.00}", amount)), Convert.ToDecimal(string.Format("{0:0.00}", balance)), r0w["reference"], r0w["sap_number"]);
                        }
                    }
                    else
                    {
                        dgv.Rows.Add(false, r0w["id"], r0w["cust_code"], Convert.ToDecimal(string.Format("{0:0.00}", amount)), Convert.ToDecimal(string.Format("{0:0.00}", balance)), r0w["reference"], r0w["sap_number"]);
                    }
                }
                txtSearch.AutoCompleteCustomSource = auto;
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("id");
            dt.Columns.Add("amount");
            dt.Columns.Add("payment_type");
            dt.Columns.Add("sapnum");
            dt.Columns.Add("reference2");
            PendingOrder2.dtSelectedDeposit.Rows.Clear();
            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                if (Convert.ToBoolean(dgv.Rows[i].Cells["selectt"].Value.ToString()))
                {
                    PendingOrder2.dtSelectedDeposit.Rows.Add(Convert.ToInt32(dgv.Rows[i].Cells["id"].Value.ToString()), Convert.ToDouble(dgv.Rows[i].Cells["balance"].Value.ToString()), "FDEPS", dgv.Rows[i].Cells["sapnumber"].Value.ToString(), dgv.Rows[i].Cells["reference"].Value.ToString());
                }
                this.Hide();
            }
        }

        private void btnAddAdvancePayment_Click(object sender, EventArgs e)
        {
            AddAdvancePayment addAdvancePayment = new AddAdvancePayment();
            addAdvancePayment.ShowDialog();
            if (AddAdvancePayment.isSubmit)
            {
                loadData();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            loadData();
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.Equals(Keys.Enter))
            {
                loadData();
            }
        }
    }
}
