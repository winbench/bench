using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Mastersign.Bench.PropertyCollections;

namespace Mastersign.Bench
{
    /// <summary>
    /// <para>A collection of Bench apps.</para>
    /// <para>
    /// This class provides a facade to handle the properties and the states of the bench apps
    /// in an object-oriented fashion.</para>
    /// </summary>
    public class AppIndexFacade : IEnumerable<AppFacade>
    {
        private readonly IConfiguration Config;
        private readonly IObjectLibrary AppIndex;

        private Dictionary<string, AppFacade> cache = new Dictionary<string, AppFacade>();

        /// <summary>
        /// Initializes a new instance of <see cref="AppIndexFacade"/>.
        /// </summary>
        /// <param name="config">The Bench configuration properties.</param>
        /// <param name="appIndex">An instance of <see cref="IObjectLibrary"/> holding the configuration of Bench apps.</param>
        public AppIndexFacade(IConfiguration config, IObjectLibrary appIndex)
        {
            Config = config;
            AppIndex = appIndex;
        }

        private AppFacade GetAppFacade(string appName)
        {
            AppFacade app;
            if (!cache.TryGetValue(appName, out app))
            {
                app = new AppFacade(Config, AppIndex, appName);
                cache.Add(appName, app);
            }
            return app;
        }
        /// <summary>
        /// Gets an instance of <see cref="AppFacade"/> for the specified app.
        /// </summary>
        /// <param name="appName">The ID of an app.</param>
        /// <returns>The facade for the app, or <c>null</c>.</returns>
        public AppFacade this[string appName]
        {
            get { return Exists(appName) ? GetAppFacade(appName) : null; }
        }

        /// <summary>
        /// Gets a collection with <see cref="AppFacade"/> objects for multiple apps.
        /// </summary>
        /// <remarks>
        /// If an app ID can not be found, a <c>null</c> is placed in the returned collection.
        /// Therefore, the returned collection has always the same number of items as the given enumeration.
        /// </remarks>
        /// <param name="appNames">An enumeration with app IDs.</param>
        /// <returns>A collection with facades.</returns>
        public ICollection<AppFacade> GetApps(IEnumerable<string> appNames)
        {
            var result = new List<AppFacade>();
            foreach (var appName in appNames) result.Add(GetAppFacade(appName));
            return result;
        }

        /// <summary>
        /// Checks whether an app ID exists in the app index.
        /// </summary>
        /// <param name="appName">The app ID.</param>
        /// <returns><c>true</c> if the app was found; otherwise <c>false</c>.</returns>
        public bool Exists(string appName)
        {
            return AppIndex.ContainsGroup(appName);
        }

        /// <summary>
        /// Gets an array with facades for all apps.
        /// </summary>
        public AppFacade[] All
        {
            get
            {
                var result = new List<AppFacade>();
                foreach (var appName in AppIndex.Groups())
                {
                    result.Add(GetAppFacade(appName));
                }
                return result.ToArray();
            }
        }

        /// <summary>
        /// Gets all apps of a given category.
        /// </summary>
        /// <param name="category">The app category.</param>
        /// <returns>An array with facades for all apps in the given category.</returns>
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

        /// <summary>
        /// Gets an array with facades for all active apps.
        /// </summary>
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

        /// <summary>
        /// Gets an array with facades for all inactive apps.
        /// </summary>
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

        /// <summary>
        /// Gets an array with facades for all apps required by Bench itself.
        /// </summary>
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

        /// <summary>
        /// Gets the facades for all apps in the index.
        /// </summary>
        /// <returns>An enumerator with <see cref="AppFacade"/> objects.</returns>
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

        /// <summary>
        /// Gets an array with the paths for environment registration of all activated apps.
        /// </summary>
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

        /// <summary>
        /// Gets a dictionary with the merged environment variables of all activated apps.
        /// That excludes the <c>PATH</c> environment variable, which is handeled
        /// separatly in <see cref="EnvironmentPath"/>.
        /// </summary>
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
