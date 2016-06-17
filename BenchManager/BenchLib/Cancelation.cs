using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Bench
{
    public class Cancelation
    {
        public event EventHandler Canceled;

        private volatile bool isCanceled;

        public bool IsCanceled { get { return isCanceled; } }
        public void Cancel()
        {
            if (isCanceled) return;
            var handler = Canceled;
            isCanceled = true;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }
    }
}
