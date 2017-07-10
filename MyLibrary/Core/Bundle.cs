using System;
using System.Reflection;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;

namespace JSegarra.Core
{
    enum Kind { Assembly, GacAssembly, File };

    public class Bundle
    {
        // https://stackoverflow.com/questions/15079397/maximum-default-post-request-size-of-iis-7-how-to-increase-64kb-65kb-limit
        // https://stackoverflow.com/questions/2969175/how-to-find-size-limits-for-post-on-iis-with-asp-net?rq=1
        // https://stackoverflow.com/questions/10496478/how-to-save-managed-net-assembly-from-memory-as-exe-dll-file


        const int chunkSize = 1*1024;                             // 32K chunks. InDEV 1K chunks
        internal List<byte[]> Chunks=new List<byte[]>();


        public Bundle()
        {
            try
            {
                using (var ms = new MemoryStream())
                {
                    using (var archive = new ZipArchive(ms, ZipArchiveMode.Create, true))
                    {
                        foreach (Assembly asm in Assemblies.GetAssemblies())
                        {
                            AssemblyName aname = asm.GetName();
                            byte[] asmBytes = File.Exists(asm.Location) ? File.ReadAllBytes(asm.Location) : null;
                            if (asmBytes == null) throw new Exception("Dynamic ASSEMBLIES not yet implemented");
                            var zipArchiveTxt = archive.CreateEntry(aname.Name + ".txt", CompressionLevel.Fastest);
                            using (var entryStream = zipArchiveTxt.Open())
                            using (var streamWriter = new StreamWriter(entryStream))
                            {
                                streamWriter.Write(aname.FullName);
                                streamWriter.Close();
                            }

                            var zipArchiveDll = archive.CreateEntry(aname.Name + ".dll", CompressionLevel.Fastest);
                            using (var entryStream = zipArchiveDll.Open())
                            using (var binaryWriter = new BinaryWriter(entryStream))
                            {
                                binaryWriter.Write(asmBytes);
                                binaryWriter.Close();
                            }
                        }
                    }

                    ms.Seek(0, SeekOrigin.Begin);
                    int count= 0;
                    do
                    {
                        byte[] buffer = new byte[chunkSize];
                        count = ms.Read(buffer, 0, chunkSize);
                        Logger.Red("Offset " + count);
                        if (count > 0) Chunks.Add(buffer);
                    } while (count > 0);
                    Logger.Green(ms.Length + " have been written in " + Chunks.Count + " chunks");
                    //using (FileStream fileStream = new FileStream(@"C:\temp\1.zip", FileMode.Create, System.IO.FileAccess.Write)) ms.WriteTo(fileStream);
                }
            }
            catch(Exception e)
            {
                Logger.Red(e.Message);
            }
        }
    }
}



    /*

    Assembly yourAssembly;
var formatter = new BinaryFormatter();
var ms = new MemoryStream();
formatter.Serialize(ms, yourAssembly);
var reloadedAssembly = Assembly.Load(ms.GetBuffer()); 

    https://stackoverflow.com/questions/20079843/loading-net-assembly-from-memory-instead-of-disk

internal class AssemblyResolver
{
public static void Register()
{
AppDomain.CurrentDomain.AssemblyResolve +=
  (sender, args) =>
  {
    var an = new AssemblyName(args.Name);
    if (an.Name == "YourAssembly")
    {
      string resourcepath = "YourNamespace.YourAssembly.dll";
      Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourcepath);
      if (stream != null)
      {
        using (stream)
        {
          byte[] data = new byte[stream.Length];
          stream.Read(data, 0, data.Length);
          return Assembly.Load(data);
        }
      }
    }
    return null;
  }
}
}

    public static void Main()
{
  // Do not use any types from the dependent assembly yet.

  AssemblyResolver.Register();

  // Now you can use types from the dependent assembly!
}



    internal string Name = "";
    internal string ExtName = "";
    internal string Input = "";
    internal string Output = "";
    internal Kind Kind = Kind.Assembly;



    bool IsGac(string name)
    {
        Name = name;
        if (Gac.GetAssemblyPath(name) == "") return false;
        ExtName = Name;
        Kind = Kind.GacAssembly;
        return true;
    }

    internal Bundle(Assembly asm)
    {
        if (IsGac(asm.GetName().Name)) return;
        ExtName = asm.FullName;
        Input = asm.Location;
        Output = Path.GetFileName(Input);
    }

    internal Bundle(AssemblyName asm)
    {
        if (IsGac(asm.Name)) return;
        Assembly a = Assembly.LoadFrom(Name + ".dll");
        ExtName = a.FullName;
        Input = a.Location;
        Output = Path.GetFileName(Input);
    }

    internal Bundle(string s)
    {
        Kind = Kind.File;
        Input = Path.GetFullPath(s);
        if (!File.Exists(Input)) throw new Exception("File not found [" + s + "]");
        Name = Path.GetFileName(s);
        ExtName = Name;
        string FilePath = Path.GetDirectoryName(Input) + "\\";
        string RootPath = Path.GetDirectoryName(Path.GetFullPath(Assembly.GetEntryAssembly().Location)) + "\\";
        if (FilePath.Length < RootPath.Length)
        {
            Output = Name;
            return;
        }
        if (Input.Substring(0, RootPath.Length).ToLower() == RootPath.ToLower())
        {
            Output = Input.Substring(RootPath.Length);
            return;
        }
        Output = Name;
    }

    internal void WriteToStream(Stream a)
    {
        using (FileStream file = new FileStream(Input, FileMode.Open, FileAccess.Read, FileShare.Read))                           // Open the input file as a stream
        {
            file.CopyTo(a);                                                                                                     // Write input file to the received stream
        }
    }

    internal long Size()
    {
        if (Input == "") return 0;
        FileInfo oFileInfo = new FileInfo(Input);
        return oFileInfo.Length;
    }
    */
