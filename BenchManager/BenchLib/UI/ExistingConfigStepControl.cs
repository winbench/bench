using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Mastersign.Bench.UI
{
    /// <summary>
    /// A wizzard control, allowing the user to choose how the configuration
    /// is going to be initialized.
    /// </summary>
    internal partial class ExistingConfigStepControl : WizzardStepControl
    {
        public enum ConfigSourceType
        {
            None,
            Directory,
            ZipFile,
            GitRepo,
        }

        public ConfigSourceType ConfigSource
        {
            get
            {
                if (radExistingConfigInDirectory.Checked) return ConfigSourceType.Directory;
                if (radExistingConfigInZipFile.Checked) return ConfigSourceType.ZipFile;
                if (radExistingConfigInGitRepo.Checked) return ConfigSourceType.GitRepo;
                return ConfigSourceType.None;
            }
            set
            {
                if (ConfigSource == value) return;
                switch (value)
                {
                    case ConfigSourceType.Directory:
                        radExistingConfigInDirectory.Checked = true;
                        break;
                    case ConfigSourceType.ZipFile:
                        radExistingConfigInZipFile.Checked = true;
                        break;
                    case ConfigSourceType.GitRepo:
                        radExistingConfigInGitRepo.Checked = true;
                        break;
                    default:
                        radNoExistingConfig.Checked = true;
                        break;
                }
            }
        }

        public ExistingConfigStepControl()
        {
            Description = "Choose pre-existing configuration...";
            InitializeComponent();
            UpdateControls();
        }

        public string ConfigDirectory
        {
            get { return txtConfigTemplateDir.Text; }
            set { txtConfigTemplateDir.Text = value; }
        }

        public string ConfigZipFile
        {
            get { return txtConfigTemplateZipFile.Text; }
            set { txtConfigTemplateZipFile.Text = value;  }
        }

        public string ConfigGitRepo
        {
            get { return txtConfigGitRepo.Text; }
            set { txtConfigGitRepo.Text = value; }
        }

        private void ConfigSourceChangedHandler(object sender, EventArgs e) => UpdateControls();

        private void UpdateControls()
        {
            var existingConfigInDirectory = radExistingConfigInDirectory.Checked;
            var existingConfigInZipFile = radExistingConfigInZipFile.Checked;
            var existingConfigInGitRepo = radExistingConfigInGitRepo.Checked;
            lblConfigTemplateDir.Enabled = existingConfigInDirectory;
            txtConfigTemplateDir.Enabled = existingConfigInDirectory;
            btnConfigTemplateDirBrowse.Enabled = existingConfigInDirectory;
            lblConfigTemplateZipFile.Enabled = existingConfigInZipFile;
            txtConfigTemplateZipFile.Enabled = existingConfigInZipFile;
            btnConfigTemplateZipFileBrowse.Enabled = existingConfigInZipFile;
            lblConfigGitRepo.Enabled = existingConfigInGitRepo;
            txtConfigGitRepo.Enabled = existingConfigInGitRepo;
        }

        private void BrowseTemplateDirHandler(object sender, EventArgs e)
        {
            var dlg = new FolderBrowserDialog
            {
                Description = "Select the directory of a Bench environment or a directory with a config.md file in it.",
                SelectedPath = Directory.Exists(txtConfigTemplateDir.Text)
                    ? txtConfigTemplateDir.Text
                    : null,
            };
            var result = dlg.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                txtConfigTemplateDir.Text = dlg.SelectedPath;
            }
        }

        private void BrowseTemplateZipFileHandler(object sender, EventArgs e)
        {
            var dlg = new OpenFileDialog
            {
                CheckFileExists = true,
                Filter = "ZIP Archive (*.zip)|*.zip",
                Title = "Select ZIP file with Bench configuration...",
                InitialDirectory = File.Exists(txtConfigTemplateZipFile.Text)
                    ? Path.GetDirectoryName(txtConfigTemplateZipFile.Text) 
                    : null,
            };
            var result = dlg.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                txtConfigTemplateZipFile.Text = dlg.FileName;
            }
        }
    }
}
