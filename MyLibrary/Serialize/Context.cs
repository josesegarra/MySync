using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JSegarra.JSON;

namespace JSegarra.Serialize
{
    class Context:IContext
    {
        Dictionary<object, Reference> mapped;
        Serializer serializer;
        Json root;

        internal Context(Serializer ser,Json roo)
        {
            mapped = new Dictionary<object, Reference>();
            serializer = ser;
            root = roo;
        }

        public Reference GetMappedReference(object o)                                                                   // Returns an existing reference to an object, or creates it
        {
            Reference r;                                                                                                // This is the reference that will be returned
            if (mapped.TryGetValue(o, out r)) return r;                                                                 // If the object is already mapped then return its registered reference
            r = new Reference() { TheObject = o , Id="O_"+mapped.Count };                                               // Create a reference
            mapped.Add(o, r);                                                                                           // Map the object & the reference
            return r;                                                                                                   // Return the reference
        }
        public Serializer GetSerializer()
        {
            return serializer;
        }
        public Json GetRoot()
        {
            return root;
        }
        public object[] GetMappedObjects()
        {
            return mapped.Keys.ToArray();
        }
        public Reference[] GetMappedReferences()
        {
            return mapped.Values.ToArray();
        }

        public int MappedCount()
        {
            return mapped.Count;
        }
    }
}
