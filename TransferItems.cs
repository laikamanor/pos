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
            loadData();
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
                    if (URL.Equals("inv/trfr") || URL.Equals("pullout"))
                    {
                        dgvitems.Rows.Add(row["id"], row["transfer_id"], row["item_code"], Convert.ToDouble(row["quantity"]).ToString("n2"));
                    }
                    else
                    {
                        double variance = (Convert.ToDouble(row["actualrec"].ToString()) - Convert.ToDouble(row["quantity"].ToString()));
                        dgvitems.Rows.Add(row["id"], row["transfer_id"], row["item_code"], Convert.ToDouble(row["quantity"]).ToString("n2"),Convert.ToDouble(row["actualrec"].ToString()).ToString("n2"),variance.ToString("n2"));
                    }
                    lblDocumentStatus.Text =decodeDocStatus;
                    lblReference.Text = row["reference"].ToString();
                    lblTransDate.Text = row["transdate"].ToString();
                    lblToWhse.Text = row["to_whse"].ToString();
                    label5.Text= (URL.Equals("recv") ? "From Warehouse:" : "To Warehouse");
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
            if (gForType.Equals("For Transactions") && !this.Text.Equals("Pullout Items"))
            {
                btnCancel.Visible = true;
                btnCancel.Text = "Cancel";
                btnCancel.BackColor = Color.Firebrick;
            }
            else if(gForType.Equals("For SAP") && this.Text.Equals("Received Items"))
            {
                btnCancel.Visible = true;
                btnCancel.Text = "Update SAP #";
                btnCancel.BackColor = Color.DodgerBlue;
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
            dgvitems.Columns["actualrec"].Visible= (this.Text.Equals("Transfer Items") || this.Text.Equals("Pullout Items") ? false : true);
            dgvitems.Columns["variance"].Visible = (this.Text.Equals("Transfer Items") || this.Text.Equals("Pullout Items") ? false : true);
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
            if(this.Text.Equals("Pullout Items"))
            {
                SAP_Remarks sAP_Remarks = new SAP_Remarks();
                sAP_Remarks.ShowDialog();
                int sap_number = SAP_Remarks.sap_number;
                string remarks = SAP_Remarks.rem;
                if (sap_number.Equals(0))
                {
                    jObjectBody.Add("sap_number", null);
                }
                else
                {
                    jObjectBody.Add("sap_number", sap_number);
                }
                jObjectBody.Add("remarks", remarks.Trim());
            }
            else
            {
                SAPNumber sAPNumber = new SAPNumber();
                sAPNumber.ShowDialog();
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
            string URL = (this.Text.Equals("Pullout Items") ? "/api/sap_num/pullout/update?ids=" + "%5B" + selectedID + "%5D"  : "/api/inv/recv/update/" + selectedID);
            apiPUT(jObjectBody, URL);
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
            }
        }

        public void forCancel()
        {
            if (lblDocumentStatus.Text.Equals("Open") && this.Text.Equals("Transfer Items"))
            {
                Remarks remarkss = new Remarks();
                remarkss.ShowDialog();
                string remarks = Remarks.rem;
                if (!string.IsNullOrEmpty(remarks))
                {
                    DialogResult dialogResult = MessageBox.Show("Are you sure you want to cancel?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        string sResponse = transferc.cancelTransfer(selectedID, remarks);
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
        }
    }
}
