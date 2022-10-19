using yakov.Lab1.DirectoryControl;
using yakov.ThreadPool;

string? srcPath, destPath;

Console.WriteLine("Enter first path:");
srcPath = Console.ReadLine();

Console.WriteLine("Enter first path:");
destPath = Console.ReadLine();

TaskQueue taskQueue = new(10);
DirectoryCopier directoryCopier = new(taskQueue);

CopyOperationInfo copyInfo = directoryCopier.CopyTo(srcPath, destPath, true);
Console.WriteLine($"\n{copyInfo.SrcPath} - source \n" +
    $"{copyInfo.DestPath} - destination \n" +
    $"{copyInfo.CopyTime} - time to copy \n" +
    $"{copyInfo.CopiedFilesAmount} - files copied");

taskQueue.Dispose();


