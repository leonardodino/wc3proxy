/*
Copyright (c) 2008 Foole

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
 */
using System;
using System.Net;
using System.Windows.Forms;

namespace Foole.WC3Proxy
{
    struct WC3Version
    {
        public byte Id;
        public string Description;

        public WC3Version(byte id, string description)
        {
            Id = id;
            Description = description;
        }

        public override string ToString()
        {
            return Description;
        }
    }

    public partial class ServerInfoDlg : Form
    {
        private IPHostEntry mHost;

        public ServerInfoDlg()
        {
            InitializeComponent();

            cbxWC3Version.Items.Add(new WC3Version(0x1a, "1.26"));
            cbxWC3Version.Items.Add(new WC3Version(0x19, "1.25"));
            cbxWC3Version.Items.Add(new WC3Version(0x18, "1.24"));
            cbxWC3Version.Items.Add(new WC3Version(0x17, "1.23"));
            cbxWC3Version.Items.Add(new WC3Version(0x16, "1.22"));
            cbxWC3Version.Items.Add(new WC3Version(0x15, "1.21"));
            cbxWC3Version.Items.Add(new WC3Version(0x1b, "1.27 (Untested)"));
        }

        public IPHostEntry Host
        { 
            get { return mHost; }
            set 
            {
                // TODO: Check InvokeRequired?
                if (value == null)
                    txtServerAddress.Text = String.Empty;
                else
                    txtServerAddress.Text = value.HostName; 
            }
        }

        public bool Expansion
        { 
            get { return chkExpansion.Checked; }
            set { chkExpansion.Checked = value; }
        }

        public byte Version
        {
            get 
            {
                WC3Version vers = (WC3Version)cbxWC3Version.SelectedItem;
                return vers.Id; 
            }
            set 
            {
                foreach (WC3Version vers in cbxWC3Version.Items)
                {
                    if (vers.Id == value)
                    {
                        cbxWC3Version.SelectedItem = vers;
                        break;
                    }
                }
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (txtServerAddress.Text.Length == 0)
            {
                MessageBox.Show("Please enter a server address", "WC3 Proxy", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtServerAddress.Focus();
                return;
            }
            try
            {
                this.UseWaitCursor = true;
                mHost = Dns.GetHostEntry(txtServerAddress.Text);
                this.UseWaitCursor = false;
            } catch (Exception ex)
            {
                this.UseWaitCursor = false;
                // SocketException : No such host is known.
                MessageBox.Show("DNS Lookup failed: " + ex.Message, "WC3 Proxy", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtServerAddress.Focus();
                return;
            }

            DialogResult = DialogResult.OK;
            Hide();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Hide();
        }
    }
}