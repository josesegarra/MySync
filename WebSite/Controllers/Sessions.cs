using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JSegarra.JSON;

namespace JSegarra.RemoteHost
{

    public class Session
    {
        public Json owner;
        public Guid id;
        public Session(Json data)
        {
            owner = data;
            id = Guid.NewGuid();
        }
    }


    public class Sessions
    {
        Dictionary<Guid, object> sessions = new Dictionary<Guid, object>();
        object mylock = new object();

        public Session Add(Session s)
        {
            lock(mylock)
            {
                sessions.Add(s.id,s);
                Helper.Log("JSegarra.RemoteHost.Sessions.Add");
            }
            return s;
        }
    }
}