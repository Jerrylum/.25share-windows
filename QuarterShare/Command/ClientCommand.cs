using QuarterShare.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuarterShare.Command
{
    abstract class ClientCommand : Command
    {
        public ClientCommand(QuarterServer server, string raw) : base(server, raw) { }

        public abstract bool Handle(QuarterClient[] clients);

        public override bool Handle()
        {
            string selector = Raw.Split(' ')[1];
            QuarterClient[] targets;

            if (selector == "@a")
                targets = Server.Clients.ToArray();
            else if (selector == "@p")
                targets = Server.LatestClient != null ? new [] { Server.LatestClient } : null;
            else
                targets = Server.Clients.FindAll((QuarterClient client) => client.Id == selector).ToArray();


            if (targets == null || targets.Length == 0)
            {
                Red("Client not found\n\n");
                return false;
            }

            return Handle(targets);
        }
    }

    class AllowClientCommand : ClientCommand
    {
        public AllowClientCommand(QuarterServer server, string raw) : base(server, raw) { }

        public override bool IsMatch() => Split[0] == ("allow");

        public override bool Handle(QuarterClient[] clients)
        {
            foreach (QuarterClient c in clients)
            {
                c.Allowed = true;
                Green("Accept client #" + c.Id + "\n\n");
            }
            return true;
        }
    }

    class KickClientCommand : ClientCommand
    {
        public KickClientCommand(QuarterServer server, string raw) : base(server, raw) { }

        public override bool IsMatch() => Split[0] == "kick";

        public override bool Handle(QuarterClient[] clients)
        {
            foreach (QuarterClient c in clients)
            {
                c.Close();
                Green("Kicked #" + c.Id + "\n\n");
            }
            return true;
        }
    }

    class SendToClientCommand : ClientCommand
    {
        public SendToClientCommand(QuarterServer server, string raw) : base(server, raw) { }

        public override bool IsMatch() => Split[0] == "send";

        public override bool Handle(QuarterClient[] clients)
        {
            int secondSpaceIndex = Raw.Substring(5).IndexOf(" ");
            string msg = Raw.Substring(6 + secondSpaceIndex);
            byte[] send = Encoding.UTF8.GetBytes(msg);

            foreach (QuarterClient c in clients)
            {
                c.SendMessage(ToClientFlag.COPY_FLAG.Concat(send).ToArray());
                Green("Send to #" + c.Id + "\n\n");
            }
            return true;
        }
    }
}
