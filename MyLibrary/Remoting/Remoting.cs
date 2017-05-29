using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JSegarra.Core;
using System.Reflection;

namespace JSegarra.Remote
{
    public class Remoting
    {

        static ThreadList<Deployment> deploys= new ThreadList<Deployment>();

       

        public static Deployment Deploy(string uri,string username,string password)
        {
            try
            {
                return deploys.Add(new Deployment(uri, username,password));
            }
            catch (Exception e)
            {
                Logger.Error("In Remoting.OpenConnection: " + e.Message);
                return null;
            }
        }
    }
}
