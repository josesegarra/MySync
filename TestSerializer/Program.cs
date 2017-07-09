using System;
using System.IO;
using JSegarra.Serialize;
using JSegarra.Remote;
using Music;


namespace TestSerializer
{
    class Program
    {

        //const string host = "https://localhost:44300/api/Remote/Action";
        const string host = "http://localhost:8080/api/Remote/Action";

        static int CountRecords(Band TheBand,DateTime y1,DateTime y2)
        {
            var i = 0;
            foreach(var x in TheBand.Records)
            {
                if (y1 <= x.Released && x.Released <= y2) i++;
            }
            Console.WriteLine(TheBand.Name.PadRight(15) + i.ToString().PadLeft(3)+" records between " + y1.ToString("yyyy.MM.dd") + " and " + y2.ToString("yyyy.MM.dd"));
            return i;
        }



        static void Main(string[] args)
        {
            Band TheCure = Bands.TheCure();
            var d1 = new DateTime(1980, 1, 1);
            var d2 = new DateTime(1985, 1, 1);
            CountRecords(TheCure, d1, d2);


            Deployment deploy = Remoting.Deploy(host, "jose", "pass1");



            /*
            string BandFile=Path.GetDirectoryName(typeof(Program).Assembly.Location) + Path.DirectorySeparatorChar+ TheBand.Name.Replace(" ","_")+".txt";
            Console.WriteLine("Serializing to " + BandFile);
            Serializer ser = new Serializer();                                                      // Create a serializer
            File.WriteAllBytes(BandFile, ser.Encode(TheBand));                                           // Write the serialized bytes to disk
            Console.WriteLine("Done !!!");
            */

        }
    }
}
