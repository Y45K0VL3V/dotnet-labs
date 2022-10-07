using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace yakov.ThreadPool
{
    public interface IDynamicThreadPool
    {
        public event Action OnTaskComplete;

        public uint MaxThreadsCount { get; set; }

        //public uint GetLeftTasksCount();
        public void EnqueueTask(Action newTask);
    }
}
