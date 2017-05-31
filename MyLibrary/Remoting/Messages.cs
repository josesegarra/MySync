using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSegarra.Remote
{
    internal class Messages
    {
        static string LoginMsg = "{'action':'login','user':'@user@','password':'@password@'}".Replace('\'', '\"');


        // Build a login message
        public static byte[] Login(string username = "", string password = "")
        {
            return Encoding.Unicode.GetBytes(LoginMsg.Replace("@user@", username).Replace("@password@", password));
        }


        public static string AsString(byte[] byteArray,Encoding encoding=null)
        {
            if (encoding == null) encoding = Encoding.Unicode;
            return encoding.GetString(byteArray);
        }
    }
}
