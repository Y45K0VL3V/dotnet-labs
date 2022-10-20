using System.Runtime.InteropServices;

namespace yakov.Lab2
{
    /// <summary>
    /// Provides control over extern handles.
    /// </summary>
    public class OSHandle : IDisposable
    {
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

                CloseHandle(Handle);
                Handle = IntPtr.Zero;

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
