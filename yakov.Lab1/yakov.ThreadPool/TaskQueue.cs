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
        public TaskQueue(uint maxThreadsCount)
        {
            MaxThreadsCount = maxThreadsCount;
        }

        public event Action OnTaskComplete;

        private uint _maxThreadsCount;
        private ConcurrentDictionary<int, CancellationTokenSource> _threadsState = new();
        private ConcurrentQueue<Action> _tasks = new();

        public uint MaxThreadsCount
        {
            get => _maxThreadsCount;
            set
            {
                if (_disposed) throw new ObjectDisposedException(null);

                ChangeMaxThreadsAmount(value);
                _maxThreadsCount = value;
            }
        }

        private void ChangeMaxThreadsAmount(uint destValue)
        {
            var threadsDifference = _threadsState.Count - destValue;

            if (threadsDifference > 0)
            {
                lock (_threadsState)
                {
                    foreach (var threadState in _threadsState.TakeLast((int)Math.Abs(threadsDifference)))
                        threadState.Value.Cancel();
                }
            }
            else
            {
                for (int i = (int)threadsDifference; i != 0;)
                {
                    CancellationTokenSource tokenSource = new();
                    Thread thread = new Thread(() => Execute(tokenSource.Token));

                    if (_threadsState.TryAdd(thread.ManagedThreadId, tokenSource))
                        i--;
                }
            }
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

            _threadsState.TryRemove(Thread.CurrentThread.ManagedThreadId, out var tokenSource);
        }

        private void ThreadsStopRequest() => MaxThreadsCount = 0;

        protected virtual void TaskComplete()
        {
            Console.WriteLine("Complete");
        }

        public void EnqueueTask(Action newTask)
        {
            if (_disposed) throw new ObjectDisposedException(null); 

            _tasks.Enqueue(newTask);
        }

        #region IDisposable
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
                ThreadsStopRequest();
                _tasks.Clear();
            }

            _disposed = true;
        }
        #endregion
    }
}
