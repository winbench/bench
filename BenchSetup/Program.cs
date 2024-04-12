using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Windows.Forms;

namespace Mastersign.Bench.Setup
{
    internal static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var section = ConfigurationManager.GetSection("System.Windows.Forms.ApplicationConfigurationSection") as NameValueCollection;
            if (section != null)
            {
                section["DpiAwareness"] = "system";
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var form = CreateForm();
            Application.Run(form);
        }

        static string DefaultTargetDir
        {
            get { return Environment.GetEnvironmentVariable("SystemDrive") + @"\Bench"; }
        }

        static Stream GetResourceStream(string path)
        {
            var assembly = typeof(Program).Assembly;
            var ns = typeof(Program).Namespace;
            return assembly.GetManifestResourceStream(ns + "." + path);
        }

        static string GetResourceText(string path)
        {
            using (var s = GetResourceStream(path))
            using (var r = new StreamReader(s, Encoding.UTF8))
            {
                return r.ReadToEnd();
            }
        }

        static Form CreateForm()
        {
            var defaultFont = new Font("Segoe UI", 9f, FontStyle.Regular);
            var form = new Form
            {
                AutoScaleMode = AutoScaleMode.Font,
                AutoScaleDimensions = new SizeF(6F, 13F),
                Text = "Bench Setup",
                Icon = new Icon(GetResourceStream("BenchSetup.ico")),
                Width = 484,
                Height = 320,
                ShowIcon = true,
                ShowInTaskbar = true,
                StartPosition = FormStartPosition.CenterScreen,
                MaximizeBox = false,
                MinimizeBox = false,
                Padding = Padding.Empty,
                FormBorderStyle = FormBorderStyle.FixedDialog,
            };
            form.SuspendLayout();

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Margin = Padding.Empty,
            };
            layout.SuspendLayout();
            layout.RowCount = 5;
            layout.RowStyles.Add(new RowStyle { SizeType = SizeType.AutoSize });
            layout.RowStyles.Add(new RowStyle { SizeType = SizeType.Percent, Height = 100 });
            layout.RowStyles.Add(new RowStyle { SizeType = SizeType.AutoSize });
            layout.RowStyles.Add(new RowStyle { SizeType = SizeType.AutoSize });
            layout.RowStyles.Add(new RowStyle { SizeType = SizeType.AutoSize });
            layout.ColumnCount = 3;
            layout.ColumnStyles.Add(new ColumnStyle { SizeType = SizeType.AutoSize });
            layout.ColumnStyles.Add(new ColumnStyle { SizeType = SizeType.Percent, Width = 100 });
            layout.ColumnStyles.Add(new ColumnStyle { SizeType = SizeType.AutoSize });

            var header = new TableLayoutPanel
            {
                Height = 96,
                Dock = DockStyle.Fill,
                BackgroundImage = new Bitmap(GetResourceStream("Header.png")),
                BackgroundImageLayout = ImageLayout.None,
                Margin = Padding.Empty,
            };
            layout.Controls.Add(header);
            layout.SetRow(header, 0);
            layout.SetColumn(header, 0);
            layout.SetColumnSpan(header, 3);

            var titleText = new Label
            {
                Text = GetResourceText("Title.txt"),
                Font = new Font("Segoe UI", 18f, FontStyle.Regular),
                Margin = new Padding(24),
                AutoSize = true,
                BackColor = Color.Transparent,
                MaximumSize = new Size(form.ClientSize.Width - 48, header.Height - 48),
                AutoEllipsis = true,
            };
            header.Controls.Add(titleText);

            var infoTextContainer = new TableLayoutPanel
            {
                AutoScroll = true,
                AutoScrollMargin = new Size(0, 0),
                Dock = DockStyle.Fill,
                Margin = Padding.Empty,
                Padding = new Padding(0),
                Font = defaultFont,
                BackColor = SystemColors.Window,
            };
            infoTextContainer.SuspendLayout();
            infoTextContainer.RowCount = 1;
            infoTextContainer.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            infoTextContainer.ColumnCount = 1;
            infoTextContainer.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            layout.Controls.Add(infoTextContainer);
            layout.SetRow(infoTextContainer, 1);
            layout.SetColumn(infoTextContainer, 0);
            layout.SetColumnSpan(infoTextContainer, 3);

