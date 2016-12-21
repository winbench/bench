using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Mastersign.Bench
{
    internal class TaskInfoLogger : IDisposable
    {
        private bool onlyErrors;
        private TextWriter writer;

        public TaskInfoLogger(string file, bool onlyErrors)
        {
            this.onlyErrors = onlyErrors;
            writer = new StreamWriter(file, false, Encoding.UTF8);
        }

        public void Dispose()
        {
            if (writer != null)
            {
                writer.Dispose();
                writer = null;
            }
        }

        public void Log(TaskInfo info)
        {
            if (writer == null) return;
            if (onlyErrors && !(info is TaskError)) return;
            writer.WriteLine("[{0}] {1}: ({2}) {3}",
                info.Timestamp.ToString("yyyy-MM-dd HH-mm-ss"),
                info is TaskError ? "ERROR" : "INFO",
                info.AppId ?? "global",
                info.Message);
            if (info.DetailedMessage != null)
            {
                writer.WriteLine("[{0}] DETAIL:",
                    info.Timestamp.ToString("yyyy-MM-dd HH-mm-ss"));
                writer.WriteLine(info.DetailedMessage);
            }
            var err = info as TaskError;
            if (err != null)
            {
                if (err.Exception != null)
                {
                    writer.Write("[{0}] EXCEPTION: ",
                        info.Timestamp.ToString("yyyy-MM-dd HH-mm-ss"));
                    writer.WriteLine(err.Exception.ToString());
                }
            }
            writer.Flush();
        }
    }
}
