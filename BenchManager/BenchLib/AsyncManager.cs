using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Mastersign.Bench
{
    public static class AsyncManager
    {
        public static Thread StartTask(ThreadStart action)
        {
            var t = new Thread(action);
            t.Start();
            return t;
        }
    }
}
