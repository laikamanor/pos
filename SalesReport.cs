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
        DataTable dtBranch;
        DataTable dtWarehouse;
        branch_class branchc = new branch_class();
        warehouse_class warehousec = new warehouse_class();
        utility_class utilityc = new utility_class();
        user_clas userc = new user_clas();
        paymenttype_class paymenttypec = new paymenttype_class();
        DataTable dtSalesAgent = new DataTable();
        int cBranch = 1, cUser = 1, cTransType = 1, cCheck = 0, cFromDate = 1, cToDate = 1, cFromTime = 1, cToTime = 1, cWarehouse = 1;
        public SalesReport()
        {
            InitializeComponent();
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
            dtWarehouse = warehousec.returnWarehouse(branchCode);
            cmbWarehouse.Items.Clear();
            int isAdmin = 0;
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
                            else if (y.Key.Equals("isAdmin") || y.Key.Equals("isManager"))
                            {
                                if (y.Value.ToString().ToLower() == "true")
                                {
                                    cmbWarehouse.Items.Add("All");
                                    foreach (DataRow row in dtWarehouse.Rows)
                                    {
                                        cmbWarehouse.Items.Add(row["whsename"].ToString());
                                        cmbWarehouse.SelectedIndex = 0;
                                    }
                                    return;
                                }
                                else
                                {
                                    isAdmin += 1;
                                }
                            }
                        }
                    }
                }
            }
            if (isAdmin > 0)
            {
                string whseName = "";
                foreach (DataRow row in dtWarehouse.Rows)
                {
                    if (row["whsecode"].ToString() == warehouse)
                    {
                        whseName = row["whsename"].ToString();
                        cmbWarehouse.Items.Add(whseName);
                    }
                }
                cmbWarehouse.SelectedIndex = cmbWarehouse.Items.IndexOf(whseName);
            }
        }

        private void SalesReport_Load(object sender, EventArgs e)
        {
            dtBranch = new DataTable();
            dtWarehouse = new DataTable();
            loadBranch();
            loadWarehouse();
            loadSalesAgent();
            loadTransType();
            loadData();
            cBranch = 0;
            cUser = 0;
            cTransType = 0;
            cFromDate = 0;
            cToDate = 0;
            cFromTime = 0;
            cToTime = 0;
            cWarehouse = 0;
            cmbFromTime.SelectedIndex = 0;
            cmbToTime.SelectedIndex = cmbToTime.Items.Count - 1;
            dgv.Columns["gross"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgv.Columns["doctotal"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvitems.Columns["item"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
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

        public void loadSalesAgent()
        {
            string sBranch = "?branch=" + findCode(cmbBranch.Text, "Branch");
            DataTable adtUsers = new DataTable();
            adtUsers = userc.returnUsers(sBranch + "&isSales=1");
            dtSalesAgent = adtUsers;

            cmbsales.Items.Clear();
            cmbsales.Items.Add("All");
            foreach (DataRow r0w in adtUsers.Rows)
            {
                cmbsales.Items.Add(r0w["username"].ToString());
            }
            cmbsales.SelectedIndex = 0;
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
            cmbTransType.SelectedIndex = (cmbTransType.Items.Contains("CASH") ? cmbTransType.Items.IndexOf("CASH") : 0);
        }

        public void clearBillsField()
        {
            txtGrossPrice.Text = "0.00";
            txtDiscountAmount.Text = "0.00";
            txtlAmountPayable.Text = "0.00";
            txtTotalPayment.Text = "0.00";
            txtTenderAmount.Text = "0.00";
            txtChange.Text = "0.00";
            checkSelectAll.Checked = false;
        }

        public void loadData()
        {
            clearBillsField();
            dgvitems.Rows.Clear();
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

                        if (r0wUsers["username"].ToString() == cmbsales.Text)
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
                    string sBranch = "&branch=" + findCode(cmbBranch.Text, "Branch");
                    string sWarehouse = "&whse=" + findCode(cmbWarehouse.Text, "Warehouse");
                    string sTransType = (cmbTransType.SelectedIndex <= 0 ? "&transtype=" : "&transtype=" + cmbTransType.Text);
                    string sDate = "?from_date=" + dtFromDate.Value.ToString("yyyy-MM-dd") + "&to_date=" + dtToDate.Value.ToString("yyyy-MM-dd");
                    string sFromTime = cFromTime > 0 ? "&from_time=" : "&from_time=" + cmbFromTime.Text;

                    string sToTime = cToTime > 0 ? "&to_time=" : "&to_time=" + cmbToTime.Text;
                    var request = new RestRequest("/api/sales/report" + sDate + sSales + sWarehouse + sTransType + sFromTime + sToTime);
                    Console.WriteLine("/api/sales/report" + sDate + sSales + sWarehouse + sTransType + sFromTime + sToTime);
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
                                        if (y.Key.Equals("row"))
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
                                                dgv.Rows.Add(false, id, transNumber, referenceNumber, Convert.ToDecimal(string.Format("{0:0.00}", gross)), Convert.ToDecimal(string.Format("{0:0.00}", docTotal)), customerCode, transType, processedBy);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        lblOrderCount.Text = "ORDERS (" + dgv.Rows.Count.ToString("N0") + ")";
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

        private void dtDate_ValueChanged(object sender, EventArgs e)
        {
           if(cFromDate <= 0)
            {
                loadData();
            }
        }

        private void cmbsales_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cUser <= 0)
            {
                loadData();
            }
        }

        private void cmbBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cBranch <= 0)
            {
                loadWarehouse();
            }
        }

        private void cmbFromTime_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cFromTime <= 0)
            {
                loadData();
            }
        }

        private void cmbWarehouse_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cWarehouse <= 0)
            {
                loadData();
            }
        }

        private void cmbToTime_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cToTime <= 0)
            {
                loadData();
            }
        }

        public void selectOrders(bool value)
        {

            Cursor.Current = Cursors.WaitCursor;
            //DataTable dt = new DataTable();
            if (Login.jsonResult != null)
            {
                dgvitems.Rows.Clear();
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

                    if (value)
                    {
                        for (int i = 0; i < dgv.Rows.Count; i++)
                        {
                            dgv.Rows[i].Cells["selectt"].Value = checkSelectAll.Checked;
                        }
                    }
                    else
                    {
                        int isCheckAll_int = 0;
                        for (int i = 0; i < dgv.Rows.Count; i++)
                        {
                            if (Convert.ToBoolean(dgv.Rows[i].Cells["selectt"].Value.ToString()) == true)
                            {
                                isCheckAll_int += 1;
                            }
                        }
                        if (checkSelectAll.Checked && !isCheckAll_int.Equals(dgv.Rows.Count))
                        {
                            cCheck = 1;
                            checkSelectAll.Checked = false;
                        }
                        else if (!checkSelectAll.Checked && isCheckAll_int.Equals(dgv.Rows.Count))
                        {
                            checkSelectAll.Checked = true;
                        }
                    }

                    dgv.CommitEdit(DataGridViewDataErrorContexts.Commit);
                    JArray jarrayBody = new JArray();
                    for (int i = 0; i < dgv.Rows.Count; i++)
                    {

                        if (Convert.ToBoolean(dgv.Rows[i].Cells["selectt"].Value.ToString()) == true)
                        {
                            jarrayBody.Add(Convert.ToInt32(dgv.Rows[i].Cells["ID"].Value));
                        }
                    }
                    JObject jsonObjectBody = new JObject();
                    jsonObjectBody.Add("ids", jarrayBody);

                    string sTransType = cmbTransType.SelectedIndex == 0 ? "" : cmbTransType.Text;

                    var client = new RestClient(utilityc.URL);
                    client.Timeout = -1;
                    var request = new RestRequest("/api/sales/summary_trans?transtype=" + sTransType + "&transdate=");
                    request.AddHeader("Authorization", "Bearer " + token);
                    request.AddParameter("application/json", jsonObjectBody, ParameterType.RequestBody);
                    request.Method = Method.PUT;
                    var response = client.Execute(request);
                    dgvitems.Rows.Clear();
                    if (response.ErrorMessage == null)
                    {
                        if (response.Content.ToString().Substring(0, 1).Equals("{"))
                        {
                            JObject jObject = new JObject();
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
                                        if (x.Value.ToString() != "{}")
                                        {
                                            JObject jObjectData = JObject.Parse(x.Value.ToString());
                                            foreach (var y in jObjectData)
                                            {
                                                if (y.Key.Equals("header"))
                                                {
                                                    JObject jObjectHeader = JObject.Parse(y.Value.ToString());
                                                    foreach (var z in jObjectHeader)
                                                    {
                                                        if (z.Key.Equals("gross"))
                                                        {
                                                            txtGrossPrice.Text = string.IsNullOrEmpty(z.Value.ToString()) ? "0.00" : Convert.ToDouble(z.Value.ToString()).ToString("n2");
                                                        }
                                                        else if (z.Key.Equals("disc_amount"))
                                                        {
                                                            txtDiscountAmount.Text = string.IsNullOrEmpty(z.Value.ToString()) ? "0.00" : Convert.ToDouble(z.Value.ToString()).ToString("n2");
                                                        }
                                                        else if (z.Key.Equals("disc_amount"))
                                                        {
                                                            txtDiscountAmount.Text = string.IsNullOrEmpty(z.Value.ToString()) ? "0.00" : Convert.ToDouble(z.Value.ToString()).ToString("n2");
                                                        }
                                                        else if (z.Key.Equals("doctotal"))
                                                        {
                                                            txtlAmountPayable.Text = string.IsNullOrEmpty(z.Value.ToString()) ? "0.00" : Convert.ToDouble(z.Value.ToString()).ToString("n2");
                                                        }
                                                        else if (z.Key.Equals("tenderamt"))
                                                        {
                                                            txtTenderAmount.Text = string.IsNullOrEmpty(z.Value.ToString()) ? "0.00" : Convert.ToDouble(z.Value.ToString()).ToString("n2");
                                                        }
                                                        else if (z.Key.Equals("change"))
                                                        {
                                                            txtChange.Text = string.IsNullOrEmpty(z.Value.ToString()) ? "0.00" : Convert.ToDouble(z.Value.ToString()).ToString("n2");
                                                        }
                                                    }
                                                }
                                                else if (y.Key.Equals("row"))
                                                {
                                                    JArray jArrayRow = JArray.Parse(y.Value.ToString());
                                                    for (int i = 0; i < jArrayRow.Count(); i++)
                                                    {
                                                        JObject data = JObject.Parse(jArrayRow[i].ToString());
                                                        String itemName = "";
                                                        double quantity = 0.00, price = 0.00, discountPercent = 0.00, totalPrice = 0.00, discamt = 0.00;
                                                        bool free = false;
                                                        foreach (var z in data)
                                                        {
                                                            if (z.Key.Equals("item_code"))
                                                            {
                                                                itemName = z.Value.ToString();
                                                            }
                                                            else if (z.Key.Equals("quantity"))
                                                            {
                                                                quantity = Convert.ToDouble(z.Value.ToString());
                                                            }
                                                            else if (z.Key.Equals("unit_price"))
                                                            {
                                                                price = Convert.ToDouble(z.Value.ToString());
                                                            }
                                                            else if (z.Key.Equals("discprcnt"))
                                                            {
                                                                discountPercent = Convert.ToDouble(z.Value.ToString());
                                                            }
                                                            else if (z.Key.Equals("linetotal"))
                                                            {
                                                                totalPrice = Convert.ToDouble(z.Value.ToString());
                                                            }
                                                            else if (z.Key.Equals("free"))
                                                            {
                                                                free = Convert.ToBoolean(z.Value.ToString());
                                                            }

                                                            else if (z.Key.Equals("disc_amount"))
                                                            {
                                                                discamt = Convert.ToDouble(z.Value.ToString());
                                                            }
                                                        }
                                                        dgvitems.Rows.Add(itemName, Convert.ToDecimal(string.Format("{0:0.00}", quantity)), Convert.ToDecimal(string.Format("{0:0.00}", price)), Convert.ToDecimal(string.Format("{0:0.00}", discountPercent)), Convert.ToDecimal(string.Format("{0:0.00}", discamt)), Convert.ToDecimal(string.Format("{0:0.00}", totalPrice)), free);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                lblItemsCount.Text = "ITEMS (" + dgvitems.Rows.Count.ToString("N0") + ")";
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
                                    Cursor.Current = Cursors.Default;
                                    MessageBox.Show("Your login session is expired. Please login again", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                                else
                                {
                                    Cursor.Current = Cursors.Default;
                                    MessageBox.Show(msg, "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show(response.Content.ToString(), "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
        }


        private void dgv_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgv.Rows.Count > 0)
            {
                if (e.ColumnIndex == 0)
                {
                    selectOrders(false);
                }
            }
        }
    

        private void dgvitems_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvitems.Rows.Count > 0 && dgv.Rows.Count > 0)
            {
                if (e.RowIndex >= 0)
                {
                    try
                    {
                        JArray jarrayBody = new JArray();
                        for (int i = 0; i < dgv.Rows.Count; i++)
                        {
                            if (Convert.ToBoolean(dgv.Rows[i].Cells["selectt"].Value.ToString()) == true)
                            {
                                jarrayBody.Add(Convert.ToInt32(dgv.Rows[i].Cells["ID"].Value));
                            }
                        }

                        Cursor.Current = Cursors.WaitCursor;
                        if (Login.jsonResult != null)
                        {
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
                                JObject jsonObjectBody = new JObject();
                                jsonObjectBody.Add("ids", jarrayBody);
                                jsonObjectBody.Add("discount", Convert.ToDouble(dgvitems.CurrentRow.Cells["discpercent"].Value.ToString()));
                                jsonObjectBody.Add("item_code", dgvitems.CurrentRow.Cells["item"].Value.ToString());
                                var request = new RestRequest("/api/sales/item/transaction/details");
                                request.AddHeader("Authorization", "Bearer " + token);
                                Console.WriteLine(jsonObjectBody);
                                request.AddParameter("application/json", jsonObjectBody, ParameterType.RequestBody);
                                request.Method = Method.PUT;
                                var response = client.Execute(request);
                                ItemDiscount itemDisc = new ItemDiscount();
                                if (response.ErrorMessage == null)
                                {
                                    itemDisc.jsonResponse = response.Content.ToString();
                                }
                                else
                                {
                                    itemDisc.jsonResponse = response.ErrorMessage;
                                }
                                itemDisc.ShowDialog();
                                Cursor.Current = Cursors.Default;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
            }
        }

        private void dtToDate_ValueChanged(object sender, EventArgs e)
        {
            if (cToDate <= 0)
            {
                loadData();
            }
        }

        private void checkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            if (dgv.Rows.Count > 0)
            {
                //toggleSelectAll(checkSelectAll.Checked);
                //MessageBox.Show(cCheck.ToString());
                if (cCheck == 0)
                {
                    selectOrders(true);
                }
                else
                {
                    cCheck = 0;
                }
            }
        }

        private void dgv_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (dgv.Rows.Count > 0) 
            //{
            //    SalesReportItems salesReportItems = new SalesReportItems();
            //    salesReportItems.URLDetails = "/api/sales/details/" + dgv.CurrentRow.Cells["id"].Value.ToString();
            //    salesReportItems.ShowDialog();
            //}
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
