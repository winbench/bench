using System;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Windows.Forms;

namespace Mastersign.Bench.Setup
{
    internal static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var form = new Form
            {
                Text = "Bench Setup",
                Icon = new Icon(GetResourceStream("BenchSetup.ico")),
                Width = 400,
                Height = 300,
                ShowIcon = true,
                ShowInTaskbar = true,
                StartPosition = FormStartPosition.CenterScreen,
                MaximizeBox = false,
                MinimizeBox = false,
            };
            form.Load += delegate
            {
                form.BeginInvoke((Action)(() =>
                {
                    Setup(form, true, false);
                    form.Close();
                }));
            };
            var ctx = new ApplicationContext(form);
            Application.Run(ctx);
        }

        static Stream GetResourceStream(string path)
        {
            var assembly = typeof(Program).Assembly;
            var ns = typeof(Program).Namespace;
            return assembly.GetManifestResourceStream(ns + "." + path);
        }

        static void Setup(Form mainForm, bool interactive, bool force)
        {
            var sysRootPath = Environment.GetEnvironmentVariable("SystemDrive") + "\\";
            var dlg = new FolderBrowserDialog
            {
                Description =
                    "Choose the target directory for the Bench environment. " +
                    "You probably have to create a new directory for that." + Environment.NewLine +
                    "A good default location is: " + sysRootPath + "Bench",
                ShowNewFolderButton = true,
                RootFolder = Environment.SpecialFolder.MyComputer,
                SelectedPath = sysRootPath,
            };
            if (dlg.ShowDialog(mainForm) != DialogResult.OK) return;
            var targetDir = dlg.SelectedPath;
            var a = new ZipArchive(GetResourceStream("Bench.zip"));
            var collision = false;
            if (Directory.Exists(targetDir))
            {
                foreach (var entry in a.Entries)
                {
                    if (File.Exists(Path.Combine(targetDir, entry.FullName)))
                    {
                        collision = true;
                        break;
                    }
                }
                if (collision)
                {
                    if (!force)
                    {
                        if (interactive)
                        {
                            if (MessageBox.Show(mainForm,
                                "The target directory already contains Bench files." +
                                " Do you want to replace existing files?",
                                "Bench Setup",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Warning) != DialogResult.Yes)
                            {
                                MessageBox.Show(mainForm,
                                    "Bench Setup cancelled.",
                                    "Bench Setup", MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                                Environment.Exit(1);
                            }
                        }
                        else
                        {
                            Environment.Exit(1);
                        }
                    }

                    foreach (var entry in a.Entries)
                    {
                        var filePath = Path.Combine(targetDir, entry.FullName);
                        try
                        {
                            File.Delete(filePath);
                        }
                        catch (Exception ex)
                        {
                            if (interactive)
                            {
                                MessageBox.Show(mainForm,
                                    "Failed to delete file in target directory:" + Environment.NewLine +
                                    ex.Message + Environment.NewLine +
                                    Environment.NewLine +
                                    filePath + Environment.NewLine +
                                    Environment.NewLine +
                                    "Bench Setup cancelled.",
                                    "Bench Setup",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                            }
                            Environment.Exit(2);
                        }
                    }
                }
            }
            else
            {
                Directory.CreateDirectory(targetDir);
            }
            try
            {
                a.ExtractToDirectory(targetDir);
            }
            catch (Exception ex)
            {
                if (interactive)
                {
                    MessageBox.Show(mainForm,
                        "Failed to extract Bench files:" + Environment.NewLine +
                        ex.Message,
                        "Bench Setup",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
                Environment.Exit(3);
            }

            // TODO call environment setup auto\bin\bench.exe --verbose transfer install
        }
    }
}
