using QuarterShare.Command;
using QuarterShare.Connection;
using QuarterShare.Worker;
using System;
using System.Text;

namespace QuarterShare
{
    class Program
    {
        private static QuarterServer UsingServer;

        public static void Main(string[] args)
        {
            ServerConfig config = UserInput.ResolveShellArgument(args);

            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("Press any key to start the server");
            Console.ReadKey();
            Console.Clear();
            Console.SetCursorPosition(0, 0);

            ServerWorker SThread = new ServerWorker(config);
            UsingServer = SThread.Server;

            while (true) UserInput.ResolveInternalCommand(UsingServer, Console.ReadLine());
        }

        public static void Close(int exitCode = 0)
        {
            if (UsingServer != null)
                UsingServer.Close();
            Environment.Exit(exitCode); // exit, close all thread
        }
    }
}
