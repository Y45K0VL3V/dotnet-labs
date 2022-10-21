
namespace yakov.Lab2
{
    /// <summary>
    /// Realize mutex for thread locks.
    /// </summary>
    public class Mutex : IMutex
    {
        /// <summary>
        /// Contains thread's ID, that locked mutex. 
        /// ID = -1 means mutex not locked.
        /// </summary>
        private int _activeThreadId = -1;
        
        /// <summary>
        /// Lock mutex.
        /// </summary>
        public void Lock()
        {
            var currentThreadId = Thread.CurrentThread.ManagedThreadId;
            while (Interlocked.CompareExchange(ref _activeThreadId, currentThreadId, -1) != -1)
                Thread.Sleep(10);
        }

        /// <summary>
        /// Unlock mutex.
        /// </summary>
        public void Unlock()
        {
            var currentThreadId = Thread.CurrentThread.ManagedThreadId;
            Interlocked.CompareExchange(ref _activeThreadId, -1, currentThreadId);
        }
    }
}