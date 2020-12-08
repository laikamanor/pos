﻿using System;
using System.Windows.Forms;
using RestSharp;
using Newtonsoft.Json.Linq;
using AB.UI_Class;
namespace AB
{
    public partial class Login : Form
    {
        public static JObject jsonResult = new JObject();
        UI_Class.utility_class utilityc = new utility_class();
        public Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            login();
        }

       

        public void login()
        {
            Cursor.Current = Cursors.WaitCursor;
            var client = new RestClient(utilityc.URL);
            client.AddDefaultHeader("Content-Type", "application/json");
            client.Timeout = -1;

            var request = new RestRequest("/api/auth/login?username=" + txtUsername.Text.ToString().Trim() + "&password=" + txtPassword.Text.ToString().Trim());
            Console.Write("/api/auth/login?username=" + txtUsername.Text.ToString().Trim() + "&password=" + txtPassword.Text.ToString().Trim());
            var response = client.Execute(request);
            if (response.ErrorMessage == null)
            {
                jsonResult = JObject.Parse(response.Content.ToString());
                String msg = "Object message key is empty";
                Boolean isSuccess = false;
                foreach (var x in jsonResult)
                {
                    if (x.Key.Equals("message"))
                    {
                        msg = x.Value.ToString();
                    }
                }

                foreach (var x in jsonResult)
                {
                    //string name = x.Key;
                    JToken value = x.Value;

                    if (x.Key.Equals("success") && value.ToString().Equals("True"))
                    {
                        isSuccess = true;
                        break;
                    }
                    else
                    {
                        isSuccess = false;
                        break;
                    }
                }
                Cursor.Current = Cursors.Default;
                MessageBox.Show(msg, "Validation", MessageBoxButtons.OK, isSuccess ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
                if (isSuccess)
                {
                    this.Hide();
                    MainMenu mainMenu = new MainMenu();
                    mainMenu.ShowDialog();
                }
            }
            else
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(response.ErrorMessage, "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void txtUsername_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                login();
            }
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                login();
            }
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void Login_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void checkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.PasswordChar = checkShowPassword.Checked ? '\0' : '*';
        }
    }
}