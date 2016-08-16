using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Bench
{
    /// <summary>
    /// This class represents an information about a Bench task.
    /// It is a base class for specialized kinds of task information.
    /// Every task information contains a timestamp and a message.
    /// Optionally it can be associated with an app.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This class is the base class for a couple of specialized
    /// info types.
    /// </para>
    /// <list type="bullet">
    ///     <item><see cref="TaskProgress"/></item>
    ///     <item><see cref="TaskError"/></item>
    /// </list>
    /// </remarks>
    public class TaskInfo
    {
        /// <summary>
        /// The point in time, when this info was created.
        /// </summary>
        public DateTime Timestamp { get; private set; }

        /// <summary>
        /// The ID of the affected app, or <c>null</c>.
        /// </summary>
        public string AppId { get; private set; }

        /// <summary>
        /// A short message describing the event.
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// A detailed message describing the event, or <c>null</c>.
        /// </summary>
        public string DetailedMessage { get; private set; }

        /// <summary>
        /// Initializes a new instance of <see cref="TaskInfo"/>.
        /// </summary>
        /// <param name="message">A short message desccribing the event.</param>
        /// <param name="appId">The ID of the afected app or <c>null</c>.</param>
        /// <param name="detailedMessage">A detailed message describing the event or <c>null</c>.</param>
        public TaskInfo(string message, string appId = null, string detailedMessage = null)
        {
            if (message == null) throw new ArgumentNullException("message");
            Timestamp = DateTime.Now;
            AppId = appId;
            Message = message;
            DetailedMessage = detailedMessage;
        }
    }

    /// <summary>
    /// This class represents a progress update of a Bench task.
    /// </summary>
    public class TaskProgress : TaskInfo
    {
        /// <summary>
        /// The new progress value int the interval between <c>0.0</c> and <c>1.0</c>.
        /// </summary>
        public float Progress { get; private set; }

        /// <summary>
        /// Initializes a new instance of <see cref="TaskProgress"/>.
        /// </summary>
        /// <param name="message">A short message describing the event.</param>
        /// <param name="progress">The new task progress.</param>
        /// <param name="appId">Th ID of the affected app or <c>null</c>.</param>
        /// <param name="detailedMessage">A detailed message describing the event or <c>null</c>.</param>
        public TaskProgress(string message, float progress, string appId = null, string detailedMessage = null)
            : base(message, appId, detailedMessage)
        {
            Progress = progress;
        }

        /// <summary>
        /// Projects the progress value of this progress info into a sub-interval.
        /// </summary>
        /// <param name="globalBase">The offset of the sub-interval.</param>
        /// <param name="factor">The scale factor for the progress value.</param>
        /// <returns>A new progress info with the scaled progress value.</returns>
        public TaskProgress ScaleProgress(float globalBase, float factor)
        {
            return new TaskProgress(
                Message,
                globalBase + Progress * factor,
                AppId,
                DetailedMessage);
        }
    }

    /// <summary>
    /// This class represents an error which occurred during the execution of a Bench task.
    /// </summary>
    public class TaskError : TaskInfo
    {
        /// <summary>
        /// The exception causing the error, or <c>null</c>.
        /// </summary>
        public Exception Exception { get; private set; }

        /// <summary>
        /// Initializes a new instance of <see cref="TaskError"/>.
        /// </summary>
        /// <param name="message">A short message describing the error.</param>
        /// <param name="appId">Th ID of the affected app or <c>null</c>.</param>
        /// <param name="detailedMessage">A detailed message describing the error or <c>null</c>.</param>
        /// <param name="exception">The execption that caused the error or <c>null</c>.</param>
        public TaskError(string message, string appId = null, string detailedMessage = null, Exception exception = null)
            : base(message, appId, detailedMessage)
        {
            Exception = exception;
        }
    }
}
