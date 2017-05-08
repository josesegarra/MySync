using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JSegarra.Core;
using JSegarra.JSON;


namespace JSegarra.Serialize
{
    class ValueDecoder:IDecoder
    {
        static IDecoder defDecoder=new ValueDecoder();

        public object Decode(Json j, Type wt,IContext c)                                                                // Deserializes an object    
        {
            if (j.Kind == JsonKind.Null) return null;

            string s=j.Value.AsString();
            if (wt==typeof(System.DateTime)) return DateTime.ParseExact(s,"yyyy-MM-dd HH:mm:ss.fff",null);

            return s;
        }

        public void Init(Serializer s)
        {
        
        }

        public static IDecoder JFactory(Json k,Type t)
        {
            if (t != null)
            {
                if (t.IsEnum) return defDecoder;
                if (t.IsPrimitive) return defDecoder;

            }
            if (k != null)
            {
                if (k.Kind == JsonKind.Boolean || k.Kind == JsonKind.String || k.Kind == JsonKind.Integer || k.Kind == JsonKind.Real) return defDecoder;

            }
            return null;
        }
    }
}
