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
        /// <returns>True, if assembly loaded</returns>
        public bool LoadNew(string path)
        {
            try
            {
                var assembly = Assembly.LoadFrom(path);
                LoadedAssemblies.Add(assembly);
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}