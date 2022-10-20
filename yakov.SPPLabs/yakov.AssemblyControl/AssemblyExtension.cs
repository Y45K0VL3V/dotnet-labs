using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace yakov.AssemblyControl
{
    public static class AssemblyExtension
    {
        public static IEnumerable<Type>? GetTypes(this Assembly assembly, Func<Type, bool> selector)
        {
            if (assembly == null) throw new ArgumentNullException();

            return assembly.GetTypes().Where(selector);
        }

        public static IEnumerable<Type>? GetTypes(this Assembly assembly, Func<Type, bool> selector, Func<Type, string> order)
        {
            if (assembly == null) throw new ArgumentNullException();
            
            return assembly.GetTypes().Where(selector).OrderBy(order);
        }
    }
}
