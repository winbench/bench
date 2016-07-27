using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Bench
{
    /// <summary>
    /// <para>This class represents a possible cancelation of a running task.</para>
    /// <para>
    /// Using the method <see cref="Cancel"/>, the running task can be cancelled.
    /// And with the property <see cref="IsCanceled"/> can be checked whether the task was canceled.
    /// </para>
    /// </summary>
    /// <remarks>This class is <strong>not thead safe</strong>.</remarks>
    public class Cancelation
    {
        /// <summary>
        /// This event is fired, when the related task is cancelled.
        /// </summary>
        public event EventHandler Canceled;

        private volatile bool isCanceled;

        /// <summary>
        /// Checks whether the related task is cancelled or not.
        /// </summary>
        public bool IsCanceled { get { return isCanceled; } }

        /// <summary>
        /// Requests the cancelation of the related task.
        /// </summary>
        /// <remarks>
        /// This method can be called multiple times.
        /// </remarks>
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
