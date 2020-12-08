namespace AB
{
    partial class ItemRequest
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ItemRequest));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpConfirmation = new System.Windows.Forms.TabPage();
            this.panelConfirmation = new System.Windows.Forms.Panel();
            this.tpForSAP = new System.Windows.Forms.TabPage();
            this.panelSAP = new System.Windows.Forms.Panel();
            this.tpLogs = new System.Windows.Forms.TabPage();
            this.panelLogs = new System.Windows.Forms.Panel();
            this.tabControl1.SuspendLayout();
            this.tpConfirmation.SuspendLayout();
            this.tpForSAP.SuspendLayout();
            this.tpLogs.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tpConfirmation);
            this.tabControl1.Controls.Add(this.tpForSAP);
            this.tabControl1.Controls.Add(this.tpLogs);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(881, 497);
            this.tabControl1.TabIndex = 13;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tpConfirmation
            // 
            this.tpConfirmation.Controls.Add(this.panelConfirmation);
            this.tpConfirmation.Location = new System.Drawing.Point(4, 22);
            this.tpConfirmation.Name = "tpConfirmation";
            this.tpConfirmation.Padding = new System.Windows.Forms.Padding(3);
            this.tpConfirmation.Size = new System.Drawing.Size(873, 471);
            this.tpConfirmation.TabIndex = 0;
            this.tpConfirmation.Text = "For Confirmation";
            this.tpConfirmation.UseVisualStyleBackColor = true;
            // 
            // panelConfirmation
            // 
            this.panelConfirmation.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelConfirmation.Location = new System.Drawing.Point(6, 6);
            this.panelConfirmation.Name = "panelConfirmation";
            this.panelConfirmation.Size = new System.Drawing.Size(861, 459);
            this.panelConfirmation.TabIndex = 0;
            // 
            // tpForSAP
            // 
            this.tpForSAP.Controls.Add(this.panelSAP);
            this.tpForSAP.Location = new System.Drawing.Point(4, 22);
            this.tpForSAP.Name = "tpForSAP";
            this.tpForSAP.Padding = new System.Windows.Forms.Padding(3);
            this.tpForSAP.Size = new System.Drawing.Size(873, 471);
            this.tpForSAP.TabIndex = 1;
            this.tpForSAP.Text = "For SAP";
            this.tpForSAP.UseVisualStyleBackColor = true;
            // 
            // panelSAP
            // 
            this.panelSAP.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelSAP.Location = new System.Drawing.Point(6, 6);
            this.panelSAP.Name = "panelSAP";
            this.panelSAP.Size = new System.Drawing.Size(861, 459);
            this.panelSAP.TabIndex = 1;
            // 
            // tpLogs
            // 
            this.tpLogs.Controls.Add(this.panelLogs);
            this.tpLogs.Location = new System.Drawing.Point(4, 22);
            this.tpLogs.Name = "tpLogs";
            this.tpLogs.Size = new System.Drawing.Size(873, 471);
            this.tpLogs.TabIndex = 2;
            this.tpLogs.Text = "Done";
            this.tpLogs.UseVisualStyleBackColor = true;
            // 
            // panelLogs
            // 
            this.panelLogs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelLogs.Location = new System.Drawing.Point(6, 6);
            this.panelLogs.Name = "panelLogs";
            this.panelLogs.Size = new System.Drawing.Size(861, 459);
            this.panelLogs.TabIndex = 2;
            // 
            // ItemRequest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(905, 521);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ItemRequest";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Item Request";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.ItemRequest_Load);
            this.tabControl1.ResumeLayout(false);
            this.tpConfirmation.ResumeLayout(false);
            this.tpForSAP.ResumeLayout(false);
            this.tpLogs.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpConfirmation;
        private System.Windows.Forms.TabPage tpForSAP;
        private System.Windows.Forms.TabPage tpLogs;
        private System.Windows.Forms.Panel panelConfirmation;
        private System.Windows.Forms.Panel panelSAP;
        private System.Windows.Forms.Panel panelLogs;
    }
}