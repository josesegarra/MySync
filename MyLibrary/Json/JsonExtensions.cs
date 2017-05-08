using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSegarra.JSON
{


    public static class JsonExtensions
    {
        public static string ToString(this Json value, string OnNotFound = null)
        {
            if (value == null)
            {
                if (OnNotFound == null) throw new Exception("Cannot convert NULL json to String");
                return OnNotFound;
            }
            return JsonStringEncoder.ToString(value);
        }

        public static bool ToBool(this Json j)
        {
            if (j == null) return false;
            if (j.Kind == Json.Boolean) return (bool)j.Value;
            if (j.Kind == Json.Integer) return (int)j.Value != 0;
            if (j.Kind == Json.Real) return (double)j.Value != 0;
            if (j.Kind == Json.String) return (j.ToString() != "");
            if (j.Kind == Json.Null) return false;
            throw new Exception("Cannot convert from JSON." + j.Kind + " to BOOLEAN");
        }

   
        public static byte[] ToByteArray(this Json j)
        {
            if (j == null) return null;
            if (j.Kind == Json.Null) return null;
            if (j.Kind == Json.ByteArray) return (byte[])j.Value;
            throw new Exception("Cannot convert from JSON." + j.Kind + " to BYTE[]");
        }
    }
}
