using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mastersign.Bench.Dashboard
{
    public partial class ExportForm : Form
    {
        private readonly IBenchManager man;

        public ExportForm(IBenchManager man)
        {
            this.man = man;
            InitializeComponent();
            picIcon.Image = new Icon(Icon, new Size(48, 48)).ToBitmap();
            InitializeSelection();
            UpdateSelectionControls();
            RegisterEventHandler();
        }

        private Dictionary<CheckBox, TransferPaths> selectionLookup;
        private Dictionary<TransferPaths, bool> availableSelection = new Dictionary<TransferPaths, bool>();
        private readonly Dictionary<TransferPaths, bool> exportSelection = new Dictionary<TransferPaths, bool>();
        private readonly Dictionary<TransferPaths, bool> cloneSelection = new Dictionary<TransferPaths, bool>();
        private string exportTarget;
        private string cloneTarget;

        private void InitializeSelection()
        {
            selectionLookup = new Dictionary<CheckBox, TransferPaths>
            {
                { chkUserConfiguration,  TransferPaths.UserConfiguration },
                { chkHomeDirectory, TransferPaths.HomeDirectory },
                { chkProjects, TransferPaths.ProjectDirectory },
                { chkAppLibraries, TransferPaths.AppLibraries },
                { chkRequiredCache, TransferPaths.RequiredAppResourceCache },
                { chkCache, TransferPaths.AppResourceCache },
                { chkRequiredApps, TransferPaths.RequiredApps },
                { chkApps, TransferPaths.Apps }
            };
            availableSelection = new Dictionary<TransferPaths, bool>
            {
                { TransferPaths.UserConfiguration, true },
                { TransferPaths.HomeDirectory, man.Config.CanTransferHomeDirectory },
                { TransferPaths.ProjectDirectory, man.Config.CanTransferProjectDirectory },
                { TransferPaths.AppLibraries, true },
                { TransferPaths.RequiredAppResourceCache, man.Config.CanTransferAppResourceCache },
                { TransferPaths.AppResourceCache, man.Config.CanTransferAppResourceCache },
                { TransferPaths.RequiredApps, true },
                { TransferPaths.Apps, true },
            };
            foreach (var s in selectionLookup.Values)
            {
                exportSelection[s] = false;
                cloneSelection[s] = false;
            }
            exportSelection[TransferPaths.UserConfiguration] = true;
            exportSelection[TransferPaths.AppLibraries] = true;
            cloneSelection[TransferPaths.UserConfiguration] = true;
            cloneSelection[TransferPaths.AppLibraries] = true;
            cloneSelection[TransferPaths.AppResourceCache] = man.Config.CanTransferAppResourceCache;
        }

        private void RegisterEventHandler()
        {
            foreach (var c in selectionLookup.Keys)
            {
                c.AutoCheck = false;
                c.Click += SelectionCheckBoxClickHandler;
            }
        }

        private void SelectionCheckBoxClickHandler(object sender, EventArgs e)
        {
            ToggleSelection(selectionLookup[(CheckBox)sender]);
        }

        public bool ExportMode => radExport.Checked;

        private Dictionary<TransferPaths, bool> SelectionSet => ExportMode ? exportSelection : cloneSelection;

        public TransferPaths ContentSelection
            => SelectionSet
                .Where(kvp => kvp.Value && availableSelection[kvp.Key])
                .Aggregate(TransferPaths.System, (s, kvp) => s | kvp.Key);

        private static bool Contains(TransferPaths a, TransferPaths b)
        {
            return (a & b) == b;
        }

        private bool ActivatedImplicitly(CheckBox chk)
        {
            var selection = selectionLookup[chk];
            foreach (var s in SelectionSet.Keys)
            {
                if (selection == s) continue;
                if (!SelectionSet[s]) continue;
                if (Contains(s, selection)) return true;
            }
            return false;
        }

        private void UpdateSelectionControls()
        {
            var commonSelection = ContentSelection;
            foreach (var c in selectionLookup.Keys)
            {
                var activatedImplicitly = ActivatedImplicitly(c);
                var selection = selectionLookup[c];
                var isActive = commonSelection.HasFlag(selection);
                if (c == chkHomeDirectory)
                    c.Enabled = man.Config.CanTransferHomeDirectory;
                else if (c == chkProjects)
                    c.Enabled = man.Config.CanTransferProjectDirectory;
                else if (c == chkRequiredCache)
                    c.Enabled = man.Config.CanTransferAppResourceCache && !activatedImplicitly;
                else if (c == chkCache)
                    c.Enabled = man.Config.CanTransferAppResourceCache && !activatedImplicitly;
                else
                    c.Enabled = !activatedImplicitly;
                c.Checked = isActive && availableSelection[selection];
            }
        }

        private void ToggleSelection(TransferPaths selection)
        {
            SelectionSet[selection] = !SelectionSet[selection];
            UpdateSelectionControls();
        }

        public string TargetPath
        {
            get { return ExportMode ? exportTarget : cloneTarget; }
            private set
            {
                if (ExportMode)
                    exportTarget = value;
                else
                    cloneTarget = value;
            }
        }

        private void UpdateTargetControls()
        {
            txtTarget.Text = TargetPath;
            btnBrowse.Image = ExportMode
                ? Properties.Resources.browse_file_16
                : Properties.Resources.browse_folder_16;
        }

        private void TargetTextChangedHandler(object sender, EventArgs e)
        {
            TargetPath = txtTarget.Text;
        }

        private void ModeSelectionChangedHandler(object sender, EventArgs e)
        {
            UpdateSelectionControls();
            UpdateTargetControls();
        }

        private void BrowseTargetClickHandler(object sender, EventArgs e)
        {
            if (ExportMode)
                BrowseForTargetFile();
            else
                BrowseForTargetDirectory();
        }

        private void BrowseForTargetFile()
        {
            var dlg = new SaveFileDialog
            {
                InitialDirectory = Environment.GetEnvironmentVariable("SystemDrive"),
                Title = "Save Bench environment transfer package...",
                OverwritePrompt = true,
                CheckPathExists = true,
                AddExtension = true,
                Filter = "SFX Archive (*.exe)|*.exe|7-Zip Archive (*.7z)|*.7z|ZIP Archive (*.zip)|*.zip",
                FilterIndex = 0,
                ValidateNames = true,
                FileName = txtTarget.Text,
            };
            var result = dlg.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                txtTarget.Text = dlg.FileName;
            }
        }

        private void BrowseForTargetDirectory()
        {
            var dlg = new FolderBrowserDialog
            {
                SelectedPath = string.IsNullOrWhiteSpace(txtTarget.Text)
                    ? Environment.GetEnvironmentVariable("SystemDrive")
                    : txtTarget.Text,
                Description = "Select the target directory for the new Bench environment.",
                ShowNewFolderButton = true,
            };
            var result = dlg.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                txtTarget.Text = dlg.SelectedPath;
            }
        }

        private void CancelHandler(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void OkHandler(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
