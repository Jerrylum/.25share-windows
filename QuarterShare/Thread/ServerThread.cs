using QuarterShare.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace QuarterShare.Thread
{
    class ServerThread
    {
        public QuarterServer Server;

        public ServerThread(IPAddress host, int port)
        {
            Server = new QuarterServer(host, port);

            new System.Threading.Thread(MainThread).Start(Server);
        }

        private static void MainThread(object data)
        {
            //System.Threading.Thread.CurrentThread.IsBackground = true;
            QuarterServer server = (QuarterServer)data;

            ColorPrint(
                "@@@@@@@@@@@  Quarter Share\n@@@@@@@@@@@\n@@@@@@@@@@@  Use command ");
            Color(ConsoleColor.Blue, ".help");
            ColorPrint(
                " to learn more about the server's\n@@@###@###@  internal commands\n@@@@@#@#@@@\n" +
                "@@@###@###@  The server is running at " + Program.UsingHost.ToString() + ":" + Program.UsingPort + "\n" +
                "@@@#@@@@@#@\n@#@###@###@\n@@@@@@@@@@@\n@@@@@@@@@@@\n@@@@@@@@@@@\n\n");

            while (true)
            {
                try
                {
                    TcpClient client = server.Listener.AcceptTcpClient();
                    new ClientThread(server, client);
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

        private static void Color(ConsoleColor color, object o = null)
        {
            Console.ForegroundColor = color;
            if (o != null)
                Console.Write(o);
            Console.ForegroundColor = ConsoleColor.White;
        }

        private static void Red(object o = null)
        {
            Color(ConsoleColor.DarkRed, o);
        }

        private static void White(object o = null)
        {
            Color(ConsoleColor.White, o);
        }
    }
}
