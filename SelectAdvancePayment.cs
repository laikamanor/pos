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
            dtResponse = advancepaymentc.loadData("O");
            dgv.Rows.Clear();
            if (dtResponse.Rows.Count > 0)
            {
                foreach (DataRow r0w in dtResponse.Rows)
                {
                    dgv.Rows.Add(false, r0w["id"], r0w["cust_code"], Convert.ToDouble(r0w["amount"]).ToString("n2"), Convert.ToDouble(r0w["balance"]).ToString("n2"),r0w["reference2"],r0w["sap_number"]);
                }
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
    }
}
