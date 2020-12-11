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
using Newtonsoft.Json.Linq;

namespace AB
{
    public partial class AdjustmentIn : Form
    {
        string gAdjType = "";
        utility_class utilityc = new utility_class();
        public AdjustmentIn(string adjType)
        {
            gAdjType = adjType;
            InitializeComponent();
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            AddAdjustmentIn addAdjustmentIn = new AddAdjustmentIn(gAdjType);
            addAdjustmentIn.ShowDialog();
        }

        private void AdjustmentIn_Load(object sender, EventArgs e)
        {
            this.Text = gAdjType.Equals("in") ? "Adjusment In" : "Adjustment Out";
            loadData();
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
                    string sSearch = string.IsNullOrEmpty(txtSearch.Text.Trim()) ? "" : "?transdate=" + txtSearch.Text.Trim();
                    var request = new RestRequest("/api/inv_adj/in/get_all/" + sSearch);
                    request.AddHeader("Authorization", "Bearer " + token);
                    var response = client.Execute(request);
                    JObject jObject = JObject.Parse(response.Content.ToString());
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
                            panel1.Controls.Clear();
                            AutoCompleteStringCollection auto = new AutoCompleteStringCollection();
                            foreach (var x in jObject)
                            {
                                if (x.Key.Equals("data"))
                                {
                                    if (x.Value.ToString() != "[]")
                                    {
                                        int labelY = 13, lineY = 45;
                                        JArray jsonArray = JArray.Parse(x.Value.ToString());
                                        for (int i = 0; i < jsonArray.Count(); i++)
                                        {
                                            JObject data = JObject.Parse(jsonArray[i].ToString());
                                            Label labelReference = new Label();
                                            labelReference.AutoSize = true;
                                            labelReference.Location = new Point(16, labelY);
                                            labelReference.Font = new Font("Arial", 15, FontStyle.Bold);
                                            Panel panelLine = new Panel();
                                            panelLine.BackColor = Color.Black;
                                            panelLine.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                                            panelLine.Size = new Size(734, 3);
                                            panelLine.Location = new Point(4, lineY);
                                            foreach (var q in data)
                                            {
                                                if (q.Key.Equals("reference"))
                                                {
                                                    labelReference.Text = q.Value.ToString();
                                                    auto.Add(q.Value.ToString());
                                                }
                                                else if (q.Key.Equals("id"))
                                                {
                                                    labelReference.Tag = q.Value.ToString();
                                                }
                                            }

                                            labelReference.Click += new EventHandler(labelReference_click);

                                            panel1.Controls.Add(labelReference);
                                            panel1.Controls.Add(panelLine);
                                            txtSearch.AutoCompleteCustomSource = auto;
                                            labelY += 46;
                                            lineY += 49;
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

        private void labelReference_click(object sender, EventArgs e)
        {
            Label label = sender as Label;
            AdjustmentIn_Details ajdDetails = new AdjustmentIn_Details();
            ajdDetails.selectedID = Convert.ToInt32(label.Tag.ToString());
            ajdDetails.lblReference.Text = label.Text;
            ajdDetails.ShowDialog();
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
    }
}
