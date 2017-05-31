using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JSegarra.Remote;
using System.IO;
using System.Xml;

namespace MyDemo
{
    public class Program
    {

        public static XmlDocument Test()
        {
            return new XmlDocument();
        }

        public static void CheckDrives()
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();

            foreach (DriveInfo d in allDrives)
            {
                var s = "Drive " + d.Name + "  type: " + d.DriveType;
                if (!d.IsReady) s = s + " not ready";
                else s = s + " " + d.DriveFormat + " available " + d.TotalFreeSpace + " / " + d.TotalSize;
                Console.WriteLine(s);
            }
        }

        public static void Add(int a,int b)
        {
            Console.Write(a + " + " + b + "=" + (a + b));
        }


        static void Main(string[] args)
        {
            Deployment deploy= Remoting.Deploy("http://localhost:3019/register","userid","password");

            //connection.Execute(CheckDrives.GetType());


            if (File.Exists(@"C:\pepe.txt")) Console.WriteLine("SI"); else Console.WriteLine("NOPE");
            deploy.Execute(() => Add(1,2));
            deploy.Execute(() => CheckDrives());

        }
    }
}
