using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JSegarra.Core;
using JSegarra.JSON;

namespace JSegarra.Serialize
{
    class KeyValueDecoder : IDecoder
    {
        Type BaseType;
        KeyValueDecoder(Type t)
        {
            //Logger.Red("Creating KeyValue decoder for type " + t.Name); ;
            BaseType = t;
        }
        public object Decode(Json j, Type wt, IContext c)                                                                // Deserializes an object    
        {
            if (j.Kind == JsonKind.Null) return null;

            //Logger.Red("-----------------------------------------------------");
            //Logger.Red("Executing KeyValue decoder for type " + wt.Name); ;

            return "NOPE";
        }

        public void Init(Serializer s)
        {
            //Logger.Red("Init KeyValue decoder for type " + BaseType.Name); ;

        }

        public static IDecoder JFactory(Json k, Type t)
        {
            if (t == null) return null;
            if (!t.IsGenericType || t.GetGenericTypeDefinition() != typeof(KeyValuePair<,>))    return null;                   // This factory is for generic types matching KeyValuePair<,>
            //Logger.Yellow(t.Name + " YES");
            return new KeyValueDecoder(t);                                                                                  // Return the KeyValueEncoder
        }
    }
}
