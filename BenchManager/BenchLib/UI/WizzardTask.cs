using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Bench.UI
{
    /// <summary>
    /// The base class for tasks, which involve the interaction with the user via a wizzard.
    /// </summary>
    internal abstract class WizzardTask
    {
        public abstract WizzardStepControl[] StepControls { get; }

        public bool IsCanceled { get; private set; }

        public virtual void Before()
        {
            IsCanceled = false;
        }

        public virtual void After()
        {
            IsCanceled = false;
        }

        public virtual void Cancel()
        {
            IsCanceled = true;
        }
    }
}
