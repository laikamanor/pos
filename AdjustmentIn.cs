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
    public partial class AdjustmentIn : Form
    {
        string gAdjType = "";
        public AdjustmentIn(string adjType)
        {
            gAdjType = adjType;
            InitializeComponent();
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            AddAdjustmentIn addAdjustmentIn = new AddAdjustmentIn(gAdjType);
            addAdjustmentIn.ShowDialog();
        }

        private void AdjustmentIn_Load(object sender, EventArgs e)
        {
            this.Text = gAdjType.Equals("in") ? "Adjusment In" : "Adjustment Out";
        }
    }
}
