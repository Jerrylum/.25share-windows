using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace QuarterShare.Connection
{
    struct ServerConfig
    {
        public bool DefaultAllow;
        public Dictionary<string, bool> Flags;
        public IPAddress Host;
        public int Port;
    };
}
