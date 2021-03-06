﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using WshShell = IWshRuntimeLibrary.WshShell;
using WshShortcut = IWshRuntimeLibrary.WshShortcut;
using static Mastersign.Sequence.Sequence;
using System.Threading;

namespace Mastersign.Bench
{
    /// <summary>
    /// A collection of static methods to help with file system operations.
    /// </summary>
    public static class FileSystem
    {
        private static WshShell wshShell;

        private const int UNAUTHORIZED_RETRY_LIMIT = 20;
        private const int UNAUTHORIZED_RETRY_INTERVAL_MS = 100;

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

        private static string NormalizePath(string path)
        {
            if (path.StartsWith(@"\\?\")) return path;
            path = Path.GetFullPath(path);
            path = path.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            return path.Length >= 240
                ? @"\\?\" + path
                : path;
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
            path = NormalizePath(path);
            if (Directory.Exists(path))
            {
                Debug.WriteLine("Cleaning directory: " + path);
                ForceEmptyDirectory(path);
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
            path = NormalizePath(path);
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
            path = NormalizePath(path);
            if (!Directory.Exists(path)) return;
            Debug.WriteLine("Purging directory: " + path);
            ForceEmptyDirectory(path);
            ForceDeleteDirectory(path);
        }

        private static void ForceEmptyDirectory(string targetDir)
        {
            targetDir = NormalizePath(targetDir);
            File.SetAttributes(targetDir, FileAttributes.Normal);

            string[] files = Directory.GetFiles(targetDir);
            string[] dirs = Directory.GetDirectories(targetDir);

            foreach (string file in files)
            {
                ForceDeleteFile(file);
            }

            foreach (string dir in dirs)
            {
                if (!File.GetAttributes(dir).HasFlag(FileAttributes.ReparsePoint))
                {
                    // empty folder if it is not a junction (directory symlink)
                    ForceEmptyDirectory(dir);
                }
                ForceDeleteDirectory(dir);
            }
        }

        private static void ForceDeleteDirectory(string targetDir)
        {
            // poll to work around short time locks from anti virus software
            for (int i = 0; i < UNAUTHORIZED_RETRY_LIMIT; i++)
            {
                try
                {
                    // expect the directory to be empty
                    Directory.Delete(targetDir, false);
                    return;
                }
                catch (UnauthorizedAccessException)
                {
                    if (i >= UNAUTHORIZED_RETRY_LIMIT - 1) throw;
                    Thread.Sleep(UNAUTHORIZED_RETRY_INTERVAL_MS);
                }
                catch (DirectoryNotFoundException)
                {
                    // directory was already deleted
                    return;
                }
            }
        }

        private static void ForceDeleteFile(string targetFile)
        {
            targetFile = NormalizePath(targetFile);
            File.SetAttributes(targetFile, FileAttributes.Normal);
            // poll to work around short time locks from anti virus software
            for (int i = 0; i < UNAUTHORIZED_RETRY_LIMIT; i++)
            {
                try
                {
                    File.Delete(targetFile);
                    return;
                }
                catch (UnauthorizedAccessException)
                {
                    if (i >= UNAUTHORIZED_RETRY_LIMIT - 1) throw;
                    Thread.Sleep(UNAUTHORIZED_RETRY_INTERVAL_MS);
                }
                catch (FileNotFoundException)
                {
                    // file was already deleted
                    return;
                }
            }
        }

        /// <summary>
        /// Moves all the content from one directory to another.
        /// </summary>
        /// <param name="sourceDir">A path to the source directory.</param>
        /// <param name="targetDir">A path to the target directory.</param>
        public static void MoveContent(string sourceDir, string targetDir)
        {
            sourceDir = NormalizePath(sourceDir);
            targetDir = NormalizePath(targetDir);

            Debug.WriteLine("Moving content from: " + sourceDir + " to: " + targetDir);
            AsureDir(targetDir);

            foreach (var dir in Directory.GetDirectories(sourceDir))
            {
                var currentDir = NormalizePath(dir);
                var tp = NormalizePath(Path.Combine(targetDir, Path.GetFileName(currentDir)));
                if (Directory.Exists(tp))
                {
                    MoveContent(currentDir, tp);
                    ForceEmptyDirectory(currentDir);
                    ForceDeleteDirectory(currentDir);
                }
                else
                {
                    Directory.Move(currentDir, tp);
                }
            }
            foreach (var file in Directory.GetFiles(sourceDir))
            {
                var currentFile = NormalizePath(file);
                var tp = NormalizePath(Path.Combine(targetDir, Path.GetFileName(currentFile)));
                if (File.Exists(tp)) ForceDeleteFile(tp);
                File.Move(currentFile, tp);
            }
        }

        /// <summary>
        /// Copies a directory with all its content to another location.
        /// </summary>
        /// <param name="sourceDir">A path to the source directory.</param>
        /// <param name="targetDir">A path to the target directory.</param>
        /// <param name="subDirs"><c>true</c> if subdirectories are copied recursively; otherwise <c>false</c>.</param>
        /// <param name="excludeDirs">An array with directory names to exclude during copying.</param>
        /// <param name="excludeFiles">An array with file names to exclude during the copying.</param>
        public static void CopyDir(string sourceDir, string targetDir, bool subDirs,
            string[] excludeDirs = null, string[] excludeFiles = null)
        {
            sourceDir = NormalizePath(sourceDir);
            targetDir = NormalizePath(targetDir);

            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDir);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDir);
            }

            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(targetDir))
            {
                Directory.CreateDirectory(targetDir);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                if (excludeFiles != null &&
                    Seq(excludeFiles).Any(fileName => string.Equals(file.Name, fileName, StringComparison.OrdinalIgnoreCase)))
                {
                    continue;
                }
                string temppath = NormalizePath(Path.Combine(targetDir, file.Name));
                file.CopyTo(temppath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (subDirs)
            {
                DirectoryInfo[] dirs = dir.GetDirectories();
                foreach (DirectoryInfo subdir in dirs)
                {
                    if (excludeDirs != null &&
                        Seq(excludeDirs).Any(dirName => string.Equals(subdir.Name, dirName, StringComparison.OrdinalIgnoreCase)))
                    {
                        Debug.WriteLine("Skipping: " + subdir.FullName);
                        continue;
                    }
                    string temppath = Path.Combine(targetDir, subdir.Name);
                    CopyDir(subdir.FullName, temppath, subDirs,
                        excludeDirs: excludeDirs, excludeFiles: excludeFiles);
                }
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
