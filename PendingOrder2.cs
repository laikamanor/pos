﻿using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AB.UI_Class;
namespace AB
{
    public partial class PendingOrder2 : Form
    {
        UI_Class.utility_class utilityc = new utility_class();
        DataTable dtSalesAgent = new DataTable();
        public static DataTable dtSelectedDeposit;
        DataTable dtPayment = new DataTable();
        string gSalesType = "", gForType = "";
        public PendingOrder2(string salesType, string forType)
        {
            gSalesType = salesType;
            gForType = forType;
            InitializeComponent();
        }

        private void PendingOrder_Load(object sender, EventArgs e)
        {

            if (gForType == "for Payment")
            {
                dtDate.Visible = false;
                label1.Visible = false;
                dtDate.Value = DateTime.Now;
            }
            else
            {
                dtDate.Visible = true;
                label1.Visible = true;
                dtDate.Value = DateTime.Now;
            }

            if(gForType == "for Payment")
            {
                btnConfirm.Text = "PAY";
                button1.Visible = true;
                //btnVoid.Visible = true;
                btnPaymentMethod.Visible = true;
            }
            else
            {
                btnConfirm.Text = "CONFIRM";
                button1.Visible = false;
                //btnVoid.Visible = false;
                btnPaymentMethod.Visible = false;
            }
            if (gForType.Equals("for Payment") || gForType.Equals("for Confirmation"))
            {
                btnVoid.Visible = true;
            }
            else
            {
                btnVoid.Visible = false;
            }
            dtSelectedDeposit = new DataTable();
            dtSelectedDeposit.Columns.Clear();
            dtSelectedDeposit.Columns.Add("id");
            dtSelectedDeposit.Columns.Add("amount");
            dtSelectedDeposit.Columns.Add("payment_type");
            dtSelectedDeposit.Columns.Add("sapnum");
            dtSelectedDeposit.Columns.Add("reference2");
            dtPayment.Columns.Add("id");
            dtPayment.Columns.Add("amount");
            dtPayment.Columns.Add("payment_type");
            dtPayment.Columns.Add("sapnum");
            dtPayment.Columns.Add("reference2");
            dtSelectedDeposit.Rows.Clear();
            loadSalesAgent();
            counts();
        }

