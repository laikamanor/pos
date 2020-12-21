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
using AB.UI_Class;
using RestSharp;
using Newtonsoft.Json.Linq;
namespace AB
{
    public partial class AddUser : Form
    {
        public static bool isSubmit = false;
        branch_class branchc = new branch_class();
        warehouse_class warehousec = new warehouse_class();
        utility_class utilityc = new utility_class();
        public int userID = 0;
        string cUsername = "", cFullName = "", cBranch = "", cWarehouse = "", cStatus = "";
        bool cAdmin = false, cSales = false, cCashier = false, cManager = false, cCanAddSap = false, cTransfer = false, cReceive = false, cVoid = false, cARSales = false, cAgentSales = false, cCashSales = false, cDiscount = false, cChecker = false, cAuditor = false, cEndbal = false, cPullOut = false;
        public AddUser()
        {
            InitializeComponent();
        }
        

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (txtUsername.Text.ToString().Trim() == "")
            {
                MessageBox.Show("Username is required", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsername.Focus();
            }
            else if (txtFullName.Text.ToString().Trim() == "")
            {
                MessageBox.Show("Full Name is required", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtFullName.Focus();
            }
            else if (txtPassword.Text.ToString().Trim() == "" && this.Text == "Add User")
            {
                MessageBox.Show("Password is required", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
            }
            else if (txtPassword.Text.ToString().Trim() != txtConfirmPassword.Text.ToString().Trim() && this.Text == "Add User")
            {
                MessageBox.Show("Password did not match", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtConfirmPassword.Focus();
            }
            else if (cmbBranch.SelectedIndex == -1)
            {
                MessageBox.Show("Branch is required", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbBranch.Focus();
            }
            else if (cmbWarehouse.SelectedIndex == -1)
            {
                MessageBox.Show("Warehouse is required", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbWarehouse.Focus();
            }
            else if (cmbStatus.SelectedIndex == -1)
            {
                MessageBox.Show("Status is required", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbStatus.Focus();
            }
            else if (!string.IsNullOrEmpty(txtPassword.Text.Trim()) && txtPassword.Text.Trim() != txtConfirmPassword.Text.Trim() && this.Text == "Edit User")
            {
                MessageBox.Show("Password did not match", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtConfirmPassword.Focus();
            }
            else
            {
                if (this.Text == "Add User")
                {
                    if (insertUser())
                    {
                        isSubmit = true;
                        MessageBox.Show("User added", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        clearFields();
                    }
                    else
                    {
                        isSubmit = false;
                        MessageBox.Show("User not " + (this.Text == "Add User" ? "Added" : "Edited"), "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        clearFields();
                    }
                }
                else if (this.Text == "Edit User")
                {
                    if (updateUser())
                    {
                        isSubmit = true;
                        MessageBox.Show("User edited", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        clearFields();
                    }
                    else
                    {
                        isSubmit = false;
                        MessageBox.Show("User not " + (this.Text == "Add User" ? "Added" : "Edited"), "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        clearFields();
                    }
                }
                else
                {
                    isSubmit = false;
                    MessageBox.Show("User not " + (this.Text == "Add User" ? "Added" : "Edited"), "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    clearFields();
                }
            }
        }

        public void clearFields()
        {
            txtUsername.Clear();
            txtFullName.Clear();
            cmbBranch.SelectedIndex = -1;
            cmbWarehouse.SelectedIndex = -1;
            cmbStatus.SelectedIndex = -1;
            cmbAdmin.Checked = false;
            cmbSales.Checked = false;
            cmbAddSAP.Checked = false;
            cmbReceive.Checked = false;
            cmbCashier.Checked = false;
            cmbManager.Checked = false;
            cmbTransfer.Checked = false;
            cmbVoid.Checked = false;
            cmbARSales.Checked = false;
            cmbAgentSales.Checked = false;
            cmbCashSales.Checked = false;
            cmbDiscount.Checked = false;
            cmbChecker.Checked = false;
            cmbAuditor.Checked = false;
            cmbEndbal.Checked = false;
            cmbPullOut.Checked = false;
        }

        public void getUserDetails()
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
                    var request = new RestRequest("/api/auth/user/details/" + userID);
                    request.AddHeader("Authorization", "Bearer " + token);
                    var response = client.Execute(request);
                    JObject jObject = new JObject();
                    jObject = JObject.Parse(response.Content.ToString());
                    clearFields();
                    string userName = "",
fullName = "", branch = "", warehouse = "", status = "";
                    bool admin = false, sales = false, cashier = false, manager = false, canAddSAP = false, transfer = false, receive = false, voidd = false;
                    bool isSuccess = false, arSales = false, cashSales = false, agentSales = false, discount = false, checker = false, auditor = false, endbal = false, pullout = false;
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
                                        if (y.Key.Equals("username"))
                                        {
                                            userName = y.Value.ToString();
                                        }
                                        else if (y.Key.Equals("fullname"))
                                        {
                                            fullName = y.Value.ToString();
                                        }
                                        else if (y.Key.Equals("branch"))
                                        {
                                            branch = y.Value.ToString();
                                        }
                                        else if (y.Key.Equals("whse"))
                                        {
                                            warehouse = y.Value.ToString();
                                        }
                                        else if (y.Key.Equals("isAdmin"))
                                        {
                                            if (y.Value.ToString().Equals("False") || y.Value.ToString().Equals(""))
                                            {
                                                admin = false;
                                            }
                                            else
                                            {
                                                admin = true;
                                            }
                                        }
                                        else if (y.Key.Equals("isSales"))
                                        {
                                            if (y.Value.ToString().Equals("False") || y.Value.ToString().Equals(""))
                                            {
                                                sales = false;
                                            }
                                            else
                                            {
                                                sales = true;
                                            }
                                        }
                                        else if (y.Key.Equals("isCashier"))
                                        {
                                            if (y.Value.ToString().Equals("False") || y.Value.ToString().Equals(""))
                                            {
                                                cashier = false;
                                            }
                                            else
                                            {
                                                cashier = true;
                                            }
                                        }
                                        else if (y.Key.Equals("isManager"))
                                        {
                                            if (y.Value.ToString().Equals("False") || y.Value.ToString().Equals(""))
                                            {
                                                manager = false;
                                            }
                                            else
                                            {
                                                manager = true;
                                            }
                                        }
                                        else if (y.Key.Equals("isCanAddSap"))
                                        {
                                            if (y.Value.ToString().Equals("False") || y.Value.ToString().Equals(""))
                                            {
                                                canAddSAP = false;
                                            }
                                            else
                                            {
                                                canAddSAP = true;
                                            }
                                        }
                                        else if (y.Key.Equals("isTransfer"))
                                        {
                                            if (y.Value.ToString().Equals("False") || y.Value.ToString().Equals(""))
                                            {
                                                transfer = false;
                                            }
                                            else
                                            {
                                                transfer = true;
                                            }
                                        }
                                        else if (y.Key.Equals("isReceive"))
                                        {
                                            if (y.Value.ToString().Equals("False") || y.Value.ToString().Equals(""))
                                            {
                                                receive = false;
                                            }
                                            else
                                            {
                                                receive = true;
                                            }
                                        }
                                        else if (y.Key.Equals("isVoid"))
                                        {
                                            if (y.Value.ToString().Equals("False") || y.Value.ToString().Equals(""))
                                            {
                                                voidd = false;
                                            }
                                            else
                                            {
                                                voidd = true;
                                            }
                                        }
                                        else if (y.Key.Equals("isARSales"))
                                        {
                                            if (y.Value.ToString().Equals("False") || y.Value.ToString().Equals(""))
                                            {
                                                arSales = false;
                                            }
                                            else
                                            {
                                                arSales = true;
                                            }
                                        }
                                        else if (y.Key.Equals("isAgentSales"))
                                        {
                                            if (y.Value.ToString().Equals("False") || y.Value.ToString().Equals(""))
                                            {
                                                agentSales = false;
                                            }
                                            else
                                            {
                                                agentSales = true;
                                            }
                                        }
                                        else if (y.Key.Equals("isCashSales"))
                                        {
                                            if (y.Value.ToString().Equals("False") || y.Value.ToString().Equals(""))
                                            {
                                                cashSales = false;
                                            }
                                            else
                                            {
                                                cashSales = true;
                                            }
                                        }

                                        else if (y.Key.Equals("isActive"))
                                        {
                                            if (y.Value.ToString().Equals("False") || y.Value.ToString().Equals(""))
                                            {
                                                status = "In Active";
                                            }
                                            else
                                            {
                                                status = "Active";
                                            }
                                        }
                                        else if (y.Key.Equals("isDiscount"))
                                        {
                                            if (y.Value.ToString().Equals("False") || y.Value.ToString().Equals(""))
                                            {
                                                discount = false;
                                            }
                                            else
                                            {
                                                discount = true;
                                            }
                                        }
                                        else if (y.Key.Equals("isChecker"))
                                        {
                                            if (y.Value.ToString().Equals("False") || y.Value.ToString().Equals(""))
                                            {
                                                checker = false;
                                            }
                                            else
                                            {
                                                checker = true;
                                            }
                                        }
                                        else if (y.Key.Equals("isAuditor"))
                                        {
                                            if (y.Value.ToString().Equals("False") || y.Value.ToString().Equals(""))
                                            {
                                                auditor = false;
                                            }
                                            else
                                            {
                                                auditor = true;
                                            }
                                        }
                                        else if (y.Key.Equals("isAllowEnding"))
                                        {
                                            if (y.Value.ToString().Equals("False") || y.Value.ToString().Equals(""))
                                            {
                                                endbal = false;
                                            }
                                            else
                                            {
                                                endbal = true;
                                            }
                                        }
                                        else if (y.Key.Equals("isAllowPullOut"))
                                        {
                                            if (y.Value.ToString().Equals("False") || y.Value.ToString().Equals(""))
                                            {
                                                pullout = false;
                                            }
                                            else
                                            {
                                                pullout = true;
                                            }
                                        }
                                    }
                                }
                                txtUsername.Text = cUsername = userName;
                                txtFullName.Text = cFullName = fullName;
                                cmbBranch.Text = cBranch = branch;
                                cmbWarehouse.Text = cWarehouse = warehouse;
                                cmbAdmin.Checked = cAdmin = admin;
                                cmbSales.Checked = cSales = sales;
                                cmbCashier.Checked = cCashier = cashier;
                                cmbManager.Checked = cManager = manager;
                                cmbAddSAP.Checked = cCanAddSap = canAddSAP;
                                cmbTransfer.Checked = cTransfer = transfer;
                                cmbReceive.Checked = cReceive = receive;
                                cmbVoid.Checked = cVoid = voidd;
                                cmbStatus.Text = cStatus = status;
                                cmbARSales.Checked = cARSales = arSales;
                                cmbAgentSales.Checked = cAgentSales = agentSales;
                                cmbCashSales.Checked = cCashSales = cashSales;
                                cmbDiscount.Checked = cDiscount = discount;

                                cmbChecker.Checked = cChecker = checker;
                                cmbAuditor.Checked = cAuditor = auditor;
                                cmbEndbal.Checked = cEndbal = endbal;
                                cmbPullOut.Checked = cPullOut = pullout;
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
                    Cursor.Current = Cursors.Default;
                }
            }
        }

        public bool updateUser()
        {
            bool result = false;

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
                    //string branch = (cmbBranch.Text.Equals("") || cmbBranch.Text == "All" ? "" : cmbBranch.Text);
                    var request = new RestRequest("/api/auth/user/update/" + userID);
                    request.AddHeader("Authorization", "Bearer " + token);
                    request.Method = Method.PUT;

                    JObject jObject = new JObject();

                    if (txtUsername.Text != cUsername && !string.IsNullOrEmpty(txtUsername.Text.Trim()))
                    {
                        jObject.Add("username", txtUsername.Text.Trim());
                    }
                     if (txtFullName.Text != cFullName && !string.IsNullOrEmpty(txtFullName.Text.Trim()))
                    {
                        jObject.Add("fullname", txtFullName.Text.Trim());
                    }
                     if (!string.IsNullOrEmpty(txtConfirmPassword.Text.Trim()))
                    {
                        jObject.Add("password", txtConfirmPassword.Text.Trim());
                    }
                     if (cmbBranch.Text != cBranch && cmbBranch.SelectedIndex != -1)
                    {
                        jObject.Add("branch", cmbBranch.Text);
                    }
                     if (cmbWarehouse.Text != cWarehouse && cmbWarehouse.SelectedIndex != -1)
                    {
                        jObject.Add("whse", cmbWarehouse.Text);
                    }
                     if (cmbAdmin.Checked != cAdmin)
                    {
                        if (cmbAdmin.Checked)
                        {
                            jObject.Add("isAdmin", true);
                        }
                        else
                        {
                            jObject.Add("isAdmin", null);
                        }
                    }
                     if (cmbSales.Checked != cSales)
                    {
                        if (cmbSales.Checked)
                        {
                            jObject.Add("isSales", true);
                        }
                        else
                        {
                            jObject.Add("isSales", null);
                        }
                    }
                     if (cmbCashier.Checked != cCashier)
                    {
                        if (cmbCashier.Checked)
                        {
                            jObject.Add("isCashier", true);
                        }
                        else
                        {
                            jObject.Add("isCashier", null);
                        }
                    }
                     if (cmbManager.Checked != cManager)
                    {
                        if (cmbManager.Checked)
                        {
                            jObject.Add("isManager", true);
                        }
                        else
                        {
                            jObject.Add("isManager", null);
                        }
                    }
                     if (cmbAddSAP.Checked != cCanAddSap)
                    {
                        if (cmbAddSAP.Checked)
                        {
                            jObject.Add("isCanAddSap", true);
                        }
                        else
                        {
                            jObject.Add("isCanAddSap", null);
                        }
                    }
                     if (cmbTransfer.Checked != cTransfer)
                    {
                        if (cmbTransfer.Checked)
                        {
                            jObject.Add("isTransfer", true);
                        }
                        else
                        {
                            jObject.Add("isTransfer", null);
                        }
                    }
                     if (cmbReceive.Checked != cReceive)
                    {
                        if (cmbReceive.Checked)
                        {
                            jObject.Add("isReceive", true);
                        }
                        else
                        {
                            jObject.Add("isReceive", null);
                        }
                    }
                     if (cmbVoid.Checked != cVoid)
                    {
                        if (cmbVoid.Checked)
                        {
                            jObject.Add("isVoid", true);
                        }
                        else
                        {
                            jObject.Add("isVoid", null);
                        }
                    }
                     if (cmbStatus.Text != cStatus && cmbStatus.SelectedIndex != -1)
                    {
                        if (cmbStatus.Text == "Active")
                        {
                            jObject.Add("isActive", true);
                        }
                        else
                        {
                            jObject.Add("isActive", false);
                        }
                    }
                     if (cmbARSales.Checked != cARSales)
                    {
                        if (cmbARSales.Checked)
                        {
                            jObject.Add("isARSales", true);
                        }
                        else
                        {
                            jObject.Add("isARSales", null);
                        }
                    }
                     if (cmbAgentSales.Checked != cAgentSales)
                    {
                        if (cmbAgentSales.Checked)
                        {
                            jObject.Add("isAgentSales", true);
                        }
                        else
                        {
                            jObject.Add("isAgentSales", null);
                        }
                    }
                     if (cmbCashSales.Checked != cCashSales)
                    {
                        if (cmbCashSales.Checked)
                        {
                            jObject.Add("isCashSales", true);
                        }
                        else
                        {
                            jObject.Add("isCashSales", null);
                        }
                    }
                    if (cmbDiscount.Checked != cDiscount)
                    {
                        if (cmbDiscount.Checked)
                        {
                            jObject.Add("isDiscount", true);
                        }
                        else
                        {
                            jObject.Add("isDiscount", null);
                        }
                    }
                    if (cmbChecker.Checked != cChecker)
                    {
                        if (cmbChecker.Checked)
                        {
                            jObject.Add("isChecker", true);
                        }
                        else
                        {
                            jObject.Add("isChecker", null);
                        }
                    }
                    if (cmbAuditor.Checked != cAuditor)
                    {
                        if (cmbAuditor.Checked)
                        {
                            jObject.Add("isAuditor", true);
                        }
                        else
                        {
                            jObject.Add("isAuditor", null);
                        }
                    }
                    if (cmbEndbal.Checked != cEndbal)
                    {
                        if (cmbEndbal.Checked)
                        {
                            jObject.Add("isAllowEnding", true);
                        }
                        else
                        {
                            jObject.Add("isAllowEnding", null);
                        }
                    }
                    if (cmbPullOut.Checked != cPullOut)
                    {
                        if (cmbPullOut.Checked)
                        {
                            jObject.Add("isAllowPullOut", true);
                        }
                        else
                        {
                            jObject.Add("isAllowPullOut", null);
                        }
                    }

                    Console.WriteLine(jObject);
                    request.AddParameter("application/json", jObject, ParameterType.RequestBody);
                    var response = client.Execute(request);
                    jObject = JObject.Parse(response.Content.ToString());
                    foreach (var x in jObject)
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
            }

            return result;
        }

        public bool insertUser()
        {
            string username = txtUsername.Text.ToString().Trim(),
                fullname = txtFullName.Text.ToString().Trim(),
                password = txtConfirmPassword.Text.Trim(),
                branch = cmbBranch.Text.ToString(),
                warehouse = cmbWarehouse.Text.ToString();

            bool admin = cmbAdmin.Checked ? true : false,
                sales = cmbSales.Checked ? true : false,
                cashier = cmbCashier.Checked ? true : false,
                manager = cmbManager.Checked ? true : false,
                can_add_sap = cmbAddSAP.Checked ? true : false,
                transfer = cmbTransfer.Checked ? true : false,
                receive = cmbReceive.Checked ? true : false,
                voidd = cmbVoid.Checked ? true : false,
                arsales = cmbARSales.Checked ? true : false,
                agentsales = cmbAgentSales.Checked ? true : false,
                cashsales = cmbCashSales.Checked ? true : false,
                status = cmbStatus.SelectedIndex.Equals(0) ? true : false,
                 discount = cmbDiscount.Checked ? true : false,

                  checker = cmbChecker.Checked ? true : false,
                   auditor = cmbAuditor.Checked ? true : false,
                    endbal = cmbEndbal.Checked ? true : false,
                     pullout = cmbPullOut.Checked ? true : false;


            bool isSuccess = false;

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
                    //string branch = (cmbBranch.Text.Equals("") || cmbBranch.Text == "All" ? "" : cmbBranch.Text);
                    var request = new RestRequest("/api/auth/user/new");
                    request.AddHeader("Authorization", "Bearer " + token);
                    request.Method = Method.POST;

                    JObject jObject = new JObject();
                    jObject.Add("username", (username == String.Empty ? null : username));
                    jObject.Add("fullname", (fullname == String.Empty ? null : fullname));
                    jObject.Add("password", (password == String.Empty ? null : password));
                    jObject.Add("branch", (branch == String.Empty ? null : branch));
                    jObject.Add("whse", (warehouse == String.Empty ? null : warehouse));
                    if (admin)
                    {
                        jObject.Add("isAdmin", true);
                    }
                    else
                    {
                        jObject.Add("isAdmin", null);
                    }
                    if (sales)
                    {
                        jObject.Add("isSales", true);
                    }
                    else
                    {
                        jObject.Add("isSales", null);
                    }
                    if (cashier)
                    {
                        jObject.Add("isCashier", true);
                    }
                    else
                    {
                        jObject.Add("isCashier", null);
                    }
                    if (manager)
                    {
                        jObject.Add("isManager", true);
                    }
                    else
                    {
                        jObject.Add("isManager", null);
                    }
                    if (can_add_sap)
                    {
                        jObject.Add("isCanAddSap", true);
                    }
                    else
                    {
                        jObject.Add("isCanAddSap", null);
                    }
                    if (transfer)
                    {
                        jObject.Add("isTransfer", true);
                    }
                    else
                    {
                        jObject.Add("isTransfer", null);
                    }
                    if (receive)
                    {
                        jObject.Add("isReceive", true);
                    }
                    else
                    {
                        jObject.Add("isReceive", null);
                    }
                    if (voidd)
                    {
                        jObject.Add("isVoid", true);
                    }
                    else
                    {
                        jObject.Add("isVoid", null);
                    }
                    if (status)
                    {
                        jObject.Add("isActive", true);
                    }
                    else
                    {
                        jObject.Add("isActive", null);
                    }
                    if (arsales)
                    {
                        jObject.Add("isARSales", true);
                    }
                    else
                    {
                        jObject.Add("isARSales", null);
                    }
                    if (agentsales)
                    {
                        jObject.Add("isAgentSales", true);
                    }
                    else
                    {
                        jObject.Add("isAgentSales", null);
                    }
                    if (cashsales)
                    {
                        jObject.Add("isCashSales", true);
                    }
                    else
                    {
                        jObject.Add("isCashSales", null);
                    }
                    if (discount)
                    {
                        jObject.Add("isDiscount", true);
                    }
                    else
                    {
                        jObject.Add("isDiscount", null);
                    }

                    if (checker)
                    {
                        jObject.Add("isChecker", true);
                    }
                    else
                    {
                        jObject.Add("isChecker", null);
                    }
                    if (auditor)
                    {
                        jObject.Add("isAuditor", true);
                    }
                    else
                    {
                        jObject.Add("isAuditor", null);
                    }
                    if (endbal)
                    {
                        jObject.Add("isAllowEnding", true);
                    }
                    else
                    {
                        jObject.Add("isAllowEnding", null);
                    }
                    if (pullout)
                    {
                        jObject.Add("isAllowPullOut", true);
                    }
                    else
                    {
                        jObject.Add("isAllowPullOut", null);
                    }
                    //request.AddJsonBody(jObject);
                    Console.WriteLine(jObject);
                    request.AddParameter("application/json", jObject, ParameterType.RequestBody);
                    var response = client.Execute(request);
                    jObject = JObject.Parse(response.Content.ToString());
                    foreach (var x in jObject)
                    {
                        if (x.Key.Equals("success"))
                        {
                            isSuccess = Convert.ToBoolean(x.Value.ToString());
                        }
                    }
                    if (!isSuccess)
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
                    Cursor.Current = Cursors.Default;
                }
            }
            return isSuccess;
        }

        private void AddUser_Load(object sender, EventArgs e)
        {
            loadBranches();
            if (this.Text== "Edit User")
            {
                getUserDetails();
            }
        }

        public void loadBranches()
        {
            DataTable dataTable = new DataTable();
            dataTable = branchc.returnBranches();
            cmbBranch.Items.Clear();
            foreach (DataRow r0w in dataTable.Rows)
            {
                cmbBranch.Items.Add(r0w["code"].ToString());
            }
        }

        private void cmbBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dataTable = new DataTable();
            dataTable = warehousec.returnWarehouse(cmbBranch.Text);
            cmbWarehouse.Items.Clear();
            foreach (DataRow r0w in dataTable.Rows)
            {
                cmbWarehouse.Items.Add(r0w["whsecode"].ToString());
            }
        }
    }
}
