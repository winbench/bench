using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Mastersign.Bench
{
    /// <summary>
    /// This static class contains convenience methods for handling threads.
    /// </summary>
    public static class AsyncManager
    {
        /// <summary>
        /// Create a new thread with the given <see cref="ThreadStart"/> and start it immediately.
        /// </summary>
        /// <param name="action">The action to run in the created thread.</param>
        /// <returns>The new <see cref="Thread"/> instance.</returns>
        public static Thread StartTask(ThreadStart action)
        {
            var t = new Thread(action);
            t.Start();
            return t;
        }
    }
}
