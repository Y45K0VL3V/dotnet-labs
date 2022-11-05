using System.Reflection;
using yakov.AssemblyControl;
using yakov.Logger;

string path = @"C:\University\СПП\yakov.SPPLabs\yakov.Lab4\bin\Debug\net6.0\yakov.Lab4.dll"; 

AssemblyLoader assemblyLoader = new();
Assembly? loadedAssembly;
if ((loadedAssembly = assemblyLoader.LoadNew(path)) != null)
{
    var types = loadedAssembly.GetTypes(t => t.IsPublic, t => t.Namespace + t.Name);

    if (types != null)
        foreach (var type in types)
            Console.WriteLine(type.FullName);
}

LogBuffer logger = new(@"C:\Users\yakov\OneDrive\Рабочий стол\log.txt", 2000, 10);

int i = 1;
for (; i <= 10; i++)
    logger.Add($"string {i}");

for (; i <= 15; i++)
    logger.Add($"string {i}");

Thread.Sleep(3000);

Console.ReadLine();

