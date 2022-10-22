using NLog;
using System.Runtime.InteropServices;

namespace yakov.Lab2
{
    /// <summary>
    /// Provides control over extern handles.
    /// </summary>
    public class OSHandle : IDisposable
    {
        /// <summary>
        /// Logger.
        /// </summary>
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Current handle.
        /// </summary>
        public IntPtr Handle { get; set; }

        /// <summary>
        /// Close the handle.
        /// </summary>
        /// <param name="handle">Handle to close</param>
        /// <returns>True, if handle closed</returns>
        [DllImport("Kernel32")]
        private extern static Boolean CloseHandle(IntPtr handle);
        
        /// <summary>
        /// Auto-closing handle.
        /// </summary>
        ~OSHandle()
        {
            Dispose(disposing: false);
        }

        #region IDisposable
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // Close managed resources.
                }

                try 
                {
                    if (CloseHandle(Handle))
                    {
                        Handle = IntPtr.Zero;
                        Console.WriteLine("Descriptor closed.");
                        _logger.Info("Descriptor closed.");
                    }
                    else
                    {
                        Console.WriteLine("Descriptor not closed.");
                        _logger.Info("Descriptor not closed.");
                    }
                }
                catch (Exception ex) 
                {
                    _logger.Error(ex.Message);
                }
                
                disposed = true;
            }
        }

        /// <summary>
        /// Manual close handle.
        /// </summary>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