            var infoText = new Label
            {
                Text = GetResourceText("Info.txt"),
                Dock = DockStyle.Left,
                AutoSize = true,
                Margin = new Padding(8, 8, 8, 0),
                MaximumSize = new Size(form.ClientSize.Width - 32, int.MaxValue),
            };
            infoTextContainer.Controls.Add(infoText);
            infoTextContainer.SetRow(infoText, 0);
            infoTextContainer.SetColumn(infoText, 0);

            var targetTopMargin = 8;
            var targetLabel = new Label
            {
                Text = "Target Directory:",
                Margin = new Padding(8, 6 + targetTopMargin, 0, 0),
                Padding = Padding.Empty,
                AutoSize = true,
                Font = defaultFont,
            };
            layout.Controls.Add(targetLabel);
            layout.SetRow(targetLabel, 2);
            layout.SetColumn(targetLabel, 0);

            var targetTextBox = new TextBox
            {
                Text = DefaultTargetDir,
                Font = defaultFont,
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 1 + targetTopMargin, 0, 0),
            };
            layout.Controls.Add(targetTextBox);
            layout.SetRow(targetTextBox, 2);
            layout.SetColumn(targetTextBox, 1);

            var browseButton = new Button
            {
                Text = "...",
                Margin = new Padding(1, 0 + targetTopMargin, 16, 0),
                Padding = Padding.Empty,
                Size = new Size(30, 25),
                Font = defaultFont,
                Dock = DockStyle.Fill,
            };
            layout.Controls.Add(browseButton);
            layout.SetRow(browseButton, 2);
            layout.SetColumn(browseButton, 2);

            var startButton = new Button
            {
                Text = "Extract",
                Margin = new Padding(16, 8, 16, 16),
                Padding = new Padding(6, 4, 6, 4),
                Size = new Size(64, 32),
                Font = defaultFont,
                Dock = DockStyle.Right,
            };
            layout.Controls.Add(startButton);
            layout.SetRow(startButton, 3);
            layout.SetColumnSpan(startButton, 3);

            infoTextContainer.ResumeLayout();
            layout.ResumeLayout();
            form.Controls.Add(layout);
            form.AcceptButton = startButton;
            form.ResumeLayout();

            form.Resize += delegate
            {
                titleText.MaximumSize = new Size(form.ClientSize.Width - 48, header.Height - 48);
                infoText.MaximumSize = new Size(form.ClientSize.Width - 32, int.MaxValue);
            };

            form.KeyPreview = true;
            form.KeyDown += (sender, ea) =>
            {
                if (ea.KeyCode == Keys.Escape) { form.Close(); }
            };

            browseButton.Click += delegate
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
                    SelectedPath = targetTextBox.Text,
                };
                if (dlg.ShowDialog(form) != DialogResult.OK) return;
                targetTextBox.Text = dlg.SelectedPath;
            };

            startButton.Click += delegate
            {
                layout.Enabled = false;
                try
                {
                    Setup(form, targetTextBox.Text);
                }
                finally
                {
                    layout.Enabled = true;
                }
                form.Close();
            };

            return form;
        }

        static void Setup(Form mainForm, string targetDir, bool interactive = true, bool force = false)
        {
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
                        if (!File.Exists(filePath)) continue;
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

            var benchExe = Path.Combine(targetDir, "auto", "bin", "bench.exe");
            if (!File.Exists(benchExe))
            {
                MessageBox.Show(mainForm,
                    "Can not find Bench CLI in the extracted environment." + Environment.NewLine +
                    Environment.NewLine +
                    benchExe + Environment.NewLine +
                    Environment.NewLine +
                    "The transfer package is probably incomplete. Setup cancelled.",
                    "Bench Setup",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                Environment.Exit(4);
            }

            var commandline = GetResourceText("Commandline.txt").Trim();
            var parts = commandline.Split(new char[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);
            var exe = parts[0];
            if (!Path.IsPathRooted(exe))
            {
                exe = Path.Combine(targetDir, exe);
            }
            var args = parts.Length > 1 ? parts[1] : string.Empty;
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    WorkingDirectory = targetDir,
                    FileName = exe,
                    Arguments = args,
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(mainForm,
                    "Starting setup of the Bench environment failed." + Environment.NewLine +
                    Environment.NewLine +
                    ex.GetType().FullName + ":" + Environment.NewLine +
                    ex.Message + Environment.NewLine +
                    Environment.NewLine +
                    "Setup cancelled.",
                    "Bench Setup",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                Environment.Exit(5);
            }
        }
    }
}
