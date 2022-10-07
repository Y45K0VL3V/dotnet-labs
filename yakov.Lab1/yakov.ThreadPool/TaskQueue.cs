using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace yakov.ThreadPool
{
    public class TaskQueue : IThreadPool, IDisposable
    {
        public TaskQueue(int maxThreadsCount)
        {
            MaxThreadsCount = maxThreadsCount;

            for (int i = 0; i < maxThreadsCount; i++)
            {
                CancellationTokenSource tokenSource = new();
                Thread thread = new Thread(() => Execute(tokenSource.Token));
                _threadsState.Add(thread.ManagedThreadId, tokenSource);
            }
        }

        private int _maxThreadsCount;
        public int MaxThreadsCount
        {
            get => _maxThreadsCount;
            set
            {
                _maxThreadsCount = value;
            }
        }

        private Dictionary<int, CancellationTokenSource> _threadsState = new();

        public event Action OnTaskComplete;

        protected virtual void TaskComplete()
        { 

        }

        private ConcurrentQueue<Action> _tasks = new();
        public void EnqueueTask(Action newTask)
        {
            _tasks.Enqueue(newTask);
        }

        private void Execute(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                if (_tasks.TryDequeue(out Action? currentTask))
                {
                    currentTask.Invoke();
                    OnTaskComplete?.Invoke();
                }
            }
        }

        private bool _disposed = false;

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {

                _tasks.Clear();
                _threadsState.Clear();
            }

            _disposed = true;
        }
    }
}
