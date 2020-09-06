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
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Timers;

namespace Foole.WC3Proxy
{
    public struct GameInfo
    {
        public int GameId;
        public string Name;
        public string Map;
        public int Port;
        public int SlotCount;
        public int CurrentPlayers;
        public int PlayerSlots;
    }

    public delegate void FoundServerHandler(GameInfo Server);
    public delegate void QuerySentHandler();

    class Browser
    {
        private Socket mBrowseSocket;
        private byte[] mBrowsePacket;
        private IPEndPoint mServerEP;
        private IPEndPoint mClientEP = new IPEndPoint(IPAddress.Broadcast, 6112);
        private Timer mQueryTimer;
        private bool mQuerying;
        private byte[] mBuffer = new byte[512];
        private bool mExpansion;
        private int mProxyPort;
        private byte mVersion; // 1.22 = 0x16, 1.21 = 0x15

        public event FoundServerHandler FoundServer;
        public event QuerySentHandler QuerySent;

        public Browser(IPAddress ServerAddress, int ProxyPort, byte Version, bool Expansion)
        {
            mProxyPort = ProxyPort;
            mVersion = Version;
            mExpansion = Expansion;
            mQueryTimer = new Timer(1000);
            mQueryTimer.AutoReset = true;
            mQueryTimer.Elapsed += new ElapsedEventHandler(mQueryTimer_Elapsed);
            // WC3 always listens on UDP 6112
            mServerEP = new IPEndPoint(ServerAddress, 6112);
        }

        public byte Version
        {
            get { return mVersion; }
            set
            {
                mVersion = value;
                UpdateBrowsePacket();
            }
        }

        public bool Expansion
        {
            get { return mExpansion; }
            set
            {
                mExpansion = value;
                UpdateBrowsePacket();
            }
        }
        
        public IPAddress ServerAddress
        {
            get { return mServerEP.Address; }
            set { mServerEP.Address = value; }
        }
        
        public void Run()
        {
            UpdateBrowsePacket();

            mBrowseSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            mBrowseSocket.Bind(new IPEndPoint(IPAddress.Any, 0));
            mBrowseSocket.EnableBroadcast = true;

            mQueryTimer.Start();
        }

        public void Stop()
        {
            mQueryTimer.Stop();
            mBrowseSocket.Close();
            mBrowseSocket = null;
        }

        private void mQueryTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            ProcessResponses();

            if (mQuerying) return;
            mQuerying = true;

            SendQuery();

            mQuerying = false;

            ProcessResponses();
        }

        public void SendQuery()
        {
            mBrowseSocket.SendTo(mBrowsePacket, mServerEP);
            if (QuerySent != null) QuerySent();
        }

        public bool ProcessResponses()
        {
            bool receivedany = false;

            while (mBrowseSocket.Poll(0, SelectMode.SelectRead))
            {
                int len = 0;
                try
                {
                    len = mBrowseSocket.Receive(mBuffer);
                } catch (SocketException)
                {
                    // "An existing connection was forcibly closed by the remote host"
                    break;
                }
                if (len == 0) break;

                if (ExtractGameInfo(mBuffer, len) == false) continue;

                receivedany = true;
                ModifyGameName(mBuffer);
                ModifyGamePort(mBuffer, len, mProxyPort);
                mBrowseSocket.SendTo(mBuffer, len, SocketFlags.None, mClientEP);
            }

            return receivedany;
        }

        // Extracts the server's details from the query response and raises an event for it
        private bool ExtractGameInfo(byte[] response, int Length)
        {
            if (response[0] != 0xf7 || response[1] != 0x30) return false;

            GameInfo game = new GameInfo();

            game.GameId = BitConverter.ToInt32(response, 0xc);
            game.Name = StringFromArray(response, 0x14);

            //int cryptstart = 0x14 + game.Name.Length + 1 + 1; // one extra byte after the server name
            int cryptstart = 0x14 + Encoding.UTF8.GetByteCount(game.Name) + 1 + 1; // one extra byte after the server name
            byte[] decrypted = Decrypt(response, cryptstart);
            game.Map = StringFromArray(decrypted, 0xd);

            game.Port = BitConverter.ToUInt16(response, Length - 2);
            game.SlotCount = BitConverter.ToInt32(response, Length - 22);
            game.CurrentPlayers = BitConverter.ToInt32(response, Length - 14);
            game.PlayerSlots = BitConverter.ToInt32(response, Length - 10);

            if (FoundServer != null) FoundServer(game);
            SendGameAnnounce(game);

            return true;
        }

        public void SendGameCancelled(int GameId)
        {
            byte[] packet = new byte[] { 0xf7, 0x33, 0x08, 0x00, (byte)GameId, 0x00, 0x00, 0x00 };
            mBrowseSocket.SendTo(packet, mClientEP);
        }

        // The client wont update the player count unless this is sent
        public void SendGameAnnounce(GameInfo Game)
        {
            int players = Game.SlotCount - Game.PlayerSlots + Game.CurrentPlayers;
            byte[] packet = new byte[] { 0xf7, 0x32, 0x10, 0x00, (byte)Game.GameId, 0x00, 0x00, 0x00, (byte)players, 0, 0, 0, (byte)Game.SlotCount, 0, 0, 0 };
            mBrowseSocket.SendTo(packet, mClientEP);
        }

        //This is also used to decrypt recorded game file headers
        private byte[] Decrypt(byte[] Data, int Offset)
        {
            // TODO: calculate the real result length (Data.Length * 8 / 9?).
            // in=37, out=30.  in=3a, out=32.
            MemoryStream output = new MemoryStream();
            int pos = 0;
            byte mask = 0;
            while (true)
            {
                byte b = Data[pos + Offset];
                if (b == 0) break;
                if (pos % 8 == 0)
                {
                    mask = b;
                } else
                {
                    if ((mask & (0x1 << (pos % 8))) == 0)
                        output.WriteByte((byte)(b - 1));
                    else
                        output.WriteByte(b);
                }
                pos++;
            }
            return output.ToArray();
        }

        private string StringFromArray(byte[] Data, int Offset)
        {

            /*
            StringBuilder sb = new StringBuilder();
            while (true)
            {
                char c = (char)Data[Offset++];
                if (c == 0) break;
                sb.Append(c);
            }
            return sb.ToString();
            */

            int length = 0;
            while (Data[Offset + (++length)] != 0x0);

            return Encoding.UTF8.GetString(Data, Offset, length);

        }

        // Replace "Local Game" with "Proxy Game"
        // This will not work properly for other languages
        private void ModifyGameName(byte[] Response)
        {
            Response[0x14] = (byte)'P';
            Response[0x15] = (byte)'r';
            Response[0x16] = (byte)'o';
            Response[0x17] = (byte)'x';
            Response[0x18] = (byte)'y';
        }

        private void ModifyGamePort(byte[] Response, int Length, int Port)
        {
            int index = Length - 2;
            Response[index] = (byte)(Port & 0xff);
            Response[index + 1] = (byte)(Port >> 8);
        }

        // Dummy version with hard coded query packet
        private void UpdateBrowsePacket()
        {
            if (mExpansion) // TFT - PX3W instead of 3RAW
                mBrowsePacket = new byte[] { 0xf7, 0x2f, 0x10, 0x00, 0x50, 0x58, 0x33, 0x57, mVersion, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            else // ROC
                mBrowsePacket = new byte[] { 0xf7, 0x2f, 0x10, 0x00, 0x33, 0x52, 0x41, 0x57, mVersion, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
        }
    }
}
