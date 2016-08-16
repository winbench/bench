using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Bench
{
    /// <summary>
    /// This class represents the result of a Bench task.
    /// </summary>
    public class ActionResult
    {
        /// <summary>
        /// Is <c>true</c> if the execution of the task was successful.
        /// </summary>
        public bool Success { get; private set; }

        /// <summary>
        /// Is <c>true</c> if the execution of the task was canceled.
        /// </summary>
        public bool Canceled { get; private set; }

        /// <summary>
        /// An array with the IDs of all apps affected by the executed task.
        /// </summary>
        public string[] AffectedApps { get; private set; }

        /// <summary>
        /// A number of info objects created during the task execution.
        /// The info objects can be progress messages, infos or errors.
        /// </summary>
        public TaskInfo[] Infos { get; private set; }

        /// <summary>
        /// Initializes a new instance of <see cref="ActionResult"/>.
        /// </summary>
        /// <param name="infos">An enumeration of <see cref="TaskInfo"/> objects or <c>null</c>.</param>
        /// <param name="canceled">A flag, indicating if the task execution was canceled.</param>
        public ActionResult(IEnumerable<TaskInfo> infos = null, bool canceled = false)
        {
            if (infos == null) throw new ArgumentNullException("infos");

            var infoList = new List<TaskInfo>();
            var appList = new List<string>();
            Canceled = canceled;
            Success = true;
            foreach(var info in infos ?? new TaskInfo[0])
            {
                if (info.AppId != null)
                {
                    if (!appList.Contains(info.AppId)) appList.Add(info.AppId);
                    infoList.Add(info);
                    if (Success && info is TaskError) Success = false;
                }
            }
            Infos = infoList.ToArray();
            AffectedApps = appList.ToArray();
        }

        /// <summary>
        /// A filtered enumeration, containing only errors from <see cref="Infos"/>.
        /// </summary>
        public IEnumerable<TaskError> Errors
        {
            get
            {
                foreach (var info in Infos)
                {
                    if (info is TaskError) yield return (TaskError)info;
                }
            }
        }
    }
}
