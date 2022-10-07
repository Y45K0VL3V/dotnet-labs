using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace yakov.ThreadPool
{
    public class TaskQueue : IDynamicThreadPool, IDisposable
    {
        /// <summary>
        /// Create TaskQueue object.
        /// </summary>
        /// <param name="maxThreadsCount">Maximum amount of threads to set.</param>
        public TaskQueue(uint maxThreadsCount)
        {
            MaxThreadsCount = maxThreadsCount;
        }

        /// <summary>
        /// Event, that is invokes, when thread complete any action.
        /// </summary>
        public event Action OnTaskComplete;

        /// <summary>
        /// Determines maximum amount of threads, that can be created in this pool.
        /// </summary>
        private uint _maxThreadsCount;
        /// <summary>
        /// Contains thread IDs and connected to these threads cancellation tokens.
        /// </summary>
        private ConcurrentDictionary<int, CancellationTokenSource> _threadsState = new();
        /// <summary>
        /// Contains tasks to invoke.
        /// </summary>
        private ConcurrentQueue<Action> _tasks = new();

        /// <summary>
        /// Maximum threads amount control.
        /// Dynamic create/remove of threads.
        /// </summary>
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

        /// <summary>
        /// Mechanism for creating/removing threads.
        /// </summary>
        /// <param name="destValue">Destination value of threads.</param>
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
                        i++;
                }
            }
        }

        /// <summary>
        /// Listen method for each thread of pool.
        /// While not closed try to get new task from queue.
        /// </summary>
        /// <param name="cancellationToken">Token to request for close.</param>
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

        /// <summary>
        /// Close all threads.
        /// </summary>
        private void ThreadsStopRequest() => MaxThreadsCount = 0;

        /// <summary>
        /// Basic event handler for task complete.
        /// Logging information.
        /// </summary>
        protected virtual void TaskComplete()
        {
            Console.WriteLine("Complete");
        }

        /// <summary>
        /// Adding new task.
        /// </summary>
        /// <param name="newTask">Task to add.</param>
        /// <exception cref="ObjectDisposedException">Throws when pool object disposed.</exception>
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
