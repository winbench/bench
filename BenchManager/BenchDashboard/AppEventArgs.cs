using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Bench.Dashboard
{
    public class AppEventArgs : EventArgs
    {
        public string ID { get; private set; }

        public AppEventArgs(string appId)
        {
            ID = appId;
        }
    }
}
