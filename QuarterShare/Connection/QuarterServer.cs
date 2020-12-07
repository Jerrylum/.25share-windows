using QuarterShare.Command;
using QuarterShare.Crypto;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace QuarterShare.Connection
{
    class QuarterServer
    {
        public bool DefaultAllow;
        public Dictionary<string, bool> Flags { get; }
        public IPAddress Host { get; }
        public int Port { get; }
        public RSACrypto RSACrypto { get; }
        public TcpListener Listener { get; }
        public QuarterClient LatestClient { get; }
        public ArrayList Clients { get; }

        private int ClientCount;


        public QuarterServer(ServerConfig config)
        {
            DefaultAllow = config.DefaultAllow;
            Flags = config.Flags;
            Host = config.Host;
            Port = config.Port;
            RSACrypto = new RSACrypto();
            LatestClient = null;
            Clients = new ArrayList();
            ClientCount = 1;

            try
            {
                Listener = new TcpListener(Host, Port);
                Listener.Server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, 1);
                Listener.Start();
            } catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Close()
        {
            try
            {
                Listener.Stop();
            }
            catch
            {

            }
        }

        public void AddClient(QuarterClient Client)
        {
            Client.Id = ClientCount + "";
            ClientCount++;
            Clients.Add(Client);
        }

        public void RemoveClient(QuarterClient Client)
        {
            Clients.Remove(Client);
        }

        public byte[] GetFirstRSAPublicKeySendOutMessage()
        {
            return Encoding.Default.GetBytes(RSACrypto.Base64PublicKey);
        }

    }
}
