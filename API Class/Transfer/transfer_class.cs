﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AB.UI_Class;
using System.Data;
using RestSharp;
using Newtonsoft.Json.Linq;
namespace AB.API_Class.Transfer
{
    class transfer_class
    {
        UI_Class.utility_class utilityc = new utility_class();

        public DataTable loadData(string URL, string status,string transnum,string transDate,string forType, string branch, string whse,string towhse)
        {
            DataTable dt = new DataTable();
            if (Login.jsonResult != null)
            {
                dt.Columns.Add("id");
                dt.Columns.Add("transnumber");
                dt.Columns.Add("reference");
                dt.Columns.Add("remarks");
                dt.Columns.Add("docstatus");
                dt.Columns.Add("transdate");
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
                    Cursor.Current = Cursors.WaitCursor;
                    var client = new RestClient(utilityc.URL);
                    client.Timeout = -1;
     
                    string sforType = (forType.Equals("For SAP") ? "&sap_number=" : "");
                   
                    //MessageBox.Show("class: " + URL);
                    var request = new RestRequest(URL + "?docstatus=" + status + "&transnumber=" + transnum + "&transdate=" + transDate + sforType + branch + whse + (!URL.Equals("/api/pullout/get_all") ? towhse : ""));
                    //Console.WriteLine(URL + "?docstatus=" + status + "&transnumber=" + transnum + "&transdate=" + transDate + sforType + branch + whse + (!URL.Equals("/api/pullout/get_all") ? towhse : ""));
                    //Console.WriteLine(URL + "?docstatus=" + status + "&transnumber=" + transnum + "&transdate=" + transDate + sforType + branch + whse + (!URL.Equals("pullout") ? towhse : ""));
                    Console.WriteLine(URL + "?docstatus=" + status + "&transnumber=" + transnum + "&transdate=" + transDate + sforType + branch + whse + (!URL.Equals("/api/pullout/get_all") ? towhse : ""));
                    request.AddHeader("Authorization", "Bearer " + token);
                    var response = client.Execute(request);
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
                                if(x.Value.ToString() != "[]" || x.Value.ToString() != "")
                                {
                                    JArray jsonArray = JArray.Parse(x.Value.ToString());
                                    for (int i = 0; i < jsonArray.Count(); i++)
                                    {
                                        JObject data = JObject.Parse(jsonArray[i].ToString());
                                        int iD = 0, transNumber = 0;
                                        string referencenumber = "", remarks = "", docStatus = "";
                                        DateTime dtTransDate = new DateTime();
                                        foreach (var q in data)
                                        {
                                            if (q.Key.Equals("id"))
                                            {
                                                iD = Convert.ToInt32(q.Value.ToString());
                                            }
                                            else if (q.Key.Equals("transnumber"))
                                            {
                                                transNumber = Convert.ToInt32(q.Value.ToString());
                                            }
                                            else if (q.Key.Equals("reference"))
                                            {
                                                referencenumber = q.Value.ToString();
                                            }
                                            else if (q.Key.Equals("remarks"))
                                            {
                                                remarks = q.Value.ToString();
                                            }
                                            else if (q.Key.Equals("docstatus"))
                                            {
                                                docStatus = q.Value.ToString();
                                            }
                                            else if (q.Key.Equals("transdate"))
                                            {
                                                string replaceT = q.Value.ToString().Replace("T", "");
                                                dtTransDate = Convert.ToDateTime(replaceT);
                                            }
                                        }
                                        dt.Rows.Add(iD, transNumber, referencenumber, remarks, docStatus, dtTransDate.ToString("yyyy-MM-dd"));
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
                    Cursor.Current = Cursors.Default;
                }
            }
            return dt;
        }

        public DataTable loadItems(string URL, int id)
        {
            DataTable dt = new DataTable();
            if (Login.jsonResult != null)
            {
                dt.Columns.Add("reference");
                dt.Columns.Add("docstatus");
                dt.Columns.Add("transdate");
                dt.Columns.Add("id");
                dt.Columns.Add("transfer_id");
                dt.Columns.Add("item_code");
                dt.Columns.Add("quantity");
                dt.Columns.Add("to_whse");

                if (URL.Equals("inv/recv"))
                {
                    dt.Columns.Add("actualrec");
                }


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
                    Cursor.Current = Cursors.WaitCursor;
                    var client = new RestClient(utilityc.URL);
                    client.Timeout = -1;
                    //string branch = (cmbBranch.Text.Equals("") || cmbBranch.Text == "All" ? "" : cmbBranch.Text);
                   
                    var request = new RestRequest("/api/"  + URL + (URL.Equals("inv/recv") || URL.Equals("pullout") ? "/" : "/get") + "details/" + id);
                    //Console.WriteLine("/api/" + URL + (URL.Equals("inv/recv") ? "/" : "/get") + "details/" + id);
                    request.AddHeader("Authorization", "Bearer " + token);
                    var response = client.Execute(request);
                    JObject jObject = new JObject();
                    jObject = JObject.Parse(response.Content.ToString());
                    //Console.WriteLine(jObject);
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
                                int iD = 0, transfer_id = 0;
                                string itemName = "", toWhse = "", referenceNumber = "", docStatus = "";
                                DateTime dtTransDate = new DateTime();
                                double quantity = 0.00, actualRec = 0.00;
                                if (x.Value.ToString() != "{}")
                                {
                                    JObject jObjectData = JObject.Parse(x.Value.ToString());
                                    foreach (var q in jObjectData)
                                    {
                                        if (q.Key.Equals("reference"))
                                        {
                                            referenceNumber = q.Value.ToString();
                                        }
                                        else if (q.Key.Equals("docstatus"))
                                        {
                                            docStatus = q.Value.ToString();
                                        }
                                        else if (q.Key.Equals("transdate"))
                                        {
                                            string replaceT = q.Value.ToString().Replace("T", "");
                                            dtTransDate = Convert.ToDateTime(replaceT);
                                        }
                                        else if (q.Key.Equals((URL.Equals("inv/recv") ? "rec" : URL.Equals("inv/trfr") ? "trans" : "") + "row"))
                                        {
                                            if (q.Value.ToString() != "[]")
                                            {
                                                JArray jArrayTransRow = JArray.Parse(q.Value.ToString());
                                                for (int i = 0; i < jArrayTransRow.Count(); i++)
                                                {
                                                    JObject jObjectTransRow = JObject.Parse(jArrayTransRow[i].ToString());
                                                    foreach (var y in jObjectTransRow)
                                                    {
                                                        if (y.Key.Equals("id"))
                                                        {
                                                            iD = Convert.ToInt32(y.Value.ToString());
                                                        }
                                                        else if (y.Key.Equals("transfer_id"))
                                                        {
                                                            transfer_id = Convert.ToInt32(y.Value.ToString());

                                                        }
                                                        else if (y.Key.Equals("item_code"))
                                                        {
                                                            itemName = y.Value.ToString();
                                                        }
                                                        else if (y.Key.Equals((URL.Equals("recv") ? "from_whse" : "to_whse")))
                                                        {
                                                            toWhse = y.Value.ToString();
                                                        }
                                                        else if (y.Key.Equals("quantity"))
                                                        {
                                                            quantity = Convert.ToDouble(y.Value.ToString());
                                                        }
                                                        else if (y.Key.Equals("actualrec") && URL.Equals("inv/recv"))
                                                        {
                                                            actualRec = Convert.ToDouble(y.Value.ToString());
                                                        }
                                                    }
                                                    if (URL.Equals("inv/trfr") || URL.Equals("pullout"))
                                                    {
                                                        dt.Rows.Add(referenceNumber, docStatus, dtTransDate.ToString("yyyy-MM-dd"), iD, transfer_id, itemName, quantity, toWhse);
                                                    }
                                                    else
                                                    {
                                                        dt.Rows.Add(referenceNumber, docStatus, dtTransDate.ToString("yyyy-MM-dd"), iD, transfer_id, itemName, quantity, toWhse, actualRec);
                                                    }
                                                }
                                            }
                                        }
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
                    Cursor.Current = Cursors.Default;
                }
            }
            return dt;
        }


        public string cancelTransfer(int id,string remarks)
        {
            string result = "";
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
                    Cursor.Current = Cursors.WaitCursor;
                    var client = new RestClient(utilityc.URL);
                    client.Timeout = -1;
                    var request = new RestRequest("/api/inv/trfr/cancel/" + id);
                    request.Method = Method.PUT;
                    request.AddHeader("Authorization", "Bearer " + token);
                    JObject jObject = new JObject();
                    jObject.Add("remarks", remarks);
                    request.AddParameter("application/json", jObject, ParameterType.RequestBody);
                    var response = client.Execute(request);
                    JObject jObjectResponse = new JObject();
                    jObjectResponse = JObject.Parse(response.Content.ToString());

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
                       
                        result = jObjectResponse.ToString();
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
            return result;
        }
    }

}