
using System.Runtime.InteropServices;
using yakov.Lab2;

//IntPtr desc = Marshal.AllocHGlobal(20);

//OSHandle oSHandle = new();
//oSHandle.Handle = desc;
//oSHandle.Dispose();

//int x = 0;
//yakov.Lab2.Mutex mutexObj = new();

//for (int i = 1; i < 6; i++)
//{
//    Thread myThread = new(Print);
//    myThread.Name = $"Поток {i}";
//    myThread.Start();
//}

//void Print()
//{
//    mutexObj.Lock();
//    x = 1;
//    for (int i = 1; i < 6; i++)
//    {
//        Console.WriteLine($"{Thread.CurrentThread.Name}: {x}");
//        x++;
//        Thread.Sleep(50);
//    }
//    mutexObj.Unlock();
//}