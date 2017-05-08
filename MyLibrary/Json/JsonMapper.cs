using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSegarra.JSON
{
    internal class JsonList: List<Json>
    {
    }
    
    internal class JsonMapper: Dictionary<string, Json>
    {
        //internal Dictionary<string, Json> links = new Dictionary<string, Json>();
    }
}
