using Newtonsoft.Json.Linq;
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
using AB.API_Class.Advance_Payment;
namespace AB
{
    public partial class AdvancePayment : Form
    {
        advancepayment_class advancepaymentc = new advancepayment_class();
        utility_class utilityc = new utility_class();
        public AdvancePayment()
        {
            InitializeComponent();
        }

        private void AdvancePayment_Load(object sender, EventArgs e)
        {
            cmbStatus.SelectedIndex = 1;
        }

      public void loadData()
        {
            DataTable dtResponse = new DataTable();
            string status = "";
             if (cmbStatus.SelectedIndex.Equals(1))
            {
                status = "O";
            }
            else if (cmbStatus.SelectedIndex.Equals(2))
            {
                status = "C";
            }
            else if (cmbStatus.SelectedIndex.Equals(3))
            {
                status = "N";
            }
            else
            {
                status = "";
            }

            dtResponse = advancepaymentc.loadData(status);
            dgv.Rows.Clear();
            if(dtResponse.Rows.Count > 0)
            {
                lblNoDataFound.Visible = false;
                foreach (DataRow r0w in dtResponse.Rows)
                {
                    dgv.Rows.Add( r0w["id"], r0w["cust_code"], Convert.ToDouble(r0w["amount"]).ToString("n2"), Convert.ToDouble(r0w["balance"]).ToString("n2"), r0w["remarks"], r0w["reference2"],r0w["sap_number"], r0w["status"]);
                }
            }
            else
            {
                lblNoDataFound.Visible = true;
            }
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            AddAdvancePayment addAdvancePayment = new AddAdvancePayment();
            addAdvancePayment.ShowDialog();
            if (AddAdvancePayment.isSubmit)
            {
                loadData();
            }
        }

        private void cmbBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadData();
        }

        private void dgv_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(dgv.Rows.Count > 0)
            {
                int id = Convert.ToInt32(dgv.CurrentRow.Cells["id"].Value.ToString());
                if (e.ColumnIndex.Equals(8))
                {
                    EditAdvancePayment editAdvancePayment = new EditAdvancePayment();
                    editAdvancePayment.id = id;
                    editAdvancePayment.ShowDialog();
                    if (EditAdvancePayment.isSubmit)
                    {
                        loadData();
                    }
                }
                else if (e.ColumnIndex.Equals(9))
                {
                    DialogResult dialogResult = MessageBox.Show("Are you sure you want to cancel?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        cancelAP(id);
                    }
                }
            }
        }

        public void cancelAP(int id)
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
                    bool isSuccess = false;
                    var client = new RestClient(utilityc.URL);
                    client.Timeout = -1;
                    var request = new RestRequest("/api/deposit/cancel/" + id);
                    request.AddHeader("Authorization", "Bearer " + token);
                    request.Method = Method.PUT;
                    var response = client.Execute(request);
                    JObject jObject = JObject.Parse(response.Content.ToString());
                    Console.WriteLine(response.Content);
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
                    string msg = "";
                    foreach (var x in jObject)
                    {
                        if (x.Key.Equals("message"))
                        {
                            msg = x.Value.ToString();
                        }
                    }
                    if (isSuccess)
                    {
                        MessageBox.Show(msg, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dgv.Rows.RemoveAt(dgv.CurrentRow.Index);
                    }
                    else
                    {
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
    }
}
