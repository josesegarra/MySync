using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JSegarra.JSON;

namespace JSegarra.RemoteHost
{

    public class Authorization
    {

        public bool IsAllowed(Json jsonIn)
        {
            string userName= jsonIn["user"]?.ToString() ?? "";                                                              // Get the action
            string userPassword = jsonIn["password"]?.ToString() ?? "";                                                              // Get the action

            Helper.Log("JSegarra.RemoteHost.Authorization.IsAllowed");
            Helper.Log2("username",userName);
            Helper.Log2("password",userPassword);
            return true;
        }
    }
}