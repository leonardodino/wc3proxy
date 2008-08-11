namespace Foole.WC3Proxy
{
    partial class ServerInfoDlg
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
            System.Windows.Forms.Label lblServerAddress;
            System.Windows.Forms.Label lblWC3Version;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServerInfoDlg));
            this.txtServerAddress = new System.Windows.Forms.TextBox();
            this.chkExpansion = new System.Windows.Forms.CheckBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cbxWC3Version = new System.Windows.Forms.ComboBox();
            lblServerAddress = new System.Windows.Forms.Label();
            lblWC3Version = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblServerAddress
            // 
            lblServerAddress.AutoSize = true;
            lblServerAddress.Location = new System.Drawing.Point(8, 15);
            lblServerAddress.Name = "lblServerAddress";
            lblServerAddress.Size = new System.Drawing.Size(81, 13);
            lblServerAddress.TabIndex = 0;
            lblServerAddress.Text = "Server address:";
            // 
            // txtServerAddress
            // 
            this.txtServerAddress.Location = new System.Drawing.Point(95, 12);
            this.txtServerAddress.Name = "txtServerAddress";
            this.txtServerAddress.Size = new System.Drawing.Size(151, 20);
            this.txtServerAddress.TabIndex = 1;
            // 
            // chkExpansion
            // 
            this.chkExpansion.AutoSize = true;
            this.chkExpansion.Location = new System.Drawing.Point(11, 73);
            this.chkExpansion.Name = "chkExpansion";
            this.chkExpansion.Size = new System.Drawing.Size(95, 17);
            this.chkExpansion.TabIndex = 2;
            this.chkExpansion.Text = "Frozen Throne";
            this.chkExpansion.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(70, 103);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(83, 28);
            this.btnOk.TabIndex = 3;
            this.btnOk.Text = "&OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(159, 103);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(83, 28);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // cbxWC3Version
            // 
            this.cbxWC3Version.FormattingEnabled = true;
            this.cbxWC3Version.Location = new System.Drawing.Point(95, 38);
            this.cbxWC3Version.Name = "cbxWC3Version";
            this.cbxWC3Version.Size = new System.Drawing.Size(151, 21);
            this.cbxWC3Version.TabIndex = 5;
            // 
            // lblWC3Version
            // 
            lblWC3Version.AutoSize = true;
            lblWC3Version.Location = new System.Drawing.Point(12, 43);
            lblWC3Version.Name = "lblWC3Version";
            lblWC3Version.Size = new System.Drawing.Size(72, 13);
            lblWC3Version.TabIndex = 6;
            lblWC3Version.Text = "WC3 Version:";
            // 
            // ServerInfoDlg
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(254, 143);
            this.Controls.Add(lblWC3Version);
            this.Controls.Add(this.cbxWC3Version);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.chkExpansion);
            this.Controls.Add(this.txtServerAddress);
            this.Controls.Add(lblServerAddress);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ServerInfoDlg";
            this.Text = "Server information";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtServerAddress;
        private System.Windows.Forms.CheckBox chkExpansion;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ComboBox cbxWC3Version;
    }
}