        public void loadSalesAgent()
        {
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
                    dtSalesAgent.Columns.Clear();
                    dtSalesAgent.Columns.Add("id");
                    dtSalesAgent.Columns.Add("username");
                    dtSalesAgent.Rows.Clear();
                    cmbsales.Items.Clear();
                    var client = new RestClient(utilityc.URL);
                    client.Timeout = -1;
                    var request = new RestRequest("/api/auth/user/get_all?isSales=1");
                    request.AddHeader("Authorization", "Bearer " + token);
                    var response = client.Execute(request);
                    if (response.ErrorMessage == null)
                    {
                        JObject jObjectResponse = JObject.Parse(response.Content);
                        cmbsales.Items.Add("All");

                        bool isSuccess = false;
                        foreach (var x in jObjectResponse)
                        {
                            if (x.Key.Equals("success"))
                            {
                                isSuccess = Convert.ToBoolean(x.Value.ToString());
                            }
                        }
                        if (isSuccess)
                        {
                            foreach (var x in jObjectResponse)
                            {
                                if (x.Key.Equals("data"))
                                {
                                    if (x.Value.ToString() != "[]")
                                    {
                                        lblNoDataFound.Visible = false;
                                        JArray jsonArray = JArray.Parse(x.Value.ToString());
                                        for (int i = 0; i < jsonArray.Count(); i++)
                                        {
                                            JObject jObjectData = JObject.Parse(jsonArray[i].ToString());
                                            int id = 0;
                                            string username = "";
                                            foreach (var y in jObjectData)
                                            {
                                                if (y.Key.Equals("id"))
                                                {
                                                    id = Convert.ToInt32(y.Value.ToString());
                                                }
                                                else if (y.Key.Equals("username"))
                                                {
                                                    username = y.Value.ToString();
                                                }
                                            }
                                            dtSalesAgent.Rows.Add(id, username);
                                            cmbsales.Items.Add(username);

                                        }
                                    }
                                }
                            }
                            cmbsales.SelectedIndex = 0;
                        }
                        else
                        {
                            string msg = "No message response found";
                            foreach (var x in jObjectResponse)
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
                        Cursor.Current = Cursors.Default;
                        MessageBox.Show(response.ErrorMessage, "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            Cursor.Current = Cursors.Default;
        }

        public void loadData(string subURL)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (Login.jsonResult != null)
            {
                string token = "", branch = "";
                foreach (var x in Login.jsonResult)
                {
                    if (x.Key.Equals("token"))
                    {
                        token = x.Value.ToString();
                    }
                    else if (x.Key.Equals("data"))
                    {
                        JObject jObjectData = JObject.Parse(x.Value.ToString());
                        foreach (var y in jObjectData)
                        {
                            if (y.Key.Equals("branch"))
                            {
                                branch = y.Value.ToString();
                            }
                        }
                    }
                }
                if (!token.Equals(""))
                {
                    dgvitems.Rows.Clear();
                    var client = new RestClient(utilityc.URL);
                    client.Timeout = -1;

                    int selectedSalesAgentID = 0;
                    foreach (DataRow r0w in dtSalesAgent.Rows)
                    {
                        if (r0w["username"].Equals(cmbsales.Text))
                        {
                            selectedSalesAgentID = Convert.ToInt32(r0w["id"]);
                        }
                    }
                    string sUserID = "";
                    if (selectedSalesAgentID != 0)
                    {
                        sUserID = selectedSalesAgentID.ToString();
                    }
                    string arSalesDate = "";
                    if (gSalesType == "AR Sales" && label1.Visible == true && dtDate.Visible == true)
                    {
                        arSalesDate = "&date_created=" + dtDate.Value.ToString("yyyy-MM-dd");
                    }

                    string forQuery = subURL + "?transtype=" + gSalesType + "&transnum=" + txtsearch.Text + "&user_id=" + sUserID + arSalesDate;
                    string resultQuery;
                    if(gForType.Equals("for SAP"))
                    {
                        resultQuery = "/api/sales/for_sap/get_all?date=" + dtDate.Value.ToString("yyyy-MM-dd") + "&branch=" + branch + "&transtype=" + gSalesType + "&cust_code=" + txtsearch.Text;
                    }
                    else
                    {
                        resultQuery = forQuery;
                    }

                    // 
                    var request = new RestRequest(resultQuery);
                    request.AddHeader("Authorization", "Bearer " + token);            
                    var response = client.Execute(request);
                    JObject jObject = new JObject();

                    if (response.ErrorMessage == null)
                    {
                        jObject = JObject.Parse(response.Content.ToString());
                        dgvOrders.Rows.Clear();
                        dgvitems.Rows.Clear();
                        clearBillsField();
                        AutoCompleteStringCollection auto = new AutoCompleteStringCollection();
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
                                        lblNoDataFound.Visible = false;
                                        JArray jsonArray = JArray.Parse(x.Value.ToString());
                                        for (int i = 0; i < jsonArray.Count(); i++)
                                        {
                                            string forSAPAmountColumnName = (gForType.Equals("for SAP") ? "doctotal" : "amount_due");
                                            JObject data = JObject.Parse(jsonArray[i].ToString());
                                            int id = 0, transNumber = 0;
                                            string referenceNumber = "",
                transType = "", salesAgent = "N/A", cust_code = "";
                                            double amountDue = 0.00, tenderAmount = 0.00;
                                            DateTime dtTransDate = new DateTime();
                                            foreach (var q in data)
                                            {
                                                if (q.Key.Equals("reference"))
                                                {
                                                    referenceNumber = q.Value.ToString();
                                                }
                                                else if (q.Key.Equals("transtype"))
                                                {
                                                    transType = q.Value.ToString();
                                                }
                                                if (q.Key.Equals(forSAPAmountColumnName))
                                                {
                                                    amountDue = Convert.ToDouble(q.Value.ToString());
                                                }
                                                else if (q.Key.Equals("id"))
                                                {
                                                    id = Convert.ToInt32(q.Value.ToString());
                                                }
                                                else if (q.Key.Equals("cust_code"))
                                                {
                                                    cust_code = q.Value.ToString();
                                                }
                                                else if (q.Key.Equals("transnumber"))
                                                {
                                                    transNumber = Convert.ToInt32(q.Value.ToString());
                                                }
                                                else if (q.Key.Equals("tenderamt"))
                                                {
                                                    tenderAmount = Convert.ToDouble(q.Value.ToString());
                                                }
                                                else if (q.Key.Equals("created_user"))
                                                {
                                                    JObject jObjectCreatedUser = JObject.Parse(q.Value.ToString());
                                                    foreach (var a in jObjectCreatedUser)
                                                    {
                                                        if (a.Key.Equals("username"))
                                                        {
                                                            salesAgent = a.Value.ToString();
                                                        }
                                                    }
                                                }
                                                else if (q.Key.Equals("transdate"))
                                                {
                                                    string replaceT = q.Value.ToString().Replace("T", "");
                                                    dtTransDate = Convert.ToDateTime(replaceT);
                                                }
                                            }
                                            dgvOrders.Rows.Add(false, id, transNumber, referenceNumber, amountDue.ToString("n2"), salesAgent, transType, cust_code, tenderAmount, "", dtTransDate.ToString("yyyy-MM-dd"));
                                            auto.Add(transNumber.ToString());
                                        }
                                        txtsearch.AutoCompleteCustomSource = auto;
                                        lblOrderCount.Text = "Orders (" + dgvOrders.Rows.Count.ToString("N0") + ")";
                                    }
                                    else
                                    {
                                        lblNoDataFound.Visible = true;
                                        lblOrderCount.Text = "Orders (0)";
                                        lblpendingamount.Text = "Selected Amount: 0.00";
                                        dgvitems.Rows.Clear();
                                        clearBillsField();
                                    }
                                }
                            }
                        }
                        else
                        {
                            lblOrderCount.Text = "Orders (0)";
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
                                lblOrderCount.Text = "Orders (0)";
                                lblpendingamount.Text = "Selected Amount: 0.00";
                                dgvitems.Rows.Clear();
                                clearBillsField();
                            }
                            else
                            {
                                Cursor.Current = Cursors.Default;
                                MessageBox.Show(msg, "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                lblOrderCount.Text = "Orders (0)";
                                lblpendingamount.Text = "Selected Amount: 0.00";
                                dgvitems.Rows.Clear();
                                clearBillsField();
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show(response.ErrorMessage, "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            dgvOrders.Columns["amountdue"].HeaderText = (gForType.Equals("for SAP") ? "Doc. Total" : "Amt. Due");
            Cursor.Current = Cursors.Default;
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

        public void selectOrders()
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
                    var client = new RestClient(utilityc.URL);
                    client.Timeout = -1;

                    dgvOrders.CommitEdit(DataGridViewDataErrorContexts.Commit);

                    double totalPendingAmount = 0.00;
                    string ids = "";
                    for (int i = 0; i < dgvOrders.Rows.Count; i++)
                    {
                        if (Convert.ToBoolean(dgvOrders.Rows[i].Cells["selectt"].Value.ToString()) == true)
                        {
                            totalPendingAmount += Convert.ToDouble(dgvOrders.Rows[i].Cells["amountdue"].Value.ToString());
                            ids = ids + "," + dgvOrders.Rows[i].Cells["base_id"].Value.ToString();
                        }
                    }
                  
                    ids = (string.IsNullOrEmpty(ids) ? "" : ids.Substring(1));
                    lblpendingamount.Text = "Pending Amount: " + totalPendingAmount.ToString("n2");
                    dgvitems.Rows.Clear();
                    var request = new RestRequest("/api/sales/summary_trans?ids=%5B" + ids + "%5D");
                    //MessageBox.Show("/api/sales/summary_trans?ids=%5B" + ids + "%5D");
                    request.AddHeader("Authorization", "Bearer " + token);
                    var response = client.Execute(request);

                    if(response.ErrorMessage == null)
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
                                                    else if (z.Key.Equals("amount_due"))
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
                                                    dgvitems.Rows.Add(itemName, quantity.ToString("n2"), price.ToString("n2"), discountPercent.ToString("n2"), discamt.ToString("n2"), totalPrice.ToString("n2"), free);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            lblItemsCount.Text = "Items (" + dgvitems.Rows.Count.ToString("N0") + ")";
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
                        Cursor.Current = Cursors.Default;
                        MessageBox.Show(response.ErrorMessage, "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            Cursor.Current = Cursors.Default;
            computeChange();

            int int_selectAll = 0;
            for (int i = 0; i < dgvOrders.Rows.Count; i++)
            {
                if (Convert.ToBoolean(dgvOrders.Rows[i].Cells["selectt"].Value.ToString()) == true)
                {
                    int_selectAll += 1;
                }
            }
            if (int_selectAll <= 0 && checkSelectAll.Checked)
            {
                checkSelectAll.Checked = false;
            }
        }

        private void dgvOrders_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvOrders.Rows.Count > 0)
            {
                if (e.ColumnIndex == 0 && e.RowIndex >= 0)
                {
                    selectOrders();
                }
            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (dgvOrders.Rows.Count <= 0)
            {
                MessageBox.Show("No order found", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                int countSelected = 0;
                for (int i = 0; i < dgvOrders.Rows.Count; i++)
                {
                    if (Convert.ToBoolean(dgvOrders.Rows[i].Cells["selectt"].Value.ToString()))
                    {
                        countSelected += 1;
                    }
                }
                if (countSelected <= 0)
                {
                    MessageBox.Show("No order selected", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    DialogResult dialogResult = MessageBox.Show("Are you sure you want to " + (gForType == "for Payment" ? "pay" : "confirm") + "?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        if (gForType == "for Payment")
                        {
                            if (insertTransaction(true, 0))
                            {
                                dtSelectedDeposit.Rows.Clear();
                                string subURL = gForType == "for Payment" ? "/api/payment/new" : "/api/sales/for_confirm";
                                loadData(subURL);
                                counts();
                            }
                        }
                        else if (gForType == "for Confirmation")
                        {
                            confirmARSales();
                        }
                        else if (gForType == "for SAP")
                        {
                            string ids = "";
                            int int_hasSelected = 0;
                            for (int i = 0; i < dgvOrders.Rows.Count; i++)
                            {
                                if (Convert.ToBoolean(dgvOrders.Rows[i].Cells["selectt"].Value.ToString()) == true)
                                {
                                    ids = ids + "," + dgvOrders.Rows[i].Cells["base_id"].Value.ToString();
                                    int_hasSelected += 1;
                                }
                            }
                            ids = (string.IsNullOrEmpty(ids) ? "" : ids.Substring(1));
                            if(int_hasSelected > 0)
                            {
                                forSAPAR_SAPNumber forSAPAR_SAPNumber = new forSAPAR_SAPNumber();
                                forSAPAR_SAPNumber.ids = ids;
                                forSAPAR_SAPNumber.ShowDialog();
                                if (forSAPAR_SAPNumber.isSubmit)
                                {
                                    string subURL = gForType == "for Payment" ? "/api/payment/new" : "/api/sales/for_confirm";
                                    loadData(subURL);
                                }
                            }
                        }
                    }
                }
            }
        }

        public void counts()
        {
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
                   
                    var request = new RestRequest("/api/sales/count?transdate=" + dtDate.Value.ToString("yyyy-MM-dd"));
                    request.AddHeader("Authorization", "Bearer " + token);
                    request.Method = Method.GET;

                    var response = client.Execute(request);

                    if (response.ErrorMessage == null)
                    {
                        JObject jObjectResponse = JObject.Parse(response.Content);
                        bool isSuccess = false;
                        foreach (var x in jObjectResponse)
                        {
                            if (x.Key.Equals("success"))
                            {
                                isSuccess = true;
                                break;
                            }
                        }

                        string msg = "No message response found";
                        foreach (var x in jObjectResponse)
                        {
                            if (x.Key.Equals("message"))
                            {
                                msg = x.Value.ToString();
                            }
                        }
                        if (!string.IsNullOrEmpty(msg))
                        {
                            MessageBox.Show(msg, isSuccess ? "Success" : "Validation", MessageBoxButtons.OK, isSuccess ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
                        }
                        if (isSuccess)
                        {
                            int for_payment = 0, for_confirmation = 0;
                            foreach (var x in jObjectResponse)
                            {
                                if (x.Key.Equals("data"))
                                {
                                    if (x.Value.ToString() != "[]")
                                    {
                                        JObject data = JObject.Parse(x.Value.ToString());
                                        foreach (var q in data)
                                        {
                                            if (q.Key.Equals("for_payment"))
                                            {
                                                for_payment = Convert.ToInt32(q.Value.ToString());
                                            }
                                            else if (q.Key.Equals("for_confirmation"))
                                            {
                                                for_confirmation = Convert.ToInt32(q.Value.ToString());
                                            }
                                        }
                                    }
                                }
                            }
                            btnForPayment.Text = "For Payment (" + for_payment.ToString("N0") + ")";
                            btnForConfirmation.Text = "For AR Confirmation (" + for_confirmation.ToString("N0") + ")";
                        }
                    }
                    else
                    {
                        MessageBox.Show(response.ErrorMessage, "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        public void confirmARSales()
        {
            if (dgvOrders.Rows.Count <= 0)
            {
                MessageBox.Show("No order found", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                int countSelected = 0;
                for (int i = 0; i < dgvOrders.Rows.Count; i++)
                {
                    if (Convert.ToBoolean(dgvOrders.Rows[i].Cells["selectt"].Value.ToString()))
                    {
                        countSelected += 1;
                    }
                }
                if (countSelected <= 0)
                {
                    MessageBox.Show("No order selected", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    string ids = "";
                    for (int i = 0; i < dgvOrders.Rows.Count; i++)
                    {
                        if (Convert.ToBoolean(dgvOrders.Rows[i].Cells["selectt"].Value.ToString()) == true)
                        {
                            //totalPendingAmount += Convert.ToDouble(dgvOrders.Rows[i].Cells["amountdue"].Value.ToString());
                            ids = ids + "," + dgvOrders.Rows[i].Cells["base_id"].Value.ToString();
                        }
                    }
                    ids = (string.IsNullOrEmpty(ids) ? "" : ids.Substring(1));

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
                            var request = new RestRequest("/api/sales/confirm?ids=%5B" + ids + "%5D");
                            request.AddHeader("Authorization", "Bearer " + token);
                            request.Method = Method.PUT;

                            var response = client.Execute(request);

                            if(response.ErrorMessage == null)
                            {
                                JObject jObjectResponse = JObject.Parse(response.Content);
                                bool isSuccess = false;
                                foreach (var x in jObjectResponse)
                                {
                                    if (x.Key.Equals("success"))
                                    {
                                        isSuccess = true;
                                        break;
                                    }
                                }

                                string msg = "No message response found";
                                foreach (var x in jObjectResponse)
                                {
                                    if (x.Key.Equals("message"))
                                    {
                                        msg = x.Value.ToString();
                                    }
                                }
                                MessageBox.Show(msg, isSuccess ? "Success" : "Validation", MessageBoxButtons.OK, isSuccess ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
                                if (isSuccess)
                                {
                                    string subURL = gForType == "for Payment" ? "/api/payment/new" : "/api/sales/for_confirm";
                                    loadData(subURL);
                                    counts();
                                }
                            }
                            else
                            {
                                MessageBox.Show(response.ErrorMessage, "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                }
            }
        }

        public bool insertTransaction(bool isMultipleTransaction, double ARSalesAmount)
        {
            bool result = false;
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
                    var request = new RestRequest("/api/payment/new");
                    request.AddHeader("Authorization", "Bearer " + token);
                    request.Method = Method.POST;
                    JArray jArrayBody = new JArray();
                    if (isMultipleTransaction)
                    {
                        DataTable dtSelectedDepositTEMP = new DataTable();
                        dtSelectedDepositTEMP.Columns.Add("id");
                        dtSelectedDepositTEMP.Columns.Add("amount");
                        dtSelectedDepositTEMP.Columns.Add("payment_type");
                        dtSelectedDepositTEMP.Columns.Add("sapnum");
                        dtSelectedDepositTEMP.Columns.Add("reference2");

                        foreach (DataRow row in dtSelectedDeposit.Rows)
                        {
                            dtSelectedDepositTEMP.Rows.Add(row["id"], row["amount"], row["payment_type"], row["sapnum"], row["reference2"]);
                        }

                        for (int i = 0; i < dgvOrders.Rows.Count; i++)
                        {
                            if (Convert.ToBoolean(dgvOrders.Rows[i].Cells["selectt"].Value.ToString()))
                            {
                                JObject jObjectBody = new JObject();
                                JObject jObjectHeader = new JObject();
                                jObjectHeader.Add("base_id", Convert.ToInt32(dgvOrders.Rows[i].Cells["base_id"].Value.ToString()));
                                jObjectHeader.Add("base_num", Convert.ToInt32(dgvOrders.Rows[i].Cells["transnumber"].Value.ToString()));
                                jObjectHeader.Add("cust_code", dgvOrders.Rows[i].Cells["cust_code"].Value.ToString());
                                jObjectHeader.Add("transdate", DateTime.Now.ToString("yyyy/MM/dd HH:mm"));
                                jObjectHeader.Add("remarks", null);
                                jObjectHeader.Add("reference2", null);
                                jObjectBody.Add("header", jObjectHeader);

                                JArray jArrayRows = new JArray();

                                double cash = Convert.ToDouble(dgvOrders.Rows[i].Cells["tenderamount"].Value.ToString());
                                double amountDue = Convert.ToDouble(dgvOrders.Rows[i].Cells["amountdue"].Value.ToString());

                                dtPayment.Rows.Clear();
                                JObject jObjectPay = new JObject();
                                jObjectPay.Add("id", Convert.ToInt32(dgvOrders.Rows[i].Cells["base_id"].Value.ToString()));
                                jObjectPay.Add("amount", Convert.ToDouble(dgvOrders.Rows[i].Cells["amountdue"].Value.ToString()));
                                pay(jObjectPay, dtSelectedDepositTEMP);
                                double otherAmount = 0.00;
                                if (dtPayment.Rows.Count > 0)
                                {
                                    foreach (DataRow row in dtPayment.Rows)
                                    {
                                        otherAmount += Convert.ToDouble(row["amount"]);
                                        JObject jObjectRows = new JObject();
                                        jObjectRows.Add("deposit_id", row["id"].ToString() == "" ? null : row["id"].ToString());
                                        jObjectRows.Add("amount", Convert.ToDouble(row["amount"]));
                                        jObjectRows.Add("payment_type", row["payment_type"].ToString());
                                        jObjectRows.Add("sap_number", row["sapnum"].ToString() == "" ? null : row["sapnum"].ToString());
                                        jObjectRows.Add("reference2", row["reference2"].ToString() == "" ? null : row["reference2"].ToString());
                                        jArrayRows.Add(jObjectRows);

                                    }
                                }
                              if(cash > 0)
                                {
                                    if (amountDue > otherAmount)
                                    {
                                        double num1 = (otherAmount > amountDue ? otherAmount - amountDue : amountDue - otherAmount);
                                        //double num2 = cash - num1;
                                        double num2 = (cash > num1 ? cash-num1 : num1 - cash);
                                        double finalAmount = 0.00;
                                        if(cash > num1)
                                        {
                                            finalAmount = amountDue;
                                        }
                                        else
                                        {
                                            finalAmount = cash;
                                        }

                                        JObject jObjectRows = new JObject();
                                        jObjectRows.Add("amount", finalAmount);
                                        jObjectRows.Add("payment_type", "CASH");
                                        jArrayRows.Add(jObjectRows);

                                    }
                                }

                                jObjectBody.Add("rows", jArrayRows);
                                jArrayBody.Add(jObjectBody);
                            }
                        }
                    }
                    request.AddParameter("application/json", jArrayBody, ParameterType.RequestBody);
                    var response = client.Execute(request);
                    JObject jObjectResponse = JObject.Parse(response.Content);

                    foreach (var x in jObjectResponse)
                    {
                        if (x.Key.Equals("success"))
                        {
                            if (Convert.ToBoolean(x.Value.ToString()))
                            {
                                result = true;
                            }
                        }
                    }

                    if (!result)
                    {
                        string msg = "Object message key not found";
                        foreach (var x in jObjectResponse)
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
                    else{
                        string msg = "Object message key not found";
                        foreach (var x in jObjectResponse)
                        {
                            if (x.Key.Equals("message"))
                            {
                                msg = x.Value.ToString();
                            }
                        }
                        Cursor.Current = Cursors.Default;
                        MessageBox.Show(msg, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            Cursor.Current = Cursors.Default;
            return result;
        }

        private void btnsearch_Click(object sender, EventArgs e)
        {
            string subURL = gForType == "for Payment" ? "/api/payment/new" : "/api/sales/for_confirm";
            loadData(subURL);
        }

        private void txtsearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.Equals(Keys.Enter)) {
                string subURL = gForType == "for Payment" ? "/api/payment/new" : "/api/sales/for_confirm";
                loadData(subURL);
            }
        }

        private void cmbsales_SelectedIndexChanged(object sender, EventArgs e)
        {
            string subURL = gForType == "for Payment" ? "/api/payment/new" : "/api/sales/for_confirm";
            loadData(subURL);
        }

        private void btnrefresh_Click(object sender, EventArgs e)
        {
            counts();
            string subURL = gForType == "for Payment" ? "/api/payment/new" : "/api/sales/for_confirm";
            loadData(subURL);
        }

        private void btnVoid_Click(object sender, EventArgs e)
        {
            if (dgvOrders.Rows.Count <= 0)
            {
                MessageBox.Show("No order found", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                int countSelected = 0;
                for (int i = 0; i < dgvOrders.Rows.Count; i++)
                {
                    if (Convert.ToBoolean(dgvOrders.Rows[i].Cells["selectt"].Value.ToString()))
                    {
                        countSelected += 1;
                    }
                }
                if (countSelected <= 0)
                {
                    MessageBox.Show("No order selected", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    string ids = "";
                    int count = 0;
                    string selectedRef = "Selected #: " + Environment.NewLine;
                    for (int i = 0; i < dgvOrders.Rows.Count; i++)
                    {
                        if (Convert.ToBoolean(dgvOrders.Rows[i].Cells["selectt"].Value.ToString()) == true)
                        {
                            //totalPendingAmount += Convert.ToDouble(dgvOrders.Rows[i].Cells["amountdue"].Value.ToString());
                            ids = ids + "," + dgvOrders.Rows[i].Cells["base_id"].Value.ToString();
                            selectedRef += dgvOrders.Rows[i].Cells["ordernum"].Value.ToString() + Environment.NewLine;
                            count += 1;
                        }
                    }
                    ids = (string.IsNullOrEmpty(ids) ? "" : ids.Substring(1));

                    voidForm voidd = new voidForm();
                    //voidd.baseNum = Convert.ToInt32(dgvOrders.CurrentRow.Cells["transnumber"].Value.ToString());
                    voidd.lblOrderNumber.Text = "(" + count.ToString("N0") + ")";
                    voidd.selectedReference = selectedRef;
                    voidd.selectedID = ids;
                    voidd.ShowDialog();
                    if (voidForm.isSubmit)
                    {
                        string subURL = gForType == "for Payment" ? "/api/payment/new" : "/api/sales/for_confirm";
                        loadData(subURL);
                    }
                }
            }
        }

        private void dtDate_ValueChanged(object sender, EventArgs e)
        {
            if (gSalesType == "AR Sales")
            {
                string subURL = gForType == "for Payment" ? "/api/payment/new" : "/api/sales/for_confirm";
                loadData(subURL);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SelectAdvancePayment selectAdvancePayment = new SelectAdvancePayment();
            selectAdvancePayment.ShowDialog();
            computeChange();
        }

        public void computeChange()
        {
            double totalPayment = 0.00;
            if(dtSelectedDeposit.Rows.Count > 0)
            {
                foreach (DataRow row in dtSelectedDeposit.Rows)
                {
                    totalPayment += (string.IsNullOrEmpty(row["amount"].ToString()) ? 0.00 : Convert.ToDouble(row["amount"].ToString()));
                }
            }
            txtTotalPayment.Text = totalPayment.ToString("n2");
            double num1 = totalPayment + (string.IsNullOrEmpty(txtTenderAmount.Text) ? 0.00 : Convert.ToDouble(txtTenderAmount.Text));
            double amountDue = (string.IsNullOrEmpty(txtlAmountPayable.Text) ? 0.00 : Convert.ToDouble(txtlAmountPayable.Text)); ;
            double change = num1 - amountDue;
            txtChange.Text = (change > 0 ? change : 0.00).ToString("n2");
        }


        public void pay(JObject jObjectPay, DataTable dtDeposit)
        {
            int id = 0;
            double amount = 0.00;
            foreach (var q in jObjectPay)
            {
                if (q.Key.Equals("id"))
                {
                    id = Convert.ToInt32(q.Value.ToString());
                }
                else if (q.Key.Equals("amount"))
                {
                    amount = Convert.ToDouble(q.Value.ToString());
                }
            }
            //DataTable dtRows = new DataTable();
            //dtRows.Columns.Add("id");
            //dtRows.Columns.Add("amount");
           
            while (amount > 0)
            {
                foreach(DataRow row in dtDeposit.Rows)
                {
                    if(Convert.ToDouble(row["amount"].ToString()) != 0.00)
                    {
                        double payment = 0.00;
                        if (Convert.ToDouble(row["amount"]) >= amount)
                        {
                            payment += amount;
                            row["amount"] = (Convert.ToDouble(row["amount"].ToString()) - amount);
                            dtPayment.Rows.Add(row["id"], payment,row["payment_type"], row["sapnum"], row["reference2"]);
                            amount = 0;
                            break;
                        }else if(Convert.ToDouble(row["amount"]) < amount)
                        {
                            payment += Convert.ToDouble(row["amount"]);
                            amount -= Convert.ToDouble(row["amount"].ToString());
                            dtPayment.Rows.Add(row["id"], payment, row["payment_type"], row["sapnum"], row["reference2"]);
                            row["amount"] = 0;
                        }
                    }
                }
                break;
            }
        }

        public DataTable getSelectedRows()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("base_id");
            dt.Columns.Add("base_num");
            dt.Columns.Add("cust_code");
            dt.Columns.Add("amountdue");
            dt.Columns.Add("tenderamount");
            dt.Columns.Add("tendertype");
            for (int i = 0; i < dgvOrders.Rows.Count; i++)
            {
                if (Convert.ToBoolean(dgvOrders.Rows[i].Cells["selectt"].Value.ToString()))
                {
                    dt.Rows.Add(Convert.ToInt32(dgvOrders.Rows[i].Cells["base_id"].Value.ToString()), dgvOrders.Rows[i].Cells["transnumber"].Value.ToString(), dgvOrders.Rows[i].Cells["cust_code"].Value.ToString(), Convert.ToDouble(dgvOrders.Rows[i].Cells["amountdue"].Value.ToString()), Convert.ToDouble(dgvOrders.Rows[i].Cells["tenderamount"].Value.ToString()), dgvOrders.Rows[i].Cells["tendertype"].Value.ToString());
                }
            }
            return dt;
        }

        //private void btnForPayment_Click(object sender, EventArgs e)
        //{
        //    btnForPayment.ForeColor = Color.Black;
        //    btnForConfirmation.ForeColor = Color.White;
        //    counts();
        //    string subURL = btnForPayment.ForeColor == Color.Black ? "/api/payment/new" : "/api/sales/for_confirm";
        //    btnConfirm.Text = "PAY";
        //    button1.Visible = true;
        //    btnVoid.Visible = true;
        //    btnPaymentMethod.Visible=true;
        //    loadData(subURL);
        //}

        //private void btnForConfirmation_Click(object sender, EventArgs e)
        //{
        //    btnForPayment.ForeColor = Color.White;
        //    btnForConfirmation.ForeColor = Color.Black;
        //    counts();
        //    string subURL = btnForPayment.ForeColor == Color.Black ? "/api/payment/new" : "/api/sales/for_confirm";
        //    btnConfirm.Text = "CONFIRM";
        //    button1.Visible = false;
        //    btnVoid.Visible = false;
        //    btnPaymentMethod.Visible = false;
        //    loadData(subURL);
        //}

        private void btnPaymentMethod_Click(object sender, EventArgs e)
        {
            PaymentMethodList paymentMethodList = new PaymentMethodList();
            paymentMethodList.ShowDialog();
            computeChange();
        }

        private void checkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            if (dgvOrders.Rows.Count > 0)
            {
                toggleSelectAll(checkSelectAll.Checked);
                selectOrders();
            }
        }

        public void toggleSelectAll(bool value)
        {
            for (int i = 0; i < dgvOrders.Rows.Count; i++)
            {
                dgvOrders.Rows[i].Cells["selectt"].Value = value;
            }
        }

    }
}