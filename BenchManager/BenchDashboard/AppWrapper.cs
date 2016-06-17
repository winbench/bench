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
        public AppFacade App { get { return app; } }

        public string ID { get { return app.ID; } }

        public string Version { get { return app.Version; } }

        public string Launcher { get { return app.Launcher; } }

        public int Index { get { return no; } }

        public string Typ
        {
            get
            {
                switch (app.Typ)
                {
                    case AppTyps.Default:
                        return "Default";
                    case AppTyps.Meta:
                        return "Group/Custom";
                    case AppTyps.NodePackage:
                        return "NodeJS";
                    case AppTyps.RubyPackage:
                        return "Ruby";
                    case AppTyps.Python2Package:
                        return "Python 2";
                    case AppTyps.Python3Package:
                        return "Python 3";
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

        public string ShortStatus { get { return app.ShortStatus; } }

        public string LongStatus { get { return app.LongStatus; } }

        public string IsActive
        {
            get
            {
                return app.IsActivated ? "active" : (app.IsActive ? "implicit" : "inactive");
            }
        }

        public bool IsDeactivated { get { return app.IsDeactivated; } }

        public bool IsDependency { get { return app.IsDependency; } }

        public bool IsInstalled { get { return app.IsInstalled; } }

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
