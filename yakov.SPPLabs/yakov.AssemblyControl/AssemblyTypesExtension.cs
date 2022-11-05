using System.Reflection;

namespace yakov.AssemblyControl
{
    public static class AssemblyTypesExtension
    {
        /// <summary>
        /// Get the assembly types with selection.
        /// </summary>
        /// <param name="assembly">Assembly with types</param>
        /// <param name="selector">Func to select types by need</param>
        /// <returns>IEnumerable of selected types</returns>
        /// <exception cref="ArgumentNullException">Occurs when assembly = null</exception>
        public static IEnumerable<Type> GetTypes(this Assembly assembly, Func<Type, bool> selector)
        {
            if (assembly == null) throw new ArgumentNullException();

            return assembly.GetTypes().Where(selector);
        }

        /// <summary>
        /// Get the assembly types with selection and ordering.
        /// </summary>
        /// <param name="assembly">Assembly with types</param>
        /// <param name="selector">Func to select types by need</param>
        /// <param name="order">Func to order selected types</param>
        /// <returns>IEnumerable of selected types</returns>
        /// <exception cref="ArgumentNullException">Occurs when assembly = null</exception>
        public static IEnumerable<Type> GetTypes(this Assembly assembly, Func<Type, bool> selector, Func<Type, string> order)
        {
            if (assembly == null) throw new ArgumentNullException();
            
            return assembly.GetTypes().Where(selector).OrderBy(order);
        }
    }
}
