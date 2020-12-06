using QuarterShare.Crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace QuarterShare.Connection
{
    class QuarterClient
    {
        public string Id;
        public QuarterServer Server { get; }
        public TcpClient Connection { get; }
        public IPAddress Address { get; }
        public AESCrypto AESCrypto;
        public bool Allowed;

        public string SecurityCode
        {
            get
            {
                return MD5Crypto.ToMD5(Server.RSACrypto.BinaryPublicKey.Concat(ShareKey).ToArray());
            }
        }

        private NetworkStream Steam;
        private byte[] ShareKey;
        private byte[] ShareIv;

        public QuarterClient(QuarterServer Server, TcpClient Connection)
        {
            this.Server = Server;
            this.Connection = Connection;
            Address = ((IPEndPoint)Connection.Client.RemoteEndPoint).Address;
            Allowed = Program.DEFAULT_ALLOW;
            Steam = Connection.GetStream();

            Server.AddClient(this);
        }

        public void Close()
        {
            Connection.Close();
            Server.RemoveClient(this);
        }

        public void AskAESKey()
        {
            byte[] msg = new byte[128];
            Steam.Read(msg, 0, msg.Length);


            byte[] data = Server.RSACrypto.RSADecrypt(msg);
            ShareKey = data.Skip(0).Take(32).ToArray();
            ShareIv = data.Skip(32).Take(16).ToArray();

            AESCrypto = new AESCrypto(ShareKey, ShareIv);
        }

        public byte[] RecvMessage()
        {
            byte[] msg = new byte[655366];
            int i = Steam.Read(msg, 0, msg.Length);

            if (i == 0)
                return null; // end, disconnected
            else
                return AESCrypto.ASEDecrypt(msg.Take(i).ToArray());
        }

        public void SendServerPublicKey()
        {
            Send(Server.GetFirstRSAPublicKeySendOutMessage());
        }

        public void SendMessage(byte[] binary)
        {
            Send(AESCrypto.ASEEncrypt(binary));
        }

        private void Send(byte[] send)
        {
            byte[] Out = Util.TwoByteIndictor(send.Length).Concat(send).ToArray();
            Steam.Write(Out, 0, Out.Length);
        }

    }
}
