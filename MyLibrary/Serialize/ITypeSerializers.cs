using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JSegarra.Core;
using JSegarra.JSON;


namespace JSegarra.Serialize
{


    delegate IEncoder EncoderFactory(Type t);                                                                           // Delegate for a factory function. This function returns a IEncoder for the type (or null if it can't)
    delegate IDecoder DecoderFactory(Json k,Type t);                                                                // Delegate for a factory function. This function returns a IDecoder for the JSON-type (or null if it can't)

    // During serialization, me map Objects to JSON objects. Sometimes it might be needed to keep (either of them) outside the JSON document structure (for temporary nodes or calculations)
    // This is handled in a IContext object
    public class Reference
    {
        public object TheObject = null;                                                                                 // The Object being mapped
        public Json TheJSon=null;                                                                                       // The Json object in the map   
        public string Id="";                                                                                            // Id for this mapping
    }


    // During a serialization operation a Context keeps track of the current mappings
    public interface IContext
    {
        Serializer GetSerializer();                                                                                     // This is the root Json node for the current serialization operation       
        Json GetRoot();                                                                                                 // This is the root Json node for the current serialization operation       
        object[] GetMappedObjects();                                                                                    // Returns all mapped objects 
        Reference[] GetMappedReferences();                                                                              // Returns the references for all the objects
        Reference GetMappedReference(object o);                                                                         // Returns a reference for object O. If the reference does not exists, create it 
        int MappedCount();
    }


    // A TypeSerializer object is capable of serializing a object of a given type, TypeSerializers are created and initialized by the Serializer object 
    // using a factory pattern for actual Type instances. This allows the reuse of the same TypeSerializer class for different types, which is needed to handle 
    // polymorphism (either by inheritance or by the use of generics). Examples:

    // to serialize an object of type System.String, the Serializer object will:
    //      ITypeSerializer myStringSerializer=new StringTypeSerializer();  myStringSerializer.Init(typeof(string)); Json jString=myStringSerializer.Encode( STRING, CONTEXT_OBJECT)
    //
    // to serialize an object of type System.Int and another of type System.Byte, the Serializer object will:
    // 
    //      ITypeSerializer myIntSerializer=new  IntSerializer();  myIntSerializer.Init(typeof(int));   Json jInt = myStringSerializer.Encode( INTEGER value, CONTEXT_OBJECT)
    //      ITypeSerializer myByteSerializer=new IntSerializer();  myByteSerializer.Init(typeof(byte)); Json jByte= myStringSerializer.Encode( BYTE   value, CONTEXT_OBJECT)
    // 
    // to serialize an object of type List<int> and another of type List<string>, the Serializer object will:
    // 
    //      ITypeSerializer myListIntSerializer=new GenericListSerializer();    myListIntSerializer.Init(typeof(List<int>));    Json lInt = myStringSerializer.Encode( LIST<INT>    value, CONTEXT_OBJECT)
    //      ITypeSerializer myListStrSerializer=new GenericListSerializer();    myListStrSerializer.Init(typeof(List<string>)); Json lStr = myStringSerializer.Encode( LIST<STRING> value, CONTEXT_OBJECT)
    

    public interface IEncoder
    {
        void Init(Serializer s);                                                             // Inits the Type Serializer 
        Json Encode(object o, IContext c);                                                          // Serializes an object    
    }

    public interface IDecoder
    {
        object Decode(Json j, Type t,IContext c);                                                          // Serializes an object    
        void Init(Serializer s);                                                             // Inits the Type Serializer 
    }
}
