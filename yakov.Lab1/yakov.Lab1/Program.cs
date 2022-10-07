using yakov.Lab1.DirectoryControl;
using yakov.ThreadPool;

string? srcPath, destPath;

srcPath = Console.ReadLine();
destPath = Console.ReadLine();

TaskQueue taskQueue = new(5);
DirectoryCopier directoryCopier = new(taskQueue);

directoryCopier.CopyTo(srcPath, destPath);


