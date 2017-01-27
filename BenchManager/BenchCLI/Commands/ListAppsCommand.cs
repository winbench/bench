using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Mastersign.CliTools;
using Mastersign.Docs;

namespace Mastersign.Bench.Cli.Commands
{
    class ListAppsCommand : BenchCommand
    {
        public override string Name => "apps";

        public enum AppSet
        {
            All,
            Active,
            NotActive,
            Activated,
            Deactivated,
            Installed,
            NotInstalled,
            Cached,
            NotCached,
            DefaultApps,
            MetaApps,
            ManagedPackages,
        }

        private const string OPTION_SET = "set";
        private const string OPTION_PROPERTIES = "properties";
        private const string OPTION_FILTER = "filter";
        private const string OPTION_SORT_BY = "sort-by";

        private static readonly AppSet DEF_SET = AppSet.All;
        private static readonly string DEF_PROPERTIES = string.Join(",",
            new[] { "ID", PropertyKeys.AppLabel, PropertyKeys.AppVersion, PropertyKeys.AppIsActive });
        private static readonly string DEF_FILTER = string.Empty;
        private static readonly string DEF_SORT_BY = "ID";

        public AppSet Set
            => (AppSet)Enum.Parse(
                    typeof(AppSet),
                    Arguments.GetOptionValue(OPTION_SET, DEF_SET.ToString()),
                    true);

        public string[] TableProperties
            => Arguments.GetOptionValue(OPTION_PROPERTIES, DEF_PROPERTIES)
                    .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

        public string[] Filter
            => Arguments.GetOptionValue(OPTION_FILTER, DEF_FILTER)
                    .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

        public string SortBy
            => Arguments.GetOptionValue(OPTION_SORT_BY, DEF_SORT_BY);

        protected override void InitializeArgumentParser(ArgumentParser parser)
        {
            parser.Description
                .Begin(BlockType.Paragraph)
                .Text("The ").Keyword(Name).Text(" command lists apps from the app library.")
                .End(BlockType.Paragraph)
                .Paragraph("You can specify the base set of apps and filter the apps to list.");

            var optionSet = new EnumOptionArgument<AppSet>(OPTION_SET, 's', DEF_SET);
            optionSet.Description
                .Text("Specifies the set of apps to list.");

            var optionProperties = new OptionArgument(OPTION_PROPERTIES, 'p', null);
            optionProperties.Description
                .Text("Specifies the properties to display in the listed output.")
                .Text(" This option only has an effect, if the flag ")
                .Keyword(Parent.Name).Text(" ").Keyword("--table").Text(" is set.");
            optionProperties.PossibleValueInfo
                .Text("A comma separated list of property names.");

            var optionFilter = new OptionArgument(OPTION_FILTER, 'f', null);
            optionFilter.Description
                .Text("Specifies a filter to reduce the number of listed apps.");
            optionFilter.PossibleValueInfo
                .Text("A comma separated list of criteria.")
                .Text(" E.g. ").Code("ID=JDK*,!IsInstalled,IsCached").Text(".");
            optionFilter.DefaultValueInfo
                .Text("no filter");

            var optionSortBy = new OptionArgument(OPTION_SORT_BY, 'o', null);
            optionSortBy.Description
                .Text("Specifies a property to sort the apps by.");
            optionSortBy.PossibleValueInfo
                .Text("The name of an app property.");
            optionSortBy.DefaultValueInfo
                .Text("ID");

            parser.RegisterArguments(
                optionSet,
                optionProperties,
                optionFilter,
                optionSortBy);
        }

        private DataOutputFormat Format => ((ListCommand)Parent).Format;

        private bool OutputAsTable => ((ListCommand)Parent).OutputAsTable;

