namespace Foole.WC3Proxy
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.Label lblGameName2;
            System.Windows.Forms.Label lblMap2;
            System.Windows.Forms.Label lblGamePort2;
            System.Windows.Forms.Label lblServerAddress2;
            System.Windows.Forms.Label lblPlayers2;
            System.Windows.Forms.Label lblClients2;
            System.Windows.Forms.ToolStripMenuItem mnuTools;
            System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.mnuToolsLaunchWarcraft = new System.Windows.Forms.ToolStripMenuItem();
            this.lblGamePort = new System.Windows.Forms.Label();
            this.lblServerAddress = new System.Windows.Forms.Label();
            this.lblMap = new System.Windows.Forms.Label();
            this.lblGameName = new System.Windows.Forms.Label();
            this.lblPlayers = new System.Windows.Forms.Label();
            this.lblClientCount = new System.Windows.Forms.Label();
            this.mnuMain = new System.Windows.Forms.MenuStrip();
            this.mnuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFileChangeServer = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFileExit = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuHelpAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.mIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.mIconMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuIconExit = new System.Windows.Forms.ToolStripMenuItem();
            lblGameName2 = new System.Windows.Forms.Label();
            lblMap2 = new System.Windows.Forms.Label();
            lblGamePort2 = new System.Windows.Forms.Label();
            lblServerAddress2 = new System.Windows.Forms.Label();
            lblPlayers2 = new System.Windows.Forms.Label();
            lblClients2 = new System.Windows.Forms.Label();
            mnuTools = new System.Windows.Forms.ToolStripMenuItem();
            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            tableLayoutPanel1.SuspendLayout();
            this.mnuMain.SuspendLayout();
            this.mIconMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblGameName2
            // 
            lblGameName2.AutoSize = true;
            lblGameName2.Location = new System.Drawing.Point(3, 25);
            lblGameName2.Name = "lblGameName2";
            lblGameName2.Size = new System.Drawing.Size(65, 12);
            lblGameName2.TabIndex = 3;
            lblGameName2.Text = "Game name:";
            // 
            // lblMap2
            // 
            lblMap2.AutoSize = true;
            lblMap2.Location = new System.Drawing.Point(3, 43);
            lblMap2.Name = "lblMap2";
            lblMap2.Size = new System.Drawing.Size(29, 12);
            lblMap2.TabIndex = 5;
            lblMap2.Text = "Map:";
            // 
            // lblGamePort2
            // 
            lblGamePort2.AutoSize = true;
            lblGamePort2.Location = new System.Drawing.Point(3, 61);
            lblGamePort2.Name = "lblGamePort2";
            lblGamePort2.Size = new System.Drawing.Size(65, 12);
            lblGamePort2.TabIndex = 7;
            lblGamePort2.Text = "Game port:";
            // 
            // lblServerAddress2
            // 
            lblServerAddress2.AutoSize = true;
            lblServerAddress2.Location = new System.Drawing.Point(3, 7);
            lblServerAddress2.Name = "lblServerAddress2";
            lblServerAddress2.Size = new System.Drawing.Size(95, 12);
            lblServerAddress2.TabIndex = 2;
            lblServerAddress2.Text = "Server address:";
            // 
            // lblPlayers2
            // 
            lblPlayers2.AutoSize = true;
            lblPlayers2.Location = new System.Drawing.Point(3, 79);
            lblPlayers2.Name = "lblPlayers2";
            lblPlayers2.Size = new System.Drawing.Size(53, 12);
            lblPlayers2.TabIndex = 9;
            lblPlayers2.Text = "Players:";
            // 
            // lblClients2
            // 
            lblClients2.AutoSize = true;
            lblClients2.Location = new System.Drawing.Point(3, 97);
            lblClients2.Name = "lblClients2";
            lblClients2.Size = new System.Drawing.Size(53, 12);
            lblClients2.TabIndex = 11;
            lblClients2.Text = "Clients:";
            // 
            // mnuTools
            // 
            mnuTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuToolsLaunchWarcraft});
            mnuTools.Name = "mnuTools";
            mnuTools.Size = new System.Drawing.Size(52, 21);
            mnuTools.Text = "Tools";
            // 
            // mnuToolsLaunchWarcraft
            // 
            this.mnuToolsLaunchWarcraft.Name = "mnuToolsLaunchWarcraft";
            this.mnuToolsLaunchWarcraft.Size = new System.Drawing.Size(186, 22);
            this.mnuToolsLaunchWarcraft.Text = "Launch Warcraft III";
            this.mnuToolsLaunchWarcraft.Click += new System.EventHandler(this.mnuLaunchWarcraft_Click);
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(lblClients2, 0, 6);
            tableLayoutPanel1.Controls.Add(lblPlayers2, 0, 5);
            tableLayoutPanel1.Controls.Add(lblServerAddress2, 0, 1);
            tableLayoutPanel1.Controls.Add(this.lblGamePort, 1, 4);
            tableLayoutPanel1.Controls.Add(this.lblServerAddress, 1, 1);
            tableLayoutPanel1.Controls.Add(lblGamePort2, 0, 4);
            tableLayoutPanel1.Controls.Add(lblGameName2, 0, 2);
            tableLayoutPanel1.Controls.Add(this.lblMap, 1, 3);
            tableLayoutPanel1.Controls.Add(this.lblGameName, 1, 2);
            tableLayoutPanel1.Controls.Add(lblMap2, 0, 3);
            tableLayoutPanel1.Controls.Add(this.lblPlayers, 1, 5);
            tableLayoutPanel1.Controls.Add(this.lblClientCount, 1, 6);
            tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel1.Location = new System.Drawing.Point(0, 25);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 7;
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 7F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 18F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 18F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 18F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 18F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 18F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 18F));
            tableLayoutPanel1.Size = new System.Drawing.Size(343, 122);
            tableLayoutPanel1.TabIndex = 9;
            // 
            // lblGamePort
            // 
            this.lblGamePort.AutoEllipsis = true;
            this.lblGamePort.AutoSize = true;
            this.lblGamePort.Location = new System.Drawing.Point(123, 61);
            this.lblGamePort.Name = "lblGamePort";
            this.lblGamePort.Size = new System.Drawing.Size(35, 12);
            this.lblGamePort.TabIndex = 8;
            this.lblGamePort.Text = "(N/A)";
            // 
            // lblServerAddress
            // 
            this.lblServerAddress.AutoEllipsis = true;
            this.lblServerAddress.AutoSize = true;
            this.lblServerAddress.Location = new System.Drawing.Point(123, 7);
            this.lblServerAddress.Name = "lblServerAddress";
            this.lblServerAddress.Size = new System.Drawing.Size(59, 12);
            this.lblServerAddress.TabIndex = 4;
            this.lblServerAddress.Text = "(Not set)";
            // 
            // lblMap
            // 
            this.lblMap.AutoEllipsis = true;
            this.lblMap.AutoSize = true;
            this.lblMap.Location = new System.Drawing.Point(123, 43);
            this.lblMap.Name = "lblMap";
            this.lblMap.Size = new System.Drawing.Size(35, 12);
            this.lblMap.TabIndex = 6;
            this.lblMap.Text = "(N/A)";
            // 
            // lblGameName
            // 
            this.lblGameName.AutoEllipsis = true;
            this.lblGameName.AutoSize = true;
            this.lblGameName.Location = new System.Drawing.Point(123, 25);
            this.lblGameName.Name = "lblGameName";
            this.lblGameName.Size = new System.Drawing.Size(77, 12);
            this.lblGameName.TabIndex = 4;
            this.lblGameName.Text = "(None found)";
            // 
            // lblPlayers
            // 
            this.lblPlayers.AutoEllipsis = true;
            this.lblPlayers.AutoSize = true;
            this.lblPlayers.Location = new System.Drawing.Point(123, 79);
            this.lblPlayers.Name = "lblPlayers";
            this.lblPlayers.Size = new System.Drawing.Size(35, 12);
            this.lblPlayers.TabIndex = 10;
            this.lblPlayers.Text = "(N/A)";
            // 
            // lblClientCount
            // 
            this.lblClientCount.AutoEllipsis = true;
            this.lblClientCount.AutoSize = true;
            this.lblClientCount.Location = new System.Drawing.Point(123, 97);
            this.lblClientCount.Name = "lblClientCount";
            this.lblClientCount.Size = new System.Drawing.Size(11, 12);
            this.lblClientCount.TabIndex = 12;
            this.lblClientCount.Text = "0";
            // 
            // mnuMain
            // 
            this.mnuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFile,
            mnuTools,
            this.mnuHelp});
            this.mnuMain.Location = new System.Drawing.Point(0, 0);
            this.mnuMain.Name = "mnuMain";
            this.mnuMain.Size = new System.Drawing.Size(343, 25);
            this.mnuMain.TabIndex = 0;
            // 
            // mnuFile
            // 
            this.mnuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFileChangeServer,
            this.mnuFileExit});
            this.mnuFile.Name = "mnuFile";
            this.mnuFile.Size = new System.Drawing.Size(39, 21);
            this.mnuFile.Text = "&File";
            // 
            // mnuFileChangeServer
            // 
            this.mnuFileChangeServer.Name = "mnuFileChangeServer";
            this.mnuFileChangeServer.Size = new System.Drawing.Size(160, 22);
            this.mnuFileChangeServer.Text = "Change server";
            this.mnuFileChangeServer.Click += new System.EventHandler(this.mnuChangeServer_Click);
            // 
            // mnuFileExit
            // 
            this.mnuFileExit.Name = "mnuFileExit";
            this.mnuFileExit.Size = new System.Drawing.Size(160, 22);
            this.mnuFileExit.Text = "E&xit";
            this.mnuFileExit.Click += new System.EventHandler(this.mnuFileExit_Click);
            // 
            // mnuHelp
            // 
            this.mnuHelp.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.mnuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuHelpAbout});
            this.mnuHelp.Name = "mnuHelp";
            this.mnuHelp.Size = new System.Drawing.Size(47, 21);
            this.mnuHelp.Text = "&Help";
            // 
            // mnuHelpAbout
            // 
            this.mnuHelpAbout.Name = "mnuHelpAbout";
            this.mnuHelpAbout.Size = new System.Drawing.Size(111, 22);
            this.mnuHelpAbout.Text = "&About";
            this.mnuHelpAbout.Click += new System.EventHandler(this.mnuHelpAbout_Click);
            // 
            // mIcon
            // 
            this.mIcon.ContextMenuStrip = this.mIconMenu;
            this.mIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("mIcon.Icon")));
            this.mIcon.Text = "WC3 Proxy";
            this.mIcon.Visible = true;
            this.mIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.mIcon_MouseDoubleClick);
            // 
            // mIconMenu
            // 
            this.mIconMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuIconExit});
            this.mIconMenu.Name = "mIconMenu";
            this.mIconMenu.ShowImageMargin = false;
            this.mIconMenu.Size = new System.Drawing.Size(72, 26);
            // 
            // mnuIconExit
            // 
            this.mnuIconExit.Name = "mnuIconExit";
            this.mnuIconExit.Size = new System.Drawing.Size(71, 22);
            this.mnuIconExit.Text = "Exit";
            this.mnuIconExit.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(343, 147);
            this.Controls.Add(tableLayoutPanel1);
            this.Controls.Add(this.mnuMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mnuMain;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Foole\'s WC3 Proxy";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            this.mnuMain.ResumeLayout(false);
            this.mnuMain.PerformLayout();
            this.mIconMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mnuMain;
        private System.Windows.Forms.ToolStripMenuItem mnuFile;
        private System.Windows.Forms.ToolStripMenuItem mnuFileExit;
        private System.Windows.Forms.ToolStripMenuItem mnuToolsLaunchWarcraft;
        private System.Windows.Forms.Label lblGameName;
        private System.Windows.Forms.Label lblMap;
        private System.Windows.Forms.Label lblGamePort;
        private System.Windows.Forms.Label lblServerAddress;
        private System.Windows.Forms.Label lblPlayers;
        private System.Windows.Forms.Label lblClientCount;
        private System.Windows.Forms.NotifyIcon mIcon;
        private System.Windows.Forms.ContextMenuStrip mIconMenu;
        private System.Windows.Forms.ToolStripMenuItem mnuIconExit;
        private System.Windows.Forms.ToolStripMenuItem mnuFileChangeServer;
        private System.Windows.Forms.ToolStripMenuItem mnuHelp;
        private System.Windows.Forms.ToolStripMenuItem mnuHelpAbout;
    }
}