using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using JSegarra.JSON;
using JSegarra.Core;

namespace JSegarra.Serialize
{
   
    public class Serializer
    {
        internal static Type AnyType = null;
        Json jnull = new Json(Json.Null);
        internal Dictionary<Type, IEncoder> encoders = new Dictionary<Type, IEncoder>();                                        // Collection of registered encoders
        internal List<EncoderFactory> serFactory = new List<EncoderFactory>();                                                  // Collection of registered encoder factories
        internal Dictionary<Type, IDecoder> decoders = null;                                                                    // Collection of registered decoders
        internal List<DecoderFactory>       jdecoders= new List<DecoderFactory>();                                                                   // Decoder factory from JSON
        Context context = null;

        public Serializer()
        {
            serFactory.Add(ObjectEncoder.Factory);                                                                              // Register a factory function for objects of Any type
            serFactory.Add(EnumerableEncoder.Factory);                                                                          // Register a factory function for objects implementing IEnumerable types
            serFactory.Add(KeyValueEncoder.Factory);                                                                            // Register a factory function for objects of Dictionary<,> type
            serFactory.Add(ValueEncoder.Factory);                                                                               // Register a factory function for objects of Value Types
            
            decoders= new Dictionary<Type, IDecoder>();                                                                         // Create the collection of registered decoders

            decoders.Add(typeof(System.String), new ValueDecoder());                                                            // Register a decoder for JsonKind.string of String
            decoders.Add(typeof(System.DateTime), new ValueDecoder());                                                            // Register a decoder for JsonKind.string of String
            /*decoders.Add(typeof(System.SByte), new ValueDecoder());                                                            // Register a decoder for JsonKind.Int of AnyType
            decoders.Add(typeof(System.Int16), new ValueDecoder());                                                            // Register a decoder for JsonKind.Int of AnyType
            decoders.Add(typeof(System.Int32), new ValueDecoder());                                                            // Register a decoder for JsonKind.Int of AnyType
            decoders.Add(typeof(System.Int64), new ValueDecoder());                                                            // Register a decoder for JsonKind.Int of AnyType
            decoders.Add(typeof(System.Byte), new ValueDecoder());                                                            // Register a decoder for JsonKind.Int of AnyType
            decoders.Add(typeof(System.UInt16), new ValueDecoder());                                                            // Register a decoder for JsonKind.Int of AnyType
            decoders.Add(typeof(System.UInt32), new ValueDecoder());                                                            // Register a decoder for JsonKind.Int of AnyType
            decoders.Add(typeof(System.UInt64), new ValueDecoder());                                                            // Register a decoder for JsonKind.Int of AnyType
            */
          
            jdecoders.Add(UnTypedObject.JFactory);
            jdecoders.Add(CollectionDecoder.JFactory);
            jdecoders.Add(ArrayDecoder.JFactory);
            jdecoders.Add(ValueDecoder.JFactory);
            jdecoders.Add(KeyValueDecoder.JFactory);
        }

        void rError(string kind,Type t,Type t0=null)
        {
            string r=(t0!=null? "When serializing [" + t0.Name + "]\n":"");
            throw new Exception(r+(kind!="" ?kind+" type":"Type")+" [" + t.Name + "] cannot be serialized");                    // Throws an exception
        }

        public IEncoder GetEncoder(Type t)                                                                                      // Gets a serializer for the type, building it from a factory if needed
        {
            IEncoder st;
            if (t.IsAbstract) rError("Abstract", t);                                                                            // Raise an error if serializing an abstract type
            if (t.IsInterface) rError("Interface", t);                                                                          // Raise an error if serializing an interface type
            if (encoders.TryGetValue(t, out st)) return st;                                                                     // If the received type already has a TypeSerializer Object then return it
            st = BuildEncoder(t);                                                                                               // Build the encoder
            encoders.Add(t, st);                                                                                                // Register serializer for the type, we register it before initializing it to shortcut circular references
            st.Init(this);                                                                                                      // Initialize the just built serializer
            return st;                                                                                                          // And return it
        }

        IEncoder BuildEncoder(Type t)                                                                                           // Builds a serializer for the type using one of the registered factory functions
        {
            IEncoder de = Enumerable.Range(0, serFactory.Count).OrderByDescending(x => x).                                      // Loop in descending index order the serFactory list                     
                Select(x => serFactory[x](t)).                                                                                  // Call the factory method, with the received Input Type
                Where(x => x != null).FirstOrDefault();                                                                         // If factory returned Not NULL then we have a encoder for the Type
            if (de == null) throw new Exception("Cannot build a Encoder for: " + t.FullName); else return de;                   // Return encoder or throw an error
        }

