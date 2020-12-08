using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AB
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtAdd.Text.Trim()))
            {
                dgv.Rows.Add(txtAdd.Text.Trim());
                dgv2.Rows.Add(txtAdd.Text.Trim());
            }
        }

        private void dgv_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           if(dgv.Rows.Count > 0)
            {
                if(e.ColumnIndex== 1)
                {
                    dgv.Rows.RemoveAt(dgv.CurrentRow.Index);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {



            if (!string.IsNullOrEmpty(txtSearch.Text.Trim()))
            {
                dgv2.Rows.Clear();
                foreach (DataGridViewRow row in dgv.Rows)
                {
                    if (txtSearch.Text.Trim() == row.Cells["item"].Value.ToString())
                    {
                        dgv2.Rows.Add(txtSearch.Text.Trim());
                    }
                }
            }
            else
            {
                dgv2.Rows.Clear();
                foreach (DataGridViewRow row in dgv.Rows)
                {
                    dgv2.Rows.Add(row.Cells["item"].Value.ToString());
                }
            }
        }

        private void txtAdd_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.Equals(Keys.Enter))
            {
                button1.PerformClick();
            }
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.Equals(Keys.Enter))
            {
                button2.PerformClick();
            }
        }
    }
}
