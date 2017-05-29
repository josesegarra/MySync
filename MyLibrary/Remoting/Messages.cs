using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.Remoting
{
    class Messages
    {
        // Build a login message
        public static byte[] Login(string username,string password)
        {
            byte[] buffer = new byte[10];
            return buffer;

        }
    }
}
