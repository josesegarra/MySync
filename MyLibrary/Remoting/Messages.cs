using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JSegarra.JSON;
using JSegarra.Core;

namespace JSegarra.Remote
{
    internal class Messages
    {
        static string LoginMsg = "{action:'login',user:'@user@',password:'@password@'}";


        // Build a login message
        public static string Login(string username = "", string password = "")
        {
            return LoginMsg.Replace("@user@", username).Replace("@password@", password);
        }

        public static string Chunk(string ConnectionId, int chunkNumber, int chunkTotal, byte[] chunkData)
        {
            var j = new Json(JsonKind.Object);
            j["id"] = ConnectionId;
            j["action"] = "chunk";
            j["chunk"] = chunkNumber;
            j["total"] = chunkTotal;
            j["data"] = chunkData;
            Logger.Yellow(j.ToString());
            return j.ToString();
        }
        
    }
}
