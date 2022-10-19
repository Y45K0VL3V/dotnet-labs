using System.Collections.Concurrent;
using NLog;

namespace yakov.ThreadPool
{
    /// <summary>
    /// Implement ThreadPool logic.
    /// </summary>
    public class TaskQueue : IDynamicThreadPool, IDisposable
    {
        /// <summary>
        /// Create TaskQueue object.
        /// </summary>
        /// <param name="maxThreadsCount">Maximum amount of threads to set.</param>
        public TaskQueue(uint maxThreadsCount)
        {
            _logger.Info($"TaskQueue created.");
            MaxThreadsCount = maxThreadsCount;
        }

        /// <summary>
        /// Event, that is invokes, when thread complete any action.
        /// </summary>
        public event Action? OnTaskComplete;

        /// <summary>
        /// Object to lock.
        /// </summary>
        private object _lock = new();
        /// <summary>
        /// Logger.
        /// </summary>
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        /// <summary>
        /// Determines maximum amount of threads, that can be created in this pool.
        /// </summary>
        private uint _maxThreadsCount;
        /// <summary>
        /// Contains thread IDs and connected to these threads cancellation tokens.
        /// </summary>
        private ConcurrentDictionary<int, CancellationTokenSource> _threadsState = new();
        /// <summary>
        /// Amount of enqueued tasks for all time of pool.
        /// </summary>
        private uint _queuedTasksAmount;
        /// <summary>
        /// Amount of completed tasks for all time of pool.
        /// </summary>
        private uint _completedTasksAmount;
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

                _logger.Info($"Max threads amount - {value}.");
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
                    {
                        _logger.Info($"{threadState.Key} thread - request to close.");
                        threadState.Value.Cancel();
                    }
                }
            }
            else
            {
                for (int i = (int)threadsDifference; i != 0;)
                {
                    CancellationTokenSource tokenSource = new();
                    Thread thread = new Thread(() => Execute(tokenSource.Token));
                    thread.Start();
                    _logger.Info($"{thread.ManagedThreadId} thread - started.");

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
                    lock (_lock)
                    {
                        _completedTasksAmount++;
                    }
                }
            }

            _threadsState.TryRemove(Thread.CurrentThread.ManagedThreadId, out var tokenSource);
            _logger.Info($"{Thread.CurrentThread.ManagedThreadId} thread - closed.");
        }

        /// <summary>
        /// Close all threads.
        /// </summary>
        private void ThreadsStopRequest() => MaxThreadsCount = 0;

        /// <summary>
        /// Adding new task.
        /// </summary>
        /// <param name="newTask">Task to add.</param>
        /// <exception cref="ObjectDisposedException">Throws when pool object disposed.</exception>
        public void EnqueueTask(Action newTask)
        {
            if (_disposed) throw new ObjectDisposedException(null); 

            _tasks.Enqueue(newTask);
            _queuedTasksAmount++;
        }

        /// <summary>
        /// Returns if all tasks have completed.
        /// </summary>
        /// <returns></returns>
        public bool IsAllTasksComplete() => _queuedTasksAmount == _completedTasksAmount;

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
            _logger.Info("TaskQueue - disposed.");
        }
        #endregion
    }
}
