using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32;

namespace Mastersign.Bench
{
    /// <summary>
    /// <para>
    /// This class represents all environment variables manipulated by the Bench system.
    /// </para>
    /// <para>
    /// It provides methods to load the environment variables in the current process,
    /// or write a batch file containing the variables for loading from another batch script.
    /// </para>
    /// </summary>
    public class BenchEnvironment
    {
        private static readonly string PathBackup = Environment.GetEnvironmentVariable("PATH");

        private readonly BenchConfiguration Config;

        /// <summary>
        /// Initializes a new instance of <see cref="BenchEnvironment"/>.
        /// </summary>
        /// <param name="config">The configuration of the Bench system.</param>
        public BenchEnvironment(BenchConfiguration config)
        {
            Config = config;
        }

        /// <summary>
        /// Calls the given handler for every environment variable in the configuration.
        /// </summary>
        /// <param name="set">The handler for an individual variable.</param>
        public void Load(DictionaryEntryHandler set)
        {
            if (Config.GetBooleanValue(PropertyKeys.UseProxy))
            {
                set("HTTP_PROXY",
                  Config.GetStringValue(PropertyKeys.HttpProxy).TrimEnd('/'));
                set("HTTPS_PROXY",
                  Config.GetStringValue(PropertyKeys.HttpsProxy).TrimEnd('/'));
            }
            if (Config.GetBooleanValue(PropertyKeys.OverrideHome))
            {
                var home = Config.GetStringValue(PropertyKeys.HomeDir);
                set("USERPROFILE", home);
                set("HOME", home);
                set("HOMEDRIVE", GetDrivePart(home));
                set("HOMEPATH", GetPathPart(home));
                set("APPDATA",
                    Config.GetStringValue(PropertyKeys.AppDataDir));
                set("LOCALAPPDATA",
                    Config.GetStringValue(PropertyKeys.LocalAppDataDir));
            }
            if (Config.GetBooleanValue(PropertyKeys.OverrideTemp))
            {
                var temp = Config.GetStringValue(PropertyKeys.TempDir);
                set("TEMP", temp);
                set("TMP", temp);
            }
            var env = Config.Apps.Environment;
            foreach (var k in env.Keys)
            {
                set(k, env[k]);
            }
            var customEnv = Config.GetValue(PropertyKeys.CustomEnvironment) as IDictionary<string, string>;
            if (customEnv != null)
            {
                foreach (var k in customEnv.Keys)
                {
                    if ("PATH".Equals(k, StringComparison.InvariantCultureIgnoreCase)) continue;
                    set(k, customEnv[k]);
                }
            }

            var paths = new List<string>();
            paths.Add(Config.GetStringValue(PropertyKeys.BenchAuto));
            paths.AddRange(Config.GetStringListValue(PropertyKeys.CustomPath));
            paths.AddRange(Config.Apps.EnvironmentPath);
            if (Config.GetBooleanValue(PropertyKeys.IgnoreSystemPath))
            {
                paths.Add(Environment.GetEnvironmentVariable("SystemRoot"));
                paths.Add(Path.Combine(Environment.GetEnvironmentVariable("SystemRoot"), "System32"));
                paths.Add(Path.Combine(Environment.GetEnvironmentVariable("SystemRoot"), @"System32\WindowsPowerShell\v1.0"));
            }
            else
            {
                paths.Add(PathBackup);
            }
            set("PATH", PathList(paths.ToArray()));
        }

        /// <summary>
        /// Loads the environment variables in the current process.
        /// </summary>
        public void Load()
        {
            Load((k, v) => Environment.SetEnvironmentVariable(k, v, EnvironmentVariableTarget.Process));
        }

        /// <summary>
        /// Stores the environment variables in the given generic dictionary.
        /// </summary>
        /// <param name="dict">A dictionary with the variable names as keys.</param>
        public void Load(IDictionary<string, string> dict)
        {
            Load((k, v) => dict[k] = v);
        }

        /// <summary>
        /// Stores the environment variables in the given string dictionary.
        /// </summary>
        /// <param name="dict">A dictionary with the variable names as keys.</param>
        public void Load(StringDictionary dict)
        {
            Load((k, v) => dict[k] = v);
        }

