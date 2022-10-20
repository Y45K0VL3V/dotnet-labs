using System.Reflection;
using yakov.AssemblyControl;

Console.WriteLine("Enter assembly path:");
string path = Console.ReadLine();

AssemblyLoader assemblyLoader = new();
Assembly? loadedAssembly;
if ((loadedAssembly = assemblyLoader.LoadNew(path)) != null)
{
    var types = loadedAssembly.GetTypes((t) => t.IsPublic, (t) => t.Namespace + t.Name);

    if (types != null)
        foreach (var type in types)
            Console.WriteLine(type.FullName);
}