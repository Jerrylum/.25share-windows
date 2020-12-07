using QuarterShare.Connection;
using QuarterShare.Worker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace QuarterShare.Command
{
    class UserInput : ConsoleUser
    {

        public static void PrintShellCommandHelp()
        {
            Console.WriteLine(
                "usage: .25share [--help] [--host:HOST] [--port:PORT] [--allow] [--flag:FLAG]\n" +
                "\n" +
                "FLAG\n" +
                "    p       Print on the console(default)\n" +
                "    c       Copy to clipboard\n" +
                "    t       Typing text(default)\n" +
                "\n" +
                "    You can use multiple flags at the same time.\n" +
                "    e.g. `t`, `c` and `pct` are acceptable" +
                "\n" +
                "\n" +
                "optional arguments:\n" +
                "  --help         Show this help message and exit\n" +
                "  --host HOST    The server's hostname or IP address\n" +
                "  --port PORT    The port to listen on\n" +
                "  --allow        Allow all clients to send messages to the server\n" +
                "  --flag FLAG    Mode flag\n" +
                "\n");
        }

        public static void PrintInternalCommandHelp()
        {
            new ShowHelpCommand(null, "").Handle();
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
                Program.Close();
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
                Red("Invalid argument error\n");
                Program.Close(1);
                return rtn;
            }

            return rtn;
        }

        public static void ResolveInternalCommand(QuarterServer server, string raw)
        {
            Command TheCommand = null;

            if (raw.StartsWith("."))
            {
                raw = raw.Substring(1);

                Command[] AllCommands = {
                    new ShowHelpCommand(server, raw),
                    new ShowFlagCommand(server, raw),
                    new ChangeFlagCommand(server, raw),
                    new ListClientCommand(server, raw),
                    new StopServerCommand(server, raw),
                    new AllowClientCommand(server, raw),
                    new KickClientCommand(server, raw),
                    new SendToClientCommand(server, raw)
                };

                foreach (Command c in AllCommands)
                {
                    if (c.IsMatch())
                    {
                        TheCommand = c;
                        break;
                    }
                }
            }
            else
            {
                TheCommand = new SendToClientCommand(server, "send @p " + raw);
            }

            if (TheCommand == null)
            {
                Red("Unknown command\n");
                PrintInternalCommandHelp();
                return;
            }

            try
            {
                TheCommand.Handle();
            }
            catch
            {
                Red("Command exception\n");
                PrintInternalCommandHelp();
            }
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
