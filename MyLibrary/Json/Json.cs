using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using JSegarra.Core;


namespace JSegarra.JSON
{
    public enum JsonKind { Object, Array, String, Integer, Real, Null, Boolean,ByteArray,NamedNode,Other,None};

    public class JsonName
    {
        public readonly string Name;
        public JsonName(string s)
        {
            Name = s;
        }
    }
     
    public class Json
    {
        public static bool Debug = false;
        
        public readonly object Value;
        public readonly JsonKind Kind;
        public readonly IJsonAccess Items;
        public static readonly JsonKind String = JsonKind.String;
        public static readonly JsonKind Object = JsonKind.Object;
        public static readonly JsonKind Array = JsonKind.Array;
        public static readonly JsonKind Name = JsonKind.NamedNode;
        public static readonly JsonKind Integer = JsonKind.Integer;
        public static readonly JsonKind Real = JsonKind.Real;
        public static readonly JsonKind Null = JsonKind.Null;
        public static readonly JsonKind Boolean = JsonKind.Boolean;
        public static readonly JsonKind ByteArray = JsonKind.ByteArray;

        // ----------------------------------- Constructors

        public Json(JsonKind k)
        {
            switch (k)
            {
                case JsonKind.Object: Value = new JsonMapper(); break;
                case JsonKind.Array: Value = new JsonList(); break;
                case JsonKind.String: Value = ""; break;
                case JsonKind.Integer: Value = 0; break;
                case JsonKind.Real: Value = 0.0; break;
                case JsonKind.Null: Value = null; break;
                case JsonKind.Boolean: Value = false; break;
                case JsonKind.ByteArray: Value = null; break;
            }
            Items = JsonAccesors.GetAccesor(this);
            Kind = k;
        }

        public Json(string k)
        {
            Value = k;
            Kind = Json.String;
            Items = JsonAccesors.GetAccesor(this);
        }

        
        public Json(JsonName k)
        {
            Value = k.Name;
            Kind = Json.Name;
            Items = JsonAccesors.GetAccesor(this);
        }

        public Json(int k)
        {
            Value = k;
            Kind = Json.Integer;
            Items = JsonAccesors.GetAccesor(this);
        }

        public Json(double k)
        {
            Value = k;
            Kind = Json.Real;
            Items = JsonAccesors.GetAccesor(this);
        }

        public Json(DateTime k)
        {
            Value = k;
            Items = JsonAccesors.GetAccesor(this);
        }

        public Json(bool b)
        {
            Value = b;
            Kind = Json.Boolean;
            Items = JsonAccesors.GetAccesor(this);
        }

        internal Json(JsonList b)
        {
            Value = b;
            Kind = Json.Array;
            Items = JsonAccesors.GetAccesor(this);
        }

        internal Json(JsonMapper b)
        {
            Value = b;
            Kind = Json.Object;
            Items = JsonAccesors.GetAccesor(this);
        }

        public Json(byte[] b)
        {
            Value = b;
            Kind = Json.ByteArray;
            Items = JsonAccesors.GetAccesor(this);
        }
        public Json()
        {
            Value = null;
            Kind = Json.Null;
            Items = JsonAccesors.GetAccesor(this);
        }

        public Json(object k)
        {
            Items = JsonAccesors.GetAccesor(this);
            if (k == null)
            {
                Value = null;
                Kind = Json.Null;
                return;
            }
            Value = k;
            Kind = KindFromType(k.GetType());
            if (Kind == JsonKind.Other) throw new Exception("Not implemented JSON fields of type " + k.GetType().FullName);
        }

        // -----------------------------------

        void DoDebug(string s)
        {
            if (Debug) Console.WriteLine(s);
        }

        public object this[string key]
        {
            get
            {
                Json k=_JsonGet(key);
                return k==null ? null: k.Value;
            }
            set
            {
                Json k=(value!=null && (value is Json)) ? (Json)value: new Json(value);
                _JsonSet(key, k);
            }
        }

