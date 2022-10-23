using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using yakov.ThreadPool;

namespace yakov.Lab4
{
    public static class Parallel
    {
        public static void WaitAll(Action[] actions)
        {
            TaskQueue taskQueue = new((uint)actions.Length);
            foreach (var action in actions)
                taskQueue.EnqueueTask(action);

            while (!taskQueue.IsAllTasksComplete()) { }
        }
    }
}
