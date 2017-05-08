using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JSegarra.Core;
using JSegarra.JSON;

namespace JSegarra.Serialize
{
    class CollectionDecoder:IDecoder
    {
        Type baseType;
        Type itemType;
        Type[] genParams;
        IDecoder decoder;
        CollectionDecoder(Type t)
        {
            //Logger.Red("-----------------------------------------------------");
            //Logger.Red("Creating collection decoder for type " + t.Name); ;
            baseType = t;
            genParams = baseType.GetGenericArguments();
        }
        public void Init(Serializer s)
        {
            itemType=baseType.GetMethod("GetEnumerator").ReturnType.GetProperty("Current").PropertyType;                         // Use reflection to get the enumerator type
            //Logger.Red("Enumerator for collection decoder for " + baseType.Name); ;
            //Logger.Red("                                   is " + itemType.Name); ;
            if (itemType.IsGenericType) 
            {
                //Logger.Red("               which is a generic for " + itemType.Name); ;
            }
            decoder= s.GetDecoder(itemType);
            //Logger.Red("Decoder for enumerator of             " + baseType.Name+"."+itemType.Name); ;
            //Logger.Red("                                   is " + decoder.GetType().Name);
            
        }
        public object Decode(Json j, Type wt, IContext c)                                                       // Decodes a JSON object into an array 
        {
            //Logger.Red("-----------------------------------------------------");
            //Logger.Red("Executing collection decoder for type " + wt.Name ); ;

            dynamic myList = Activator.CreateInstance(baseType.GetGenericTypeDefinition().MakeGenericType(genParams));
            
            int count = j.Items.Count;
            for (int i = 0; i < count; i++)                                                                     // Loop all elements
            {
                object o = decoder.Decode(j.Items[i], itemType, c);
                //Logger.Red("  have to include item " + o.GetType().FullName);
                
            }
            // Logger.Red("Finished executing collection decoder for type " + wt.Name); ;




            return myList;                                                                                       // otherwise return object[]
        }
        public static IDecoder JFactory(Json k,Type t)
        {
            if (t == null) return null;
            if (t.IsArray) return null;
            if (t.GetInterfaces().Contains(typeof(IEnumerable))) return new CollectionDecoder(t);
            return null;
        }
    }
}
