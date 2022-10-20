using System;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace yakov.AssemblyControl
{
    /// <summary>
    /// Provides loading assemblies (.exe & .dll).
    /// Saving them into list for next interactions.
    /// </summary>
    public class AssemblyLoader
    {
        /// <summary>
        /// Loaded assemblies.
        /// </summary>
        public List<Assembly> LoadedAssemblies { get; private set; } = new();

        /// <summary>
        /// Loads new assembly.
        /// </summary>
        /// <param name="path">Assembly path</param>
        /// <returns>Assembly instance if loaded successfully, else null</returns>
        public Assembly? LoadNew(string path)
        {
            Assembly? assembly;
            try
            {
                assembly = Assembly.LoadFrom(path);
                LoadedAssemblies.Add(assembly);
            }
            catch
            {
                return null;
            }

            return assembly;
        }
    }
}