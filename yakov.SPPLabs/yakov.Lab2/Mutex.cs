
using NLog;

namespace yakov.Lab2
{
    /// <summary>
    /// Realize mutex for thread locks.
    /// </summary>
    public class Mutex : IMutex
    {
        /// <summary>
        /// Logger.
        /// </summary>
        private static Logger _logger = LogManager.GetCurrentClassLogger();

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
            {
                _logger.Info($"{currentThreadId} wait.");                
                Thread.Sleep(10);
            }
            _logger.Info($"{currentThreadId} locked.");
        }

        /// <summary>
        /// Unlock mutex.
        /// </summary>
        public void Unlock()
        {
            var currentThreadId = Thread.CurrentThread.ManagedThreadId;
            if (Interlocked.CompareExchange(ref _activeThreadId, -1, currentThreadId) == -1)
            {
                _logger.Info($"{currentThreadId} unlocked.");
            }
        }
    }
}