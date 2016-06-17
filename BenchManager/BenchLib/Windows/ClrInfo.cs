using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Mastersign.Bench.Windows
{
    public static class ClrInfo
    {
        public static bool IsVersionSupported(Version v)
        {
            foreach (var version in GetInstalledVersions())
            {
                if (version.Major > v.Major) return true;
                if (version.Major == v.Major)
                {
                    if (version.Minor > v.Minor) return true;
                    if (version.Minor == v.Minor)
                    {
                        if (version.Build >= v.Build) return true;
                    }
                }
            }
            return false;
        }

        #region Reading installed .NET version from registry

        // https://msdn.microsoft.com/en-us/library/hh925568.aspx

        public static Version[] GetInstalledVersions()
        {
            var versions = new List<Version>();

            using (RegistryKey ndpKey = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, "")
                    .OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\"))
            {
                foreach (string versionKeyName in ndpKey.GetSubKeyNames())
                {
                    if (versionKeyName.StartsWith("v"))
                    {
                        RegistryKey versionKey = ndpKey.OpenSubKey(versionKeyName);
                        string name = (string)versionKey.GetValue("Version", "");
                        string install = versionKey.GetValue("Install", "").ToString();
                        if (install != "" && name != "")
                        {
                            var v = new Version(name);
                            //if (!versions.Contains(v))
                            versions.Add(v);
                        }
                        if (name != "")
                        {
                            continue;
                        }
                        foreach (string subKeyName in versionKey.GetSubKeyNames())
                        {
                            RegistryKey subKey = versionKey.OpenSubKey(subKeyName);
                            name = (string)subKey.GetValue("Version", "");
                            if (name != "")
                            {
                                var v = new Version(name);
                                if (!versions.Contains(v)) versions.Add(v);
                            }
                        }
                    }
                }
            }

            var v4x = Get4xVersionFromRegistry();
            if (v4x != null && !versions.Contains(v4x)) versions.Add(v4x);

            versions.Sort();

            return versions.ToArray();
        }

        private static Version Get4xVersionFromRegistry()
        {
            using (RegistryKey ndpKey = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, "")
                .OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\"))
            {
                if (ndpKey != null && ndpKey.GetValue("Release") != null)
                {
                    return CheckFor4xVersion((int)ndpKey.GetValue("Release"));
                }
                else
                {
                    return null;
                }
            }
        }

        // Checking the version using >= will enable forward compatibility,
        // however you should always compile your code on newer versions of
        // the framework to ensure your app works the same.
        private static Version CheckFor4xVersion(int releaseKey)
        {
            if (releaseKey >= 393295)
            {
                return new Version(4, 6); ;
            }
            if ((releaseKey >= 379893))
            {
                return new Version(4, 5, 2);
            }
            if ((releaseKey >= 378675))
            {
                return new Version(4, 5, 1);
            }
            if ((releaseKey >= 378389))
            {
                return new Version(4, 5);
            }
            // This line should never execute. A non-null release key should mean
            // that 4.5 or later is installed.
            return null;
        }

        #endregion
    }
}
