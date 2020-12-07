using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuarterShare.Worker
{
    abstract class ConsoleUser
    {
        protected static void Color(ConsoleColor color, object o = null)
        {
            Console.ForegroundColor = color;
            Console.Write(o);
            Console.ForegroundColor = ConsoleColor.White;
        }

        protected static void Red(object o = null)
        {
            Color(ConsoleColor.DarkRed, o);
        }

        protected static void Green(object o = null)
        {
            Color(ConsoleColor.Green, o);
        }

        protected static void White(object o = null)
        {
            Color(ConsoleColor.White, o);
        }

        protected static void Blue(object o = null)
        {
            Color(ConsoleColor.Blue, o);
        }
    }
}
