using QuarterShare.Command;
using QuarterShare.Connection;
using QuarterShare.Worker;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace QuarterShare
{
    class Program
    {
        public static bool DEFAULT_ALLOW = false;
        public static Dictionary<string, bool> DEFAULT_FLAGS = new Dictionary<string, bool>();
        public static IPAddress UsingHost;
        public static int UsingPort;


        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("Press any key to start the server");
            Console.ReadKey();
            Console.SetCursorPosition(0, 0);



            UserInput.ResolveShellArgument(args);

            ServerWorker SThread = new ServerWorker(UsingHost, UsingPort);
            QuarterServer UsingServer = SThread.Server;

            while (UserInput.ResolveInternalCommand(UsingServer, Console.ReadLine())) ;

            UsingServer.Close();
            Environment.Exit(0); // exit, close all thread
        }

    }
}
