using QuarterShare.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuarterShare.Command
{

    abstract class ServerCommand : Command
    {
        public ServerCommand(QuarterServer server, string raw) : base(server, raw) { }
    }

    class ShowHelpCommand : ServerCommand
    {
        public ShowHelpCommand(QuarterServer server, string raw) : base(server, raw) { }

        public override bool IsMatch() => Split[0] == "help";

        public override bool Handle()
        {
            Console.WriteLine(
                "COMMAND\n" +
                "    .help                            show this help message\n" +
                "    .flag                            show how the server handles messages\n" +
                "    .chflag [flag]                   change how the server handles messages\n" +
                "    .ls                              list all connected clients\n" +
                "    .allow <client>                  allow client(s) to send messages\n" +
                "    .kick <client>                   kick specified client(s)\n" +
                "    .send <client> <content>         send a message to client(s)\n" +
                "    .stop                            stop the server\n" +
                "\n" +
                "CLIENT SELECTOR\n" +
                "    @a      all clients\n" +
                "    @p      the latest client who sent a message / connected\n" +
                "    <ID>    specified client id, e.g. `5`\n" +
                "\n" +
                "FLAG\n" +
                "    p       Print on the console(default)\n" +
                "    c       Copy to clipboard\n" +
                "    t       Typing text(default)\n" +
                "\n" +
                "    You can use multiple flags at the same time.\n" +
                "    e.g. `t`, `c` and `pct` are acceptable\n" +
                "\n" +
                "NOTE\n" +
                "    1. Commands must be preceded by a period.\n" +
                "    2. Any input that does not start with a period is understood as sending\n" +
                "       the entire sentence to the latest client (@p).\n" +
                "    3. If you want to send a message that starts with a period, use command \n" +
                "       `.send @p YOUR MESSAGE`\n");
            return true;
        }
    }

    class ShowFlagCommand : ServerCommand
    {
        public ShowFlagCommand(QuarterServer server, string raw) : base(server, raw) { }

        public override bool IsMatch() => Split[0] == "flag";

        public override bool Handle()
        {
            Green("Flag: " + string.Join(", ", Server.Flags) + "\n\n");
            return true;
        }
    }

    class ChangeFlagCommand : ServerCommand
    {
        public ChangeFlagCommand(QuarterServer server, string raw) : base(server, raw) { }

        public override bool IsMatch() => Split[0] == "chflag";

        public override bool Handle()
        {
            Server.Flags = UserInput.ResolveFlag(Split[1]);
            Green("Flags changed to " + string.Join(", ", Server.Flags) + "\n\n");
            return true;
        }
    }

    class ListClientCommand : ServerCommand
    {
        public ListClientCommand(QuarterServer server, string raw) : base(server, raw) { }

        public override bool IsMatch() => Split[0] == "ls";

        public override bool Handle()
        {
            int len = Server.Clients.Count;

            if (len == 0)
            {
                Console.WriteLine("No client");
            }
            else
            {
                Console.WriteLine("Total of " + len + " client(s)");
                foreach (QuarterClient client in Server.Clients)
                {
                    Console.WriteLine("#" + client.Id + "\t" + client.Address);
                }
            }
            Console.WriteLine();
            return true;
        }
    }

    class StopServerCommand : ServerCommand
    {
        public StopServerCommand(QuarterServer server, string raw) : base(server, raw) { }

        public override bool IsMatch() => Split[0] == "stop";

        public override bool Handle()
        {
            Program.Close();
            return true;
        }
    }
}
