using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AB.API_Class.Payment_Type;
using System.Globalization;

namespace AB
{
    public partial class PaymentMethodForm : Form
    {
        paymenttype_class paymenttypec = new paymenttype_class();
        public static bool isSubmit = false;
        DataTable dtPaymentTypes;
        public PaymentMethodForm()
        {
            InitializeComponent();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (cmbPaymentType.SelectedIndex == -1)
            {
                MessageBox.Show("Payment Type field is required", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbPaymentType.Focus();
            }
            else if (cmbPaymentType.SelectedIndex > 0 && string.IsNullOrEmpty(txtReference.Text.Trim()))
            {
                MessageBox.Show("Reference field is required", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtReference.Focus();
            }
            else
            {
                string getPaymentTypeCode = cmbPaymentType.Text.Substring(0, cmbPaymentType.Text.IndexOf(" - "));

                isSubmit = true;
                PaymentMethodList.dtList.Rows.Add(null, getPaymentTypeCode, txtAmount.Text, txtSAPNumber.Text, txtReference.Text.Trim());
                this.Dispose();
            }
        }

        
        private void PaymentMethodForm_Load(object sender, EventArgs e)
        {
            loadPaymentType();
           dtPaymentTypes = new DataTable();
        }

        public void loadPaymentType()
        {
            dtPaymentTypes = paymenttypec.loadPaymentType("payment");
            if(dtPaymentTypes.Rows.Count > 0)
            {
                cmbPaymentType.Items.Clear();
                foreach (DataRow row in dtPaymentTypes.Rows)
                {
                    if(row["code"].ToString() == "FDEPS")
                    {
                        continue;
                    }else if(row["code"].ToString() == "DEPS")
                    {
                        continue;
                    }else
                    {
                        cmbPaymentType.Items.Add(row["code"] + " - " + row["description"]);
                    }
                }
                if(cmbPaymentType.Items.Count > 0)
                {
                    cmbPaymentType.SelectedIndex = 0;
                }
            }
        }

        private void txtSAPNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void cmbPaymentType_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.Equals(Keys.Enter))
            {
                btnSubmit.PerformClick();
            }
        }

        private void txtAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar)
         && !char.IsDigit(e.KeyChar)
         && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if (e.KeyChar == '.'
                && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
            }
        }
    }
}
