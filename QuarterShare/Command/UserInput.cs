using QuarterShare.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace QuarterShare.Command
{
    class UserInput
    {

        public static void PrintShellCommandHelp()
        {
            Console.WriteLine(
                "usage: .25share [--help] [--host:HOST] [--port:PORT] [--allow] [--flag:FLAG]\n" +
                "\n" +
                "optional arguments:\n" +
                "  --help         Show this help message and exit\n" +
                "  --host HOST    The server's hostname or IP address\n" +
                "  --port PORT    The port to listen on\n" +
                "  --allow        Allow all clients to send messages to the server\n" +
                "  --flag FLAG    Mode flag\n" +
                "\n" +
                "FLAG\n" +
                "    p       Print on the console(default)\n" +
                "    c       Copy to clipboard\n" +
                "    t       Typing text(default)\n" +
                "\n" +
                "    You can use multiple flags at the same time.\n" +
                "    e.g. `t`, `c` and `pct` are acceptable" +
                "\n");
        }

        public static void PrintInternalCommandHelp()
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
                "    e.g. `t`, `c` and `pct` are acceptable" +
                "\n" +
                "NOTE\n" +
                "    1. Commands must be preceded by a period.\n" +
                "    2. Any input that does not start with a period is understood as sending\n" +
                "       the entire sentence to the latest client (@p).\n" +
                "    3. If you want to send a message that starts with a period, use command \n" +
                "       `.send @p YOUR MESSAGE`\n");
        }


        public static ServerConfig ResolveShellArgument(string[] args)
        {
            ServerConfig rtn = new ServerConfig();

            ArgumentParser parser = new ArgumentParser(args);
            parser.AddDefault("--help", null);
            parser.AddDefault("--host", null);
            parser.AddDefault("--port", "7984");
            parser.AddDefault("--allow", null);
            parser.AddDefault("--flag", "pt");

            if (parser.Values["--help"] != null)
            {
                PrintShellCommandHelp();
                Environment.Exit(0);
                return rtn;
            }

            try
            {
                string StrHost = parser.Values["--host"];
                string StrPort = parser.Values["--port"];
                rtn.DefaultAllow = parser.Values["--allow"] != null;
                rtn.Flags = ResolveFlag(parser.Values["--flag"]);

                if (StrHost == null || IPAddress.TryParse(StrHost, out IPAddress Host) == false)
                    Host = Util.GetLocalNetworkIp();

                rtn.Host = Host;
                rtn.Port = int.Parse(StrPort);

            }
            catch
            {
                Console.WriteLine("Invalid argument error");
                Environment.Exit(1);
                return rtn;
            }

            return rtn;
        }

        public static bool ResolveInternalCommand(QuarterServer server, string raw)
        {
            //Console.WriteLine(">>" + (server == null));
            return false;
        }

        public static Dictionary<string, bool> ResolveFlag(string flag)
        {
            var flags = new Dictionary<string, bool>();

            bool Alpha(string a, string b)
            {
                return flags[a] = flag.Contains(b);
            }

            Alpha("typing", "t");
            Alpha("clipboard", "c");
            Alpha("print", "p");

            return flags;
        }
    }
}
