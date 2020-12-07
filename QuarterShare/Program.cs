using QuarterShare.Command;
using QuarterShare.Connection;
using QuarterShare.Worker;
using System;
using System.Text;

namespace QuarterShare
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("Press any key to start the server");
            Console.ReadKey();
            Console.SetCursorPosition(0, 0);



            ServerConfig config = UserInput.ResolveShellArgument(args);

            ServerWorker SThread = new ServerWorker(config);
            QuarterServer UsingServer = SThread.Server;

            while (UserInput.ResolveInternalCommand(UsingServer, Console.ReadLine())) ;

            UsingServer.Close();
            Environment.Exit(0); // exit, close all thread
        }

    }
}
