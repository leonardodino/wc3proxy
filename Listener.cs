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
using System.Net.Sockets;

namespace Foole.Net
{
	public delegate void GotConnectionDelegate(Socket ClientSocket);

	public class Listener
	{
		private Socket ListenSocket;
		private GotConnectionDelegate mGotConnection;
		private IPAddress mAddress;
		private int mPort;

		private bool mStop = false;
		private AsyncCallback cbAccept;

		public Listener(IPAddress Addr, int Port, GotConnectionDelegate GotConnection)
		{
			Init(Addr, Port, GotConnection);
		}

		public Listener(int Port, GotConnectionDelegate GotConnection)
		{
			Init(IPAddress.Any, Port, GotConnection);
		}

		public Listener(GotConnectionDelegate GotConnection)
		{
			Init(IPAddress.Any, 0, GotConnection);
		}
		
		private void Init(IPAddress Addr, int Port, GotConnectionDelegate GotConnection)
		{
			mAddress = Addr;
			mPort = Port;
			mGotConnection = GotConnection;
			cbAccept = new AsyncCallback(EndAccept);
		}

		public void Run()
		{
            Console.WriteLine("Listener - Run");
			mStop = false;

			IPEndPoint EndPoint = new IPEndPoint(mAddress, mPort);

			ListenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            ListenSocket.Bind(EndPoint);

			mPort = LocalEndPoint.Port;
			ListenSocket.Listen(20);

			BeginAccept();
		}

		private void BeginAccept()
		{
			if (mStop) return;
            Console.WriteLine("Listener - Accept");
			ListenSocket.BeginAccept(cbAccept, null);
            Console.WriteLine("Listener - Accept Done");
		} 

		private void EndAccept(IAsyncResult ar)
		{
            Console.WriteLine("Listener - EndAccept");
            if (mStop) return;

			try
            {
                Socket Client = ListenSocket.EndAccept(ar);
                mGotConnection(Client);
            } catch (ObjectDisposedException)
            {
                // Occasionally throws: System.ObjectDisposedException: Cannot access a disposed object.
                // Do nothing
            }
			BeginAccept();
		}

		public void Stop()
		{
			mStop = true;
			ListenSocket.Close();
			ListenSocket = null;
		}
		
		public IPEndPoint LocalEndPoint
		{
			get
			{
				return (IPEndPoint)ListenSocket.LocalEndPoint;
			}
		}
	}
}
