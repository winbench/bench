using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using Mastersign.Bench.Dashboard.Properties;

namespace Mastersign.Bench.Dashboard
{
    class AppWrapper : INotifyPropertyChanged
    {
        private readonly AppFacade app;
        private readonly int no;

        public AppWrapper(AppFacade app, int no)
        {
            this.app = app;
            this.no = no;
        }

        [Browsable(false)]
        public AppFacade App => app;

        public string ID => app.ID;

        public string Label => app.Label;

        public string Name => app.Name;

        public string Namespace => app.Namespace ?? "(default)";

        public string AppLibrary => app.AppLibrary?.ID ?? "user";

        public string Category => app.Category;

        public string Version 
            => app.IsInstalled && !app.IsVersionUpToDate
               ? app.InstalledVersion + " \u2192 " + app.Version
               : app.Version;

        public string License => app.License;

        public Uri LicenseUrl => app.LicenseUrl;

        public string Launcher => app.Launcher;

        public int Index => no;

        public string Typ
        {
            get
            {
                switch (app.Typ)
                {
                    case AppTyps.Default:
                        return "Default";
                    case AppTyps.Group:
                        return "Group";
                    case AppTyps.Meta:
                        return "Custom";
                    case AppTyps.NodePackage:
                        return "NodeJS";
                    case AppTyps.RubyPackage:
                        return "Ruby";
                    case AppTyps.PythonPackage:
                        return "Python";
                    case AppTyps.Python2Package:
                        return "Python 2";
                    case AppTyps.Python3Package:
                        return "Python 3";
                    case AppTyps.PythonWheel:
                        return "Python Wheel";
                    case AppTyps.Python2Wheel:
                        return "Python 2 Wheel";
                    case AppTyps.Python3Wheel:
                        return "Python 3 Wheel";
                    case AppTyps.NuGetPackage:
                        return "NuGet";
                    default:
                        return app.Typ;
                }
            }
        }

        public Bitmap StatusIcon
        {
            get
            {
                switch (app.StatusIcon)
                {
                    case AppStatusIcon.OK:
                        return Resources.ok_16;
                    case AppStatusIcon.Info:
                        return Resources.info_16;
                    case AppStatusIcon.Cached:
                        return Resources.cached_16;
                    case AppStatusIcon.Tolerated:
                        return Resources.tolerated_16;
                    case AppStatusIcon.Blocked:
                        return Resources.blocked_16;
                    case AppStatusIcon.Task:
                        return Resources.task_16;
                    case AppStatusIcon.Warning:
                        return Resources.warning_16;
                    default:
                        return Resources.none_16;
                }
            }
        }

        public string ShortStatus => app.ShortStatus;

        public string LongStatus => app.LongStatus;

        public string IsActive => app.IsActivated ? "activated" : (app.IsActive ? "implicit" : "inactive");

        public string IsSuppressed => app.IsDeactivated ? "deactivated" : (app.IsSuppressed ? "implicit" : "supported");

        public bool IsDependency => app.IsDependency;

        public bool IsInstalled => app.IsInstalled;

        public int SearchScore { private set; get; }

        public bool Match(string[] searchTokens)
        {
            SearchScore = App.MatchSearchString(searchTokens);
            return SearchScore > 0;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyChanges()
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs("StatusIcon"));
                handler(this, new PropertyChangedEventArgs("IsActive"));
                handler(this, new PropertyChangedEventArgs("IsDeactivated"));
                handler(this, new PropertyChangedEventArgs("IsInstalled"));
                handler(this, new PropertyChangedEventArgs("ShortStatus"));
                handler(this, new PropertyChangedEventArgs("LongStatus"));
            }
        }
    }
}
