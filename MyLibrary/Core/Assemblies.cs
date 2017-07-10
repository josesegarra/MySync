using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace JSegarra.Core
{
    public class Assemblies
    {

        public static IEnumerable<Assembly> GetAssemblies()
        {
            var hash = new HashSet<string>();
            var stack = new Stack<Assembly>();

            foreach(Assembly cAsm in AppDomain.CurrentDomain.GetAssemblies())
            {
                AssemblyName nAsm = cAsm.GetName();
                if (!hash.Contains(nAsm.FullName))                                                         // If not already in the list
                {
                    if (!cAsm.GlobalAssemblyCache) stack.Push(cAsm);
                    hash.Add(nAsm.FullName);
                }
            }
            

            do                                                                                                      // Enter into a loop
            {
                var asm = stack.Pop();                                                                              // Pop assembly
                yield return asm;                                                                                   // Return
                foreach (AssemblyName reference in asm.GetReferencedAssemblies())                                   // Loop all referenced assemblies
                {
                    if (!hash.Contains(reference.FullName))                                                         // If not already in the list
                    {
                        Assembly curAsm = Assembly.Load(reference);                                                    // Load the assembly
                        if (!curAsm.GlobalAssemblyCache) stack.Push(curAsm);
                        hash.Add(reference.FullName);
                    }
                }
            }
            while (stack.Count > 0);
        }
    }
}
