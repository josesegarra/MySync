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
        internal string Host;
        internal string Id;
        internal string ConnectionId = "";
        internal string DeployId = "";
        internal Uri uri;


        IEnumerable<string> GetAssemblyFiles(Assembly assembly)
        {
            var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assemblyName in assembly.GetReferencedAssemblies())
                yield return loadedAssemblies.SingleOrDefault(a => a.FullName == assemblyName.FullName)?.Location;
        }

        public Deployment(string uri,string userName,string passWord)
        {
            this.uri = new Uri(uri);
            Logger.Green(this.uri.Scheme);


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
