using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSegarra.JSON
{
    public interface IJsonAccess
    {
        Json this[string key] { get; set; }
        Json this[int key] { get; set; }
        int Count { get; }
    }

    class JsonObjectAccesor:IJsonAccess
    {
        Json Owner;

        internal JsonObjectAccesor(Json k)
        {
            Owner = k;
        }
        public Json this[string key]
        {
            get
            {
                Json s = Owner._JsonGet(key);
                return s;
            }
            set
            {
                Owner._JsonSet(key, value);
            }
        }

        public Json this[int key]
        {
            get
            {
                return this[key.ToString()];
            }
            set
            {
                this[key.ToString()] = value;
            }
        }

        public int Count
        {
            get
            {
                return ((JsonMapper)Owner.Value).Count;
            }
        }
    }

    class JsonNoAccesor : IJsonAccess
    {
        public Json this[string key]
        {
            get
            {
                throw new Exception("Primitive JSON types do not have properties"); 
            }
            set
            {
                throw new Exception("Primitive JSON types do not have properties");
            }
        }

        public Json this[int key]
        {
            get
            {
                throw new Exception("Primitive JSON types do not have properties");
            }
            set
            {
                throw new Exception("Primitive JSON types do not have properties");
            }
        }
        public int Count
        {
            get
            {
                return 0;
            }
        }
    }

    class JsonArrayAccesor : IJsonAccess
    {
        Json Owner;

        internal JsonArrayAccesor(Json k)
        {
            Owner = k;
        }   
        
        public Json this[string key]
        {
            get
            {
                int i;
                if (!Int32.TryParse(key, out i)) throw new Exception("JSON array needs integer (or parseable to integer) keys");
                return this[i];
            }
            set
            {
                int i;
                if (!Int32.TryParse(key, out i)) throw new Exception("JSON array needs integer (or parseable to integer) keys");
                this[i]=value;
            }
        }

        public int Count
        {
            get
            {
                return  ((JsonList)Owner.Value).Count;
            }
        }

        public Json this[int key]
        {
            get
            {
                JsonList d = (JsonList)Owner.Value;
                if (key >= 0 && key < d.Count) return d[key]; else return null;
            }
            set
            {
                throw new Exception("JSON array accessor not implemented");
            }
        }

    }
    class JsonAccesors
    {
        static JsonNoAccesor noaccess = new JsonNoAccesor();
        internal static IJsonAccess GetAccesor(Json node)
        {
            if (node.Kind == JsonKind.Object) return new JsonObjectAccesor(node);
            if (node.Kind == JsonKind.Array) return new JsonArrayAccesor(node);
            return noaccess;
        }
    }
    

}
