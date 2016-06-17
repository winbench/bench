using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Bench
{
    public class AppIndexFacade : IEnumerable<AppFacade>
    {
        private readonly IConfiguration AppIndex;

        private Dictionary<string, AppFacade> cache = new Dictionary<string, AppFacade>();

        private AppFacade GetAppFacade(string appName)
        {
            AppFacade app;
            if (!cache.TryGetValue(appName, out app))
            {
                app = new AppFacade(AppIndex, appName);
                cache.Add(appName, app);
            }
            return app;
        }

        public AppIndexFacade(IConfiguration appIndex)
        {
            AppIndex = appIndex;
        }

        public AppFacade this[string appName]
        {
            get { return Exists(appName) ? GetAppFacade(appName) : null; }
        }

        public ICollection<AppFacade> GetApps(IEnumerable<string> appNames)
        {
            var result = new List<AppFacade>();
            foreach (var appName in appNames) result.Add(GetAppFacade(appName));
            return result;
        }

        public bool Exists(string appName)
        {
            return AppIndex.ContainsGroup(appName);
        }

        public AppFacade[] ByCategory(string category)
        {
            var appNames = AppIndex.GroupsByCategory(category);
            var result = new List<AppFacade>();
            foreach (var appName in appNames)
            {
                result.Add(GetAppFacade(appName));
            }
            return result.ToArray();
        }

        public AppFacade[] ActiveApps
        {
            get
            {
                var result = new List<AppFacade>();
                foreach (var appName in AppIndex.Groups())
                {
                    var app = GetAppFacade(appName);
                    if (app.IsActive)
                    {
                        result.Add(app);
                    }
                }
                return result.ToArray();
            }
        }

        public AppFacade[] InactiveApps
        {
            get
            {
                var result = new List<AppFacade>();
                foreach (var appName in AppIndex.Groups())
                {
                    var app = GetAppFacade(appName);
                    if (!app.IsActive)
                    {
                        result.Add(app);
                    }
                }
                return result.ToArray();
            }
        }

        public AppFacade[] RequiredApps
        {
            get
            {
                var result = new List<AppFacade>();
                foreach (var appName in AppIndex.Groups())
                {
                    var app = GetAppFacade(appName);
                    if (app.IsRequired)
                    {
                        result.Add(app);
                    }
                }
                return result.ToArray();
            }
        }

        public IEnumerator<AppFacade> GetEnumerator()
        {
            foreach (var appName in AppIndex.Groups())
            {
                yield return GetAppFacade(appName);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public string[] EnvironmentPath
        {
            get
            {
                var result = new List<string>();
                foreach (var app in ActiveApps)
                {
                    if (!app.Register) continue;
                    foreach (var p in app.Path)
                    {
                        if (!result.Contains(p))
                        {
                            result.Add(p);
                        }
                    }
                }
                return result.ToArray();
            }
        }

        public IDictionary<string, string> Environment
        {
            get
            {
                var result = new Dictionary<string, string>();
                var apps = ActiveApps;
                foreach (var app in apps)
                {
                    var e = app.Environment;
                    if (e != null)
                    {
                        foreach (var k in e.Keys)
                        {
                            result[k] = e[k];
                        }
                    }
                }
                return result;
            }
        }
    }
}
