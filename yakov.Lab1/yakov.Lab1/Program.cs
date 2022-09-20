using System.Transactions;
using yakov.ThreadPool;

var threadPool = new TaskQueue(5);
for (int i = 0; i < 50; i++)
    threadPool.EnqueueTask(Write);

threadPool.Dispose();
Console.ReadLine();

void Write()
{
    Console.WriteLine("dffd");
}