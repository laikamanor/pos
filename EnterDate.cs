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
    public partial class EnterDate : Form
    {
        string gReportType = "";
        public static DateTime dateEntered = new DateTime();
        public EnterDate(string reportType)
        {
            gReportType = reportType;
            InitializeComponent();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            dateEntered = dateTimePicker1.Value;
            reportsDialog reportDialog = new reportsDialog(gReportType);
            reportDialog.ShowDialog();
        }

        private void EnterDate_Load(object sender, EventArgs e)
        {
            dateTimePicker1.Value = DateTime.Now;
        }
    }
}
