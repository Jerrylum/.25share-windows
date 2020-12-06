using QuarterShare.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QuarterShare.Worker
{
    class ServerWorker : Worker
    {
        public QuarterServer Server;

        public ServerWorker(IPAddress host, int port)
        {
            Server = new QuarterServer(host, port);

            new Thread(MainThread).Start(Server);
        }

        private static void MainThread(object data)
        {
            QuarterServer server = (QuarterServer)data;

            ColorPrint(
                "@@@@@@@@@@@  Quarter Share\n@@@@@@@@@@@\n@@@@@@@@@@@  Use command ");
            Blue(".help");
            ColorPrint(
                " to learn more about the server's\n@@@###@###@  internal commands\n@@@@@#@#@@@\n" +
                "@@@###@###@  The server is running at " + Program.UsingHost.ToString() + ":" + Program.UsingPort + "\n" +
                "@@@#@@@@@#@\n@#@###@###@\n@@@@@@@@@@@\n@@@@@@@@@@@\n@@@@@@@@@@@\n\n");

            while (true)
            {
                try
                {
                    TcpClient client = server.Listener.AcceptTcpClient();
                    new ClientWorker(server, client);
                }
                catch
                {
                    // server shutdown or connection fail
                }
            }

        }

        private static void ColorPrint(string str)
        {
            for (int i = 0; i < str.Length; i ++)
            {
                char c = str[i];
                if (c == '@')
                    Red("■");
                else if (c == '#')
                    White("■");
                else
                    White(c);
            }
        }

    }
}
