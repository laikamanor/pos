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
using RestSharp;
using AB.UI_Class;
using ClosedXML.Excel;
namespace AB
{
    public partial class SalesTransactions : Form
    {
        DataTable dtBranch, dtWarehouse;
        branch_class branchc = new branch_class();
        warehouse_class warehousec = new warehouse_class();
        utility_class utilityc = new utility_class();

        int cDocStatus = 1, cBranch = 1, cWarehouse = 1, cDate = 1;
        public SalesTransactions()
        {
            InitializeComponent();
        }

        private void dgv_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgv.Rows.Count > 0)
            {
                SalesTransactions_Items salesItems = new SalesTransactions_Items();
                salesItems.selectedID = Convert.ToInt32(dgv.CurrentRow.Cells["id"].Value.ToString());
                salesItems.lblReference.Text = dgv.CurrentRow.Cells["reference"].Value.ToString();
                salesItems.lblSAP.Text = dgv.CurrentRow.Cells["sap_number"].Value.ToString();
                salesItems.lblDocStatus.Text = dgv.CurrentRow.Cells["docstatus"].Value.ToString();
                salesItems.lblTransdate.Text = dgv.CurrentRow.Cells["transdate"].Value.ToString();
                salesItems.ShowDialog();
            }
        }

        private void SalesTransactions_Load(object sender, EventArgs e)
        {
            dtBranch = new DataTable();
            dtWarehouse = new DataTable();
            cmbDocStatus.SelectedIndex = 0;
            dtTransDate.Value = DateTime.Now;
            dtTransDate.Visible = false;
            loadBranch();
            loadWarehouse();
            loadData();
            cDocStatus = 0;
            cBranch = 0;
            cWarehouse = 0;
            cDate = 0;
            btnGenerateExcel.Visible = canGenerateExcel();
            dgv.Columns["doctotal"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
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
            dtWarehouse = warehousec.returnWarehouse(branchCode);
            cmbWhse.Items.Clear();
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
                                    cmbWhse.Items.Add("All");
                                    foreach (DataRow row in dtWarehouse.Rows)
                                    {
                                        cmbWhse.Items.Add(row["whsename"].ToString());
                                        cmbWhse.SelectedIndex = 0;
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
                        cmbWhse.Items.Add(whseName);
                    }
                }
                cmbWhse.SelectedIndex = cmbWhse.Items.IndexOf(whseName);
            }
        }

        private void cmbBranch_SelectedValueChanged(object sender, EventArgs e)
        {
            if(cBranch <= 0)
            {
                loadWarehouse();
            }
        }

        private void cmbWhse_SelectedValueChanged(object sender, EventArgs e)
        {
            if(cWarehouse <= 0)
            {
                loadData();
            }
        }

        private void dtTransDate_ValueChanged(object sender, EventArgs e)
        {
            if(cDate <= 0)
            {
                loadData();
            }
        }

        public bool canGenerateExcel()
        {
            bool isCanGenerateExcel = false;
            if (Login.jsonResult != null)
            {
                foreach (var x in Login.jsonResult)
                {
                    if (x.Key.Equals("data"))
                    {
                        JObject jObjectData = JObject.Parse(x.Value.ToString());
                        foreach (var y in jObjectData)
                        {
                            if (y.Key.Equals("isCanAddSap"))
                            {
                                if (y.Value.ToString().ToLower() == "true")
                                {
                                    isCanGenerateExcel = true;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            return isCanGenerateExcel;
        }

        private void btnGenerateExcel_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
            try
            {
                if (canGenerateExcel())
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("name");
                    dt.Columns.Add("amount");
                    dt.Columns.Add("sap_number");

                    for (int i = 0; i < dgv.Rows.Count; i++)
                    {
                        string name = dgv.Rows[i].Cells["cust_code"].Value.ToString(),
                            amount = dgv.Rows[i].Cells["doctotal"].Value.ToString(),
                            sap_number = dgv.Rows[i].Cells["sap_number"].Value.ToString();

                        dt.Rows.Add(name, amount, sap_number);
                    }

                    saveFileDialog1.Title = "Save As Excel File";
                    saveFileDialog1.Filter = "Excel Document (*.xlsx) | *.xlsx";
                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {

                        using (XLWorkbook wb = new XLWorkbook())
                        {
                            wb.Worksheets.Add(dt, "Sheet1");
                            wb.Protect(true, true, "atlantic");
                            wb.SaveAs(saveFileDialog1.FileName.ToString());
                        }
                        string path = System.IO.Path.GetDirectoryName(saveFileDialog1.FileName);
                        this.Cursor = Cursors.Default;
                        MessageBox.Show("Saved" + Environment.NewLine + path, "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Access Denied", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            this.Cursor = Cursors.Default;
        }

        private void checkTransDate_CheckedChanged(object sender, EventArgs e)
        {
            dtTransDate.Visible = checkTransDate.Checked;
            loadData();
        }


        private void checkSAP_CheckedChanged(object sender, EventArgs e)
        {
            loadData();
        }

        private void cmbDocStatus_SelectedValueChanged(object sender, EventArgs e)
        {
           if(cDocStatus <= 0)
            {
                loadData();
            }
        }

        public void loadData()
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
                    var client = new RestClient(utilityc.URL);
                    client.Timeout = -1;

                    string sDocStatus = "";
                    if (cmbDocStatus.Text == "All")
                    {
                        sDocStatus = "";
                    }
                    else if (cmbDocStatus.Text == "Open")
                    {
                        sDocStatus = "?docstatus=O";
                    }
                    else if (cmbDocStatus.Text == "Closed")
                    {
                        sDocStatus = "?docstatus=C";
                    }
                    else if (cmbDocStatus.Text == "Cancelled")
                    {
                        sDocStatus = "?docstatus=N";
                    }

                    //(checkSAP.Checked ? (string.IsNullOrEmpty(sDocStatus) ? "?" : "&" + "sap_number=1") : (string.IsNullOrEmpty(sDocStatus) ? "?" : "&" + "sap_number="));
                    string sSAPNumber = "";
                    string firstS = string.IsNullOrEmpty(sDocStatus) ? "?" : "&";
                    if (checkSAP.Checked)
                    {
                      sSAPNumber=  firstS + "sap_number=1";
                    }else
                    {
                        sSAPNumber = firstS + "sap_number=";
                    }

                    string warehouseCode = "", branchCode = "";
                    //WAREHOUSE
                    foreach (DataRow row in dtWarehouse.Rows)
                    {
                        if (cmbWhse.Text.Equals(row["whsename"].ToString()))
                        {
                            warehouseCode = row["whsecode"].ToString();
                            break;
                        }
                    }
                    //BRANCH
                    foreach (DataRow row in dtBranch.Rows)
                    {
                        if (cmbBranch.Text.Equals(row["name"].ToString()))
                        {
                            branchCode = row["code"].ToString();
                            break;
                        }
                    }
                    string sWarehouse = string.IsNullOrEmpty(warehouseCode) ? "" : "&whsecode=" + warehouseCode;
                    string sBranch = string.IsNullOrEmpty(branchCode) ? "" : "&branch=" + branchCode;
                    string sTransdate = !checkTransDate.Checked ? "&transdate=" : "&transdate=" + dtTransDate.Value.ToString("yyyy-MM-dd");

                    var request = new RestRequest("/api/sales/get_all" + sDocStatus + sSAPNumber + sBranch + sWarehouse + sTransdate);
                    Console.WriteLine("/api/inv/item_request/get_all" + sDocStatus + sSAPNumber + sBranch + sWarehouse + sTransdate);
                    request.AddHeader("Authorization", "Bearer " + token);
                    var response = client.Execute(request);
                    JObject jObjectResponse = JObject.Parse(response.Content);

                    bool isSuccess = false;
                    //AutoCompleteStringCollection auto = new AutoCompleteStringCollection();
                    dgv.Rows.Clear();
                    string msg = "";
                    foreach (var x in jObjectResponse)
                    {
                        if (x.Key.Equals("success"))
                        {
                            isSuccess = Convert.ToBoolean(x.Value.ToString());
                        }
                        else if (x.Key.Equals("message"))
                        {
                            msg = x.Value.ToString();
                        }

                    }
                    if (isSuccess)
                    {
                        foreach (var z in jObjectResponse)
                        {
                            if (z.Key.Equals("data"))
                            {
                                if (z.Value.ToString() != "[]")
                                {
                                    int id = 0;
                                    string referenceNumber = "", custCode = "", transType = "", sapNumber = "", docStatus = "";
                                    double docTotal = 0.00;
                                    DateTime dtTransDate = new DateTime();
                                    JArray jsonArray = JArray.Parse(z.Value.ToString());
                                    for (int i = 0; i < jsonArray.Count(); i++)
                                    {
                                        JObject jObjectData = JObject.Parse(jsonArray[i].ToString());
                                        foreach (var y in jObjectData)
                                        {
                                            if (y.Key.Equals("id"))
                                            {
                                                id = Convert.ToInt32(y.Value.ToString());
                                            }
                                            else if (y.Key.Equals("reference"))
                                            {
                                                referenceNumber = y.Value.ToString();
                                            }
                                            else if (y.Key.Equals("transdate"))
                                            {
                                                string replaceT = y.Value.ToString().Replace("T", "");
                                                dtTransDate = Convert.ToDateTime(replaceT);
                                            }
                                            else if (y.Key.ToString() == "cust_code")
                                            {
                                                custCode = y.Value.ToString();
                                            }
                                            else if (y.Key.ToString() == "doctotal")
                                            {
                                                docTotal = Convert.ToDouble(y.Value.ToString());
                                            }
                                            else if (y.Key.ToString() == "transtype")
                                            {
                                                transType = y.Value.ToString();
                                            }
                                            else if (y.Key.ToString() == "sap_number")
                                            {
                                                sapNumber = y.Value.ToString();
                                            }
                                            else if (y.Key.ToString() == "docstatus")
                                            {
                                                docStatus = y.Value.ToString();
                                            }
                                        }
                                        string docStatusEncode = docStatus.Equals("O") ? "Open" : docStatus.Equals("C") ? "Closed" : "Cancelled";
                                        dgv.Rows.Add(id, referenceNumber, custCode, docTotal.ToString("n2") ,transType, sapNumber, docStatusEncode, dtTransDate.ToString("yyyy-MM-dd"));
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show(msg, "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            lblNoDataFound.Visible = (dgv.Rows.Count > 0 ? false : true);
        }
    }
}
