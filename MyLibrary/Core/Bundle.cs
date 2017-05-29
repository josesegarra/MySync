using System;
using System.Reflection;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSegarra.Core
{
    enum Kind { Assembly, GacAssembly, File };

    internal class Bundle
    {
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
    }
}