        public Json Add(object value)
        {
            Json k = (value != null && (value is Json)) ? (Json)value : new Json(value);
            _JsonAdd(k);
            return k;
        }

        public string[] GetProperties()
        {
            if (Kind != JsonKind.Object) throw new Exception("json.GetProperties() only works with json object");
            JsonMapper d = (JsonMapper)Value;
            return d.Keys.ToArray();
        }

     

        public Json[] GetValues()
        {
            if (Kind != JsonKind.Object && Kind != JsonKind.Array) throw new Exception("json.GetValues() only works with json objects or arrays ");
            if (Kind != JsonKind.Object) return ((JsonMapper)Value).Values.ToArray();
            return ((JsonList)Value).ToArray();
        }
        
        
        internal Json _JsonGet(string key)
        {
            if (Kind != JsonKind.Object) throw new Exception("json[string].get only works with json object values");
            JsonMapper d = (JsonMapper)Value;
            Json j;
            if (!d.TryGetValue(key, out j)) return null;
            return j;
        }
        internal void _JsonSet(string key, Json c)
        {
            if (Kind != JsonKind.Object) throw new Exception("json[string].set only works with json object values");
            JsonMapper d = (JsonMapper)Value;
            d[key] = c;
        }

        internal void _JsonAdd(Json c)
        {
            if (Kind != JsonKind.Array) throw new Exception("json.add only works with json array values");
            JsonList d = (JsonList)Value;
            d.Add(c);
        }



        public JsonKind KindFromType(Type t)
        {
            if (t == typeof(bool))      return JsonKind.Boolean;
            if (t == typeof(byte))      return JsonKind.Integer;
            if (t == typeof(sbyte))     return JsonKind.Integer;
            if (t == typeof(char))      return JsonKind.Integer;
            if (t == typeof(decimal))   return JsonKind.Real;
            if (t == typeof(double))    return JsonKind.Real;
            if (t == typeof(float))     return JsonKind.Real;
            if (t == typeof(int))       return JsonKind.Integer;
            if (t == typeof(uint))      return JsonKind.Integer;
            if (t == typeof(long))      return JsonKind.Integer;
            if (t == typeof(ulong))     return JsonKind.Integer;
            if (t == typeof(short))     return JsonKind.Integer;
            if (t == typeof(ushort))    return JsonKind.Integer;
            if (t == typeof(string))    return JsonKind.String;
            if (t == typeof(byte[]))    return JsonKind.ByteArray;
            if (t.IsArray) return JsonKind.Array;
            return JsonKind.Other;
        }

        public static Json ParseFile(string jsonFile)
        {
            Parser n = new Parser(File.ReadAllText(jsonFile));
            return n.ParseValue();
        }

        public static Json Parse(string jsonString)
        {
            Parser n = new Parser(jsonString);
            return n.ParseValue();
        }

        
        public override string ToString()
        {
            return this.ToString(null);
        }
        
 
        public bool IsComplex()
        {
            return (Kind == Json.Object || Kind == Json.Array);
        }

        public byte[] ToBinary()
        {
            return JsonBinaryEncoder.ToBinary(this);
        }
    }

    internal class Parser
    {
        const string WORD_BREAK = "{}[],:\"";
        enum TOKEN { NONE, CURLY_OPEN, CURLY_CLOSE, SQUARED_OPEN, SQUARED_CLOSE, COLON, COMMA, STRING, NUMBER, TRUE, FALSE, IDENT,BASE64,NULL,FETCH };
        string curWord = "";
        StringReader source;
        StringBuilder word = new StringBuilder();
       

