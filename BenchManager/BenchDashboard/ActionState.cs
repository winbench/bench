using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mastersign.Bench.Dashboard
{
    public enum ActionState
    {
        None,
        BusyWithoutErrors,
        BusyWithErrors,
        BusyCanceled,
        FinishedWithoutErrors,
        FinishedWithErrors,
        Canceled,
    }
}
