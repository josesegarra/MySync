using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JSegarra.Core;
using JSegarra.JSON;

namespace JSegarra.Serialize
{

    class UnTypedObject:IDecoder
    {

        static UnTypedObject defDecoder = new UnTypedObject();

        public void Init(Serializer s)
        {
        }


        public object Decode(Json j, Type wt, IContext c)                                                                       // Decodes a JSON object into a C# object   
        {
            Serializer s = c.GetSerializer();                                                                                   // Get the serializer
            Json j0 = null;
            do { j0=j;j=s.HandleLinks(j);} while (j0!=j);                                                                       // Follow all the links while they exist (for multiple assignments=
            if (j == null || j.Kind == JsonKind.Null) return null;                                                              // If null then return    
            Type t = Utils.FindType(j["#type"].AsString());                                                                     // Look for a #type node in the object
            if (t == null) throw new Exception("Bad type in #type value: " + j.ToString());                                     // Which must exist always
            IDecoder d=s.GetDecoder(t);
            return d.Decode(j, wt, c);
        }

        public static IDecoder JFactory(Json k,Type t)
        {
            if (k!=null && k.Kind == JsonKind.Object) return defDecoder;
            if (t != null)
            {
                if (t.IsEnum) return null;
                if (t.IsPrimitive) return null;
                return new ObjectDecoder(t);
            }
            return null;
        }

    }
}
