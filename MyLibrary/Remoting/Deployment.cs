using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Reflection;
using JSegarra.Core;

namespace JSegarra.Remote
{
    public class Deployment
    {
        delegate byte[] SendMessageDel(Uri where, byte[] message);
        internal string ConnectionId = "";
        internal string DeployId = "";
        internal Uri uri;
        SendMessageDel msgFunc;

 
        static Dictionary<string, SendMessageDel> transports = new Dictionary<string, SendMessageDel>()
        {
            {"http", HttpTransport.SendMessage }
        };



        IEnumerable<string> GetAssemblyFiles(Assembly assembly)
        {
            var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assemblyName in assembly.GetReferencedAssemblies())
                yield return loadedAssemblies.SingleOrDefault(a => a.FullName == assemblyName.FullName)?.Location;
        }

        public Deployment(string theUri,string userName,string passWord)
        {
            uri = new Uri(theUri);
            if (!transports.TryGetValue(uri.Scheme, out msgFunc)) throw new Exception("Unknown schema: " + uri.Scheme);
            msgFunc(uri, Messages.Login(userName, passWord));



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
