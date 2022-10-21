using System.Reflection;
using yakov.AssemblyControl;
using yakov.Lab4;

Console.WriteLine("Enter assembly path:");
string path = Console.ReadLine();

AssemblyLoader assemblyLoader = new();
Assembly? loadedAssembly;
if ((loadedAssembly = assemblyLoader.LoadNew(path)) != null)
{
    var types = loadedAssembly.GetTypes((t) => t.IsPublic && t.GetCustomAttribute(typeof(ExportClassAttribute)) != null);

    if (types != null)
        foreach (var type in types)
            Console.WriteLine(type.FullName);
}