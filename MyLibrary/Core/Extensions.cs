using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSegarra.Core
{
    public static class Extensions
    {



        public static string Right(this string str, int length)
        {
            if (str == null) return String.Empty;
            if (length < str.Length) return str.Substring(str.Length - length);
            return str;
        }

        public static string Left(this string str, int length,bool trail=false)
        {
            if (str == null) return String.Empty;
            if (length < str.Length) return str.Substring(0,length)+(trail?"...":"");
            return str;
        }


        public static int NumberPrintableChars(this string str)
        {
            int nc = 0;
            for (int i = 0; i < str.Length; i++) if (str[i] >= ' ') nc++;
            return nc;
        }

        public static string BigTrim(this string str)
        {
            string s = "";
            for (int i = 0; i < str.Length; i++) if (str[i] >= ' ') s = s + str[i]; else s = s + " ";
            while (s.IndexOf("  ") > -1) s = s.Replace("  ", " ");
            return s;
        }
        
        public static string AsString(this object str)
        {
            if (str == null) return ""; else return str.ToString();
        }

        public static T2 AddV<T1,T2>(this Dictionary<T1,T2> dic,T1 key,T2 value)
        {
            dic.Add(key, value);
            return value;
        }

        public static T AddV<T>(this List<T> list, T value)
        {
            if (value == null) return default(T);
            list.Add(value);
            return value;
        }

        public static bool AddN<T>(this List<T> list, T value)
        {
            if (value == null) return false;
            list.Add(value);
            return true;
        }

       

    }
}