        protected override bool ExecuteCommand(string[] args)
        {
            var cfg = LoadConfiguration();
            var apps = new List<Dictionary<string, object>>();
            var set = Set;
            var filter = Filter;
            var sortBy = SortBy;
            foreach (var app in cfg.Apps)
            {
                if (!IsIncludedInSet(app, set)) continue;
                var props = GetProperties(app);
                var match = true;
                foreach (var f in filter)
                {
                    if (!MatchesFilter(f, props))
                    {
                        match = false;
                        break;
                    }
                }
                if (match) apps.Add(props);
            }
            apps.Sort((o1, o2) =>
            {
                object v1 = null;
                object v2 = null;
                o1.TryGetValue(sortBy, out v1);
                o2.TryGetValue(sortBy, out v2);
                if (v1 == null && v2 == null) return 0;
                if (v1 == null) return -1;
                if (v2 == null) return 1;
                if (v1 is bool) return ((bool)v1).CompareTo((bool)v2);
                if (v1 is string) return ((string)v1).CompareTo((string)v2);
                return 0;
            });
            if (OutputAsTable)
            {
                using (var w = TableWriterFactory.Create(Format))
                {
                    w.Initialize(TableProperties);
                    foreach (var app in apps)
                    {
                        var values = new List<object>();
                        foreach (var item in TableProperties)
                        {
                            object v;
                            values.Add(app.TryGetValue(item, out v) ? v : null);
                        }
                        w.Write(values.ToArray());
                    }
                }
            }
            else
            {
                foreach (var app in apps)
                {
                    WriteLine((string)app["ID"]);
                }
            }
            return true;
        }

        private Dictionary<string, object> GetProperties(AppFacade app)
        {
            var result = new Dictionary<string, object>();
            result["ID"] = app.ID;
            foreach (var kvp in app.KnownProperties)
            {
                result[kvp.Key] = kvp.Value;
            }
            foreach (var kvp in app.UnknownProperties)
            {
                result[kvp.Key] = kvp.Value;
            }
            return result;
        }

        private bool IsIncludedInSet(AppFacade app, AppSet set)
        {
            switch (set)
            {
                case AppSet.All: return true;
                case AppSet.Active: return app.IsActive;
                case AppSet.NotActive: return !app.IsActive;
                case AppSet.Activated: return app.IsActivated;
                case AppSet.Deactivated: return app.IsDeactivated;
                case AppSet.Installed: return app.IsInstalled;
                case AppSet.NotInstalled: return !app.IsInstalled;
                case AppSet.Cached: return app.IsResourceCached;
                case AppSet.NotCached: return app.HasResource && !app.IsResourceCached;
                case AppSet.DefaultApps: return app.Typ == AppTyps.Default;
                case AppSet.MetaApps: return app.Typ == AppTyps.Meta;
                case AppSet.ManagedPackages: return app.IsManagedPackage;
                default: throw new NotSupportedException();
            }
        }

        private Regex CreatePattern(string pattern)
        {
            var parts = pattern.Split(new[] { '*' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < parts.Length; i++)
            {
                parts[i] = Regex.Escape(parts[i]);
            }
            var pattern2 = string.Join(".*", parts);
            if (!pattern.StartsWith("*")) pattern2 = "^" + pattern2;
            if (!pattern.EndsWith("*")) pattern2 = pattern2 + "$";
            return new Regex(pattern2);
        }

        private bool MatchesFilter(string filter, Dictionary<string, object> properties)
        {
            if (filter.Contains("="))
            {
                var equalPos = filter.IndexOf("=");
                var property = filter.Substring(0, equalPos).Trim();
                var pattern = filter.Substring(equalPos + 1).Trim();
                object v;
                if (!properties.TryGetValue(property, out v)) return false;
                var text = v as string;
                if (string.IsNullOrEmpty(text)) return pattern.Length == 0 || pattern == "*";
                return CreatePattern(pattern).IsMatch(text);
            }
            else
            {
                var neg = filter.StartsWith("!");
                if (neg) filter = filter.Substring(1);
                object v;
                if (properties.TryGetValue(filter, out v))
                {
                    return (v is bool) && (neg ? !((bool)v) : (bool)v);
                }
                return false;
            }
        }
    }
}
