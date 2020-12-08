using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AB
{
    public partial class PaymentMethodList : Form
    {
        public static DataTable dtList;
        public static bool isSubmit = false;
        public PaymentMethodList()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            PaymentMethodForm paymentMethodForm = new PaymentMethodForm();
            paymentMethodForm.ShowDialog();
            if (PaymentMethodForm.isSubmit)
            {
                dgv.Rows.Clear();
                foreach (DataRow row in dtList.Rows)
                {   
                    dgv.Rows.Add(row["payment_type"].ToString(), row["amount"].ToString(), row["sapnum"].ToString(), row["reference2"].ToString());
                }
            }

            totalAmount();
        }

        public void totalAmount()
        {
            double total = 0.00;
            for(int i = 0; i < dgv.Rows.Count; i++)
            {
                total += (dgv.Rows[i].Cells["amount"].Value.ToString() == "" ? 0.00 : Convert.ToDouble(dgv.Rows[i].Cells["amount"].Value.ToString()));
            }
            lblSelectedAmount.Text = total.ToString("n2");
        }

        private void PaymentMethodList_Load(object sender, EventArgs e)
        {
            dtList = new DataTable();
            dtList.Columns.Clear();
            dtList.Columns.Add("id");
            dtList.Columns.Add("payment_type");
            dtList.Columns.Add("amount");
            dtList.Columns.Add("sapnum");
            dtList.Columns.Add("reference2");

            loadData();
        }

        public void loadData()
        {
            if (PendingOrder2.dtSelectedDeposit.Rows.Count > 0)
            {
                int hasNoFDEPS = 0;
                foreach (DataRow row in PendingOrder2.dtSelectedDeposit.Rows)
                {
                    if (row["payment_type"].ToString() != "FDEPS")
                    {
                        hasNoFDEPS += 1;
                    }
                }
                if (hasNoFDEPS > 0)
                {
                    dgv.Rows.Clear();
                    dtList.Rows.Clear();
                }
                foreach (DataRow row in PendingOrder2.dtSelectedDeposit.Rows)
                {
                    if (row["payment_type"].ToString() != "FDEPS")
                    {
                        dtList.Rows.Add(row["payment_type"], row["amount"], row["sapnum"], row["reference2"]);
                        dgv.Rows.Add(row["payment_type"], row["amount"], row["sapnum"], row["reference2"]);
                    }
                }
            }
        }

        private void dgv_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgv.Rows.Count > 0)
            {
                if (e.ColumnIndex == 4 && e.RowIndex >= 0)
                {
                    DialogResult dialogResult = MessageBox.Show("Are you sure you want to remove?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        dtList.Rows.RemoveAt(dgv.CurrentRow.Index);
                        if(PendingOrder2.dtSelectedDeposit.Rows.Count > 0)
                        {
                            PendingOrder2.dtSelectedDeposit.Rows.RemoveAt(dgv.CurrentRow.Index);
                        }
                        dgv.Rows.RemoveAt(dgv.CurrentRow.Index);
                        totalAmount();
                    }
                }
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if(dgv.Rows.Count > 0)
            {
                if(dtList.Rows.Count > 0)
                {
                    //PendingOrder2.dtSelectedDeposit.Rows.Clear();
                    foreach (DataRow row in dtList.Rows)
                    {
                       if(row["amount"].ToString() != "")
                        {
                            PendingOrder2.dtSelectedDeposit.Rows.Add(null, Convert.ToDouble(row["amount"].ToString()), row["payment_type"].ToString(), row["sapnum"].ToString(), row["reference2"].ToString());
                        }
                    }
                }

                isSubmit = true;
                this.Dispose();
            }
            else
            {
                MessageBox.Show("No order selected. If you want to close the form click the close button", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
