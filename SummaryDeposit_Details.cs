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
using Newtonsoft.Json.Linq;

namespace AB
{
    public partial class SummaryDeposit_Details : Form
    {
        public SummaryDeposit_Details()
        {
            InitializeComponent();
        }
        utility_class utilityc = new utility_class();
        int cFromDate = 1, cToDate = 1;
        private void SummaryDeposit_Details_Load(object sender, EventArgs e)
        {
            dtFromDate.Value = DateTime.Now;
            dtToDate.Value = DateTime.Now;
            loadData();
            cFromDate = 0;
            cToDate = 0;
        }

        public void loadData()
        {
            dgv.Rows.Clear();
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
                    bool isSuccess = false;
                    var client = new RestClient(utilityc.URL);
                    client.Timeout = -1;
                    var request = new RestRequest("/api/deposit/summary/details?cust_code=" + lblCustomerCode.Text + "&from_date=" + dtFromDate.Value.ToString("yyyy-MM-dd") + "&to_date=" + dtToDate.Value.ToString("yyyy-MM-dd"));
                    Console.WriteLine("/api/deposit/summary/details?" + lblCustomerCode.Text + "&from_date=" + dtFromDate.Value.ToString("yyyy-MM-dd") + "&to_date=" + dtToDate.Value.ToString("yyyy-MM-dd"));
                    request.AddHeader("Authorization", "Bearer " + token);
                    request.Method = Method.GET;
                    var response = client.Execute(request);
                    if(response.ErrorMessage == null)
                    {
                        if (response.Content.ToString().Substring(0, 1).Equals("{"))
                        {
                            JObject jObject = JObject.Parse(response.Content.ToString());
                            foreach (var x in jObject)
                            {
                                if (x.Key.Equals("success"))
                                {
                                    if (Convert.ToBoolean(x.Value.ToString()))
                                    {
                                        isSuccess = true;
                                        break;
                                    }
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
                                            double totalRunningBalance = 0.00;
                                            for (int i = 0; i < jsonArray.Count(); i++)
                                            {
                                                JObject data = JObject.Parse(jsonArray[i].ToString());
                                                string ref1 = "", ref2 = "", transType = "";
                                                double depIn = 0.00, depOut = 0.00;
                                                DateTime dtTransDate = new DateTime();
                                                foreach (var q in data)
                                                {
                                                    if (q.Key.Equals("ref1"))
                                                    {
                                                        ref1 = q.Value.ToString();
                                                    }
                                                    else if (q.Key.Equals("ref2"))
                                                    {
                                                        ref2 = q.Value.ToString();
                                                    }
                                                    else if (q.Key.Equals("transtype"))
                                                    {
                                                        transType = q.Value.ToString();
                                                    }
                                                    else if (q.Key.Equals("dep_in"))
                                                    {
                                                        depIn = string.IsNullOrEmpty(q.Value.ToString().Trim()) ? 0.00 : Convert.ToDouble(q.Value.ToString());
                                                    }
                                                    else if (q.Key.Equals("dep_out"))
                                                    {
                                                        depOut = string.IsNullOrEmpty(q.Value.ToString().Trim()) ? 0.00 : Convert.ToDouble(q.Value.ToString());
                                                    }
                                                    else if (q.Key.Equals("transdate"))
                                                    {
                                                        dtTransDate = Convert.ToDateTime(q.Value.ToString());
                                                    }
                                                }
                                                totalRunningBalance += depIn - depOut; ;
                                                dgv.Rows.Add(dtTransDate.ToString("yyyy-MM-dd"), ref1, ref2, transType, Convert.ToDecimal(string.Format("{0:0.00}", depIn)), Convert.ToDecimal(string.Format("{0:0.00}", depOut)), Convert.ToDecimal(string.Format("{0:0.00}", totalRunningBalance)));
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show(response.Content.ToString(), "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                        else
                        {
                            JObject jObject = JObject.Parse(response.Content.ToString());
                            string msg = "";
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
                    else
                    {
                        MessageBox.Show(response.ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        private void dtFromDate_ValueChanged(object sender, EventArgs e)
        {
            if(cFromDate <= 0)
            {
                loadData();
            }
        }
    }
}
