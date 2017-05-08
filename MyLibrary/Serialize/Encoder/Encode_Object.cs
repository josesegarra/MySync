using System;
using System.Collections.Generic;
using System.Reflection;
using JSegarra.JSON;
using JSegarra.Core;

namespace JSegarra.Serialize
{  
    class ObjectEncoder : IEncoder
    {   
        List<CoDec> members = null;                                                          // Serializators for the members of T    
        Type BaseType;
        
        ObjectEncoder(Type t)
        {
            BaseType = t;
        }

        public void Init(Serializer ser)                                                                                // This builds the list of serializable items (ie: properties) for the received Type t
        {
            Logger.Green("Initializing Object serializer for " + BaseType.Name );
            members=CoDec.InitFromType(BaseType,ser);
        }
  
        public Json Encode(object o, IContext j)
        {
            Reference r = j.GetMappedReference(o);                                                                      // Get a reference to object o
            if (r.TheJSon == null)                                                                                      // If reference was uncomplete, let's complete it
            {
                r.TheJSon = new Json(Json.Object);                                                                      // Create an Json Array in r.TheJson
                r.TheJSon["#type"] = o.GetType().FullName;                                                              // Store a special field with the type
                foreach (var x in members)
                {
                    object value = x.GetValue(o);                                                                       // Get the value    
                    r.TheJSon[x.Name] = value!=null ? x.Encoder.Encode(value, j):null;                                  // Serialize every intem from the list of serializable members
                }
            }
            return new Json(new JsonName(r.Id));                                                                        // Return a name to the referenced JSON
        }
        
        public static IEncoder Factory(Type t)                                                                          // Factory function for Object serializers
        {
            if (t == typeof(System.String)) return null;                                                                // Although Strings are objects, they have their specific Serializer
            if (t.IsValueType)                                                                                          // Structs are ValueTypes, that are not Primitive, neither Enum
            {
                if (t.IsPrimitive) return null;
                if (t.IsEnum) return null;
            }   
            return new ObjectEncoder(t);
        }
    }
}
