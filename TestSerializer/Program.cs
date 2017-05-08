using System;
using System.IO;
using JSegarra.Serialize;
using Music;

namespace TestSerializer
{
    class Program
    {
        static void Main(string[] args)
        {
            Band TheBand = Bands.TheCure();
            string BandFile=Path.GetDirectoryName(typeof(Program).Assembly.Location) + Path.DirectorySeparatorChar+ TheBand.Name.Replace(" ","_")+".txt";
            Console.WriteLine("Serializing to " + BandFile);
            Serializer ser = new Serializer();                                                      // Create a serializer
            File.WriteAllBytes(BandFile, ser.Encode(TheBand));                                           // Write the serialized bytes to disk
            Console.WriteLine("Done !!!");


        }
    }
}
