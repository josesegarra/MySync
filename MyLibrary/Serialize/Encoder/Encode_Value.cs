using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JSegarra.Core;
using JSegarra.JSON;


namespace JSegarra.Serialize
{
    class ValueEncoder: IEncoder
    {
        internal enum PrKind { Kstring, Kint, Kenum, KdateTime };
        PrKind Kind;                                                                                                    // What kind of serialization should be used
        internal ValueEncoder(PrKind kind)                                                                            // The constructor, just store TYPE and KIND
        {
            Kind = kind;
        }

        public Json Encode(object o, IContext j)                                                                         // Encode value to JSON
        {
            switch(Kind)                                                                                                // Depending on the KIND create a JSON node with the right encoding
            {
                case PrKind.Kenum: return new Json(Convert.ChangeType(o, Enum.GetUnderlyingType(o.GetType())));         // enums are converted to their underlying INT type, and then JSON(INT)
                case PrKind.Kstring: return new Json(o.ToString());                                                     // strings are encoded using JSON(STRING)
                case PrKind.Kint: return new Json(Convert.ToInt32(o));                                                  // ints are encoded using JSON(INT)
                case PrKind.KdateTime: return new Json(((DateTime)o).ToString("yyyy-MM-dd HH:mm:ss.fff"));              // dateTime are encoded as JSON(STRING)    
            }
            throw new Exception("Should never happen: PrimitiveSerializer.Encode  ");
        }

        public void Init(Serializer s)
        {
        }

        public static IEncoder Factory(Type t)                                                                          // Factory function for Array Serializers
        {
            if (t.IsEnum) return new ValueEncoder(PrKind.Kenum);
            if (t == typeof(System.String)) return new ValueEncoder(ValueEncoder.PrKind.Kstring);
            
            if (t == typeof(System.SByte)) return new ValueEncoder(ValueEncoder.PrKind.Kint);
            if (t == typeof(System.Int16)) return new ValueEncoder(ValueEncoder.PrKind.Kint);
            if (t == typeof(System.Int32)) return new ValueEncoder(ValueEncoder.PrKind.Kint);
            if (t == typeof(System.Int64)) return new ValueEncoder(ValueEncoder.PrKind.Kint);

            if (t == typeof(System.Char)) return new ValueEncoder(ValueEncoder.PrKind.Kint);
            
            if (t == typeof(System.Byte)) return new ValueEncoder(ValueEncoder.PrKind.Kint);
            if (t == typeof(System.UInt16)) return new ValueEncoder(ValueEncoder.PrKind.Kint);
            if (t == typeof(System.UInt32)) return new ValueEncoder(ValueEncoder.PrKind.Kint);
            if (t == typeof(System.UInt64)) return new ValueEncoder(ValueEncoder.PrKind.Kint);

            if (t == typeof(System.DateTime)) return new ValueEncoder(ValueEncoder.PrKind.KdateTime);
            
            return null;
        }
    }
}
