using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace yakov.ThreadPool
{
    public sealed class TaskQueue : IThreadPool, IDisposable
    {
        public TaskQueue(int maxThreadsCount)
        {
            MaxThreadsCount = maxThreadsCount;
            _taskListener = new(Listen);
            _taskListener.Start();
        }

        public readonly int MaxThreadsCount;
        private List<Thread> _threads = new();
        private void AddActionThread(Action task)
        {
            Thread newActionThread = new(new ThreadStart(task));
            _threads.Add(newActionThread);
            newActionThread.Start();
        }

        private Queue<Action> _tasks = new();
        public void EnqueueTask(Action newTask)
        {
            newTask += TaskCallback;
            _tasks.Enqueue(newTask);
        }

        private void TaskCallback()
        {
            var currentThread = Thread.CurrentThread;
            _threads.Remove(currentThread);
            currentThread.Abort();
        }

        private Thread _taskListener;
        private void Listen()
        {
            while (true)
            {
                if (_tasks.TryDequeue(out Action? currentTask))
                {
                    
                }
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
