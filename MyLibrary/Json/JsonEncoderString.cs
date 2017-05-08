using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * //foreach (FieldInfo f in j.GetType().GetFields()) Console.WriteLine("Field: " + f.Name);
    byte[] s = Serializer.Encode(j);
File.WriteAllBytes("VALUE.BIN", s);
Pepelo p = new Pepelo();
PropertyInfo[] pr=p.GetType().GetProperties(BindingFlags.Default);
Console.WriteLine("Properties found " + pr.Length);
File.WriteAllBytes("PEPELO.BIN", Serializer.Encode(p));
*/



namespace JSegarra.JSON
{
    public static class JsonStringEncoder
    {
        static readonly string Margin = new String(' ', 2);

        public static string ToString(Json item)
        {
            return ToString(item, 0);
        }
        static string ToString(Json item,int index)
        {
            string s = new String(' ', index);
            if (item.Kind == Json.Null) return s + "null";
            if (item.Kind == Json.ByteArray) return s + "(BYTE_ARRAY: " + ((byte[])item.Value).Length + ")";
            if (item.Kind == Json.Name) return s + "@"+(string)item.Value;
            if (item.Kind == Json.Integer || item.Kind == Json.Real || item.Kind == Json.Boolean || item.Kind == Json.String)
                return s + (item.Kind == Json.String ? "\"" : "") + item.Value.ToString() + (item.Kind == Json.String ? "\"" : "");
            if (item.Kind == Json.Object) return ObjectToString(s, index, (JsonMapper)item.Value);
            if (item.Kind == Json.Array) return ArrayToString(s, index, (JsonList)item.Value);
            throw new Exception("ToString() not implemented for kind " + item.Kind);
        }

        static string ObjectToString(string s, int index, JsonMapper d)
        {
            StringBuilder sb = new StringBuilder("{\n");
            char b = ' ';
            foreach (KeyValuePair<string, Json> kv in d)
            {
                sb.Append(s + Margin + b + kv.Key + " : ");
                if (!kv.Value.IsComplex()) sb.Append(kv.Value.ToString() + "\n");
                else sb.Append(ToString(kv.Value,index + Margin.Length + 1));
                b = ',';
            }
            /*foreach (KeyValuePair<string, Json> kv in d.links)
            {
                sb.Append(s + Margin + b + "@"+kv.Key + " : ");
                if (!kv.Value.IsComplex()) sb.Append(kv.Value.ToString() + "\n");
                else sb.Append(ToString(kv.Value, index + Margin.Length + 1));
                b = ',';
            }*/
            sb.Append(s + "}\n");
            return sb.ToString();
        }

        static string ArrayToString(string s, int index, JsonList d)
        {
            StringBuilder sb = new StringBuilder();
            char b = ' ';
            foreach (Json kv in d)
            {
                sb.Append(s + Margin + b);
                if (!kv.IsComplex()) sb.Append(kv.ToString() + "\n"); else sb.Append(ToString(kv,index + Margin.Length + 1));
                b = ',';
            }
            sb.Append(s );
            string r = sb.ToString();
            
            string[] k0 = r.Split('"');
            for(int i=0;i<k0.Length;i++)
            {
                if (i % 2 == 0) 
                {
                    while (k0[i].IndexOf("  ") > 0) k0[i] = k0[i].Replace("  ", " ");
                }
                k0[i] = k0[i].Replace("\n", "").Trim();
            }
            string r2 = String.Join("\"", k0);
            return r2.Length < 100 ? "["+r2 +"]\n": "[\n"+r+"]\n";
        }


    }
}
