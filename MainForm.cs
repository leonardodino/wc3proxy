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
using System.Collections.Generic;
using System.Diagnostics; // for Process
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

using Microsoft.Win32; // for Registry

using Foole.Net; // For Listener
using System.IO;

namespace Foole.WC3Proxy
{
    public partial class MainForm : Form
    {
        private Listener mListener; // This waits for proxy connections
        private List<TcpProxy> mProxies; // A collection of game proxies.  Usually we would only need 1 proxy.
        private Browser mBrowser; // This sends game info queries to the server and forwards the responses to the client

        private IPHostEntry mServerHost;
        private IPEndPoint mServerEP;
        private byte mVersion;
        private bool mExpansion;

        // TODO: Possibly move these (and associated code) into the Browser class
        private bool mFoundGame;
        private DateTime mLastFoundServer;
        private GameInfo mGameInfo;

        private readonly string mCaption = "WC3 Proxy";
        private readonly int mBalloonTipTimeout = 1000;

        private static readonly string mRegPath = @"HKEY_CURRENT_USER\Software\Foole\WC3 Proxy";

        private delegate void SimpleDelegate();

        // TODO: Configurable command line arguments for war3?
        // window       Windowed mode
        // fullscreen   (Default)
        // gametype     ?
        // loadfile     Loads a map or replay
        // datadir      ?
        // classic      This will load in RoC mode even if you have TFT installed.
        // swtnl        Software Transform & Lighting
        // opengl
        // d3d          (Default)

        static void Main(string[] args)
        {
            IPHostEntry serverhost = null;
            byte version = 0;
            bool expansion = false;

            string servername = (string)Registry.GetValue(mRegPath, "ServerName", null);
            if (servername != null)
            {
                expansion = ((int)Registry.GetValue(mRegPath, "Expansion", 0)) != 0;
                try
                {
                    serverhost = Dns.GetHostEntry(servername);
                } catch { }

                version = (byte)(int)Registry.GetValue(mRegPath, "WC3Version", 0);
            }

            if (serverhost == null || version == 0)
                if (ShowInfoDialog(ref serverhost, ref version, ref expansion) == false) return;

            MainForm mainform = new MainForm(serverhost, version, expansion);

            Application.Run(mainform);
        }

        private static bool ShowInfoDialog(ref IPHostEntry Host, ref byte Version, ref bool Expansion)
        {
            ServerInfoDlg dlg = new ServerInfoDlg();
            if (Host != null)
            {
                dlg.Host = Host;
                dlg.Expansion = Expansion;
                dlg.Version = Version;
            }
            if (dlg.ShowDialog() == DialogResult.Cancel)
                return false;

            Host = dlg.Host;
            Version = dlg.Version;
            Expansion = dlg.Expansion;
            dlg.Dispose();

            // TODO: Should this store the ip address or the hostname?
            Registry.SetValue(mRegPath, "ServerName", Host.HostName, RegistryValueKind.String);
            Registry.SetValue(mRegPath, "Expansion", Expansion ? 1 : 0, RegistryValueKind.DWord);
            Registry.SetValue(mRegPath, "WC3Version", Version, RegistryValueKind.DWord);
            return true;
        }

        public MainForm(IPHostEntry ServerHost, byte Version, bool Expansion)
        {
            InitializeComponent();

            this.ServerHost = ServerHost;
            this.Version = Version;
            this.Expansion = Expansion;
        }

        public IPHostEntry ServerHost
        {
            get { return mServerHost; }
            set 
            {
                OnLostGame();

                mServerHost = value;
                mServerEP = new IPEndPoint(mServerHost.AddressList[0], 0);

                string addrdesc;
                if (mServerHost.AddressList[0].ToString() == mServerHost.HostName)
                    addrdesc = mServerHost.HostName;
                else
                    addrdesc = String.Format("{0} ({1})", mServerHost.HostName, mServerHost.AddressList[0].ToString());

                lblServerAddress.Text = addrdesc;

                if (mBrowser != null) mBrowser.ServerAddress = mServerHost.AddressList[0];
            }
        }

        public bool Expansion
        {
            get { return mExpansion; }
            set
            {
                mExpansion = value; 
                if (mBrowser != null) mBrowser.Expansion = value;
            }
        }

        public byte Version
        {
            get { return mVersion; }
            set
            {
                mVersion = value;
                if (mBrowser != null) mBrowser.Version = value;
            }
        }

        private void ResetGameInfo()
        {
            mIcon.ShowBalloonTip(mBalloonTipTimeout, mCaption, "Lost game", ToolTipIcon.Info);

            lblGameName.Text = "(None found)";
            lblMap.Text = "(N/A)";
            lblGamePort.Text = "(N/A)";
            lblPlayers.Text = "(N/A)";

            mServerEP.Port = 0;

            mFoundGame = false;
        }

        private void DisplayGameInfo()
        {
            if (InvokeRequired)
            {
                Invoke(new SimpleDelegate(DisplayGameInfo));
                return;
            }

            if (mFoundGame == false) mIcon.ShowBalloonTip(mBalloonTipTimeout, mCaption, "Found game: " + mGameInfo.Name, ToolTipIcon.Info);

            lblGameName.Text = mGameInfo.Name;
            lblMap.Text = mGameInfo.Map;
            lblGamePort.Text = mGameInfo.Port.ToString();
            lblPlayers.Text = String.Format("{0} / {1} / {2}", mGameInfo.CurrentPlayers, mGameInfo.PlayerSlots, mGameInfo.SlotCount);

            mServerEP.Port = mGameInfo.Port;
        }

