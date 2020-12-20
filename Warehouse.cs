using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AB.API_Class.Branch;
using AB.API_Class.Warehouse;
namespace AB
{
    public partial class Warehouse : Form
    {
        public Warehouse()
        {
            InitializeComponent();
        }
        DataTable dtBranches, dtWarehouse;
        branch_class branchc = new branch_class();
        warehouse_class warehousec = new warehouse_class();
        int cBranch = 0;
        private void Warehouse_Load(object sender, EventArgs e)
        {
            dtBranches = new DataTable();
            dtWarehouse = new DataTable();
            loadBranches();
            loadData();
        }

        public void loadBranches()
        {
            dtBranches = branchc.returnBranches();
            cmbBranches.Items.Clear();
            cmbBranches.Items.Add("All");
            foreach (DataRow row in dtBranches.Rows)
            {
                cmbBranches.Items.Add(row["name"].ToString());
            }
            cmbBranches.SelectedIndex = 0;
        }

        public string findCode(string value, string typee)
        {
            string result = "";
            if (typee.Equals("Warehouse"))
            {
                foreach (DataRow row in dtWarehouse.Rows)
                {
                    if (row["whsename"].ToString() == value)
                    {
                        result = row["whsecode"].ToString();
                        break;
                    }
                }
            }
            else
            {
                foreach (DataRow row in dtBranches.Rows)
                {
                    if (row["name"].ToString() == value)
                    {
                        result = row["code"].ToString();
                        break;
                    }
                }
            }
            return result;
        }

        private void cmbBranches_SelectedValueChanged(object sender, EventArgs e)
        {
            if(cBranch <= 0)
            {
                loadData();
            }
        }

        public void loadData()
        {
            string branchCode = findCode(cmbBranches.Text, "Branch");
            dtWarehouse = warehousec.returnWarehouse(branchCode);
            dgv.Rows.Clear();
            foreach (DataRow row in dtWarehouse.Rows)
            {
                dgv.Rows.Add(row["id"].ToString(), row["branch"].ToString(), row["whsecode"].ToString(), row["whsename"].ToString());
            }
        }

    }
}
