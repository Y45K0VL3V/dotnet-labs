using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace yakov.ThreadPool
{
    public interface IDynamicThreadPool
    {
        /// <summary>
        /// Actions to invoke on task end.
        /// </summary>
        public event Action OnTaskComplete;

        /// <summary>
        /// Maximum amount of threads.
        /// </summary>
        public uint MaxThreadsCount { get; set; }

        /// <summary>
        /// Add new task to queue.
        /// </summary>
        /// <param name="newTask">Task to add.</param>
        public void EnqueueTask(Action newTask);
        /// <summary>
        /// Show if all tasks complete.
        /// </summary>
        /// <returns>True, if all tasks, that were added - complete. Other way - false.</returns>
        public bool IsAllTasksComplete();
    }
}
