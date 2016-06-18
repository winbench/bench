using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Bench
{
    public class ActionResult
    {
        public bool Success { get; private set; }

        public bool Canceled { get; private set; }

        public string[] AffectedApps { get; private set; }

        public TaskInfo[] Infos { get; private set; }

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
