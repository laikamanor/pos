using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AB.API_Class.Transfer;
using Newtonsoft.Json.Linq;
using AB.UI_Class;
using RestSharp;

namespace AB
{
    public partial class TransferItems : Form
    {
        transfer_class transferc = new transfer_class();
        utility_class utilityc = new utility_class();
        public int selectedID = 0;
        public static bool isSubmit = false;
        string gForType = "";
        public TransferItems(string forType)
        {
            gForType = forType;
            InitializeComponent();
        }

        private void TransferItems_Load(object sender, EventArgs e)
        {
            dgvitems.Columns["itemname"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            loadData();
        }

        public void checkVariance()
        {
            for (int i = 0; i < dgvitems.Rows.Count; i++)
            {
                if (Convert.ToDouble(dgvitems.Rows[i].Cells["variance"].Value.ToString()) == 0)
                {
                    //dgvitems.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                }
                else if(Convert.ToDouble(dgvitems.Rows[i].Cells["variance"].Value.ToString()) < 0){
                    dgvitems.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(248, 255, 43);
                }
                else if (Convert.ToDouble(dgvitems.Rows[i].Cells["variance"].Value.ToString()) > 0){
                    dgvitems.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(0, 227, 76);
                }
            }
        }

        public void loadData()
        {
            DataTable dtItems = new DataTable();
            string URL = "";
            if (this.Text.Equals("Transfer Items"))
            {
                URL = "inv/trfr";
            }
            else if (this.Text.Equals("Received Items"))
            {
                URL = "inv/recv";
            }
            else
            {
                URL = "pullout";

            }
            dtItems = transferc.loadItems(URL, selectedID);
            if(dtItems.Rows.Count > 0)
            {
                foreach(DataRow row in dtItems.Rows)
                {
                    string decodeDocStatus = row["docstatus"].ToString() == "O" ? "Open" : row["docstatus"].ToString() == "C" ? "Closed" : "Cancelled";
                    double quantity = Convert.ToDouble(row["quantity"].ToString());
                    if (URL.Equals("pullout"))
                    {
                        dgvitems.Rows.Add(row["id"], row["transfer_id"], row["item_code"], Convert.ToDecimal(string.Format("{0:0.00}", quantity)));
                    }
                    else
                    {
                        double actualRec = Convert.ToDouble(row["actualrec"].ToString());
                        double variance = (actualRec - quantity);
                        dgvitems.Rows.Add(row["id"], row["transfer_id"], row["item_code"], Convert.ToDecimal(string.Format("{0:0.00}", quantity)), Convert.ToDecimal(string.Format("{0:0.00}", actualRec)), Convert.ToDecimal(string.Format("{0:0.00}", variance)));
                    }
                    lblDocumentStatus.Text =decodeDocStatus;
                    lblReference.Text = row["reference"].ToString();
                    lblTransDate.Text = row["transdate"].ToString();
                    lblToWhse.Text = row["to_whse"].ToString();
                    label5.Text= (URL.Equals("inv/recv") ? "From Warehouse:" : "To Warehouse");
                }
            }
            if(this.Text=="Received Items")
            {
                for (int i = 0; i < dgvitems.Rows.Count; i++)
                {
                    //MessageBox.Show(Convert.ToDouble(dgvitems.Rows[i].Cells["variance"].Value.ToString()).ToString("n2"));
                    if (Convert.ToDouble(dgvitems.Rows[i].Cells["variance"].Value.ToString()) == 0.00)
                    {
                        dgvitems.Rows[i].Cells["variance"].Style.ForeColor = Color.Black;
                    }
                    else if (Convert.ToDouble(dgvitems.Rows[i].Cells["variance"].Value.ToString()) > 0.00)
                    {
                        dgvitems.Rows[i].Cells["variance"].Style.ForeColor = Color.Blue;
                    }
                    else if (Convert.ToDouble(dgvitems.Rows[i].Cells["variance"].Value.ToString()) < 0.00)
                    {
                        dgvitems.Rows[i].Cells["variance"].Style.ForeColor = Color.Red;
                    }
                }
            }
            if (gForType.Equals("For Transactions") && this.Text.Equals("Pullout Items"))
            {
                btnCancel.Visible = true;
                btnCancel.Text = "Confirm";
                btnCancel.BackColor = Color.DodgerBlue;
            }
            else if (gForType.Equals("For SAP") && this.Text.Equals("Received Items"))
            {
                btnCancel.Visible = true;
                btnCancel.Text = "Update SAP #";
                btnCancel.BackColor = Color.DodgerBlue;
            }
            else if(gForType.Equals("For Transactions") && this.Text.Equals("Received Items"))
            {
                btnCancel.Visible = true;
                btnCancel.Text = "Cancel";
                btnCancel.BackColor = Color.Firebrick;
            }
            else if (gForType.Equals("For SAP") && this.Text.Equals("Pullout Items"))
            {   
                btnCancel.Visible = true;
                btnCancel.Text = "Update SAP #";
                btnCancel.BackColor = Color.DodgerBlue;
            }
            else
            {
                btnCancel.Visible = false;
            }
            dgvitems.Columns["actualrec"].Visible= (this.Text.Equals("Pullout Items") ? false : true);
            dgvitems.Columns["variance"].Visible = ( this.Text.Equals("Pullout Items") ? false : true);
            if (this.Text == "Transfer Items")
            {
                checkVariance();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if(gForType.Equals("For Transactions"))
            {
                forCancel();
            }
            else
            {
                forUpdatingSAP();
            }
        }

        public void forUpdatingSAP()
        {
            JObject jObjectBody = new JObject();
            if (this.Text.Equals("Pullout Items"))
            {
                SAPWarehouse sAPWarehouse = new SAPWarehouse();
                sAPWarehouse.ShowDialog();
                if (SAPWarehouse.isSubmit)
                {
                    int sapNumber = SAPWarehouse.sapNumber;
                    string warehouseCode = SAPWarehouse.warehouseCode;
                    jObjectBody.Add("sap_number", sapNumber);
                    jObjectBody.Add("to_whse", warehouseCode);
                    string URL = "/api/pullout/transfer/" + selectedID;
                    apiPUT(jObjectBody, URL);
                }
            }
            else
            {
                SAPNumber sAPNumber = new SAPNumber();
                sAPNumber.ShowDialog();
                if (SAPNumber.isSubmit)
                {
                    int sap_number = SAPNumber.sap_number;

                    if (sap_number.Equals(0))
                    {
                        jObjectBody.Add("sap_number", null);
                    }
                    else
                    {
                        jObjectBody.Add("sap_number", sap_number);
                    }
                }
                string URL = (this.Text.Equals("Pullout Items") ? "/api/sap_num/pullout/update?ids=" + "%5B" + selectedID + "%5D" : "/api/inv/recv/update/" + selectedID);
                apiPUT(jObjectBody, URL);
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

                    Console.WriteLine(body);
                    request.AddParameter("application/json", body, ParameterType.RequestBody);
                    var response = client.Execute(request);
                    if (response.ErrorMessage == null)
                    {
                        JObject jObjectResponse = JObject.Parse(response.Content);

                        foreach (var x in jObjectResponse)
                        {
                            if (x.Key.Equals("success"))
                            {
                                isSubmit = true;
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
                        MessageBox.Show(msg, "", MessageBoxButtons.OK, isSubmit ? MessageBoxIcon.Information : MessageBoxIcon.Warning);

                        if (isSubmit)
                        {
                            this.Dispose();
                        }
                    }
                    else
                    {
                        MessageBox.Show(response.ErrorMessage, "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    
                }
            }
        }

        public void forCancel()
        {
            if (this.Text.Equals("Received Items") || this.Text.Equals("Transfer Items"))
            {
                    Remarks remarkss = new Remarks();
                    remarkss.ShowDialog();
                    string remarks = Remarks.rem;
                if (Remarks.isSubmit)
                {

                    DialogResult dialogResult = MessageBox.Show("Are you sure you want to cancel?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        string type = this.Text.Equals("Received Items") ? "recv" : "trfr";
                        string sResponse = transferc.cancelTransfer(selectedID, remarks, type);
                        JObject jObjectResponse = JObject.Parse(sResponse);
                        string msg = "";
                        foreach (var x in jObjectResponse)
                        {
                            if (x.Key.Equals("message"))
                            {
                                msg = x.Value.ToString();
                            }
                        }
                        if (!string.IsNullOrEmpty(msg))
                        {
                            MessageBox.Show(msg, "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            isSubmit = true;
                            this.Dispose();
                        }
                    }
                }
            }
            else if (this.Text == "Pullout Items")
            {
                DialogResult dialogResult = MessageBox.Show("Are you sure you want to confirm?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    JObject jObjectBody = new JObject();
                    string URL = "/api/pullout/confirm/" + selectedID;
                    apiPUT(jObjectBody, URL);
                }
            }
        }
    }
}
