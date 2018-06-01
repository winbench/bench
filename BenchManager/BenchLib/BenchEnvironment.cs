using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace Mastersign.Bench
{
    /// <summary>
    /// This class represents all environment variables manipulated by the Bench system.
    /// </summary>
    /// <remarks>
    /// This class provides methods to load the environment variables in the current process,
    /// or to write a batch file, containing the variables for loading from another batch script.
    /// At last it provides methods to write environment variables to the Windows user profile.
    /// In that way, the Bench apps are available on the <c>PATH</c> of the Windows command line.
    /// </remarks>
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
            if (Config.GetBooleanValue(ConfigPropertyKeys.UseProxy))
            {
                set("HTTP_PROXY",
                  Config.GetStringValue(ConfigPropertyKeys.HttpProxy).TrimEnd('/'));
                set("HTTPS_PROXY",
                  Config.GetStringValue(ConfigPropertyKeys.HttpsProxy).TrimEnd('/'));
            }
            if (Config.GetBooleanValue(ConfigPropertyKeys.OverrideHome))
            {
                set("USERNAME", Config.GetStringValue(ConfigPropertyKeys.UserName));
                set("USEREMAIL", Config.GetStringValue(ConfigPropertyKeys.UserEmail));
                var home = Config.GetStringValue(ConfigPropertyKeys.HomeDir);
                set("USERPROFILE", home);
                set("HOME", home);
                set("HOMEDRIVE", GetDrivePart(home));
                set("HOMEPATH", GetPathPart(home));
                set("APPDATA",
                    Config.GetStringValue(ConfigPropertyKeys.AppDataDir));
                set("LOCALAPPDATA",
                    Config.GetStringValue(ConfigPropertyKeys.LocalAppDataDir));
            }
            else
            {
                set("HOME", Environment.GetEnvironmentVariable("USERPROFILE"));
            }
            if (Config.GetBooleanValue(ConfigPropertyKeys.OverrideTemp))
            {
                var temp = Config.GetStringValue(ConfigPropertyKeys.TempDir);
                set("TEMP", temp);
                set("TMP", temp);
            }
            var env = Config.Apps.Environment;
            foreach (var k in env.Keys)
            {
                set(k, env[k]);
            }
            var customEnv = Config.GetValue(ConfigPropertyKeys.CustomEnvironment) as IDictionary<string, string>;
            if (customEnv != null)
            {
                foreach (var k in customEnv.Keys)
                {
                    if ("PATH".Equals(k, StringComparison.InvariantCultureIgnoreCase)) continue;
                    set(k, customEnv[k]);
                }
            }

            var paths = new List<string>();
            paths.Add(Config.GetStringValue(ConfigPropertyKeys.BenchBin));
            paths.AddRange(Config.GetStringListValue(ConfigPropertyKeys.CustomPath));
            paths.AddRange(Config.Apps.EnvironmentPath);
            if (Config.GetBooleanValue(ConfigPropertyKeys.IgnoreSystemPath))
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
        /// Writes the CMD environment script of the Bench system.
        /// </summary>
        /// <remarks>
        /// The environment script is stored in the root of the Bench directory structure
        /// and called <c>env.cmd</c>.
        /// </remarks>
        public void WriteCmdEnvironmentScript()
        {
            var envFilePath = Path.Combine(Config.BenchRootDir, "env.cmd");
            using (var w = new StreamWriter(envFilePath, false, Encoding.Default))
            {
                w.WriteLine("@ECHO OFF");
                w.WriteLine("REM **** MD Bench Environment Setup ****");
                w.WriteLine();
                if (Config.GetBooleanValue(ConfigPropertyKeys.UseProxy))
                {
                    w.WriteLine("SET HTTP_PROXY={0}",
                        Config.GetStringValue(ConfigPropertyKeys.HttpProxy).TrimEnd('/'));
                    w.WriteLine("SET HTTPS_PROXY={0}",
                        Config.GetStringValue(ConfigPropertyKeys.HttpsProxy).TrimEnd('/'));
                }
                if (Config.GetBooleanValue(ConfigPropertyKeys.OverrideHome))
                {
                    var userName = Config.GetStringValue(ConfigPropertyKeys.UserName);
                    if (!string.IsNullOrEmpty(userName)) w.WriteLine("SET USERNAME={0}", userName);
                    var userEmail = Config.GetStringValue(ConfigPropertyKeys.UserEmail);
                    if (!string.IsNullOrEmpty(userEmail)) w.WriteLine("SET USEREMAIL={0}", userEmail);
                }

                w.WriteLine("SET BENCH_DRIVE=%~d0");
                w.WriteLine("SET BENCH_HOME=%~dp0");
                w.WriteLine("CALL :CLEAN_BENCH_HOME");
                w.WriteLine("SET BENCH_BIN=" + TryUseVar(Config.GetStringValue(ConfigPropertyKeys.BenchBin)));
                w.WriteLine("SET /P BENCH_VERSION=<\"%BENCH_HOME%\\res\\version.txt\"");
                w.WriteLine("SET BENCH_APPS={0}",
                    TryUseVar(Config.GetStringValue(ConfigPropertyKeys.AppsInstallDir), true));

                w.WriteLine("SET L=%BENCH_APPS%");
                if (Config.GetBooleanValue(ConfigPropertyKeys.OverrideHome))
                {
                    w.WriteLine("SET HOME={0}",
                        TryUseVar(Config.GetStringValue(ConfigPropertyKeys.HomeDir), true));
                    w.WriteLine("CALL :SET_HOME_PATH \"%HOME%\"");
                    w.WriteLine("CALL :SET_HOME_DRIVE \"%HOME%\"");
                    w.WriteLine("SET USERPROFILE=%HOME%");
                    w.WriteLine("SET APPDATA={0}",
                        TryUseVar(Config.GetStringValue(ConfigPropertyKeys.AppDataDir)));
                    w.WriteLine("SET LOCALAPPDATA={0}",
                        TryUseVar(Config.GetStringValue(ConfigPropertyKeys.LocalAppDataDir)));
                }
                else
                {
                    w.WriteLine("SET HOME=%USERPROFILE%");
                }
                if (Config.GetBooleanValue(ConfigPropertyKeys.OverrideTemp))
                {
                    w.WriteLine("SET TEMP={0}",
                        TryUseVar(Config.GetStringValue(ConfigPropertyKeys.TempDir)));
                    w.WriteLine("SET TMP=%TEMP%");
                }
                var benchPath = new List<string>();
                benchPath.AddRange(Config.GetStringListValue(ConfigPropertyKeys.CustomPath));
                benchPath.AddRange(Config.Apps.EnvironmentPath);
                for (var i = 0; i < benchPath.Count; i++)
                {
                    benchPath[i] = TryUseVar(benchPath[i]);
                }
                w.WriteLine("SET BENCH_PATH={0}", PathList(benchPath.ToArray()));
                if (Config.GetBooleanValue(ConfigPropertyKeys.IgnoreSystemPath))
                {
                    w.WriteLine("SET PATH={0}", PathList(
                        "%BENCH_BIN%",
                        "%BENCH_PATH%",
                        "%SystemRoot%",
                        @"%SystemRoot%\System32",
                        @"%SystemRoot%\System32\WindowsPowerShell\v1.0"));
                }
                else if (!Config.GetBooleanValue(ConfigPropertyKeys.RegisterInUserProfile))
                {
                    w.WriteLine("SET PATH={0}", PathList(
                        "%BENCH_BIN%",
                        "%BENCH_PATH%",
                        "%PATH%"));
                }

                var env = Config.Apps.Environment;
                foreach (var k in env.Keys)
                {
                    w.WriteLine("SET {0}={1}", k, TryUseVar(env[k]));
                }
                var customEnv = Config.GetValue(ConfigPropertyKeys.CustomEnvironment) as IDictionary<string, string>;
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
                w.WriteLine(":CLEAN_BENCH_HOME");
                w.WriteLine("SET BENCH_HOME=%BENCH_HOME%#+#");
                w.WriteLine("SET BENCH_HOME=%BENCH_HOME:\\#+#=#+#%");
                w.WriteLine("SET BENCH_HOME=%BENCH_HOME:#+#=%");
                w.WriteLine("GOTO:EOF");
                if (Config.GetBooleanValue(ConfigPropertyKeys.OverrideHome))
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
        /// Registers the Bench environment in the Windows user profile.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Stores the following environment variables are set or updated in the user profile:
        /// </para>
        /// <list type="bullet">
        ///     <item><c>BENCH_VERSION</c></item>
        ///     <item><c>BENCH_HOME</c></item>
        ///     <item><c>BENCH_PATH</c></item>
        ///     <item><c>PATH</c> is changed by adding <c>%BENCH_PATH%</c></item>
        /// </list>
        /// </remarks>
        public void RegisterInUserProfile()
        {
            if (Config.GetBooleanValue(ConfigPropertyKeys.UseProxy))
            {
                SetEnvironmentVar("HTTP_PROXY",
                  Config.GetStringValue(ConfigPropertyKeys.HttpProxy).TrimEnd('/'), false);
                SetEnvironmentVar("HTTPS_PROXY",
                  Config.GetStringValue(ConfigPropertyKeys.HttpsProxy).TrimEnd('/'), false);
            }

            var version = Config.GetStringValue(ConfigPropertyKeys.Version);
            var libDir = Config.GetStringValue(ConfigPropertyKeys.AppsInstallDir);
            SetEnvironmentVar("BENCH_VERSION", version);
            SetEnvironmentVar("BENCH_HOME", Config.BenchRootDir);
            SetEnvironmentVar("BENCH_APPS", libDir);

            var benchPath = new List<string>();
            benchPath.Add(Config.GetStringValue(ConfigPropertyKeys.BenchBin));
            benchPath.AddRange(Config.GetStringListValue(ConfigPropertyKeys.CustomPath));
            benchPath.AddRange(Config.Apps.EnvironmentPath);
            SetEnvironmentVar("BENCH_PATH", PathList(benchPath.ToArray()));

            var paths = GetCurrentUserPaths();
            if (paths.Contains("%BENCH_PATH%")) paths.Remove("%BENCH_PATH%");
            paths.Insert(0, "%BENCH_PATH%");
            SetEnvironmentVar("PATH", PathList(paths.ToArray()), true);

            var env = Config.Apps.Environment;
            foreach (var k in env.Keys)
            {
                SetEnvironmentVar(k, env[k]);
            }
            var customEnv = Config.GetValue(ConfigPropertyKeys.CustomEnvironment) as IDictionary<string, string>;
            if (customEnv != null)
            {
                foreach (var k in customEnv.Keys)
                {
                    if ("PATH".Equals(k, StringComparison.InvariantCultureIgnoreCase)) continue;
                    SetEnvironmentVar(k, customEnv[k]);
                }
            }
        }

        /// <summary>
        /// Unregisters the Bench environment from the Windows user profile.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The following environment variables are deleted or updated in the user profile:
        /// </para>
        /// <list type="bullet">
        ///     <item><c>BENCH_VERSION</c></item>
        ///     <item><c>BENCH_HOME</c></item>
        ///     <item><c>BENCH_PATH</c></item>
        ///     <item><c>PATH</c> is changed by removing <c>%BENCH_PATH%</c></item>
        /// </list>
        /// </remarks>
        public void UnregisterFromUserProfile()
        {
            var currentEnvRootPath = Environment.GetEnvironmentVariable("BENCH_HOME");
            if (!string.Equals(currentEnvRootPath, Config.BenchRootDir,
                StringComparison.InvariantCultureIgnoreCase))
            {
                return;
            }

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
                Dir = NormalizePath(Config.GetStringValue(ConfigPropertyKeys.AppsInstallDir)),
                PathVar = "L",
                DriveVar = (string)null
            };
            var homeBase = new
            {
                Dir = NormalizePath(Config.GetStringValue(ConfigPropertyKeys.HomeDir)),
                PathVar = "HOME",
                DriveVar = "HOMEDRIVE"
            };
            var benchBase = new
            {
                Dir = NormalizePath(Config.GetStringValue(ConfigPropertyKeys.BenchRoot)),
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

        private static void SetEnvironmentVar(string name, string value, bool expand = false)
        {
            using (var key = Registry.CurrentUser.OpenSubKey("Environment", true))
            {
                key.SetValue(name, value, expand ? RegistryValueKind.ExpandString : RegistryValueKind.String);
            }
            Task.Run((Action)NotifyWindowsAboutSettingChange);
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

        private static void NotifyWindowsAboutSettingChange()
        {
            IntPtr HWND_BROADCAST = new IntPtr(0xffff);
            int WM_SETTINGCHANGE = 0x001A;
            SendMessageTimeout(HWND_BROADCAST, WM_SETTINGCHANGE, IntPtr.Zero, "Environment", 0, 1000, IntPtr.Zero);
        }
    }
}
