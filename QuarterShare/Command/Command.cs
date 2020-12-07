using QuarterShare.Connection;
using QuarterShare.Worker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuarterShare.Command
{
    abstract class Command : ConsoleUser
    {
        public QuarterServer Server;
        public string Raw;
        public string[] Split;

        public abstract bool IsMatch();
        public abstract bool Handle();

        public Command(QuarterServer server, string raw)
        {
            Server = server;
            Raw = raw;
            Split = raw.Split(' ');
        }
    }
}
