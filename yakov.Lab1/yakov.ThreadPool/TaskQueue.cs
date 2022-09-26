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
            lock (_threads)
            {
                if (_threads.Contains(currentThread))
                    _threads.Remove(currentThread);
            }
        }

        private Thread _taskListener;
        private void Listen()
        {
            while (true)
            {
                if (_tasks.TryDequeue(out Action? currentTask))
                {
                    while (_threads.Count >= MaxThreadsCount) { }
                    AddActionThread(currentTask);
                }
            }
        }

        private void ThreadAbort(Thread thread, bool isExeptionThrow)
        {
            try
            {
                thread.Abort();
            }
            catch (PlatformNotSupportedException)
            { }
            catch
            {
                if (isExeptionThrow)
                    throw;
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
                ThreadAbort(_taskListener, isExeptionThrow: false);

                lock (_threads)
                {
                    foreach (var thread in _threads)
                        ThreadAbort(thread, isExeptionThrow: false);
                }

                _tasks.Clear();
                _threads.Clear();
            }

            _disposed = true;
        }
    }
}
