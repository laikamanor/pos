using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AB.API_Class.Customer_Type;
using AB.API_Class.Warehouse;
using AB.UI_Class;
using Newtonsoft.Json.Linq;
using RestSharp;
namespace AB
{
    public partial class AddCustomer : Form
    {
        customertype_class customertypec = new customertype_class();
        warehouse_class warehousec = new warehouse_class();
        utility_class utilityc = new utility_class();
        DataTable dtCustomerTypes, dtWarehouses;

        public static bool isSubmit = false;
        public AddCustomer()
        {
            InitializeComponent();
        }

        private void AddCustomer_Load(object sender, EventArgs e)
        {
            loadCustomerTypes();
            loadWarehouses();
        }

        public void loadCustomerTypes()
        {
            dtCustomerTypes = new DataTable();
            dtCustomerTypes = customertypec.loadCustomerTypes();
            if(dtCustomerTypes.Rows.Count > 0)
            {
                cmbCustomerType.Items.Clear();
                foreach(DataRow row in dtCustomerTypes.Rows)
                {
                    cmbCustomerType.Items.Add(row["name"].ToString());
                }
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCode.Text.Trim()))
            {
                MessageBox.Show("Code field is required", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCode.Focus();
            }
            else if (string.IsNullOrEmpty(txtName.Text.Trim()))
            {
                MessageBox.Show("Name field is required", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
            }
            else if (string.IsNullOrEmpty(cmbCustomerType.Text.Trim()))
            {
                MessageBox.Show("Customer Type field is required", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbCustomerType.Focus();
            }
            else
            {
                insertCustomer();
            }
        }

        public void loadWarehouses()
        {
            dtWarehouses = new DataTable();
            dtWarehouses = warehousec.returnWarehouse("");
            if (dtWarehouses.Rows.Count > 0)
            {
                cmbWarehouse.Items.Clear();
                foreach (DataRow row in dtWarehouses.Rows)
                {
                    cmbWarehouse.Items.Add(row["whsecode"].ToString());
                }
            }
        }

        public void insertCustomer()
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
                    //string branch = (cmbBranch.Text.Equals("") || cmbBranch.Text == "All" ? "" : cmbBranch.Text);
                    var request = new RestRequest("/api/customer/new");
                    request.AddHeader("Authorization", "Bearer " + token);
                    request.Method = Method.POST;

                    JObject jObject = new JObject();
                    jObject.Add("code", (txtCode.Text == String.Empty ? null : txtCode.Text));
                    jObject.Add("name", (txtName.Text == String.Empty ? null : txtName.Text));

                    int cust_id = 0;
                    foreach(DataRow row in dtCustomerTypes.Rows)
                    {
                        if(cmbCustomerType.Text == row["name"].ToString())
                        {
                            cust_id = Convert.ToInt32(row["id"].ToString());
                        }
                    }
                    jObject.Add("cust_type", cust_id);
                    jObject.Add("birthdate", dtBirthDate.Value.ToString("yyyy-MM-dd"));
                    jObject.Add("address", (txtAddress.Text == String.Empty ? null : txtAddress.Text));
                    jObject.Add("contact", (txtContact.Text == String.Empty ? null : txtContact.Text));
                    jObject.Add("whse", (cmbWarehouse.SelectedIndex== -1 ?  null : cmbWarehouse.Text.Trim()));

                    Console.WriteLine(jObject);
                    request.AddParameter("application/json", jObject, ParameterType.RequestBody);
                    var response = client.Execute(request);
                    jObject = JObject.Parse(response.Content.ToString());
                    bool isSuccess = false;

                    string msg = "No message response found";
                    foreach (var x in jObject)
                    {
                        if (x.Key.Equals("message"))
                        {
                            msg = x.Value.ToString();
                        }
                    }

                    foreach (var x in jObject)
                    {
                        if (x.Key.Equals("success"))
                        {
                            isSuccess = Convert.ToBoolean(x.Value.ToString());
                            txtCode.Clear();
                            txtName.Clear();
                            isSubmit = true;
                            MessageBox.Show(msg, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtCode.Clear();
                            txtName.Clear();
                            txtAddress.Clear();
                            txtContact.Clear();
                            cmbCustomerType.SelectedIndex = -1;
                            cmbWarehouse.SelectedIndex = -1;
                        }
                    }

                    if (!isSuccess)
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
                    Cursor.Current = Cursors.Default;
                }
            }
        }


    }
}
