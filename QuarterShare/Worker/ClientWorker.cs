using QuarterShare.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QuarterShare.Worker
{
    class ClientWorker : Worker
    {
        private QuarterClient Client;

        public ClientWorker(QuarterServer server, TcpClient tcp)
        {
            Client = new QuarterClient(server, tcp);

            new Thread(MainThread).Start(Client);
        }

        private static void MainThread(object data)
        {
            QuarterClient client = (QuarterClient)data;

            Green("\nConnected by " + client.Address.ToString() + "\n");

            try
            {
                client.SendServerPublicKey();
                client.AskAESKey();

                ShowWelcomeMessage(client);

                while (true)
                {
                    byte[] result = client.RecvMessage();
                    if (result == null)
                        throw new Exception("Closed");

                    if (!client.Allowed)
                        continue;


                    byte[] MsgID = result.Take(4).ToArray();
                    byte[] Msg = result.Skip(4).ToArray();

                    HandleMessage(client, Msg);
                    client.SendMessage(ToClientFlag.PONG_FLAG.Concat(MsgID).ToArray());
                }
            }
            catch
            {
                Red("Disconnected by " + client.Address.ToString() + "\n");
            }

            client.Close();
            Red("Closed " + client.Address.ToString() + "\n");
        }

        private static void ShowWelcomeMessage(QuarterClient client)
        {
            White("Security Code:\n");
            string code = client.SecurityCode;
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Console.Write(new string(code.Skip(j * 4 + i * 16).Take(4).ToArray()));
                    Console.Write(" ");
                }
                Console.WriteLine("");
            }
            White(
                "\nTo ensure your messages are secured, please verify that the security code\n" +
                "displayed on the screen is exactly the same as the one displayed on the client.\n\n");

            if (!client.Allowed)
            {
                White("Use command ");
                Blue(".allow " + client.Id);
                White(" to allow the client to send messages\n\n");
            }
        }

        private static void HandleMessage(QuarterClient client, byte[] msg)
        {
            string str = Encoding.UTF8.GetString(msg);

            if (client.Server.Flags["typing"])
                Keyboard.SendString(str);

            if (client.Server.Flags["clipboard"])
                Keyboard.SetClipboard(str);

            if (client.Server.Flags["print"])
            {
                Blue("#" + client.Id + "> ");
                White(str + "\n");
            }
        }
    }
}
