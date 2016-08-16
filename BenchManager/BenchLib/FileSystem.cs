using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using WshShell = IWshRuntimeLibrary.WshShell;
using WshShortcut = IWshRuntimeLibrary.WshShortcut;

namespace Mastersign.Bench
{
    /// <summary>
    /// A collection of static methods to help with file system operations.
    /// </summary>
    public static class FileSystem
    {
        private static WshShell wshShell;

        /// <summary>
        /// Returns an instance of the COM object <c>WshShell</c>.
        /// </summary>
        public static WshShell WindowsScriptingHostShell
        {
            get
            {
                if (wshShell == null) wshShell = new WshShell();
                return wshShell;
            }
        }

        /// <summary>
        /// Makes sure, the given path references an empty directory.
        /// If the directory does not exist, it is created, including missing parent folders.
        /// If the directory exists, all content is recursively deleted.
        /// </summary>
        /// <param name="path">An path to the directory.</param>
        /// <returns>A path to the directory.</returns>
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

        /// <summary>
        /// Makes sure a directory exists. Creates it, if it does not exist yet.
        /// </summary>
        /// <param name="path">A path to the directory.</param>
        /// <returns>A path to the directory.</returns>
        public static string AsureDir(string path)
        {
            if (!Directory.Exists(path))
            {
                Debug.WriteLine("Creating directory: " + path);
                Directory.CreateDirectory(path);
            }
            return path;
        }

        /// <summary>
        /// Deletes a directory and all of its content.
        /// </summary>
        /// <param name="path">A path to the directory.</param>
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

        /// <summary>
        /// Moves all the content from one directory to another.
        /// </summary>
        /// <param name="sourceDir">A path to the source directory.</param>
        /// <param name="targetDir">A path to the target directory.</param>
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

        /// <summary>
        /// Creates a Windows shortcut, or link respectively.
        /// </summary>
        /// <param name="file">A path to the shortcut file (<c>*.lnk</c>).</param>
        /// <param name="target">A path to the target file of the shortcut.</param>
        /// <param name="arguments">An command line argument string to pass to the target file, or <c>null</c>.</param>
        /// <param name="workingDir">A path to the working directory to run the target file in, or <c>null</c>.</param>
        /// <param name="iconPath">A path to the icon for the shortcut (<c>*.exe</c> or <c>*.ico</c>), or <c>null</c>.</param>
        /// <param name="windowStyle">The window style to start the target file with, or <c>null</c>.</param>
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

        /// <summary>
        /// An enumeration of possible window styles, when starting a file by the Windows shell.
        /// </summary>
        public enum ShortcutWindowStyle : int
        {
            /// <summary>
            /// The created window is in normal state.
            /// </summary>
            Default = 1,

            /// <summary>
            /// The created window is request to be maximized.
            /// </summary>
            Maximized = 3,

            /// <summary>
            /// The created window is request to be minimized.
            /// </summary>
            Minimized = 7,
        }
    }
}