        private void ExecuteWC3(bool Expansion)
        {
            string programkey = Expansion ? "ProgramX" : "Program";
            string program = (string)Registry.GetValue(@"HKEY_CURRENT_USER\Software\Blizzard Entertainment\Warcraft III", programkey, null);

            if (program == null)
            {

                string currentDir = Directory.GetCurrentDirectory();

                program = Path.Combine(currentDir, "war3.exe");

                if (!File.Exists(program))
                {

                    MessageBox.Show("Unable to locate Warcraft 3 executable");
                    return;

                }

            }

            try
            {
                Process.Start(program);
            } catch (Exception e)
            {
                string message = string.Format("Unable to launch WC3: {0}\n{1}", e.Message, program);
                MessageBox.Show(message, mCaption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            // TODO: If the file doesnt exist, just launch war3.exe?
        }

        private void mnuFileExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void mnuLaunchWarcraft_Click(object sender, EventArgs e)
        {
            ExecuteWC3(Expansion);
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            StartTcpProxy();
            StartBrowser();
        }

        private void StartBrowser()
        {
            mBrowser = new Browser(ServerHost.AddressList[0], mListener.LocalEndPoint.Port, Version, Expansion);
            mBrowser.QuerySent += new QuerySentHandler(mBrowser_QuerySent);
            mBrowser.FoundServer += new FoundServerHandler(mBrowser_FoundServer);
            mBrowser.Run();
        }

        void mBrowser_FoundServer(GameInfo Game)
        {
            mGameInfo = Game;
            DisplayGameInfo();

            mFoundGame = true;
            mLastFoundServer = DateTime.Now;
        }

        void mBrowser_QuerySent()
        {
            // TODO: show an activity indicator?

            // We don't receive the "server cancelled" messages
            // because they are only ever broadcast to the host's LAN.
            if (mFoundGame == true)
            {
                TimeSpan interval = DateTime.Now - mLastFoundServer;
                if (interval.TotalSeconds > 3)
                    OnLostGame();
            }
        }

        private void OnLostGame()
        {
            if (mBrowser != null) mBrowser.SendGameCancelled(mGameInfo.GameId);
            if (mFoundGame) Invoke(new SimpleDelegate(ResetGameInfo));
        }

        private void StartTcpProxy()
        {
            mProxies = new List<TcpProxy>();

            mListener = new Listener(new GotConnectionDelegate(GotConnection));
            try
            {
                mListener.Run();
            }
            catch (SocketException ex)
            {
                MessageBox.Show("Unable to start listener\n" + ex.Message);
            }
        }

        private void GotConnection(Socket ClientSocket)
        {
            string message = String.Format("Got a connection from {0}", ClientSocket.RemoteEndPoint.ToString());
            mIcon.ShowBalloonTip(mBalloonTipTimeout, mCaption, message, ToolTipIcon.Info);

            TcpProxy proxy = new TcpProxy(ClientSocket, mServerEP);
            proxy.ProxyDisconnected += new ProxyDisconnectedHandler(ProxyDisconnected);
            lock (mProxies) mProxies.Add(proxy);

            proxy.Run();

            UpdateClientCount();
        }

        private void UpdateClientCount()
        {
            if (InvokeRequired) 
            {
                Invoke(new SimpleDelegate(UpdateClientCount));
                return;
            }
            lblClientCount.Text = mProxies.Count.ToString();
        }

        private void ProxyDisconnected(TcpProxy p)
        {
            mIcon.ShowBalloonTip(mBalloonTipTimeout, mCaption, "Client disconnected", ToolTipIcon.Info);

            lock (mProxies)
                if (mProxies.Contains(p)) mProxies.Remove(p);

            UpdateClientCount();
        }

        private void StopTcpProxy()
        {
            mListener.Stop();
            foreach (TcpProxy p in mProxies)
                p.Stop();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopTcpProxy();
            if (mBrowser != null) mBrowser.Stop();
            if (mFoundGame) mBrowser.SendGameCancelled(mGameInfo.GameId);
        }

        private void mIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            WindowState = FormWindowState.Normal;
            Focus();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            ShowInTaskbar = (WindowState != FormWindowState.Minimized);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void mnuChangeServer_Click(object sender, EventArgs e)
        {
            IPHostEntry host = ServerHost;
            bool expansion = Expansion;
            byte version = Version;

            if (ShowInfoDialog(ref host, ref version, ref expansion))
            {
                ServerHost = host;
                Version = version;
                Expansion = expansion;
            }
        }

        private void mnuHelpAbout_Click(object sender, EventArgs e)
        {
            new AboutBox().ShowDialog();
        }

        private void clearRecordsToolStripMenuItem_Click(object sender, EventArgs e)
        {

            Registry.CurrentUser.DeleteSubKey(@"Software\Foole\WC3 Proxy");
            Registry.CurrentUser.DeleteSubKey(@"Software\Foole");
            MessageBox.Show("Registry records cleared.", mCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

    }
}