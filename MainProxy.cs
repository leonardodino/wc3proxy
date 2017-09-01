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

using Foole.Net; // For Listener
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
            if (args.Length != 1) {
                Console.WriteLine("Usage: wc3proxy ipaddress");
                return;
            }

            IPHostEntry serverhost;
            
            serverhost = new IPHostEntry();
            serverhost.HostName = args[0];
            serverhost.AddressList = new IPAddress[] {IPAddress.Parse(args[0])};

            // List of different values for different versions.
            // 0x15 = , "1.21"
            // 0x16 = , "1.22"
            // 0x17 = , "1.23"
            // 0x18 = , "1.24"
            // 0x19 = , "1.25"
            // 0x1a = , "1.26"
            // 0x1b = , "1.27"
            // 0x1c = , "1.28"
            // 0x1d = , "1.29"
            // 0x1e = , "1.30"
            // 0x1f = , "1.31"
            byte version = 0x1f;
            bool expansion = true; // For Reign of Chaos, put false. For Frozen Throne, put true.

            MainProxy mainform = new MainProxy(serverhost, version, expansion);
            mainform.CleanupReplays();

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
                Console.WriteLine("Setting IP");
                OnLostGame();

                mServerHost = value;
                mServerEP = new IPEndPoint(mServerHost.AddressList[0], 0);

                string addrdesc;
                if (mServerHost.AddressList[0].ToString() == mServerHost.HostName)
                    addrdesc = mServerHost.HostName;
                else
                    addrdesc = String.Format("{0} ({1})", mServerHost.HostName, mServerHost.AddressList[0].ToString());

                Console.WriteLine(String.Format("Starting proxy on {0}", addrdesc));
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

        private void CleanupReplays()
        {
            TimeSpan maxAge = new TimeSpan(60, 0, 0, 0);
            String replay_directory = Environment.GetEnvironmentVariable("HOME") + @"/Library/Application Support/Blizzard/Warcraft III/Replay";
            if (Directory.Exists(replay_directory)) {
                foreach (var file in Directory.EnumerateFiles(replay_directory, "*.w3g", SearchOption.AllDirectories))
                {
                    FileInfo fi = new FileInfo(file);
                    DateTime time = fi.CreationTime;
                    if (DateTime.Now.Subtract(time) > maxAge)
                    {
                        Console.WriteLine("Deleting: " + file);
                        File.Delete(file);
                    }
                }
            }
        }

        private void CopyReplay()
        {
            string lastReplay = "/Users/msanfacon/Library/Application Support/Blizzard/Warcraft III/Replay/LastReplay.w3g";
            if (File.Exists(lastReplay)) {
                FileInfo fi = new FileInfo(lastReplay);
                DateTime time = fi.CreationTime;

                string newFile = "/Users/msanfacon/Library/Application Support/Blizzard/Warcraft III/Replay/Autosaved/" + time.ToString("yyyyMMdd_HH_mm") + ".w3g";
                if (!File.Exists(newFile)) {
                    Console.WriteLine("Copied LastReplay.w3g into " + newFile + "\n");
                    File.Copy(lastReplay, newFile);
                }
            }
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
                CopyReplay();
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
