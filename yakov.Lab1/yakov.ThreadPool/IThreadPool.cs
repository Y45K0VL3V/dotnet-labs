using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace yakov.ThreadPool
{
    public interface IThreadPool
    {
        void AddTask(Action newTask);

    }
}
