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
using AB.UI_Class;
using RestSharp;
using AB.API_Class.Payment_Type;
namespace AB
{
    public partial class forSAPIP2 : Form
    {
        utility_class utilityc = new utility_class();
        paymenttype_class paymenttypec = new paymenttype_class();
        string gForType = "", gSalesType = "";
        DataTable dtPaymentTypes;
        int cPaymentType = 1, cDate = 1;
        public forSAPIP2(string salesType, string forType)
        {
            gForType = forType;
            gSalesType = salesType;
            InitializeComponent();
        }

        private void checkTransDate_CheckedChanged(object sender, EventArgs e)
        {
            dtTransDate.Visible = !checkTransDate.Checked;
            loadData();
        }

        private void dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        public bool haveSelected()
        {
            int int_result = 0;
            bool result = false;    
            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                if (Convert.ToBoolean(dgv.Rows[i].Cells["selectt"].Value.ToString()) == true)
                {
                    int_result += 1;
                }
            }
            result = int_result > 0;
            return result;
        }

        private void btnProceed_Click(object sender, EventArgs e)
        {
            if (dgv.Rows.Count <= 0)
            {
                MessageBox.Show("No data found", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }else if (!haveSelected())
            {
                MessageBox.Show("No data selected", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string ids = "";
                for (int i = 0; i < dgv.Rows.Count; i++)
                {
                    if (Convert.ToBoolean(dgv.Rows[i].Cells["selectt"].Value.ToString()) == true)
                    {
                        ids = ids + "," + dgv.Rows[i].Cells["id"].Value.ToString();
                    }
                }
                ids = (string.IsNullOrEmpty(ids) ? "" : ids.Substring(1));
                string sIDs = string.IsNullOrEmpty(ids) ? "" : "?ids=%5B" + ids + "%5D";
                JObject jObjectBody = new JObject();
                SAPNumber sAPNumber = new SAPNumber();
                sAPNumber.ShowDialog();
                if (SAPNumber.isSubmit)
                {
                    jObjectBody.Add("sap_number", SAPNumber.sap_number);
                    apiPUT(jObjectBody, "/api/sap_num/payment/update" + sIDs);
                }
            }

        }

        public void apiPUT(JObject body, string URL)
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
                    var request = new RestRequest(URL);
                    Console.WriteLine(URL);
                    request.AddHeader("Authorization", "Bearer " + token);
                    request.Method = Method.PUT;


                    request.AddParameter("application/json", body, ParameterType.RequestBody);
                    var response = client.Execute(request);
                    Console.WriteLine(response.Content);
                    JObject jObjectResponse = JObject.Parse(response.Content);

                    foreach (var x in jObjectResponse)
                    {
                        if (x.Key.Equals("success"))
                        {
                            loadData();
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
                    MessageBox.Show(msg, "", MessageBoxButtons.OK,  MessageBoxIcon.Information);

                }
            }
        }

        private void dtTransDate_ValueChanged(object sender, EventArgs e)
        {
            if(cDate <= 0)
            {
                loadData();
            }
        }

        private void forSAPIP2_Load(object sender, EventArgs e)
        {
            dtPaymentTypes = new DataTable();
            dtTransDate.Value = DateTime.Now;
            loadPaymentTypes();
            cmbPaymentType.SelectedIndex = 0;
            checkTransDate.Checked = true;
            loadData();
            cPaymentType = 0;
            cDate = 0;
        }

        private void cmbPaymentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cPaymentType <= 0)
            {
                loadData();
            }
        }

        private void dgv_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgv.Rows.Count > 0)
            {
                if (e.ColumnIndex.Equals(0))
                {
                    selectData();
                }
            }
        }

        public void selectData()
        {
            if (dgv.Rows.Count > 0)
            {
                dgv.CommitEdit(DataGridViewDataErrorContexts.Commit);
                if (Convert.ToBoolean(dgv.CurrentRow.Cells["selectt"].Value.ToString()) == true)
                {
                    double amount = 0.00;
                    for (int i = 0; i < dgv.Rows.Count; i++)
                    {
                        if (Convert.ToBoolean(dgv.Rows[i].Cells["selectt"].Value.ToString()) == true)
                        {
                            amount += Convert.ToDouble(dgv.Rows[i].Cells["amount"].Value.ToString());
                        }
                    }
                    lblTotalAmount.Text = amount.ToString("n2");
                }
            }
        }

        private void checkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            if(dgv.Rows.Count > 0)
            {
                toggleSelectAll(checkSelectAll.Checked);
                selectData();
                if (!checkSelectAll.Checked)
                {
                    lblTotalAmount.Text = "0.00";
                }
            }
            else if(dgv.Rows.Count <= 0 && checkSelectAll.Checked)
            {
                lblTotalAmount.Text = "0.00";
                MessageBox.Show("No data to select", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void toggleSelectAll(bool value)
        {
            if (dgv.Rows.Count > 0)
            {
                for (int i = 0; i < dgv.Rows.Count; i++)
                {
                    dgv.Rows[i].Cells["selectt"].Value = value;
                }
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

        public void loadPaymentTypes()
        {
            dtPaymentTypes = paymenttypec.loadPaymentType("payment");
            cmbPaymentType.Items.Clear();
            cmbPaymentType.Items.Add("All");
            foreach (DataRow row in dtPaymentTypes.Rows)
            {
                cmbPaymentType.Items.Add(row["description"].ToString());
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
                    string sTransDate = checkTransDate.Checked ? "?transdate=" : "?transdate=" + dtTransDate.Value.ToString("yyyy-MM-dd");
                    string sSalesType = string.IsNullOrEmpty(gSalesType) ? "" : "&sales_type=" + gSalesType;

                    
                    string paymentCode = "";
                    foreach (DataRow row in dtPaymentTypes.Rows)
                    {
                        if (cmbPaymentType.SelectedIndex != 0 || !cmbPaymentType.Text.Equals("All"))
                        {
                            if (cmbPaymentType.Text.Equals(row["description"].ToString()))
                            {
                                paymentCode = row["code"].ToString();
                                break;
                            }
                        }
                    }

                    string sPaymentType = string.IsNullOrEmpty(paymentCode) ? "&payment_type=" : "&payment_type=" + paymentCode;
                    string sSearch = "&search=" + txtSearch.Text.Trim();
                    string sSAPNumber = "&sap_number=";
                    var request = new RestRequest("/api/sap_num/payment/update" + sTransDate + sSalesType + sSalesType + sPaymentType + sSearch + sSAPNumber);
                    Console.WriteLine("/api/sap_num/payment/update" + sTransDate + sSalesType + sSalesType + sPaymentType + sSearch + sSAPNumber);
                    //Console.WriteLine("/api/sap_num/payment/update" + sTransDate + sSalesType + sSalesType + sPaymentType + sSearch);
                    request.AddHeader("Authorization", "Bearer " + token);
                    var response = client.Execute(request);
                    JObject jObjectResponse = JObject.Parse(response.Content);
                    bool isSuccess = false;
                    AutoCompleteStringCollection auto = new AutoCompleteStringCollection();
                    dgv.Rows.Clear();
                    foreach (var x in jObjectResponse)
                    {
                        if (x.Key.Equals("success"))
                        {
                            isSuccess = Convert.ToBoolean(x.Value.ToString());
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
                                    JArray jsonArray = JArray.Parse(z.Value.ToString());
                                    for (int i = 0; i < jsonArray.Count(); i++)
                                    {
                                        JObject jObjectData = JObject.Parse(jsonArray[i].ToString());
                                        int id = 0, paymentID = 0;
                                        string paymentType = "", customerCode = "", paymentReference = "";
                                        double amount = 0.00;
                                        foreach (var y in jObjectData)
                                        {
                                            if (y.Key.Equals("id"))
                                            {
                                                id = Convert.ToInt32(y.Value.ToString());
                                            }
                                            else if (y.Key.Equals("payment_id"))
                                            {
                                                paymentID = Convert.ToInt32(y.Value.ToString());
                                            }
                                            else if (y.Key.Equals("payment_type"))
                                            {
                                                paymentType = y.Value.ToString();
                                            }
                                            else if (y.Key.Equals("cust_code"))
                                            {
                                                customerCode = y.Value.ToString();
                                            }
                                            else if (y.Key.Equals("amount"))
                                            {
                                                amount = Convert.ToDouble(y.Value.ToString());
                                            }
                                            else if (y.Key.Equals("reference"))
                                            {
                                                paymentReference = y.Value.ToString();
                                            }
                                        }
                                        auto.Add(customerCode);
                                        dgv.Rows.Add(false, id, paymentID, customerCode, paymentType, amount, paymentReference);
                                    }
                                }
                            }
                        }
                        txtSearch.AutoCompleteCustomSource = auto;
                    }
                }
            }
            lblNoDataFound.Visible = (dgv.Rows.Count > 0 ? false : true);
        }
    }
}