        TOKEN NextToken() 
        {
            int i;
            curWord = "";
            while ((i=source.Peek())!=-1) 
            {
                char c=Convert.ToChar(i);
                if (!Char.IsWhiteSpace(c))
                {
                    if (c == '{') return TOKEN.CURLY_OPEN;
                    if (c == '}') return TOKEN.CURLY_CLOSE;
                    if (c == '[') return TOKEN.SQUARED_OPEN;
                    if (c == ']') return TOKEN.SQUARED_CLOSE;
                    if (c == ',') return TOKEN.COMMA;
                    if (c == '"') return TOKEN.STRING;
                    if (c == '*') return TOKEN.BASE64;
                    if (c == ':') return TOKEN.COLON;
                    if (c == '-' || (c>='0' && c<='9')) return TOKEN.NUMBER;
                    curWord = NextWord();
                    if (curWord.ToLower() == "null") return TOKEN.NULL;
                    if (curWord.ToLower() == "true") return TOKEN.TRUE;
                    if (curWord.ToLower() == "false") return TOKEN.FALSE;
                    if (curWord != "") return TOKEN.IDENT;
                    throw new Exception("Unkown TOKEN " + c);
                }
                source.Read();
            }
            return TOKEN.NONE;
        }

        TOKEN NextTokenDEBUG()
        {
            TOKEN t = NextToken();
            Logger.DebugPrint("NextToken Token: " + t + " " + curWord);
            return t;
        } 
        internal Parser(string jsonString)
        {
            source = new StringReader(jsonString);
        }

        internal Json ParseValue()
        {
            return ParseValue(TOKEN.FETCH);
        }
        Json ParseValue(TOKEN token=TOKEN.FETCH)
        {
            if (token == TOKEN.FETCH) token = NextToken();
            if (token == TOKEN.STRING) return new Json(ParseString());
            if (token == TOKEN.BASE64) return new Json(ParseBase64());
            else if (token == TOKEN.TRUE) return new Json(true);
            else if (token == TOKEN.NULL) return new Json(Json.Null);
            else if (token == TOKEN.FALSE) return new Json(false);
            else if (token == TOKEN.NUMBER) return new Json(ParseNumber());
            else if (token == TOKEN.CURLY_OPEN) return ParseObject();
            else if (token == TOKEN.SQUARED_OPEN) return ParseList();
            else if (token == TOKEN.IDENT)
            {
                return new Json(new JsonName(curWord));
            }
            throw new Exception("ParseValue: Unexpected TOKEN "+token+" "+curWord);
        }

        string NextWord()
        {
            word.Clear();
            int i;
            while ((i=source.Peek())!=-1) 
            {
                char c=Convert.ToChar(i);
                if (Char.IsWhiteSpace(c) || WORD_BREAK.IndexOf(c) != -1) return word.ToString();
                word.Append(c);
                source.Read();
            }
            return word.ToString();
            //throw new Exception("Unexpected END of WORD");
        }

        Json ParseObject()
        {
            
            JsonMapper values = new JsonMapper();
            TOKEN t;
            source.Read();                                                                                          // Consume "{"
            
            while ((t = NextToken())!=TOKEN.CURLY_CLOSE)                                                            // read until finding '}'
            {
                if (t == TOKEN.STRING || t == TOKEN.IDENT) ParseObjectMember(t,values);
                else if (t == TOKEN.NONE) throw new Exception("Unexpected EOF when Parsing object definition");
                else throw new Exception("Unexpected TOKEN " + t);
            }
            source.Read();                                                                                          // Consume "}"
            return new Json(values);
        }


        void KeepList(TOKEN expected)
        {
            TOKEN t = NextToken();
            if (t != TOKEN.COMMA && t != expected) throw new Exception("Unexpected END of "+(expected==TOKEN.SQUARED_CLOSE ? "LIST":"OBJECT")+ " and have "+t);
            if (t  == TOKEN.COMMA) source.Read();                                                                  // Consume comma
   
        }

        Json ParseList()
        {
            JsonList values = new JsonList();
            TOKEN t;
            source.Read();                                                                                          // Consume "{"
            while ((t = NextToken()) != TOKEN.SQUARED_CLOSE)                                                            // read until finding '}'
            {
                values.Add(ParseValue(t));
                KeepList(TOKEN.SQUARED_CLOSE);
            }
            source.Read();                                                                                          // Consume "}"
            return new Json(values);
        }

