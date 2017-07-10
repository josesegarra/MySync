using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using JSegarra.Remote;
using System.IO;
using System.Xml;

namespace MyDemo
{


    public interface ITestEmbeded
    {
        string Demo();
    }

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


        public static byte[] ReadFully(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }

        static ITestEmbeded DoIoc()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "MyDemo.Embeded.dll";
            byte[] bytes;
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                bytes = ReadFully(stream);
            }
            Assembly easm = Assembly.Load(bytes);
            foreach(Type t in easm.GetTypes())
            {
                if (t.GetInterfaces().Contains(typeof(ITestEmbeded)))
                {
                    return (ITestEmbeded)Activator.CreateInstance(t);
                }
            }
            return null;
        }




        const string host = "http://localhost:8080/api/Remote/Action";
        //const string host = "http://app-72.apphb.com/api/Remote/Action";

        static void Main(string[] args)
        {
            //ITestEmbeded doi = DoIoc();
            //if (doi != null) Console.WriteLine("Embedded: " + doi.Demo());


            Deployment deploy = Remoting.Deploy(host, "jose", "pass1");

            //connection.Execute(CheckDrives.GetType());


            //if (File.Exists(@"C:\pepe.txt")) Console.WriteLine("SI"); else Console.WriteLine("NOPE");
            //deploy.Execute(() => Add(1,2));
            //deploy.Execute(() => CheckDrives());

        }
    }
}
