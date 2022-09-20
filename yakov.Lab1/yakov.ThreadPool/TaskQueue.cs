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
        }

        public readonly int MaxThreadsCount;
        private List<Thread> _threads = new();


        private Queue<Action> _tasks = new();
        public void AddTask(Action newTask)
        {
            _tasks.Enqueue(newTask);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
