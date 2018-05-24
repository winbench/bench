using Mastersign.Bench.Markdown;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mastersign.Bench.Dashboard
{
    public class WindowPositionManager
    {
        private class Position
        {
            public FormWindowState State { get; set; }

            public Rectangle Bounds { get; set; }

            public Position(FormWindowState state, Rectangle bounds)
            {
                State = state;
                Bounds = bounds;
            }

            public void Apply(Form form)
            {
                if (Bounds.IsEmpty) return;
                form.Bounds = Bounds;
                form.WindowState = State;
            }

            public static Position FromForm(Form form)
                => new Position(
                    form.WindowState, 
                    form.WindowState == FormWindowState.Minimized 
                        ? Rectangle.Empty 
                        : form.Bounds);

            private static readonly Regex pattern = new Regex(
                @"^(?<state>Minimized|Normal|Maximized),\s*(?<x>[+-]?\d+),\s*(?<y>[+-]?\d+),\s*(?<w>[+-]?\d+),\s*(?<h>[+-]?\d+)$",
                RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture);

            public static Position Parse(string v, Position def)
            {
                if (string.IsNullOrWhiteSpace(v)) return def;
                var m = pattern.Match(v.Trim());
                if (!m.Success) return def;
                return new Position(
                    (FormWindowState)Enum.Parse(typeof(FormWindowState), m.Groups["state"].Value),
                    new Rectangle(
                        int.Parse(m.Groups["x"].Value),
                        int.Parse(m.Groups["y"].Value),
                        int.Parse(m.Groups["w"].Value),
                        int.Parse(m.Groups["h"].Value)));
            }

            public override string ToString()
                => Bounds.IsEmpty
                    ? "default"
                    : $"{State}, {Bounds.X}, {Bounds.Y}, {Bounds.Width}, {Bounds.Height}";
        }

        private class FormConfig
        {
            public string PropertyName { get; private set; }
            public Position DefaultPosition { get; private set; }

            public FormConfig(string propertyName, Position defaultPosition)
            {
                PropertyName = propertyName;
                DefaultPosition = defaultPosition;
            }
        }

        private readonly Core core;

        private readonly Dictionary<Form, FormConfig> forms = new Dictionary<Form, FormConfig>();

        public WindowPositionManager(Core core)
        {
            this.core = core;
        }

        private bool SavePositions => core.Config.GetBooleanValue(ConfigPropertyKeys.DashboardSavePositions);

        public void RegisterForm(Form form, string propertyName, Rectangle defaultPosition, FormWindowState defaultState)
        {
            forms.Add(form, new FormConfig(propertyName, new Position(defaultState, defaultPosition)));
            form.Load += FormLoadHandler;
            form.FormClosed += FormClosedHandler;
        }

        public void RegisterForm(Form form, string propertyName)
            => RegisterForm(form, propertyName, Rectangle.Empty, FormWindowState.Normal);

        public void UnregisterForm(Form form)
        {
            if (!forms.ContainsKey(form)) return;
            form.FormClosed -= FormClosedHandler;
            form.Load -= FormLoadHandler;
            forms.Remove(form);
        }

        private void FormLoadHandler(object sender, EventArgs e)
        {
            if (!(sender is Form form)) return;
            var formConfig = forms[form];
            var propertyValue = core.Config.GetStringValue(formConfig.PropertyName);
            var position = Position.Parse(propertyValue, formConfig.DefaultPosition);
            position.Apply(form);
        }

        private void FormClosedHandler(object sender, EventArgs e)
        {
            if (!SavePositions || !(sender is Form form)) return;
            var formConfig = forms[form];
            var position = Position.FromForm(form);
            if (position.Bounds.IsEmpty) return;
            var configFile = core.Config.GetStringValue(ConfigPropertyKeys.UserConfigFile);
            MarkdownPropertyEditor.UpdateFile(configFile, new Dictionary<string, string>
                { { formConfig.PropertyName, position.ToString() } });
            UnregisterForm(form);
        }
    }
}
