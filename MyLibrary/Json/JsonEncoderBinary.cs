using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace JSegarra.JSON
{
    public static class JsonBinaryEncoder
    {
        public static byte[] ToBinary(Json item)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (StreamWriter wr = new StreamWriter(ms, Encoding.Unicode))
                {
                    ToStream(item, wr);
                    wr.Flush();
                    ms.Position = 0;
                    return ms.ToArray();
                }
            }
        }

        public static Json ToJson(byte[] buffer)
        {
            byte[] b=Encoding.Unicode.GetPreamble();
            string s = Encoding.Unicode.GetString(buffer, b.Length, buffer.Length - b.Length);
            Json j = Json.Parse(s);
            return j;
        }


        static long ToStream(Json item,StreamWriter ms)
        {
            if (item.Kind == Json.Null) return SimpleStringToStream(ms,"null");
            if (item.Kind == Json.Integer || item.Kind == Json.Real || item.Kind == Json.Boolean) return SimpleStringToStream(ms, item.Value.ToString());
            if (item.Kind == Json.Name)         return SimpleStringToStream(ms, "@" + item.Value.ToString());
            if (item.Kind == Json.String) return StringToStream(ms,(string)item.Value);
            if (item.Kind == Json.Object) return ObjectToStream(ms, (JsonMapper)item.Value);
            if (item.Kind == Json.Array) return ArrayToStream(ms, (JsonList)item.Value);
            if (item.Kind == Json.ByteArray) return ByteArrayToStream(ms, (byte[])item.Value);
            throw new Exception("ToStream() not implemented for kind " + item.Kind);
        }

        static long SimpleStringToStream(StreamWriter ms,string v)
        {
            long k = ms.BaseStream.Position;
            ms.Write(v);
            ms.Flush();
            return ms.BaseStream.Position - k;
        }


        static long ByteArrayToStream(StreamWriter ms,byte[] v)
        {
            long k = ms.BaseStream.Position; 
            ms.Write("*\""+ Convert.ToBase64String(v)+"\"");
            ms.Flush();
            return ms.BaseStream.Position - k;
        }

        static long StringToStream(StreamWriter ms,string v)
        {
            long k = ms.BaseStream.Position;
            char[] charArray = v.ToCharArray();
            ms.Write('"');
            int nc = 0;
            foreach (var c in charArray)
            {
                nc = nc + 1;
                switch (c)
                {
                    case '"': ms.Write("\\\""); break;
                    case '\\': ms.Write("\\\\"); break;
                    case '\b': ms.Write("\\b"); break;
                    case '\f': ms.Write("\\f"); break;
                    case '\n': ms.Write("\\n"); break;
                    case '\r': ms.Write("\\r"); break;
                    case '\t': ms.Write("\\t"); break;
                    default:
                        int codepoint = Convert.ToInt32(c);
                        if ((codepoint >= 32) && (codepoint <= 126)) ms.Write(c);
                        else ms.Write("\\u"+codepoint.ToString("x4"));
                        break;
                }
            }
            ms.Write('"');
            ms.Flush();
            return ms.BaseStream.Position - k;
        }

        static long ObjectToStream(StreamWriter ms, JsonMapper d)
        {
            long k = ms.BaseStream.Position; 
            ms.Write("{");
            string b = " ";
            foreach (KeyValuePair<string, Json> kv in d)
            {
                ms.Write(b);
                StringToStream(ms, kv.Key);
                ms.Write(" : ");
                ToStream(kv.Value, ms);
                b = ",";
            }
            /*
            foreach (KeyValuePair<string, Json> kv in d.links)
            {
                ms.Write(b);
                StringToStream(ms, "@"+kv.Key);
                ms.Write(" : ");
                ToStream(kv.Value, ms);
                b = ","; 
            }
            */
            ms.Write("}");
            ms.Flush();
            return ms.BaseStream.Position - k;
        }
        static long ArrayToStream(StreamWriter ms, JsonList d)
        {
            long k = ms.BaseStream.Position;
            ms.Write("[");
            string b = " ";
            foreach (Json kv in d)
            {
                ms.Write(b);
                ToStream(kv,ms);
                b = ",";
            }
            ms.Write("]\n");
            ms.Flush();
            return ms.BaseStream.Position - k;
        }
    }
}
