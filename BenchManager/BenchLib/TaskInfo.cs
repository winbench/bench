using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Bench
{
    public class TaskInfo
    {
        public DateTime Timestamp { get; private set; }

        public string AppId { get; private set; }

        public string Message { get; private set; }

        public string DetailedMessage { get; private set; }

        public TaskInfo(string message, string appId = null, string detailedMessage = null)
        {
            if (message == null) throw new ArgumentNullException("message");
            Timestamp = DateTime.Now;
            AppId = appId;
            Message = message;
            DetailedMessage = detailedMessage;
        }
    }

    public class TaskProgress : TaskInfo
    {
        public float Progress { get; private set; }

        public TaskProgress(string message, float progress, string appId = null, string detailedMessage = null)
            : base(message, appId, detailedMessage)
        {
            Progress = progress;
        }

        public TaskProgress ScaleProgress(float globalBase, float factor)
        {
            return new TaskProgress(
                Message,
                globalBase + Progress * factor,
                AppId,
                DetailedMessage);
        }
    }

    public class TaskError : TaskInfo
    {
        public Exception Exception { get; private set; }

        public TaskError(string message, string appId = null, string detailedMessage = null, Exception exception = null)
            : base(message, appId, detailedMessage)
        {
            Exception = exception;
        }
    }
}
