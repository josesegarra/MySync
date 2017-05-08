using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using JSegarra.Core;

namespace JSegarra.Serialize
{
    // To serialize/deserialize and Object, a list of CoDec_Member is built
    class CoDec
    {
        static BindingFlags WhichMembers = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic;        // Members of T that will be considered for serialization
        internal delegate void setValue(object obj,object value);
        internal delegate object getValue(object obj); 
        internal string Name;                                                                                           // Name of the property
        internal setValue SetValue;                                                                                     // Delegate to fetch the value of the property from an object
        internal IDecoder Decoder=null;                                                                                      // Decoder capable of decoding the value of the property
        internal getValue GetValue;                                                                                     // Delegate to set the value of the property from an object
        internal IEncoder Encoder=null;                                                                                      // Encoder capable of encoding the value of the property
        internal Type MemberType;

        CoDec(string name, getValue gv, setValue sv, Type t)
        {
            Name = name;
            SetValue = sv;
            GetValue = gv;
            MemberType = t;
        }

        internal CoDec Init(Serializer s)
        {
            
            Decoder = s.GetDecoder(MemberType);
            Encoder = s.GetEncoder(MemberType);
            return this;
        }

        static string IsSerializable(Type t)
        {
            if (t.IsSubclassOf(typeof(Delegate))) return "delegate";                                           // Delegate fields cannot be serialized     
            if (t.IsAbstract) return "abstract";                                           // Delegate fields cannot be serialized     
            if (t.IsInterface) return "interface";
            return String.Empty;
        }

        internal static string WhyNot(MemberInfo mf)
        {
            return Create(mf).ToString();
        }

        static object Create(MemberInfo mf)                                                                 // Creates a CoDec if possible of a string description of why not
        {
            if (mf.MemberType == MemberTypes.Field)                                                                     // If the Member Implements a field
            {
                FieldInfo f0 = (FieldInfo)mf;                                                                                   // Get the member as a Field
                if (f0.IsStatic) return "static";
                if (f0.IsDefined(typeof(CompilerGeneratedAttribute), false)) return "auto";       // Static & Backing fields do not need to be serialized
                string s = IsSerializable(f0.FieldType);
                if (s != String.Empty) return s;
                return new CoDec(f0.Name, f0.GetValue, f0.SetValue, f0.FieldType);
            }
            if (mf.MemberType == MemberTypes.Property)                                                                  // If the member is a property
            {
                PropertyInfo f0 = (PropertyInfo)mf;                                                                     // Get the member as a Property
                if (!f0.CanRead) return "Unreadable";
                if (!f0.CanWrite) return "Unwritable";
                string s = IsSerializable(f0.PropertyType);
                if (s != String.Empty) return s;
                return new CoDec(f0.Name, f0.GetValue, f0.SetValue, f0.PropertyType);
            }
            return "Unsupported";
        }


        internal static CoDec Create(MemberInfo mf, Serializer s)                                                // Creates & register in L a Object_Member capable of handling MemberInfo 
        {
            object b = Create(mf);
            if (b is CoDec)
            {
                Logger.Green("For member " + mf.Name);
                return ((CoDec)b).Init(s);
            }
            Logger.Grey("Skipping member " + mf.Name);
            return null;
        }

        public override string ToString()
        {
            string s = (Name + ":" + MemberType.Name).PadRight(50);
            s = s + " decoder: " + (Decoder != null ? Decoder.GetType().Name : "").PadRight(50);
            s = s + " encoder: " + (Encoder != null ? Encoder.GetType().Name : "").PadRight(50);
            return s.Trim();
        }

        internal static List<CoDec> InitFromType(Type BaseType, Serializer ser)
        {
            List<CoDec> members = new List<CoDec>();
            string f = "";
            foreach (MemberInfo m in BaseType.GetMembers(WhichMembers))                                                 // Loop all members of the object
                if (!members.AddN(CoDec.Create(m, ser))) f = f + m.Name + ": " + CoDec.WhyNot(m) + " | ";
            foreach (CoDec m in members) Logger.Green("  " + m.ToString());
            Logger.Green("  In " + BaseType.Name + " skipped: " + f.Left(90, true));
            return members;
        }
    }
}
