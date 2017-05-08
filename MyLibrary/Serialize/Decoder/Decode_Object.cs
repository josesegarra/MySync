using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JSegarra.Core;
using JSegarra.JSON;

namespace JSegarra.Serialize
{

    class ObjectDecoder:IDecoder
    {
      
        Type BaseType;
        List<CoDec> members = null;
        

        internal ObjectDecoder(Type t)
        {
            BaseType = t;
        }
                                                                                                          // Inits the Type Serializer 

        public void Init(Serializer s)
        {
            Logger.Green("Initializing Object deserializer for " + BaseType.Name);
            members = CoDec.InitFromType(BaseType, s);
            Logger.Green("OK");
        }


        public object Decode(Json j, Type wt, IContext c)                                                                       // Decodes a JSON object into a C# object   
        {
            Logger.Green("Decoding instance of " + BaseType.Name);
            if (j.Kind == JsonKind.Null) return null;

            object n = Activator.CreateInstance(BaseType);
            foreach (CoDec m in members)
            {
                Json v = j.Items[m.Name];
                if (v != null) 
                {
                    object k = m.Decoder.Decode(v, m.MemberType, c);
                    if (k != null)
                    {
                        m.SetValue(n, k);
                        //Logger.Green(m.MemberType.Name+":  "+m.Name + " = " + k.ToString());
                    }
                }
            }
            return n;            
        }
    }
}
