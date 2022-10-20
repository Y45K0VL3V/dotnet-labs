using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace yakov.Lab2
{
    public interface IMutex
    {
        /// <summary>
        /// Lock mutex.
        /// </summary>
        public void Lock();

        /// <summary>
        /// Unlock mutex.
        /// </summary>
        public void Unlock();
    }
}
