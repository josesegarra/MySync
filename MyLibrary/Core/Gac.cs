using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace JSegarra.Core
{
        internal class Gac
        {

            public static string GetAssemblyPath(string name)
            {
                if (name == null) throw new ArgumentNullException("name");
                string finalName = name;
                AssemblyInfo aInfo = new AssemblyInfo();
                aInfo.cchBuf = 1024; // should be fine...
                aInfo.currentAssemblyPath = new String('\0', aInfo.cchBuf);
                IAssemblyCache ac;
                int hr = CreateAssemblyCache(out ac, 0);
                if (hr >= 0)
                {
                    hr = ac.QueryAssemblyInfo(0, finalName, ref aInfo);
                    if (hr < 0) return "";
                }
                return aInfo.currentAssemblyPath.Trim();
            }


            [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("e707dcde-d1cd-11d2-bab9-00c04f8eceae")]
            private interface IAssemblyCache
            {
                void Reserved0();

                [PreserveSig]
                int QueryAssemblyInfo(int flags, [MarshalAs(UnmanagedType.LPWStr)] string assemblyName, ref AssemblyInfo assemblyInfo);
            }

            [StructLayout(LayoutKind.Sequential)]
            private struct AssemblyInfo
            {
                public int cbAssemblyInfo;
                public int assemblyFlags;
                public long assemblySizeInKB;
                [MarshalAs(UnmanagedType.LPWStr)]
                public string currentAssemblyPath;
                public int cchBuf; // size of path buf.
            }

            [DllImport("fusion.dll")]
            private static extern int CreateAssemblyCache(out IAssemblyCache ppAsmCache, int reserved);


            bool isMsCoreLib(string n)
            {
                // At compile time MSCORLIB.DLL is loaded from the framework
                //      C:\WINDOWS\MICROSOFT.NET\FRAMEWORK\V4.0.30319\MSCORLIB.DLL
                // At runtime when loading the assembly in the APP domain, MSCORLIB.DLL is referenced from the GAC
                //      C:\WINDOWS\MICROSOFT.NET\ASSEMBLY\GAC_32\MSCORLIB\V4.0_4.0.0.0__B77A5C561934E089\MSCORLIB.DLL
                // So the path comparasion between compile & runtime for MSCORLIB will will fail
                return (n.ToUpper().Right(12) == "MSCORLIB.DLL");

            }
        }
}