        /// <summary>
        /// Writes the environment file of the Bench system.
        /// </summary>
        /// <remarks>
        /// The environment file is stored in the root of the Bench directory structure
        /// and called <c>env.cmd</c>.
        /// </remarks>
        public void WriteEnvironmentFile()
        {
            var envFilePath = Path.Combine(Config.BenchRootDir, "env.cmd");
            using (var w = new StreamWriter(envFilePath, false, Encoding.Default))
            {
                w.WriteLine("@ECHO OFF");
                w.WriteLine("REM **** MD Bench Environment Setup ****");
                w.WriteLine();
                if (Config.GetBooleanValue(PropertyKeys.UseProxy))
                {
                    w.WriteLine("SET HTTP_PROXY={0}",
                        Config.GetStringValue(PropertyKeys.HttpProxy).TrimEnd('/'));
                    w.WriteLine("SET HTTPS_PROXY={0}",
                        Config.GetStringValue(PropertyKeys.HttpsProxy).TrimEnd('/'));
                }
                if (Config.GetBooleanValue(PropertyKeys.OverrideHome))
                {
                    var userName = Config.GetStringValue(PropertyKeys.UserName);
                    if (!string.IsNullOrEmpty(userName)) w.WriteLine("SET USERNAME={0}", userName);
                    var userEmail = Config.GetStringValue(PropertyKeys.UserEmail);
                    if (!string.IsNullOrEmpty(userEmail)) w.WriteLine("SET USEREMAIL={0}", userEmail);
                }

                w.WriteLine("SET BENCH_AUTO=%~dp0auto");
                w.WriteLine("CALL :SET_BENCH_HOME \"%BENCH_AUTO%\\..\"");
                w.WriteLine("SET /P BENCH_VERSION=<\"%BENCH_HOME%\\res\\version.txt\"");
                w.WriteLine("CALL :SET_BENCH_DRIVE \"%BENCH_AUTO%\"");
                w.WriteLine("SET BENCH_APPS={0}",
                    TryUseVar(Config.GetStringValue(PropertyKeys.LibDir), true));

                w.WriteLine("SET L=%BENCH_APPS%");
                if (Config.GetBooleanValue(PropertyKeys.OverrideHome))
                {
                    w.WriteLine("SET HOME={0}",
                        TryUseVar(Config.GetStringValue(PropertyKeys.HomeDir), true));
                    w.WriteLine("CALL :SET_HOME_PATH \"%HOME%\"");
                    w.WriteLine("CALL :SET_HOME_DRIVE \"%HOME%\"");
                    w.WriteLine("SET USERPROFILE=%HOME%");
                    w.WriteLine("SET APPDATA={0}",
                        TryUseVar(Config.GetStringValue(PropertyKeys.AppDataDir)));
                    w.WriteLine("SET LOCALAPPDATA={0}",
                        TryUseVar(Config.GetStringValue(PropertyKeys.LocalAppDataDir)));
                }
                if (Config.GetBooleanValue(PropertyKeys.OverrideTemp))
                {
                    w.WriteLine("SET TEMP={0}",
                        TryUseVar(Config.GetStringValue(PropertyKeys.TempDir)));
                    w.WriteLine("SET TMP=%TEMP%");
                }
                var benchPath = new List<string>();
                benchPath.AddRange(Config.GetStringListValue(PropertyKeys.CustomPath));
                benchPath.AddRange(Config.Apps.EnvironmentPath);
                for (var i = 0; i < benchPath.Count; i++)
                {
                    benchPath[i] = TryUseVar(benchPath[i]);
                }
                w.WriteLine("SET BENCH_PATH={0}", PathList(benchPath.ToArray()));
                if (Config.GetBooleanValue(PropertyKeys.IgnoreSystemPath))
                {
                    w.WriteLine("SET PATH={0}", PathList(
                        "%BENCH_AUTO%",
                        "%BENCH_PATH%",
                        "%SystemRoot%",
                        @"%SystemRoot%\System32",
                        @"%SystemRoot%\System32\WindowsPowerShell\v1.0"));
                }
                else if (!Config.GetBooleanValue(PropertyKeys.RegisterInUserProfile))
                {
                    w.WriteLine("SET PATH={0}", PathList(
                        "%BENCH_AUTO%",
                        "%BENCH_PATH%",
                        "%PATH%"));
                }

                var env = Config.Apps.Environment;
                foreach (var k in env.Keys)
                {
                    w.WriteLine("SET {0}={1}", k, TryUseVar(env[k]));
                }
                var customEnv = Config.GetValue(PropertyKeys.CustomEnvironment) as IDictionary<string, string>;
                if (customEnv != null)
                {
                    foreach (var k in customEnv.Keys)
                    {
                        if ("PATH".Equals(k, StringComparison.InvariantCultureIgnoreCase)) continue;
                        w.WriteLine("SET {0}={1}", k, TryUseVar(customEnv[k]));
                    }
                }

                w.WriteLine("GOTO:EOF");
                w.WriteLine();
                w.WriteLine(":SET_BENCH_HOME");
                w.WriteLine("SET BENCH_HOME=%~dpfn1");
                w.WriteLine("GOTO:EOF");
                w.WriteLine();
                w.WriteLine(":SET_BENCH_DRIVE");
                w.WriteLine("SET BENCH_DRIVE=%~d1");
                w.WriteLine("GOTO:EOF");
                if (Config.GetBooleanValue(PropertyKeys.OverrideHome))
                {
                    w.WriteLine();
                    w.WriteLine(":SET_HOME_PATH");
                    w.WriteLine("SET HOMEPATH=%~dpfn1");
                    w.WriteLine("GOTO:EOF");
                    w.WriteLine();
                    w.WriteLine(":SET_HOME_DRIVE");
                    w.WriteLine("SET HOMEDRIVE=%~d1");
                    w.WriteLine("GOTO:EOF");
                }
            }
        }

