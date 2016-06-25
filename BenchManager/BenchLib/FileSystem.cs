using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using WshShell = IWshRuntimeLibrary.WshShell;
using WshShortcut = IWshRuntimeLibrary.WshShortcut;

namespace Mastersign.Bench
{
    public static class FileSystem
    {
        private static WshShell wshShell;

        public static WshShell WindowsScriptingHostShell
        {
            get
            {
                if (wshShell == null) wshShell = new WshShell();
                return wshShell;
            }
        }

        public static string EmptyDir(string path)
        {
            if (Directory.Exists(path))
            {
                Debug.WriteLine("Cleaning directory: " + path);
                File.SetAttributes(path, FileAttributes.Normal);
                foreach (var dir in Directory.GetDirectories(path))
                {
                    ForceDeleteDirectory(dir);
                }
                foreach (var file in Directory.GetFiles(path))
                {
                    File.SetAttributes(file, FileAttributes.Normal);
                    File.Delete(file);
                }
            }
            else
            {
                Debug.WriteLine("Creating directory: " + path);
                Directory.CreateDirectory(path);
            }
            return path;
        }

        public static string AsureDir(string path)
        {
            if (!Directory.Exists(path))
            {
                Debug.WriteLine("Creating directory: " + path);
                Directory.CreateDirectory(path);
            }
            return path;
        }

        public static void PurgeDir(string path)
        {
            if (!Directory.Exists(path)) return;
            Debug.WriteLine("Purging directory: " + path);
            ForceDeleteDirectory(path);
        }

        private static void ForceDeleteDirectory(string targetDir)
        {
            File.SetAttributes(targetDir, FileAttributes.Normal);

            string[] files = Directory.GetFiles(targetDir);
            string[] dirs = Directory.GetDirectories(targetDir);

            foreach (string file in files)
            {
                ForceDeleteFile(file);
            }

            foreach (string dir in dirs)
            {
                ForceDeleteDirectory(dir);
            }

            Directory.Delete(targetDir, false);
        }

        private static void ForceDeleteFile(string targetFile)
        {
            File.SetAttributes(targetFile, FileAttributes.Normal);
            File.Delete(targetFile);
        }

        public static void MoveContent(string sourceDir, string targetDir)
        {
            Debug.WriteLine("Moving content from: " + sourceDir + " to: " + targetDir);
            AsureDir(targetDir);
            foreach (var dir in Directory.GetDirectories(sourceDir))
            {
                var tp = Path.Combine(targetDir, Path.GetFileName(dir));
                if (Directory.Exists(tp))
                {
                    MoveContent(dir, tp);
                    ForceDeleteDirectory(dir);
                }
                else
                {
                    Directory.Move(dir, tp);
                }
            }
            foreach (var file in Directory.GetFiles(sourceDir))
            {
                var tp = Path.Combine(targetDir, Path.GetFileName(file));
                if (File.Exists(tp)) ForceDeleteFile(tp);
                File.Move(file, tp);
            }
        }

        public static void CreateShortcut(string file, string target,
            string arguments = null, string workingDir = null, string iconPath = null,
            ShortcutWindowStyle windowStyle = ShortcutWindowStyle.Default)
        {
            var s = (WshShortcut)WindowsScriptingHostShell.CreateShortcut(file);
            s.TargetPath = target;
            if (arguments != null) s.Arguments = arguments;
            if (workingDir != null) s.WorkingDirectory = workingDir;
            if (iconPath != null) s.IconLocation = iconPath;
            s.WindowStyle = (int)windowStyle;
            s.Save();
        }

        public enum ShortcutWindowStyle : int
        {
            Default = 1,
            Maximized = 3,
            Minimized = 7,
        }
    }
}
