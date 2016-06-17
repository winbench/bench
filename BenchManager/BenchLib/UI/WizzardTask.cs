using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Bench.UI
{
    public abstract class WizzardTask
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
