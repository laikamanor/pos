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
using Newtonsoft.Json.Linq;
using RestSharp;

namespace AB
{
    public partial class CashTransactionReportItems : Form
    {
        utility_class utilityc = new utility_class();
        public string URLDetails = "";
        public CashTransactionReportItems()
        {
            InitializeComponent();
        }

        private void CashTransactionReportItems_Load(object sender, EventArgs e)
        {
            loadData();
        }

        public void loadData()
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
                    dgvitems.Rows.Clear();
                    var client = new RestClient(utilityc.URL);
                    client.Timeout = -1;

                    var request = new RestRequest(URLDetails);
                    request.AddHeader("Authorization", "Bearer " + token);
                    var response = client.Execute(request);
                    JObject jObject = JObject.Parse(response.Content);
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
                                JObject jObjectData = JObject.Parse(x.Value.ToString());
                                foreach (var w in jObjectData)
                                {
                                    if (w.Key.Equals("payrows"))
                                    {
                                        JArray jsonArraySalesRow = JArray.Parse(w.Value.ToString());
                                        for (int i = 0; i < jsonArraySalesRow.Count(); i++)
                                        {
                                            JObject jObjectSalesRow = JObject.Parse(jsonArraySalesRow[i].ToString());
                                            int ID = 0, paymentID = 0;
                                            string paymentType = "", referenceNumber = "", sapNumber = "";
                                            double amountt = 0.00;
                                            foreach (var e in jObjectSalesRow)
                                            {
                                                if (e.Key.Equals("id"))
                                                {
                                                    ID = Convert.ToInt32(e.Value.ToString());
                                                }
                                                else if (e.Key.Equals("payment_id"))
                                                {
                                                    paymentID = Convert.ToInt32(e.Value.ToString());
                                                }
                                                else if (e.Key.Equals("payment_type"))
                                                {
                                                    paymentType =e.Value.ToString();
                                                }
                                                else if (e.Key.Equals("reference"))
                                                {
                                                    referenceNumber = e.Value.ToString();
                                                }
                                                else if (e.Key.Equals("sap_number"))
                                                {
                                                    sapNumber = e.Value.ToString();
                                                }
                                                else if (e.Key.Equals("amount"))
                                                {
                                                    amountt = Convert.ToDouble(e.Value.ToString());
                                                }

                                            }
                                            dgvitems.Rows.Add(ID,paymentID,paymentType,amountt.ToString("n2"),referenceNumber,sapNumber);
                                        }
                                    }
                                    else if (w.Key.Equals("reference"))
                                    {
                                        txtReference.Text = w.Value.ToString();
                                    }
                                    else if (w.Key.Equals("transdate"))
                                    {
                                        DateTime dtTransDate = new DateTime();
                                        string replaceT = w.Value.ToString().Replace("T", "");
                                        dtTransDate = Convert.ToDateTime(replaceT);
                                        txtTransDate.Text = dtTransDate.ToString("yyyy-MM-dd");
                                    }
                                    else if (w.Key.Equals("cust_code"))
                                    {
                                        txtCustomerCode.Text = w.Value.ToString();
                                    }
                                    else if (w.Key.Equals("docstatus"))
                                    {
                                        string decodeDocStatus = w.Value.ToString() == "O" ? "Open" : w.Value.ToString() == "C" ? "Closed" : "Cancelled";
                                        txtDocStatus.Text = decodeDocStatus;
                                    }
                                    else if (w.Key.Equals("sap_number"))
                                    {
                                        txtSAPNumber.Text = (string.IsNullOrEmpty(w.Value.ToString()) ? "N/A" : w.Value.ToString());
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