        void ParseObjectMember(TOKEN kind, JsonMapper values)
        {
            string s = (kind==TOKEN.IDENT ? curWord: ParseString());
            if (NextToken() != TOKEN.COLON) throw new Exception("Expected COLON");
            source.Read();
            values[s]=ParseValue();
            KeepList(TOKEN.CURLY_CLOSE);
        }

        object ParseNumber()
        {
            int i;
            int np = 0;
            StringBuilder s = new StringBuilder();
            s.Append(Convert.ToChar(source.Read()));

            while ((i = source.Peek()) != -1)
            {
                char c = Convert.ToChar(i);
                if (c >= '0' && c <= '9') s.Append(c);
                else if (c == '.' && np == 0)
                {
                    s.Append(c);
                    np = 1;
                }
                else return ToNumber(np,s.ToString());
                source.Read();
            }
            throw new Exception("Unexpected END of number");
        }

        object ToNumber(int np,string s)
        {
            if (np == 0)
            {
                byte sb1;
                if (byte.TryParse(s, out sb1)) return sb1;
                sbyte sb0;
                if (sbyte.TryParse(s, out sb0)) return sb0;
                short ss1;
                if (short.TryParse(s, out ss1)) return ss1;
                ushort ss0;
                if (ushort.TryParse(s, out ss0)) return ss0;
                int su1;
                if (int.TryParse(s, out su1)) return su1;
                uint su0;
                if (uint.TryParse(s, out su0)) return su0;
                long sl1;
                if (long.TryParse(s, out sl1)) return sl1;
                ulong sl0;
                if (ulong.TryParse(s, out sl0)) return sl0;
            }
            float f0;
            if (float.TryParse(s, out f0)) return f0;       // 32 bits
            double d0;
            if (double.TryParse(s, out d0)) return d0;       // 64 bits
            decimal d1;
            if (decimal.TryParse(s, out d1)) return d1;       // 128 bits
            throw new Exception("Number out of range cannot be parsed: "+s);
        }

        string ParseString()
        {
            int i;
            source.Read();                                              // Skip opening ["]
            StringBuilder s = new StringBuilder();
            while ((i = source.Peek()) != -1)
            {
                char c = Convert.ToChar(i);
                if (c == '"')
                {
                    source.Read();
                    return s.ToString();
                }
                if (c=='\\') 
                {       
                    source.Read();
                    i = source.Peek();
                    if (i==-1) break; 
                    c = Convert.ToChar(i);
                    if (c == '"' || c == '\\' || c == '/') s.Append(c);
                    else if (c == 'b') s.Append('\b');
                    else if (c == 'f') s.Append('\f');
                    else if (c == 'n') s.Append('\n');
                    else if (c == 'r') s.Append('\r');
                    else if (c == 't') s.Append('\t');
                    else if (c == 'u') s.Append(ParseUnicode());
                    else throw new Exception("Invalid scape sequence: \\" + c);
                } else s.Append(c);
                source.Read();
            }
            throw new Exception("Unexpected END of string");
        }

        byte[] ParseBase64()
        {
            source.Read();                                                                              // Consume the initial *
            if (Convert.ToChar(source.Read())!='"') throw new Exception("Expected \" after *");         // Expected " after * 
            int i;
            StringBuilder s = new StringBuilder();
            while ((i = source.Peek()) != -1)
            {
                char c = Convert.ToChar(i);
                if (c == '"')
                {
                    source.Read();
                    return Convert.FromBase64String(s.ToString());
                }
                s.Append(c);
                source.Read();
            }
            throw new Exception("Unexpected END of BASE64 string");
        }
        
        char ParseUnicode()
        {
            source.Read();                                                                  // Skip the u
            var hex = new char[4];
            for (int i = 0; i < 4; i++)
            {
                int j = source.Read();
                if (j == -1) throw new Exception("Unterminated UNICODE point");
                hex[i] = Convert.ToChar(j);
            }
            return (char)Convert.ToInt32(new string(hex), 16);    
        }
    }
}
