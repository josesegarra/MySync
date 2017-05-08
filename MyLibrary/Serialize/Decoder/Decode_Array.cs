using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JSegarra.Core;
using JSegarra.JSON;

namespace JSegarra.Serialize
{
    class ArrayDecoder:IDecoder
    {
        static ArrayDecoder defDecoder = new ArrayDecoder();
        
        public void Init(Serializer s)
        {

        }
        public object Decode(Json j, Type wt, IContext c)                                                       // Decodes a JSON object into an array 
        {
            Serializer s = c.GetSerializer();                                                                   // Get the serializer
            j = s.HandleLinks(j);                                                                               // Follow links
            if (j==null || j.Kind == JsonKind.Null) return null;                                                // If null then return    
            

            object[] items = new object[j.Items.Count];                                                         // Initially we use an object[] array
            HashSet<Type> types = new HashSet<Type>();                                                          // This will hold the types of the elements of the array
            for (int i = 0; i < items.Length;i++)                                                               // Loop all elements
            {
                items[i] = s.Decode(j.Items[i], c);                                                             // Get element[i] by decoding JSON j.Items[i]            
                if (items[i]!=null) types.Add(items[i].GetType());                                              // If after decoded is not null, add its type 
            }
            if (types.Count == 1) return ArrayToTypedDynamic(items, (from t in types select t).First());        // If there is only one type T, create T[] from object[] and return it
            return items;                                                                                       // otherwise return object[]
        }
        object ArrayToTypedDynamic(object[] items, Type t)                                                      // Change the type of an array using dynamic
        {
            dynamic myList = Activator.CreateInstance(typeof(List<>).MakeGenericType(t));
            foreach (object x in items) myList.Add((dynamic)x);
            return myList.ToArray();
        }

        public static IDecoder JFactory(Json k,Type t)
        {
            if (k != null && k.Kind == JsonKind.Array) return defDecoder;
            if (t != null && t.IsArray) return defDecoder;
            return null;
        }

        object ArrayToTypedReflection(object[] items, Type t)                                                   // Change the type of an array using reflection
        {
            object myList = Activator.CreateInstance(typeof(List<>).MakeGenericType(t));
            MethodInfo kAdd = myList.GetType().GetMethod("Add");
            foreach (object x in items) kAdd.Invoke(myList, new object[] { x });
            MethodInfo kToArray = myList.GetType().GetMethod("ToArray");
            return kToArray.Invoke(myList, null);
        }
    }
}
