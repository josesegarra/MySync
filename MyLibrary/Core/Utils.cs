using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JSegarra.Core
{
    public class Utils
    {
        static int nNames = 0;
        static object locker = new Object();
        public static long CombineHighLowInts(uint high, uint low)
        {
            return (((long)high) << 0x20) | low;
        }
        public static DateTime ToDateTime(uint high, uint low)
        {
            long fileTime = CombineHighLowInts(high, low);
            return DateTime.FromFileTimeUtc(fileTime);
        }

        public static string NewName()
        {
            lock(locker)
            {
                nNames = nNames + 1;
                return DateTime.Now.ToString("yyyyMMdd_HHmmss") + "_" + nNames;
            }
        }

        public static Type FindType(string name)
        {
            if (name == "") return null;
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                foreach (Type type in assembly.GetTypes())
                    if (type.FullName == name) return type;
            return null;
        }
    }
}
