using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Bench
{
    public interface IBenchManager
    {
        BenchConfiguration Config { get; }

        BenchEnvironment Env { get; }

        Downloader Downloader { get; }

        IUserInterface UI { get; }

        IProcessExecutionHost ProcessExecutionHost { get; }
    }
}
