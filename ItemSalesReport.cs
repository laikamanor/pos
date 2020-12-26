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
using Newtonsoft.Json.Linq;
using RestSharp;
using AB.UI_Class;
namespace AB
{
    public partial class ItemSalesReport : Form
    {
        public ItemSalesReport()
        {
            InitializeComponent();
        }
        int cBranch = 1, cWhse = 1, cFromDate = 1, cTodate = 1, cFromTime = 1, cToTime = 1, cTransType = 1;
        warehouse_class warehousec = new warehouse_class();
        branch_class branchc = new branch_class();
        utility_class utilityc = new utility_class();
        DataTable dtBranch = new DataTable(), dtWhse = new DataTable();
        private void cmbFromTime_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cFromTime <= 0)
            {
                loadData();
            }
        }

        private void cmbToTime_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cToTime <= 0)
            {
                loadData();
            }
        }

        private void dtFromDate_ValueChanged(object sender, EventArgs e)
        {
            if(cFromDate <= 0)
            {
                loadData();
            }
        }

        private void dtToDate_ValueChanged(object sender, EventArgs e)
        {
            if(cTodate <= 0)
            {
                loadData();
            }
        }

        private void cmbTransType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cTransType <= 0)
            {
                loadData();
            }
        }

        private void cmbWhse_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cWhse <= 0)
            {
                loadData();
            }
        }

        private void cmbBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cBranch <= 0)
            {
                loadWarehouse();
            }
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.Equals(Keys.Enter))
            {
                loadData();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            loadData();
        }

        private void dgv_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(dgv.Rows.Count > 0)
            {
                if(e.RowIndex >= 0)
                {
                    string sBranch = "?branch=" + findCode(cmbBranch.Text, "Branch");
                    string sWhse = "&whsecode=" + findCode(cmbWhse.Text, "Warehouse");
                    string sTransType = "&transtype=" + (cmbTransType.SelectedIndex == 0 || cmbTransType.Text == "All" ? "" : cmbTransType.Text);
                    string sFromTime = "&from_time=" + cmbFromTime.Text;
                    string sToTime = "&to_time=" + cmbToTime.Text;
                    string sFromDate = "&from_date=" + dtFromDate.Value.ToString("yyyy-MM-dd");
                    string sToDate = "&to_date=" + dtToDate.Value.ToString("yyyy-MM-dd");
                    string sItemCode = "&item_code=" + dgv.CurrentRow.Cells["item_code"].Value.ToString();
                    string sDiscount = "&discount=" + dgv.CurrentRow.Cells["discprcnt"].Value.ToString();
                    string URL = "/api/report/item/summary/details" + sBranch + sWhse + sTransType + sFromTime + sToTime + sFromDate + sToDate + sItemCode + sDiscount;
                    ItemSalesReport_Details details = new ItemSalesReport_Details();
                    details.URL = URL;
                    details.ShowDialog();
                }
            }
        }

        private void ItemSalesReport_Load(object sender, EventArgs e)
        {
            dgv.Columns["item_code"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            loadBranches();
            loadWarehouse();
            loadTenderType();
            cmbFromTime.SelectedIndex = 0;
            cmbToTime.SelectedIndex = cmbToTime.Items.Count - 1;
            loadData();
            cBranch = 0;
            cWhse = 0;
            cFromDate = 0;
            cTodate = 0;
            cFromDate = 0;
            cToTime = 0;
            cFromTime = 0;
            cTransType = 0;
        }

        public void loadBranches()
        {
            int isAdmin = 0;
            string branch = "";
            dtBranch = branchc.returnBranches();
            cmbBranch.Items.Clear();
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
                                    foreach (DataRow row in dtBranch.Rows)
                                    {
                                        if (row["code"].ToString() == branch)
                                        {
                                            cmbBranch.Items.Add(row["name"].ToString());
                                            if (cmbBranch.Items.Count > 0)
                                            {
                                                cmbBranch.SelectedIndex = 0;
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
                                    foreach (DataRow row in dtBranch.Rows)
                                    {
                                        if (row["code"].ToString() == branch && isAdmin <= 0)
                                        {
                                            cmbBranch.Items.Add(row["name"].ToString());
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (cmbBranch.Items.Count <= 0)
                {
                    foreach (DataRow row in dtBranch.Rows)
                    {
                        cmbBranch.Items.Add(row["name"]);
                    }
                }
            }
            if (cmbBranch.Items.Count > 0)
            {
                string branchName = "";
                foreach (DataRow row in dtBranch.Rows)
                {
                    if (row["code"].ToString() == branch)
                    {
                        branchName = row["name"].ToString();
                        break;
                    }
                }
                cmbBranch.SelectedIndex = cmbBranch.Items.IndexOf(branchName);
            }
        }

        public string findCode(string value, string typee)
        {
            string result = "";
            if (typee.Equals("Warehouse"))
            {
                foreach (DataRow row in dtWhse.Rows)
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
                foreach (DataRow row in dtBranch.Rows)
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
            cmbWhse.Items.Clear();
            cmbWhse.Items.Add("All-Good");
            foreach (DataRow row in dtBranch.Rows)
            {
                if (cmbBranch.Text.Equals(row["name"].ToString()))
                {
                    branchCode = row["code"].ToString();
                    break;
                }
            }
            dtWhse = warehousec.returnWarehouse(branchCode);
            foreach (DataRow row in dtWhse.Rows)
            {
                cmbWhse.Items.Add(row["whsename"]);
            }
            if (cmbWhse.Items.Count > 0)
            {
                string whseName = "";
                foreach (DataRow row in dtWhse.Rows)
                {
                    if (row["whsecode"].ToString() == warehouse)
                    {
                        whseName = row["whsename"].ToString();
                        break;
                    }
                }
                cmbWhse.SelectedIndex = cmbWhse.Items.IndexOf(whseName);
                if (cmbWhse.Text == "")
                {
                    cmbWhse.SelectedIndex = 0;
                }
            }
        }

        public void loadTenderType()
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
                    var client = new RestClient(utilityc.URL);
                    client.Timeout = -1;
                    //string branch = "A1-S";
                    var request = new RestRequest("/api/sales/type/get_all");
                    request.AddHeader("Authorization", "Bearer " + token);
                    var response = client.Execute(request);
                    JObject jObject = new JObject();
                    cmbTransType.Items.Clear();
                    cmbTransType.Items.Add("All");
                    jObject = JObject.Parse(response.Content.ToString());
                    bool isSuccess = false;
                    foreach (var x in jObject)
                    {
                        if (x.Key.Equals("success"))
                        {
                            isSuccess = Convert.ToBoolean(x.Value.ToString());
                        }
                    }
                    if (isSuccess)
                    {
                        foreach (var x in jObject)
                        {
                            if (x.Key.Equals("data"))
                            {
                                if (x.Value.ToString() != "[]")
                                {
                                    JArray jsonArray = JArray.Parse(x.Value.ToString());
                                    for (int i = 0; i < jsonArray.Count(); i++)
                                    {
                                        JObject data = JObject.Parse(jsonArray[i].ToString());
                                        string code = "";
                                        foreach (var q in data)
                                        {
                                            if (q.Key.Equals("code"))
                                            {
                                                code = q.Value.ToString();
                                            }
                                        }
                                        cmbTransType.Items.Add(code);
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
                        MessageBox.Show(msg, "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                cmbTransType.SelectedIndex = 0;
                Cursor.Current = Cursors.Default;
            }
        }

        public void loadData()
        {
            if (Login.jsonResult != null)
            {
                Cursor.Current = Cursors.WaitCursor;
                AutoCompleteStringCollection auto = new AutoCompleteStringCollection();
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
                    var client = new RestClient(utilityc.URL);
                    client.Timeout = -1;
                    string sBranch = "?branch=" + findCode(cmbBranch.Text, "Branch");
                    string sWhse = "&whsecode=" + findCode(cmbWhse.Text, "Warehouse");
                    string sTransType = "&transtype=" + (cmbTransType.SelectedIndex == 0 || cmbTransType.Text == "All" ? "" : cmbTransType.Text);
                    string sFromTime = "&from_time=" + cmbFromTime.Text;
                    string sToTime = "&to_time=" + cmbToTime.Text;
                    string sFromDate = "&from_date=" + dtFromDate.Value.ToString("yyyy-MM-dd");
                    string sToDate = "&to_date=" + dtToDate.Value.ToString("yyyy-MM-dd");
                    var request = new RestRequest("/api/report/item/summary" + sBranch + sWhse + sTransType + sFromTime + sToTime + sFromDate + sToDate);
                    Console.WriteLine("/api/report/item/summary" + sBranch + sWhse + sTransType + sFromTime + sToTime + sFromDate + sToDate);
                    request.AddHeader("Authorization", "Bearer " + token);
                    var response = client.Execute(request);
                    JObject jObject = new JObject();
                    jObject = JObject.Parse(response.Content.ToString());
                    dgv.Rows.Clear();
                    bool isSuccess = false;
                    foreach (var x in jObject)
                    {
                        if (x.Key.Equals("success"))
                        {
                            isSuccess = Convert.ToBoolean(x.Value.ToString());
                        }
                    }
                    if (isSuccess)
                    {
                        foreach (var x in jObject)
                        {
                            if (x.Key.Equals("data"))
                            {
                                if (x.Value.ToString() != "[]")
                                {
                                    JArray jsonArray = JArray.Parse(x.Value.ToString());
                                    for (int i = 0; i < jsonArray.Count(); i++)
                                    {
                                        JObject data = JObject.Parse(jsonArray[i].ToString());
                                        string itemCode = "";
                                        double qty = 0.00, unitPrice = 0.00, discPrcnt = 0.00, discAmt = 0.00, grossAmt = 0.00, netAmt = 0.00;
                                        foreach (var q in data)
                                        {

                                            if (q.Key.Equals("item_code"))
                                            {
                                                itemCode = q.Value.ToString();
                                                auto.Add(q.Value.ToString());
                                            }
                                            else if (q.Key.Equals("unit_price"))
                                            {
                                                unitPrice = string.IsNullOrEmpty(q.Value.ToString()) ? 0.00 : Convert.ToDouble(q.Value.ToString());
                                            }
                                            else if (q.Key.Equals("quantity"))
                                            {
                                                qty = string.IsNullOrEmpty(q.Value.ToString()) ? 0.00 : Convert.ToDouble(q.Value.ToString());
                                            }
                                            else if (q.Key.Equals("discprcnt"))
                                            {
                                                discPrcnt = string.IsNullOrEmpty(q.Value.ToString()) ? 0.00 : Convert.ToDouble(q.Value.ToString());
                                            }
                                            else if (q.Key.Equals("disc_amount"))
                                            {
                                                discAmt = string.IsNullOrEmpty(q.Value.ToString()) ? 0.00 : Convert.ToDouble(q.Value.ToString());
                                            }
                                            else if (q.Key.Equals("gross_amount"))
                                            {
                                                grossAmt = string.IsNullOrEmpty(q.Value.ToString()) ? 0.00 : Convert.ToDouble(q.Value.ToString());
                                            }
                                            else if (q.Key.Equals("net_amount"))
                                            {
                                                netAmt = string.IsNullOrEmpty(q.Value.ToString()) ? 0.00 : Convert.ToDouble(q.Value.ToString());
                                            }
                                        }

                                        if (!string.IsNullOrEmpty(txtSearch.Text.ToString().Trim()))
                                        {
                                            if (txtSearch.Text.ToString().Trim().ToLower().Contains(itemCode.ToLower()))
                                            {
                                                dgv.Rows.Add(itemCode, Convert.ToDecimal(string.Format("{0:0.00}", qty)), Convert.ToDecimal(string.Format("{0:0.00}", unitPrice)), Convert.ToDecimal(string.Format("{0:0.00}", discPrcnt)), Convert.ToDecimal(string.Format("{0:0.00}", discAmt)), Convert.ToDecimal(string.Format("{0:0.00}", grossAmt)), Convert.ToDecimal(string.Format("{0:0.00}", netAmt)));
                                            }
                                        }
                                        else
                                        {
                                            dgv.Rows.Add(itemCode, Convert.ToDecimal(string.Format("{0:0.00}", qty)), Convert.ToDecimal(string.Format("{0:0.00}", unitPrice)), Convert.ToDecimal(string.Format("{0:0.00}", discPrcnt)), Convert.ToDecimal(string.Format("{0:0.00}", discAmt)), Convert.ToDecimal(string.Format("{0:0.00}", grossAmt)), Convert.ToDecimal(string.Format("{0:0.00}", netAmt)));
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
                        MessageBox.Show(msg, "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                txtSearch.AutoCompleteCustomSource = auto;
                total();
                Cursor.Current = Cursors.Default;
            }
        }

        public void total()
        {
            double totalDiscAmount = 0.00, totalGrossAmount = 0.00, totalNetAmount = 0.00;
            for(int i = 0; i < dgv.Rows.Count; i++)
            {
                totalDiscAmount += string.IsNullOrEmpty(dgv.Rows[i].Cells["disc_amount"].Value.ToString()) ? 0.00 : Convert.ToDouble(dgv.Rows[i].Cells["disc_amount"].Value.ToString());
                totalGrossAmount += string.IsNullOrEmpty(dgv.Rows[i].Cells["gross_amount"].Value.ToString()) ? 0.00 : Convert.ToDouble(dgv.Rows[i].Cells["gross_amount"].Value.ToString());
                totalNetAmount += string.IsNullOrEmpty(dgv.Rows[i].Cells["net_amount"].Value.ToString()) ? 0.00 : Convert.ToDouble(dgv.Rows[i].Cells["net_amount"].Value.ToString());
            }
            lblDiscAmount.Text = totalDiscAmount.ToString("n2");
            lblGrossAmount.Text = totalGrossAmount.ToString("n2");
            lblTotalNetAmount.Text = totalNetAmount.ToString("n2");
        }

    }
}
