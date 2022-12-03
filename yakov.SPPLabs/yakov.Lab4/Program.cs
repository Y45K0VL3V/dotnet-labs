using System.Reflection;
using yakov.AssemblyControl;
using yakov.Lab4;
using Parallel = yakov.Lab4.Parallel;

string path = @"C:\University\СПП\yakov.SPPLabs\yakov.Lab4\bin\Debug\net6.0\yakov.Lab4.dll";

AssemblyLoader assemblyLoader = new();
Assembly? loadedAssembly;
if ((loadedAssembly = assemblyLoader.LoadNew(path)) != null)
{
    var types = loadedAssembly.GetTypes((t) => t.IsPublic && t.GetCustomAttribute(typeof(ExportClassAttribute)) != null);

    if (types != null)
        foreach (var type in types)
            Console.WriteLine(type.FullName);
}

List<Action> actions = new();
for (int i = 0; i < 100; i++)
{
    int temp = i;
    Action action = () => Console.WriteLine(temp);
    actions.Add(action);
}

Parallel.WaitAll(actions.ToArray());