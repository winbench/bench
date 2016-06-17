using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Text;

namespace Mastersign.Bench
{
    public class BenchEnvironment
    {
        private static readonly string PathBackup = Environment.GetEnvironmentVariable("PATH");

        private readonly BenchConfiguration Config;

        public BenchEnvironment(BenchConfiguration config)
        {
            Config = config;
        }

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

            var paths = new List<string>(Config.Apps.EnvironmentPath);
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

        public void Load()
        {
            Load((k, v) => Environment.SetEnvironmentVariable(k, v));
        }

        public void Load(IDictionary<string, string> dict)
        {
            Load((k, v) => dict[k] = v);
        }

        public void Load(StringDictionary dict)
        {
            Load((k, v) => dict[k] = v);
        }

        public void WriteEnvironmentFile()
        {
            var envFilePath = Path.Combine(Config.GetStringValue(PropertyKeys.BenchAuto), "env.cmd");
            using (var w = new StreamWriter(envFilePath, false, Encoding.Default))
            {
                w.WriteLine("@ECHO OFF");
                w.WriteLine("REM **** MD Bench Environment Setup ****");
                w.WriteLine();
                if (Config.GetBooleanValue(PropertyKeys.UseProxy))
                {
                    w.WriteLine("SET HTTP_PROPXY={0}",
                        Config.GetStringValue(PropertyKeys.HttpProxy).TrimEnd('/'));
                    w.WriteLine("SET HTTPS_PROPXY={0}",
                        Config.GetStringValue(PropertyKeys.HttpsProxy).TrimEnd('/'));
                }
                if (Config.GetBooleanValue(PropertyKeys.OverrideHome))
                {
                    var userName = Config.GetStringValue(PropertyKeys.UserName);
                    if (!string.IsNullOrEmpty(userName)) w.WriteLine("SET USERNAME={0}", userName);
                    var userEmail = Config.GetStringValue(PropertyKeys.UserEmail);
                    if (!string.IsNullOrEmpty(userEmail)) w.WriteLine("SET USEREMAIL={0}", userEmail);
                }
                w.WriteLine("SET BENCH_AUTO=%~dp0");
                w.WriteLine("CALL :SET_BENCH_HOME \"%BENCH_AUTO%..\"");
                w.WriteLine("SET /P BENCH_VERSION=<\"%BENCH_HOME%\\res\\version.txt\"");
                w.WriteLine("CALL :SET_BENCH_DRIVE \"%BENCH_AUTO%\"");
                w.WriteLine("SET BENCH_APPS={0}",
                    TryMakeRelative(Config.GetStringValue(PropertyKeys.LibDir), true));
                w.WriteLine("SET L=%BENCH_APPS%");
                if (Config.GetBooleanValue(PropertyKeys.OverrideHome))
                {
                    w.WriteLine("SET HOME={0}",
                        TryMakeRelative(Config.GetStringValue(PropertyKeys.HomeDir), true));
                    w.WriteLine("CALL :SET_HOME_PATH \"%HOME%\"");
                    w.WriteLine("CALL :SET_HOME_DRIVE \"%HOME%\"");
                    w.WriteLine("SET USERPROFILE=%HOME%");
                    w.WriteLine("SET APPDATA={0}",
                        TryMakeRelative(Config.GetStringValue(PropertyKeys.AppDataDir)));
                    w.WriteLine("SET LOCALAPPDATA={0}",
                        TryMakeRelative(Config.GetStringValue(PropertyKeys.LocalAppDataDir)));
                }
                if (Config.GetBooleanValue(PropertyKeys.OverrideTemp))
                {
                    w.WriteLine("SET TEMP={0}",
                        TryMakeRelative(Config.GetStringValue(PropertyKeys.TempDir)));
                    w.WriteLine("SET TMP=%TEMP%");
                }
                w.WriteLine("SET BENCH_PATH={0}", PathList(Config.Apps.EnvironmentPath));
                if (Config.GetBooleanValue(PropertyKeys.IgnoreSystemPath))
                {
                    w.WriteLine("SET PATH={0}", PathList(
                        "%BENCH_AUTO%",
                        "%BENCH_PATH%",
                        "%SystemRoot%",
                        @"%SystemRoot%\System32",
                        @"%SystemRoot%\System32\WindowsPowerShell\v1.0"));
                }
                else
                {
                    w.WriteLine("SET PATH={0}", PathList(
                        "%BENCH_AUTO%", 
                        "%BENCH_PATH%", 
                        "%PATH%"));
                }

                var env = Config.Apps.Environment;
                foreach (var k in env.Keys)
                {
                    w.WriteLine("SET {0}={1}", k, TryMakeRelative(env[k]));
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

        private string PathList(params string[] paths)
        {
            return string.Join("" + Path.PathSeparator, paths);
        }

        private string TryMakeRelative(string path, bool noHome = false)
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
                if (IsRelativeTo(path, b.Dir))
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
                if (IsRelativeTo(path, drive))
                {
                    return string.Format("%{0}%{1}{2}",
                        b.DriveVar, Path.DirectorySeparatorChar,
                        path.Substring(drive.Length).Trim(
                            Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));
                }
            }
            return path;
        }

        private static bool IsRelativeTo(string path, string basePath)
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
    }
}
