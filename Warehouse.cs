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

        private void dgv_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgv.Rows.Count > 0)
            {
                if (e.ColumnIndex == 9)
                {
                    if (e.RowIndex >= 0)
                    {
                        PriceList_Row row = new PriceList_Row();
                        row.selectedID = string.IsNullOrEmpty(dgv.CurrentRow.Cells["pricelist_id"].Value.ToString()) ? 0 : Convert.ToInt32(dgv.CurrentRow.Cells["pricelist_id"].Value.ToString());
                        row.lblPriceList.Text = dgv.CurrentRow.Cells["pricelist"].Value.ToString();
                        row.ShowDialog();
                    }
                }
            }
        }

        public void loadData()
        {
            string branchCode = findCode(cmbBranches.Text, "Branch");
            dtWarehouse = warehousec.returnWarehouse(branchCode);
            dgv.Rows.Clear();
            foreach (DataRow row in dtWarehouse.Rows)
            {

                //dt.Columns.Add("pricelist");
                //dt.Columns.Add("pricelist_id");
                //dt.Columns.Add("cash_account");
                //dt.Columns.Add("short_account");
                //dt.Columns.Add("pullout_whse");

                dgv.Rows.Add(row["id"].ToString(),row["pricelist"].ToString(), row["pricelist_id"].ToString(), row["branch"].ToString(), row["whsecode"].ToString(), row["whsename"].ToString(), row["cash_account"].ToString(), row["short_account"].ToString(), row["pullout_whse"].ToString());
            }
        }

    }
}
