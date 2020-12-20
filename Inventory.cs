using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AB.UI_Class;
using AB.API_Class.Branch;
using AB.API_Class.Warehouse;
namespace AB
{
    public partial class Inventory : Form
    {
        utility_class utilityc = new utility_class();
        branch_class branchc = new branch_class();
        warehouse_class warehousec = new warehouse_class();
        DataTable dtBranches = new DataTable();
        DataTable dtWarehouse = new DataTable();
        int cBranch = 0, cWarehouse = 0, cDate = 0;
        public Inventory()
        {
            InitializeComponent();
        }

        private void Inventory_Load(object sender, EventArgs e)
        {
            loadBranches();
            dtDate.Value = DateTime.Now;
            cBranch = 0;
            cWarehouse = 0;
            cDate = 0;
        }

        public void loadBranches()
        {
            int isAdmin = 0;
            string branch = "";
            dtBranches = branchc.returnBranches();
            cmbBranches.Items.Clear();
            if (Login.jsonResult != null)
            {
                foreach (var x in Login.jsonResult)
                {
                    if (x.Key.Equals("data"))
                    {
                        JObject jObjectData = JObject.Parse(x.Value.ToString());
                        foreach (var y in jObjectData)
                        {
                            if (y.Key.Equals("branch"))
                            {
                                branch = y.Value.ToString();
                            }
                            else if (y.Key.Equals("isAdmin"))
                            {

                                if (y.Value.ToString().ToLower() == "false" || y.Value.ToString() == "")
                                {
                                    foreach (DataRow row in dtBranches.Rows)
                                    {
                                        if (row["code"].ToString() == branch)
                                        {
                                            cmbBranches.Items.Add(row["name"].ToString());
                                            if (cmbBranches.Items.Count > 0)
                                            {
                                                cmbBranches.SelectedIndex = 0;
                                            }
                                            return;
                                        }
                                    }
                                }
                                else
                                {
                                    isAdmin += 1;
                                    break;
                                }
                            }
                            else if (y.Key.Equals("isAccounting"))
                            {
                                if (y.Value.ToString().ToLower() == "false" || y.Value.ToString() == "")
                                {
                                    foreach (DataRow row in dtBranches.Rows)
                                    {
                                        if (row["code"].ToString() == branch && isAdmin <= 0)
                                        {
                                            cmbBranches.Items.Add(row["name"].ToString());
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (cmbBranches.Items.Count <= 0)
                {
                    foreach (DataRow row in dtBranches.Rows)
                    {
                        cmbBranches.Items.Add(row["name"]);
                    }
                }
            }
            if (cmbBranches.Items.Count > 0)
            {
                string branchName = "";
                foreach (DataRow row in dtBranches.Rows)
                {
                    if (row["code"].ToString() == branch)
                    {
                        branchName = row["name"].ToString();
                        break;
                    }
                }
                cmbBranches.SelectedIndex = cmbBranches.Items.IndexOf(branchName);
            }
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
            }else
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

        public void loadWarehouse()
        {
            string branchCode = "";
            string warehouse = "";
            cmbWarehouse.Items.Clear();
            cmbWarehouse.Items.Add("All");
            foreach (DataRow row in dtBranches.Rows)
            {
                if (cmbBranches.Text.Equals(row["name"].ToString()))
                {
                    branchCode = row["code"].ToString();
                    break;
                }
            }
            dtWarehouse = warehousec.returnWarehouse(branchCode);
            foreach (DataRow row in dtWarehouse.Rows)
            {
                cmbWarehouse.Items.Add(row["whsename"]);
            }
            if (cmbWarehouse.Items.Count > 0)
            {
                string whseName = "";
                foreach (DataRow row in dtWarehouse.Rows)
                {
                    if (row["whsecode"].ToString() == warehouse)
                    {
                        whseName = row["whsename"].ToString();
                        break;
                    }
                }
                cmbWarehouse.SelectedIndex = cmbWarehouse.Items.IndexOf(whseName);
                if (cmbWarehouse.Text == "")
                {
                    cmbWarehouse.SelectedIndex = 0;
                }
            }
        }

        public void loadData()
        {
            if (Login.jsonResult != null)
            {
                Cursor.Current = Cursors.WaitCursor;
                string token = "";
                foreach (var x in Login.jsonResult)
                {
                    if (x.Key.Equals("token"))
                    {
                        token = x.Value.ToString();
                    }
                }
                if (!token.Equals(""))
                {
                    AutoCompleteStringCollection auto = new AutoCompleteStringCollection();
                    dgv.Rows.Clear();
                    var client = new RestClient(utilityc.URL);
                    client.Timeout = -1;
                    var request = new RestRequest("/api/inv/warehouse/report?branch=" + findCode(cmbBranches.Text, "Branch") + "&from_date=" + dtDate.Value.ToString("yyyy-MM-dd") + "&to_date=" + dtDate.Value.ToString("yyyy-MM-dd") + "&whse=" + findCode(cmbWarehouse.Text, "Warehouse"));
                    //Console.WriteLine("/api/inv/warehouse/report?branch=" + cmbBranches.Text + "&from_date=" + dtDate.Value.ToString("yyyy-MM-dd") + "&to_date=" + dtDate.Value.ToString("yyyy-MM-dd") + "&whse=" + cmbWarehouse.Text);
                    request.AddHeader("Authorization", "Bearer " + token);
                    var response = client.Execute(request);
                    JObject jObject = new JObject();
                    jObject = JObject.Parse(response.Content.ToString());
                    //Console.WriteLine(jObject);
                    bool isSuccess = false;
                    foreach (var x in jObject)
                    {
                        if (x.Key.Equals("success"))
                        {
                            isSuccess = Convert.ToBoolean(x.Value.ToString());
                            break;
                        }
                    }
                    if (isSuccess)
                    {
                        foreach (var x in jObject)
                        {
                            if (x.Key.Equals("data"))
                            {
                                if(x.Value.ToString() != "[]")
                                {
                                    JArray jsonArray = JArray.Parse(x.Value.ToString());
                                    for (int i = 0; i < jsonArray.Count(); i++)
                                    {
                                        JObject data = JObject.Parse(jsonArray[i].ToString());
                                        string itemCode = "";
                                        double beginning = 0.00, received = 0.00, transferred = 0.00, sold = 0.00, available = 0.00, adjin = 0.00, adjout = 0.00, pullout = 0.00, transferIn = 0.00, totalIn = 0.00, totalOut = 0.00;
                                        foreach (var q in data)
                                        {
                                            if (q.Key.Equals("item_code"))
                                            {
                                                itemCode = q.Value.ToString();
                                                auto.Add(itemCode);
                                            }
                                            else if (q.Key.Equals("Beginning"))
                                            {
                                                beginning = Convert.ToDouble(q.Value.ToString());
                                            }
                                            else if (q.Key.Equals("Received"))
                                            {
                                                received = Convert.ToDouble(q.Value.ToString());
                                            }
                                            else if (q.Key.Equals("TransferIn"))
                                            {
                                                transferIn = Convert.ToDouble(q.Value.ToString());
                                            }
                                            else if (q.Key.Equals("TotalIn"))
                                            {
                                                totalIn = Convert.ToDouble(q.Value.ToString());
                                            }
                                            else if (q.Key.Equals("Transferred"))
                                            {
                                                transferred = Convert.ToDouble(q.Value.ToString());
                                            }
                                            else if (q.Key.Equals("Sold"))
                                            {
                                                sold = Convert.ToDouble(q.Value.ToString());
                                            }
                                            else if (q.Key.Equals("TotalOut"))
                                            {
                                                totalOut = Convert.ToDouble(q.Value.ToString());
                                            }
                                            else if (q.Key.Equals("Available"))
                                            {
                                                available = Convert.ToDouble(q.Value.ToString());
                                            }

                                            else if (q.Key.Equals("AdjIn"))
                                            {
                                                adjin = Convert.ToDouble(q.Value.ToString());
                                            }
                                            else if (q.Key.Equals("AdjOut"))
                                            {
                                                adjout = Convert.ToDouble(q.Value.ToString());
                                            }
                                            else if (q.Key.Equals("PullOut"))
                                            {
                                                pullout = Convert.ToDouble(q.Value.ToString());
                                            }
                                        }
                                        if (!string.IsNullOrEmpty(txtSearch.Text.ToString().Trim()))
                                        {
                                            if (txtSearch.Text.ToString().Trim().Contains(itemCode))
                                            {
                                            
                                                dgv.Rows.Add(itemCode, Convert.ToDecimal(string.Format("{0:0.00}", beginning)), Convert.ToDecimal(string.Format("{0:0.00}", received)), Convert.ToDecimal(string.Format("{0:0.00}", transferIn)), Convert.ToDecimal(string.Format("{0:0.00}", adjin)), Convert.ToDecimal(string.Format("{0:0.00}", totalIn)), Convert.ToDecimal(string.Format("{0:0.00}", transferred)), Convert.ToDecimal(string.Format("{0:0.00}", adjout)), Convert.ToDecimal(string.Format("{0:0.00}", pullout)), Convert.ToDecimal(string.Format("{0:0.00}", sold)), Convert.ToDecimal(string.Format("{0:0.00}", totalOut)), Convert.ToDecimal(string.Format("{0:0.00}", available)));
                                            }
                                        }
                                        else
                                        {
                                            dgv.Rows.Add(itemCode, Convert.ToDecimal(string.Format("{0:0.00}", beginning)), Convert.ToDecimal(string.Format("{0:0.00}", received)), Convert.ToDecimal(string.Format("{0:0.00}", transferIn)), Convert.ToDecimal(string.Format("{0:0.00}", adjin)), Convert.ToDecimal(string.Format("{0:0.00}",totalIn)), Convert.ToDecimal(string.Format("{0:0.00}", transferred)), Convert.ToDecimal(string.Format("{0:0.00}", adjout)), Convert.ToDecimal(string.Format("{0:0.00}", pullout)), Convert.ToDecimal(string.Format("{0:0.00}", sold)), Convert.ToDecimal(string.Format("{0:0.00}", totalOut)), Convert.ToDecimal(string.Format("{0:0.00}", available)));
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        string msg = "No message response found";
                        foreach (var x in jObject)
                        {
                            if (x.Key.Equals("message"))
                            {
                                msg = x.Value.ToString();
                            }
                        }
                        if(msg.Equals("Token is invalid"))
                        {
                            MessageBox.Show("Your login session is expired. Please login again", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            MessageBox.Show(msg, "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    txtSearch.AutoCompleteCustomSource = auto;
                }
                Cursor.Current = Cursors.Default;
            }
            colors();
        }

        public void colors()
        {
            int rowscount = dgv.Rows.Count;

            for (int i = 0; i < rowscount; i++)
            {
                if (!(dgv.Rows[i].Cells[1].Value == null) || !(dgv.Rows[i].Cells[5].Value == null) || !(dgv.Rows[i].Cells[10].Value == null) ||  !(dgv.Rows[i].Cells[11].Value == null))
                {
                    dgv.Rows[i].Cells[1].Style.BackColor = Color.FromArgb(230, 225, 90);
                    dgv.Rows[i].Cells[5].Style.BackColor = Color.FromArgb(230, 225, 90);
                    dgv.Rows[i].Cells[10].Style.BackColor = Color.FromArgb(230, 225, 90);
                    dgv.Rows[i].Cells[11].Style.BackColor = Color.FromArgb(230, 225, 90);
                }
                if ( !(dgv.Rows[i].Cells[2].Value == null) || !(dgv.Rows[i].Cells[3].Value == null) || !(dgv.Rows[i].Cells[4].Value == null))
                {
                    dgv.Rows[i].Cells[2].Style.BackColor = Color.FromArgb(255, 255, 128);
                    dgv.Rows[i].Cells[3].Style.BackColor = Color.FromArgb(255, 255, 128);
                    dgv.Rows[i].Cells[4].Style.BackColor = Color.FromArgb(255, 255, 128);
                }
                if (!(dgv.Rows[i].Cells[6].Value == null) || !(dgv.Rows[i].Cells[7].Value == null) || !(dgv.Rows[i].Cells[8].Value == null) || !(dgv.Rows[i].Cells[9].Value == null))
                {
                    dgv.Rows[i].Cells[6].Style.BackColor = Color.FromArgb(192, 255, 192);
                    dgv.Rows[i].Cells[7].Style.BackColor = Color.FromArgb(192, 255, 192);
                    dgv.Rows[i].Cells[8].Style.BackColor = Color.FromArgb(192, 255, 192);
                    dgv.Rows[i].Cells[9].Style.BackColor = Color.FromArgb(192, 255, 192);
                }
            }
        }

        private void dtDate_ValueChanged(object sender, EventArgs e)
        {
            if (cDate <= 0)
            {
                loadData();
            }
        }

        private void cmbBranches_SelectedValueChanged(object sender, EventArgs e)
        {
           if(cBranch <= 0)
            {
                loadWarehouse();
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

        private void cmbWarehouse_SelectedValueChanged(object sender, EventArgs e)
        {
          if(cWarehouse <= 0)
            {
                loadData();
            }
        }

        private void btnrefresh_Click(object sender, EventArgs e)
        {
            loadData();
        }

    }
}
