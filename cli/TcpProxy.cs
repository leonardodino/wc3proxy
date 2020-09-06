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

using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Foole.WC3Proxy
{
    delegate void ProxyDisconnectedHandler(TcpProxy Proxy);

    class TcpProxy
    {
        private Socket mClientSocket;
        private Socket mServerSocket;
        private EndPoint mServerEP;
        private Thread mThread;
        private bool mRunning;
        private byte[] mBuffer = new byte[2048];

        public event ProxyDisconnectedHandler ProxyDisconnected;

        public TcpProxy(Socket ClientSocket, EndPoint ServerEP)
        {
            mClientSocket = ClientSocket;
            mServerEP = ServerEP;

            mServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void Run()
        {
            mServerSocket.Connect(mServerEP);

            mRunning = true;
            mThread = new Thread(new ThreadStart(ThreadFunc));
            mThread.Start();
        }

        public void Stop()
        {
            mRunning = false;
            if (mThread != null) mThread.Join();
        }

        private void ThreadFunc()
        {
            ArrayList sockets = new ArrayList(2);
            sockets.Add(mClientSocket);
            sockets.Add(mServerSocket);

            while (mRunning)
            {
                IList readsockets = (IList)sockets.Clone();
                Socket.Select(readsockets, null, null, 1000000);
                foreach (Socket s in readsockets)
                {
                    int length = 0;
                    try
                    {
                        length = s.Receive(mBuffer);
                    } catch { }
                    if (length == 0)
                    {
                        mRunning = false;
                        if (ProxyDisconnected != null) ProxyDisconnected(this);
                        break;
                    }
                    Socket dest = (s == mServerSocket) ? mClientSocket : mServerSocket;
                    dest.Send(mBuffer, length, SocketFlags.None);
                }
            }
        }
    }
}
