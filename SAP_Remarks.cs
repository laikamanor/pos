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
    public partial class SAP_Remarks : Form
    {
        public static int sap_number = 0;
        public static string rem = "";
        public static bool isSubmit = false;
        public SAP_Remarks()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtSAP.Text.Trim()))
            {
                MessageBox.Show("SAP # field is required", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (string.IsNullOrEmpty(txtRemarks.Text.Trim()))
            {
                MessageBox.Show("Remarks field is required", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                DialogResult dialogResult = MessageBox.Show("Are you sure you want to submit?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    isSubmit = true;
                    sap_number = Convert.ToInt32(txtSAP.Text.Trim());
                    rem = txtRemarks.Text.Trim();
                    this.Dispose();
                }
            }
        }

        private void txtSAP_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void SAP_Remarks_Load(object sender, EventArgs e)
        {
            isSubmit = false;
        }
    }
}
