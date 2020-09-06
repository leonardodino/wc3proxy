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
using System.Net;
using System.Net.Sockets;
using System.Threading;

using Foole.Net;
using System.IO;

namespace Foole.WC3Proxy
{
    public class MainProxy
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

        static void Main(string[] args)
        {
            if (args.Length < 1 || args.Length > 3) {
                Console.WriteLine("Usage: wc3proxy <ip> [version] [game]");
                return;
            }
            if (args.Length < 2) {
                Array.Resize(ref args, 2);
                args[1] = "1.28";
            }
            if (args.Length < 3) {
                Array.Resize(ref args, 3);
                args[2] = "TFT";
            }

            IPHostEntry serverhost;
            
            serverhost = new IPHostEntry();
            serverhost.HostName = args[0];
            serverhost.AddressList = new IPAddress[] {IPAddress.Parse(args[0])};
            byte version = (byte)Math.Round((float.Parse(args[1]) - 1) * 100);
            bool expansion = args[2].ToLower() != "roc";

            MainProxy mainform = new MainProxy(serverhost, version, expansion);
            mainform.CompilerWorkaround();
            mainform.Banner();

            mainform.StartTcpProxy();
            mainform.StartBrowser();
            while (true) {
                Thread.Sleep(5000);
            }
        }

        public MainProxy(IPHostEntry ServerHost, byte Version, bool Expansion)
        {
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
        private void CompilerWorkaround(){
            // I couldn't get this project to compile without this file touching/importing the filesystem :shrug:
            string varname = "THIS_ENVIRONMENT_VARIABLE_DOES_NOT_EXISTS";
            string filename = "/THIS_FILE_DOES_NOT_EXISTS";
            if (Environment.GetEnvironmentVariable(varname) == "dotnet is weird") {
                Console.WriteLine(File.Exists(filename) ? new FileInfo(filename).Name : "");
            }
        }
        private void Banner() {
            string ip;
            if (mServerHost.AddressList[0].ToString() == mServerHost.HostName)
                ip = mServerHost.HostName;
            else
                ip = String.Format("{0} ({1})", mServerHost.HostName, mServerHost.AddressList[0].ToString());
            
            string version = "1." + mVersion.ToString();
            string game = mExpansion ? "The Frozen Throne" : "Reign of Chaos";
            string banner = String.Format("wc3proxy @ {0} | {1} ({2})", ip, game, version);
            string ruler = new String('=', banner.Length);
            Console.WriteLine(banner);
            Console.WriteLine(ruler+'\n');
        }

        private void ResetGameInfo()
        {
            Console.WriteLine(mCaption + " - Lost game");
            if (mServerEP != null) mServerEP.Port = 0;
            mFoundGame = false;
        }

        private void DisplayGameInfo()
        {
            if (mFoundGame == false) {
                Console.WriteLine(mCaption + " - Found game: " + mGameInfo.Name);
            }

            mServerEP.Port = mGameInfo.Port;
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
            if (mFoundGame) ResetGameInfo();
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
                Console.WriteLine("Unable to start listener\n" + ex.Message);
            }
        }

        private void GotConnection(Socket ClientSocket)
        {
            string message = String.Format("Got a connection from {0}", ClientSocket.RemoteEndPoint.ToString());
            Console.WriteLine(mCaption + " " + message);

            TcpProxy proxy = new TcpProxy(ClientSocket, mServerEP);
            proxy.ProxyDisconnected += new ProxyDisconnectedHandler(ProxyDisconnected);
            lock (mProxies) mProxies.Add(proxy);

            proxy.Run();
        }

        private void ProxyDisconnected(TcpProxy p)
        {
            Console.WriteLine(mCaption + " Client disconnected");
            lock (mProxies)
                if (mProxies.Contains(p)) mProxies.Remove(p);
        }

        private void StopTcpProxy()
        {
            mListener.Stop();
            foreach (TcpProxy p in mProxies)
                p.Stop();
        }
    }
}