        private delegate string StringTransformer(string value);

        private static List<string> GetCurrentUserPaths()
        {
            var userPath = GetUnexpandedEnvironmentVar("PATH") ?? "";
            var userPathParts = userPath.Split(Path.PathSeparator);
            var paths = new List<string>();
            foreach (var p in userPathParts)
            {
                if (p == null) continue;
                var trimmedPath = p.Trim();
                if (trimmedPath.Length == 0) continue;
                paths.Add(trimmedPath);
            }
            return paths;
        }

        /// <summary>
        /// <para>
        /// Registers the Bench environment in the Windows user profile.
        /// </para>
        /// <para>
        /// Stores the following environment variables are set or updated in the user profile:
        /// </para>
        /// <list type="bullet">
        ///     <item><c>BENCH_VERSION</c></item>
        ///     <item><c>BENCH_HOME</c></item>
        ///     <item><c>BENCH_PATH</c></item>
        ///     <item><c>PATH</c> is changed by adding <c>%BENCH_PATH%</c></item>
        /// </list>
        /// </summary>
        public void RegisterInUserProfile()
        {
            if (Config.GetBooleanValue(PropertyKeys.UseProxy))
            {
                SetEnvironmentVar("HTTP_PROXY",
                  Config.GetStringValue(PropertyKeys.HttpProxy).TrimEnd('/'), false);
                SetEnvironmentVar("HTTPS_PROXY",
                  Config.GetStringValue(PropertyKeys.HttpsProxy).TrimEnd('/'), false);
            }

            var version = Config.GetStringValue(PropertyKeys.Version);
            var libDir = Config.GetStringValue(PropertyKeys.LibDir);
            SetEnvironmentVar("BENCH_VERSION", version, false);
            SetEnvironmentVar("BENCH_HOME", Config.BenchRootDir, false);
            SetEnvironmentVar("BENCH_APPS", libDir, false);

            var useVars = (StringTransformer)(path =>
            {
                path = TryUseVar(path, "BENCH_APPS", libDir);
                path = TryUseVar(path, "BENCH_HOME", Config.BenchRootDir);
                return path;
            });

            var benchPath = new List<string>();
            benchPath.AddRange(Config.GetStringListValue(PropertyKeys.CustomPath));
            benchPath.AddRange(Config.Apps.EnvironmentPath);
            for (var i = 0; i < benchPath.Count; i++)
            {
                benchPath[i] = useVars(benchPath[i]);
            }
            SetEnvironmentVar("BENCH_PATH", PathList(benchPath.ToArray()), true);

            var paths = GetCurrentUserPaths();
            if (paths.Contains("%BENCH_PATH%")) paths.Remove("%BENCH_PATH%");
            paths.Insert(0, "%BENCH_PATH%");
            SetEnvironmentVar("PATH", PathList(paths.ToArray()), true);

            var env = Config.Apps.Environment;
            foreach (var k in env.Keys)
            {
                SetEnvironmentVar(k, useVars(env[k]), true);
            }
            var customEnv = Config.GetValue(PropertyKeys.CustomEnvironment) as IDictionary<string, string>;
            if (customEnv != null)
            {
                foreach (var k in customEnv.Keys)
                {
                    if ("PATH".Equals(k, StringComparison.InvariantCultureIgnoreCase)) continue;
                    SetEnvironmentVar(k, useVars(customEnv[k]), true);
                }
            }
        }

