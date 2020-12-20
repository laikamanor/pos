using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AB.UI_Class;
using System.Data;
using RestSharp;
using Newtonsoft.Json.Linq;

namespace AB.API_Class.Warehouse
{
    class warehouse_class
    {
        UI_Class.utility_class utilityc = new utility_class();
        public DataTable returnWarehouse(string branch)
        {
            DataTable dt = new DataTable();
            if (Login.jsonResult != null)
            {
                dt.Columns.Add("whsecode");
                dt.Columns.Add("whsename");
                dt.Columns.Add("id");
                dt.Columns.Add("branch");
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
                    var request = new RestRequest("/api/whse/get_all" + (string.IsNullOrEmpty(branch) ? "" : "?branch=" + branch));
                
                    request.AddHeader("Authorization", "Bearer " + token);
                    var response = client.Execute(request);
                    JObject jObject = new JObject();
                    if (response.Content.ToString().Substring(0, 1).Equals("{"))
                    {
                        jObject = JObject.Parse(response.Content.ToString());
                    }
                    else
                    {
                        MessageBox.Show(response.Content.ToString(), "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
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
                                    JArray jsonArray = JArray.Parse(x.Value.ToString());
                                    for (int i = 0; i < jsonArray.Count(); i++)
                                    {
                                        JObject data = JObject.Parse(jsonArray[i].ToString());
                                        int id = 0;
                                        string warehouse = "", warehouseName = "",
                                        branchCode = "";
                                        foreach (var q in data)
                                        {

                                            if (q.Key.Equals("id"))
                                            {
                                                id = Convert.ToInt32(q.Value.ToString());
                                            }
                                            else if (q.Key.Equals("whsecode"))
                                            {
                                                warehouse = q.Value.ToString();
                                            }
                                            else if (q.Key.Equals("whsename"))
                                            {
                                                warehouseName = q.Value.ToString();
                                            }
                                            else if (q.Key.Equals("branch"))
                                            {
                                                branchCode = q.Value.ToString();
                                            }
                                        }
                                        dt.Rows.Add(warehouse, warehouseName,id,branchCode);
                                    }
                                }
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
                }
                Cursor.Current = Cursors.Default;
            }
            return dt;
        }
    }
}
