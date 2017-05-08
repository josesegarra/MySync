using System;
using System.Collections.Generic;
using System.Reflection;
using JSegarra.JSON;

namespace JSegarra.Serialize
{   
    class KeyValueEncoder : IEncoder
    {
        IEncoder kEncoder, vEncoder;
        MethodInfo kRead, vRead;
        Type tBase;
        KeyValueEncoder(Type t)
        {
            tBase = t;
            kRead = t.GetProperty("Key").GetGetMethod();                                                                    // Fetch the GetValue Method for the Key property    
            vRead = t.GetProperty("Value").GetGetMethod();                                                                  // Fetch the GetValue Method for the Value property
        }
        
        public static IEncoder Factory(Type t)                                                                              // Factory function for keyValue Serializers
        {
            if (!t.IsGenericType || t.GetGenericTypeDefinition() != typeof(KeyValuePair<,>)) return null;                   // This factory is for generic types matching KeyValuePair<,>
            return new KeyValueEncoder(t);                                                                                  // Return the KeyValueEncoder
        }
        public void Init(Serializer se)
        {
            Type[] arg = tBase.GetGenericArguments();                                                                       // Get the generic arguments
            kEncoder = se.GetEncoder(arg[0]);                                                                               // Get encoder for Key argument   
            vEncoder = se.GetEncoder(arg[1]);                                                                               // Get encoder for Value argument
        }
        public Json Encode(object o, IContext j)
        {
            Json p = new Json(Json.Array);                                                                                  // The keyValuePair will be encoded as a Json Array 
            p.Add(kEncoder.Encode(kRead.Invoke(o, null), j));                                                               // Encode the Key element
            object v=vRead.Invoke(o, null);                                                                                 // Get the value element
            p.Add(v != null ? vEncoder.Encode(v, j) : null);                                                                // Either add null or the encoded value
            return p;                                                                                                       // and return the Json node    
        }
    }    
}
