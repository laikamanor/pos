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
using AB.API_Class.User;
using AB.API_Class.Branch;
using AB.API_Class.Warehouse;
using AB.API_Class.Payment_Type;
namespace AB
{
    public partial class SalesReport : Form
    {
        DataTable dtBranch, dtWarehouse;
        branch_class branchc = new branch_class();
        utility_class utilityc = new utility_class();
        user_clas userc = new user_clas();
        warehouse_class warehousec = new warehouse_class();
        paymenttype_class paymenttypec = new paymenttype_class();
        DataTable dtSalesAgent = new DataTable();
        int cBranch = 1, cWarehouse = 1, cUser = 1, cTransType = 1;
        public SalesReport()
        {
            InitializeComponent();
        }

        private void SalesReport_Load(object sender, EventArgs e)
        {
            loadBranch();
            loadWarehouse();
            loadSalesAgent(cmbsales, false);
            loadTransType();
            loadData();
            cBranch = 0;
            cWarehouse = 0;
            cUser = 0;
            cTransType = 0;
        }
        public void loadBranch()
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
                    else
                    {
                        cmbBranch.SelectedIndex = 0;
                    }
                }
                cmbBranch.SelectedIndex = cmbBranch.Items.IndexOf(branchName);
            }
        }

        public void loadWarehouse()
        {
            string branchCode = "";
            string warehouse = "";
            foreach (DataRow row in dtBranch.Rows)
            {
                if (cmbBranch.Text.Equals(row["name"].ToString()))
                {
                    branchCode = row["code"].ToString();
                    break;
                }
              
            }

            int isAdmin = 0;
            dtWarehouse = warehousec.returnWarehouse(branchCode);
            foreach (DataRow row in dtWarehouse.Rows)
            {
                cmbWhse.Items.Add(row["whsename"]);
            }
            cmbWhse.Items.Clear();
            if (Login.jsonResult != null)
            {
                foreach (var x in Login.jsonResult)
                {
                    if (x.Key.Equals("data"))
                    {
                        JObject jObjectData = JObject.Parse(x.Value.ToString());
                        foreach (var y in jObjectData)
                        {
                            if (y.Key.Equals("whse"))
                            {
                                warehouse = y.Value.ToString();
                            }
                            else if (y.Key.Equals("isAdmin"))
                            {

                                if (y.Value.ToString().ToLower() == "false" || y.Value.ToString() == "")
                                {
                                    foreach (DataRow row in dtWarehouse.Rows)
                                    {
                                        if (row["whsecode"].ToString() == warehouse)
                                        {
                                            cmbWhse.Items.Add(row["whsename"].ToString());
                                            if (cmbWhse.Items.Count > 0)
                                            {
                                                cmbWhse.SelectedIndex = 0;
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
                                    foreach (DataRow row in dtWarehouse.Rows)
                                    {
                                        if (row["whsecode"].ToString() == warehouse && isAdmin <= 0)
                                        {
                                            cmbWhse.Items.Add(row["whsename"].ToString());
                                            break;
                                        }
                                        else
                                        {
                                            isAdmin += 1;
                                            break;
                                        }
                                    }
                                }
                            }
                            else if (y.Key.Equals("isManager"))
                            {
                                if (y.Value.ToString().ToLower() == "false" || y.Value.ToString() == "")
                                {
                                    foreach (DataRow row in dtWarehouse.Rows)
                                    {
                                        if (row["whsecode"].ToString() == warehouse && isAdmin <= 0)
                                        {
                                            cmbWhse.Items.Add(row["whsename"].ToString());
                                            break;
                                        }
                                        else
                                        {
                                            isAdmin += 1;
                                            break;
                                        }
                                    }
                                }
                            }
                            else if (y.Key.Equals("isCashier"))
                            {
                                if (y.Value.ToString().ToLower() == "false" || y.Value.ToString() == "")
                                {
                                    foreach (DataRow row in dtWarehouse.Rows)
                                    {
                                        if (row["whsecode"].ToString() == warehouse && isAdmin <= 0)
                                        {
                                            cmbWhse.Items.Add(row["whsename"].ToString());
                                            break;
                                        }
                                        else
                                        {
                                            isAdmin += 1;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (cmbWhse.Items.Count <= 0)
                {
                    cmbWhse.Items.Add("All");
                    foreach (DataRow row in dtWarehouse.Rows)
                    {
                        cmbWhse.Items.Add(row["whsename"]);
                    }
                }
            }
            if (cmbWhse.Items.Count > 0)
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
               cmbWhse.SelectedIndex = cmbWhse.Items.IndexOf(whseName);
                if (cmbWhse.Text == "")
                {
                    cmbWhse.SelectedIndex = 0;
                }
            }
        }

        public string findWarehouseCode()
        {
            string result = "";
            foreach (DataRow row in dtWarehouse.Rows)
            {
                if (row["whsename"].ToString() == cmbWhse.Text)
                {
                    result = row["whsecode"].ToString();
                    break;
                }
            }
            return result;
        }

        public string findBranchCode()
        {
            string result = "";
            foreach (DataRow row in dtBranch.Rows)
            {
                if (row["name"].ToString() == cmbBranch.Text)
                {
                    result = row["code"].ToString();
                    break;
                }
            }
            return result;
        }

        public void loadSalesAgent(ComboBox cmb, bool isCashier)
        {
            string sBranch = "?branch=" + findBranchCode();
            string sWhse = "&whse=" + findWarehouseCode();
            //string sCashier = "&isCashier=" + (isCashier ? "1" : "");
            string sCashier = (isCashier ? "&isCashier=1" : "");
            DataTable adtUsers = new DataTable();
            adtUsers = userc.returnUsers(sBranch + sWhse + sCashier);
            dtSalesAgent = adtUsers;

            cmb.Items.Clear();
            cmb.Items.Add("All");
            foreach (DataRow r0w in adtUsers.Rows)
            {
                cmb.Items.Add(r0w["fullname"].ToString());
            }
            cmb.SelectedIndex = 0;
        }

        public void loadTransType()
        {
            DataTable dtTransType = new DataTable();
            dtTransType = paymenttypec.loadPaymentType("sales");

            cmbTransType.Items.Clear();
            cmbTransType.Items.Add("All");
            foreach (DataRow r0w in dtTransType.Rows)
            {
                cmbTransType.Items.Add(r0w["code"].ToString());
            }
            cmbTransType.SelectedIndex= (cmbTransType.Items.Contains("CASH") ? cmbTransType.Items.IndexOf("CASH") : 0);
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
                    dgv.Rows.Clear();
                    var client = new RestClient(utilityc.URL);
                    client.Timeout = -1;

                    int salesID = 0;

                    foreach (DataRow r0wUsers in dtSalesAgent.Rows)
                    {

                        if (r0wUsers["fullname"].ToString() == cmbsales.Text)
                        {
                            salesID = Convert.ToInt32(r0wUsers["userid"].ToString());
                        }
                    }
                    //}
                    //foreach (DataRow r0wSales in dtCashier.Rows)
                    //{
                    //    if (r0wSales["username"].ToString() == cmbCashier.Text)
                    //    {
                    //        cashierID = Convert.ToInt32(r0wSales["userid"].ToString());
                    //    }
                    //}

                    string sSales = (salesID <= 0 ? "&user_id=" : "&user_id=" + salesID);
                    string sBranch = "&branch=" +findBranchCode();
                    string sWarehouse = "&whse=" + findWarehouseCode();
                    string sTransType = (cmbTransType.SelectedIndex <= 0 ? "&transtype=" : "&transtype=" + cmbTransType.Text);

                    var request = new RestRequest("/api/sales/report?date=" + dtDate.Value.ToString("yyyy-MM-dd") + sSales + sWarehouse + sTransType);
                    Console.WriteLine("/api/sales/report?date=" + dtDate.Value.ToString("yyyy-MM-dd") + sSales + sBranch + sWarehouse + sTransType);
                    request.AddHeader("Authorization", "Bearer " + token);
                    var response = client.Execute(request);
                    JObject jObject = new JObject();
                    jObject = JObject.Parse(response.Content.ToString());
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
                                if (x.Value.ToString() != "{}")
                                {
                                    JObject jObjectData = JObject.Parse(x.Value.ToString());
                                    foreach (var y in jObjectData)
                                    {
                                        if (y.Key.Equals("header"))
                                        {
                                            JObject jObjectHeader = JObject.Parse(y.Value.ToString());
                                            foreach (var w in jObjectHeader)
                                            {
                                                if (w.Key.Equals("cashsales"))
                                                {
                                                    lblSalesCashSales.Text = (w.Value.ToString() != "" ? Convert.ToDouble(w.Value.ToString()).ToString("n2") : "0.00");
                                                }
                                                else if (w.Key.Equals("arsales"))
                                                {
                                                    lblARSales.Text = (w.Value.ToString() != "" ? Convert.ToDouble(w.Value.ToString()).ToString("n2") : "0.00");
                                                }
                                                else if (w.Key.Equals("agentsales"))
                                                {
                                                    lblAgentARSales.Text = (w.Value.ToString() != "" ? Convert.ToDouble(w.Value.ToString()).ToString("n2") : "0.00");
                                                }
                                                else if (w.Key.Equals("disc_amount"))
                                                {
                                                    lblDiscountAmount.Text = (w.Value.ToString() != "" ? Convert.ToDouble(w.Value.ToString()).ToString("n2") : "0.00");
                                                }
                                                else if (w.Key.Equals("gross"))
                                                {
                                                    lblGrossSales.Text = (w.Value.ToString() != "" ? Convert.ToDouble(w.Value.ToString()).ToString("n2") : "0.00");
                                                }
                                            }
                                        }
                                        else if (y.Key.Equals("row"))
                                        {
                                            dgv.Rows.Clear();
                                            JArray jArraySalesRows = JArray.Parse(y.Value.ToString());
                                            for (int aa = 0; aa < jArraySalesRows.Count(); aa++)
                                            {

                                                JObject jObjectSalesRows = JObject.Parse(jArraySalesRows[aa].ToString());
                                                int transNumber = 0, id = 0;
                                                string referenceNumber = "", customerCode = "", processedBy = "", transType = "";
                                                double gross = 0.00, docTotal = 0.00;
                                                foreach (var z in jObjectSalesRows)
                                                {
                                                    if (z.Key.Equals("id"))
                                                    {
                                                        id = Convert.ToInt32(z.Value.ToString());
                                                    }
                                                    else if (z.Key.Equals("transnumber"))
                                                    {
                                                        transNumber = Convert.ToInt32(z.Value.ToString());
                                                    }
                                                    else if (z.Key.Equals("reference"))
                                                    {
                                                        referenceNumber = z.Value.ToString();
                                                    }
                                                    else if (z.Key.Equals("gross"))
                                                    {
                                                        gross = Convert.ToDouble(z.Value.ToString());
                                                    }
                                                    else if (z.Key.Equals("doctotal"))
                                                    {
                                                        docTotal = Convert.ToDouble(z.Value.ToString());
                                                    }
                                                    else if (z.Key.Equals("user"))
                                                    {
                                                        processedBy = z.Value.ToString();
                                                    }
                                                    else if (z.Key.Equals("cust_code"))
                                                    {
                                                        customerCode = z.Value.ToString();
                                                    }
                                                    else if (z.Key.Equals("transtype"))
                                                    {
                                                        transType = z.Value.ToString();
                                                    }
                                                    //else if (z.Key.Equals("SalesType"))
                                                    //{
                                                    //    salesType = z.Value.ToString();
                                                    //}
                                                    //else if (z.Key.Equals("PaymentType"))
                                                    //{
                                                    //    paymentType = z.Value.ToString();
                                                    //}
                                                }
                                                dgv.Rows.Add(id,transNumber, referenceNumber, gross.ToString("n2"), docTotal.ToString("n2"),customerCode, transType, processedBy);
                                            }
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
                        if (msg.Equals("Token is invalid"))
                        {
                            MessageBox.Show("Your login session is expired. Please login again", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            MessageBox.Show(msg, "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
                Cursor.Current = Cursors.Default;
            }
            lblNoDataFound.Visible = (dgv.Rows.Count > 0 ? false : true);
        }

        private void dtDate_ValueChanged(object sender, EventArgs e)
        {
            loadData();
        }

        private void cmbsales_SelectedValueChanged(object sender, EventArgs e)
        {
            if(cUser <= 0)
            {
                loadData();
            }
        }

        private void cmbBranch_SelectedValueChanged(object sender, EventArgs e)
        {
            if(cBranch <= 0)
            {
                loadWarehouse();
            }
        }


        private void dgv_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgv.Rows.Count > 0) 
            {
                SalesReportItems salesReportItems = new SalesReportItems();
                salesReportItems.URLDetails = "/api/sales/details/" + dgv.CurrentRow.Cells["id"].Value.ToString();
                salesReportItems.ShowDialog();
            }
        }

        private void cmbWarehouse_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cWarehouse <= 0)
            {
                if (cmbWhse.Text == "")
                {
                    loadData();
                }
                else
                {
                    loadSalesAgent(cmbsales, false);
                }
            }
        }

        private void cmbTransType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cTransType <= 0)
            {
                loadData();
            }
        }
    }
}