        /// <summary>
        /// <para>
        /// Unregisters the Bench environment from the Windows user profile.
        /// </para>
        /// <para>
        /// The following environment variables are deleted or updated in the user profile:
        /// </para>
        /// <list type="bullet">
        ///     <item><c>BENCH_VERSION</c></item>
        ///     <item><c>BENCH_HOME</c></item>
        ///     <item><c>BENCH_PATH</c></item>
        ///     <item><c>PATH</c> is changed by removing <c>%BENCH_PATH%</c></item>
        /// </list>
        /// </summary>
        public void UnregisterFromUserProfile()
        {
            DeleteEnvironmentVar("BENCH_VERSION");
            DeleteEnvironmentVar("BENCH_HOME");
            DeleteEnvironmentVar("BENCH_APPS");
            DeleteEnvironmentVar("BENCH_PATH");

            var paths = GetCurrentUserPaths();
            if (paths.Contains("%BENCH_PATH%")) paths.Remove("%BENCH_PATH%");
            SetEnvironmentVar("PATH", PathList(paths.ToArray()), true);
        }

        private string PathList(params string[] paths)
        {
            return string.Join("" + Path.PathSeparator, paths);
        }

        private string TryUseVar(string path, bool noHome = false)
        {
            var libBase = new
            {
                Dir = NormalizePath(Config.GetStringValue(PropertyKeys.LibDir)),
                PathVar = "L",
                DriveVar = (string)null
            };
            var homeBase = new
            {
                Dir = NormalizePath(Config.GetStringValue(PropertyKeys.HomeDir)),
                PathVar = "HOME",
                DriveVar = "HOMEDRIVE"
            };
            var benchBase = new
            {
                Dir = NormalizePath(Config.GetStringValue(PropertyKeys.BenchRoot)),
                PathVar = "BENCH_HOME",
                DriveVar = "BENCH_DRIVE"
            };
            var bases = noHome
                ? new[] { libBase, benchBase }
                : new[] { libBase, homeBase, benchBase };
            foreach (var b in bases)
            {
                if (IsSubPath(path, b.Dir))
                {
                    return string.Format("%{0}%{1}{2}",
                        b.PathVar, Path.DirectorySeparatorChar,
                        path.Substring(b.Dir.Length).Trim(
                            Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));
                }
            }
            foreach (var b in bases)
            {
                if (b.DriveVar == null) continue;
                var drive = GetDrivePart(b.Dir);
                if (IsSubPath(path, drive))
                {
                    return string.Format("%{0}%{1}{2}",
                        b.DriveVar, Path.DirectorySeparatorChar,
                        path.Substring(drive.Length).Trim(
                            Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));
                }
            }
            return path;
        }

        private string TryUseVar(string path, string varName, string varValue)
        {
            var basePath = NormalizePath(varValue);
            if (IsSubPath(path, basePath))
            {
                return string.Format("%{0}%{1}{2}",
                    varName, Path.DirectorySeparatorChar,
                    path.Substring(basePath.Length).Trim(
                        Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));
            }
            return path;
        }

        private static bool IsSubPath(string path, string basePath)
        {
            return path.StartsWith(basePath, StringComparison.InvariantCultureIgnoreCase);
        }

        private static string NormalizePath(string path)
        {
            if (string.IsNullOrEmpty(path)) return path;
            return path.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                + Path.DirectorySeparatorChar;
        }

        private static string GetDrivePart(string path)
        {
            return Path.GetPathRoot(path).TrimEnd(
                Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        }

        private static string GetPathPart(string path)
        {
            return path.Substring(GetDrivePart(path).Length);
        }

        private static string GetUnexpandedEnvironmentVar(string name)
        {
            using (var key = Registry.CurrentUser.OpenSubKey("Environment", false))
            {
                return key.GetValue(name, null, RegistryValueOptions.DoNotExpandEnvironmentNames) as string;
            }
        }

        private static void SetEnvironmentVar(string name, string value, bool expand)
        {
            using (var key = Registry.CurrentUser.OpenSubKey("Environment", true))
            {
                key.SetValue(name, value, expand ? RegistryValueKind.ExpandString : RegistryValueKind.String);
            }
        }

        private static void DeleteEnvironmentVar(string name)
        {
            using (var key = Registry.CurrentUser.OpenSubKey("Environment", true))
            {
                key.DeleteValue(name, false);
            }
        }

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessageTimeout(
            IntPtr hWnd, int Msg,
            IntPtr wParam, string lParam,
            uint fuFlags, uint uTimeout, IntPtr lpdwResult);

        private static void NotfyWindowsAboutSettingChange()
        {
            IntPtr HWND_BROADCAST = new IntPtr(0xffff);
            int WM_SETTINGCHANGE = 0x001A;
            uint SMTO_ABORTIFHUNG = 0x0002;
            SendMessageTimeout(HWND_BROADCAST, WM_SETTINGCHANGE,
                IntPtr.Zero, "Environment",
                SMTO_ABORTIFHUNG, 1000, IntPtr.Zero);
        }
    }
}
