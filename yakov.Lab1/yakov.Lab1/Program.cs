using yakov.Lab1.Directory;
using yakov.Lab1.Directory.Validator;
using yakov.ThreadPool;

string srcPath, destPath;

try
{
    srcPath = args?[0];
    destPath = args?[1];
}
catch (IndexOutOfRangeException ex)
{
    Console.WriteLine("Parameters:\n" +
        "1 - source path\n" +
        "2 - destination path\n");

    throw ex;
}

TaskQueue taskQueue = new(5);
DirectoryCopier directoryCopier = new(taskQueue);

DirectoryValidator validator = new();



