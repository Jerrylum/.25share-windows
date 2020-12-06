using QuarterShare.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace QuarterShare.Thread
{
    class ClientThread
    {
        private QuarterClient Client;

        public ClientThread(QuarterServer server, TcpClient tcp)
        {
            Client = new QuarterClient(server, tcp);

            var thread = new System.Threading.Thread(MainThread);
            thread.IsBackground = true;
            thread.Start(Client);
        }

        private static void MainThread(object data)
        {
            QuarterClient client = (QuarterClient)data;

            Green("\nConnected by " + client.Address.ToString());

            try
            {
                client.SendServerPublicKey();
                client.AskAESKey();

                ShowWelcomeMessage(client);

                while (true)
                {
                    byte[] result = client.RecvMessage();

                    byte[] MsgID = result.Take(4).ToArray();
                    byte[] Msg = result.Skip(4).ToArray();

                    if (result == null) throw new Exception("Closed");

                    Console.WriteLine(">>>" + Encoding.UTF8.GetString(Msg)); // TODO handle message

                    client.SendMessage(ToClientFlag.PONG_FLAG.Concat(MsgID).ToArray());
                }
            }
            catch
            {
                Red("Disconnected by " + client.Address.ToString());
            }

            client.Close();
            Red("Closed " + client.Address.ToString());
        }

        private static void ShowWelcomeMessage(QuarterClient client)
        {
            Console.WriteLine("Security Code:");
            string code = client.SecurityCode;
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Console.Write(code.Skip(i * 4 * 2).Take(4).ToArray());
                    Console.Write(" ");
                }
                Console.WriteLine("");
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(
                "\nTo ensure your messages are secured, please verify that the security code\n" +
                "displayed on the screen is exactly the same as the one displayed on the client.\n");

            if (!client.Allowed)
            {
                Console.Write("Use command ");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write(".allow " + client.Id);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(" to allow the client to send messages\n");
            }
        }

        private static void Color(ConsoleColor color, object o = null)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(o);
            Console.ForegroundColor = ConsoleColor.White;
        }

        private static void Red(object o = null)
        {
            Color(ConsoleColor.DarkRed, o);
        }

        private static void Green(object o = null)
        {
            Color(ConsoleColor.Green, o);
        }

        private static void White(object o = null)
        {
            Color(ConsoleColor.White, o);
        }
    }
}
