using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using JSegarra.JSON;

namespace JSegarra.Serialize
{
    class EnumerableEncoder: IEncoder
    {
        IEncoder ser0 = null;
        Type tBase = null;

        EnumerableEncoder(Type t)
        {
            tBase = t.IsArray ? t.GetElementType() : GetEnumeratorType(t);                                              // We know that T is enumerable, so it must implement GetEnumerator & Current
        }
        public static IEncoder Factory(Type t)                                                                          // Factory function for Array Serializers
        {
            if (t.GetInterfaces().Contains(typeof(IEnumerable))) return new EnumerableEncoder(t);                        // This factory is for types that implement IEnumerable
            return null;
        }

        Type GetEnumeratorType(Type t)
        {
            return t.GetMethod("GetEnumerator").ReturnType.GetProperty("Current").PropertyType;                         // Use reflection to get the enumerator type
        }
        public void Init(Serializer serializer)                                                                  // Init the array serializer
        {
            ser0 = serializer.GetEncoder(tBase);                                                                        // Fetch a serializer for the referenced type     
        }

        public Json Encode(object o, IContext j)                                                                        // Encode object O, using context J
        {
            Json TheList = null , TheResult =null;
            
            if (!tBase.IsValueType && tBase != typeof(string))                                                          // If this is a reference type (not string and not value)
            {
                Reference r=j.GetMappedReference(o);                                                                    //      Get a reference to object o
                if (r.TheJSon!=null) return new Json(new JsonName(r.Id));                                               //      If found then return a NamedNode to the reference node
                TheList = r.TheJSon =  new Json(Json.Array);                                                            //      If not found create a Json Array 
                TheResult = new Json(new JsonName(r.Id)); ;                                                             //      And set the result as NamedNode to the Json Array
            } else TheResult = TheList = new Json(Json.Array);                                                          // If this a value node, create the Json Array and set it as result
            foreach (var x in (IEnumerable)o) TheList.Add(x != null ? ser0.Encode(x, j) : null);                        // Fill in the Json Array
            return TheResult;                                                                                           // And return it
        }
    }
}
