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
    public partial class AdjustmentIn_Details : Form
    {
        utility_class utilityc = new utility_class();
        public int selectedID = 0;
        public AdjustmentIn_Details()
        {
            InitializeComponent();
        }

        private void AdjustmentIn_Details_Load(object sender, EventArgs e)
        {
            loadData();
            lblCount.Text = "Items (" + dgv.Rows.Count.ToString("N0") + ")";
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
                    var client = new RestClient(utilityc.URL);
                    client.Timeout = -1;
                    var request = new RestRequest("/api/inv_adj/in/details/" + selectedID);
                    request.AddHeader("Authorization", "Bearer " + token);
                    var response = client.Execute(request);
                    JObject jObject = JObject.Parse(response.Content.ToString());
                    Console.WriteLine(response.Content.ToString());
                    if (response.ErrorMessage == null)
                    {
                        bool isSuccess = false;
                        string msg = "";
                        foreach (var x in jObject)
                        {
                            if (x.Key.Equals("success"))
                            {
                                isSuccess = Convert.ToBoolean(x.Value.ToString());
                            }
                            if (x.Key.Equals("message"))
                            {
                                msg = x.Value.ToString();
                            }
                        }

                        if (isSuccess)
                        {
                            dgv.Rows.Clear();
                            foreach (var x in jObject)
                            {
                                if (x.Key.Equals("data"))
                                {
                                    if (x.Value.ToString() != "{}")
                                    {
                                        JObject data = JObject.Parse(x.Value.ToString());
                                        foreach (var q in data)
                                        {
                                            if (q.Key.Equals("rows"))
                                            {
                                                if (q.Value.ToString() != "[]")
                                                {
                                                    JArray jsonArrayRows = JArray.Parse(q.Value.ToString());
                                                    for (int ii = 0; ii < jsonArrayRows.Count(); ii++)
                                                    {
                                                        JObject rows = JObject.Parse(jsonArrayRows[ii].ToString());
                                                        int id = 0, adjusmentid = 0;
                                                        string itemCode = "", uOm = "";
                                                        double quantity = 0.00;
                                                        foreach (var w in rows)
                                                        {

                                                            if (w.Key.Equals("id"))
                                                            {
                                                                id = Convert.ToInt32(w.Value.ToString());
                                                            }
                                                            else if (w.Key.Equals("adjustin_id"))
                                                            {
                                                                adjusmentid = Convert.ToInt32(w.Value.ToString());
                                                            }
                                                            else if (w.Key.Equals("item_code"))
                                                            {
                                                                itemCode = w.Value.ToString();
                                                            }
                                                            else if (w.Key.Equals("uom"))
                                                            {
                                                                uOm = w.Value.ToString();
                                                            }
                                                            else if (w.Key.Equals("quantity"))
                                                            {
                                                                quantity = Convert.ToDouble(w.Value.ToString());
                                                            }
                                                            else if (w.Key.Equals("whsecode"))
                                                            {
                                                                lblWhse.Text = w.Value.ToString();
                                                            }
                                                        }
                                                        dgv.Rows.Add(id, adjusmentid, itemCode, quantity, uOm);
                                                    }
                                                }
                                            }
                                            else if (q.Key.Equals("remarks"))
                                            {
                                                lblRemarks.Text = q.Value.ToString();
                                            }
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
                    else
                    {
                        MessageBox.Show(response.ErrorMessage, "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }
    }
}
