using NLog;
using System.Runtime.InteropServices;
using System.Text;
using yakov.Lab2;

var file = File.Create("path.txt");
IntPtr desc = file.Handle;

OSHandle oSHandle = new();
oSHandle.Handle = desc;
oSHandle.Dispose();

Console.WriteLine("-------------------------------------");

int x = 0;
yakov.Lab2.Mutex mutexObj = new();

Logger logger = LogManager.GetCurrentClassLogger();

for (int i = 1; i < 6; i++)
{
    Thread myThread = new(Print);
    myThread.Start();
}

void Print()
{
    logger.Info($"Thread {Thread.CurrentThread.ManagedThreadId} - start");
    mutexObj.Lock();
    x = 1;
    for (int i = 1; i < 6; i++)
    {
        Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId}: {x}");
        x++;
        Thread.Sleep(50);
    }
    mutexObj.Unlock();
    logger.Info($"Thread {Thread.CurrentThread.ManagedThreadId} - finish");
}