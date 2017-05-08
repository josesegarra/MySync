using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JSegarra.JSON;

namespace JSegarra.Core
{
    public static class Logger
    {
        static int dLevel = 0;
        public static void Error(string s)
        {
            ConsoleColor c=Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(s);
            Console.ForegroundColor = c;
        }

        public static void Green(string s)
        {
            ConsoleColor c = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(s);
            Console.ForegroundColor = c;
        }

        public static void Grey(string s)
        {
            ConsoleColor c = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(s);
            Console.ForegroundColor = c;
        }

        public static void Red(string s)
        {
            ConsoleColor c = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(s);
            Console.ForegroundColor = c;
        }

        public static void Yellow(string s)
        {
            ConsoleColor c = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(s);
            Console.ForegroundColor = c;
        }
        public static void DebugPrint(string s)
        {
            string r = new String(' ', dLevel);
            ConsoleColor c = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(r+" "+s);
            Console.ForegroundColor = c;
        }
        public static void DebugEnter(string s)
        {
            DebugPrint(s);
            dLevel = dLevel + 3;
        }

        public static void DebugExit(string s="")
        {
            dLevel = dLevel - 3;
            if (dLevel < 0) dLevel = 0;
            if (s != "") DebugPrint(s);
        }

        public static void Print(string s)
        {
            ConsoleColor c = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(s);
            Console.ForegroundColor = c;
        }

        public static void Print(string title,Json j)
        {
            if (title != null) Print(title);
            foreach(string p in j.GetProperties())
            {
                string s0=p.PadLeft(30).Substring(0,30)+": ";
                Json j1 = j.Items[p];

                string s1=j1.Kind.ToString().PadRight(15)+" "+j1.ToString().PadRight(100).Substring(0,100);
                Print(s0+s1);
            }
        }
    }
}