        public byte[] Encode(object value)                                                                                      // Encodes an object
        {
            if (value==null) return Encoding.Unicode.GetBytes(jnull.ToString());                                                // If encoding null, then return null    
            IEncoder ser = GetEncoder(value.GetType());                                                                         // Get a encoder for the type of the object
            Json root = new Json(Json.Object);                                                                                  // Create the Json root object
            context = new Context(this, root);                                                                                  // Create the encoding context 
            Json n=ser.Encode(value, context);                                                                                  // Encode received value to JSON 
            if (n.Kind==JsonKind.NamedNode)                                                                                     // If an object needing references was returned then
            {
                root["#value"] = n;                                                                                             //      Create a node with the value in root
                foreach (Reference r in context.GetMappedReferences()) root[r.Id] = r.TheJSon;                                  //      Add encoded references to root JSON object
                n = root;                                                                                                       //      And set root as the element to return
            }
            return Encoding.Unicode.GetBytes(n.ToString());                                                                     // Return 
        }



        public IDecoder GetDecoder(Type wt)
        {
            return GetDecoder(null, wt);
        }
      

        public Json HandleLinks(Json k)
        {
            if (k==null) return null;
            string link="";
            if (k.Kind == JsonKind.Object) link=k["#value"].AsString(); else
            if (k.Kind == JsonKind.NamedNode) link=k.Value.AsString(); 
            if (link=="") return k;
            if (!link.StartsWith("@")) throw new Exception("Link label should start with @ " + link);
            return context.GetRoot().Items[link.Substring(1)];
        }

        string getDesc(Json j0, Type wType)
        {
            string s = "(Type: " + (wType != null ? wType.Name : " [UNKOWN]").Right(30).PadRight(30) + ") ";
            if (j0 == null) return s + " NULL";
            if (!j0.IsComplex()) return s+j0.ToString().Left(50).BigTrim();
            if (j0.Kind == Json.Object) return s+" Object: "+j0.ToString().Left(50).BigTrim();
            if (j0.Kind == Json.Array) return s+" Array: "+j0.ToString().Left(50).BigTrim();
            return s+" ERROR";
        }

        IDecoder GetDecoder(Json j0, Type wType)
        {
            Json j = HandleLinks(j0);
            Logger.Yellow("    Fetching a decoder for                     " + getDesc(j0, wType));
            IDecoder dec;
            if (wType != AnyType) if (decoders.TryGetValue(wType, out dec)) return dec;                                         // If requested a type and there is a decoder for the type, return the decoder
            dec = Enumerable.Range(0, jdecoders.Count).OrderByDescending(x => x).                                               // Search all the factories for a valid decoder    
                Select(x => jdecoders[x](j,wType)).Where(x => x != null).FirstOrDefault();
            if (dec == null) throw new Exception("Could not find a decoder factory for " + getDesc(j0, wType));                                    // Fail if nothing found
            Logger.Yellow("    Using " + dec.GetType().Name.Left(20).PadRight(20)+" as decoder for  "+getDesc(j0, wType));
            if (wType != AnyType)
            {
                //Logger.Yellow("    Registering " + dec.GetType().Name.Left(30).PadRight(30) + " as decoder for   " + td);                
                decoders.Add(wType, dec);                                                                      // If type then register the decoder we had a type
            }
            
            dec.Init(this);                                                                                                     // Init the decoder
            return dec; 
        }


       
        internal object Decode(Json j, Type dType, IContext c)
        {
            if (j.Kind == JsonKind.Null) return null;                                                                           // If node is null then return null
            IDecoder d = GetDecoder(j, dType);                                                                                  // Get a decoder for this kind of node and the expected type
            Logger.Yellow("%02 decoding "+j.AsString().Left(50).BigTrim()+"    | using "+d.GetType().FullName);
            return d.Decode(j, dType, c);                                                                                        // Execute the decoder
        }

        internal object Decode(Json j, IContext c)                                                                              // Decode JSON of an unkown type
        {   
            return Decode(j, AnyType, c);                                                                                       // Call with AnyType option
        }

        public object Decode(byte[] bytes) 
        {   
            Json root = Json.Parse(Encoding.Unicode.GetString(bytes));                                                          // First thing we do is parse the received json
            context = new Context(this, root);                                                                          // Create a context using root
            return Decode(root, context);                                                                                       // Return the result of decoding root     
        }

    }
}
