using System.Reflection;
using yakov.AssemblyControl;
using yakov.Lab4;
using yakov.ThreadPool;
using Parallel = yakov.Lab4.Parallel;

//Console.WriteLine("Enter assembly path:");
//string path = Console.ReadLine();

//AssemblyLoader assemblyLoader = new();
//Assembly? loadedAssembly;
//if ((loadedAssembly = assemblyLoader.LoadNew(path)) != null)
//{
//    var types = loadedAssembly.GetTypes((t) => t.IsPublic && t.GetCustomAttribute(typeof(ExportClassAttribute)) != null);

//    if (types != null)
//        foreach (var type in types)
//            Console.WriteLine(type.FullName);
//}

Parallel.WaitAll(new Action[] { () => Console.WriteLine("f"), () => Console.WriteLine("sdfsdf")});