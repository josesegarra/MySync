using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Reflection;
using JSegarra.Core;
using JSegarra.JSON;

namespace JSegarra.Remote
{
    public class Deployment
    {
        delegate string SendMessageDel(Uri where, string message);
        internal string ConnectionId = "";
        internal string DeployId = "";
        internal Uri uri;
        SendMessageDel msgFunc;

        static Dictionary<string, SendMessageDel> transports = new Dictionary<string, SendMessageDel>()
        {
           {"http", HttpTransport.Request},
           {"https", HttpTransport.Request}
        };



        IEnumerable<string> GetAssemblyFiles(Assembly assembly)
        {
            var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assemblyName in assembly.GetReferencedAssemblies())
                yield return loadedAssemblies.SingleOrDefault(a => a.FullName == assemblyName.FullName)?.Location;
        }


        void DoLogin(string userName, string passWord)
        {
            
            string msg = msgFunc(uri, Messages.Login(userName, passWord));                                                                  // Send a login message to the server
            if (msg == null) throw new Exception("Deployment.Login failed");                                                                // If no message the fail
            Json jsmg = Json.Parse(msg);                                                                                                    // Parse the message and decode it
            ConnectionId = jsmg.Get("id","");                                                                                                  // Get the connection ID
            if (ConnectionId == "") throw new Exception("Could not login: " + uri);
            Logger.Green("Connection Id " + ConnectionId);                                                                                  // Log connection
        }


        void DoBundle()
        {
            Bundle bundle = new Bundle();
            int i;
            for (i = 0; i < bundle.Chunks.Count; i++)
            {
                string msg = msgFunc(uri, Messages.Chunk(ConnectionId, i, bundle.Chunks.Count, bundle.Chunks[i]));                                  // Send a login message to the server
                if (msg == null) throw new Exception("Deployment.Login failed sending chunk: " + i);                                                                // If no message the fail
                Logger.Green(msg);
            }
        }

        public Deployment(string theUri,string userName,string passWord)
        {
            uri = new Uri(theUri);
            if (!transports.TryGetValue(uri.Scheme, out msgFunc)) throw new Exception("Unknown transport: " + uri.Scheme);                      // Get the Transport function
            DoLogin(userName, passWord);                                                                                                          // Do the login
            DoBundle();

        }

        void DeployAssembly(Assembly asm)
        {
            Logger.DebugEnter("Deploying : " + asm.GetName().Name + " to " + uri);

            var assemblyFiles = GetAssemblyFiles(asm);
            foreach (var s in assemblyFiles)
            {
                Logger.DebugPrint(s);
            }
            Logger.DebugExit();
        }

        public void Execute(Expression<Action> expression)
        {
            MethodCallExpression member = expression.Body as MethodCallExpression;
            if (member == null) throw new ArgumentException("Connection.Execute requires a simple method call", "expression");

            Console.WriteLine("In module " + member.Method.Module.Name);
            Console.WriteLine("Is Static " + member.Method.IsStatic);
            Console.WriteLine("In Type   " + member.Method.DeclaringType.Name);
            Console.WriteLine("Should execute " + member.Method.Name);
            Console.WriteLine("With followinf arguments");
            foreach (var s in member.Arguments)
            {
                Console.WriteLine("   " + s.GetType().Name + "   " + s.ToString());
            }
            //Type troot = member.Method.DeclaringType.Name;
            //Assembly asm=


        }
    }
}
