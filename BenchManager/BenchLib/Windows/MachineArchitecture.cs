using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace Mastersign.Bench.Windows
{
    /// <summary>
    /// This static class contains members to check if the operating system supports 64Bit code.
    /// </summary>
    public static class MachineArchitecture
    {
        // https://blogs.msdn.microsoft.com/oldnewthing/20050201-00/?p=36553

        private static bool Is64BitProcess = (IntPtr.Size == 8);

        /// <summary>
        /// Is set <c>true</c> if the executing operating system supports 64Bit code;
        /// otherwise it is set to <c>false</c>.
        /// </summary>
        public static readonly bool Is64BitOperatingSystem = Is64BitProcess || InternalCheckIsWow64();

        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsWow64Process(
            [In] IntPtr hProcess,
            [Out] out bool wow64Process
        );

        private static bool InternalCheckIsWow64()
        {
            if ((Environment.OSVersion.Version.Major == 5 && Environment.OSVersion.Version.Minor >= 1) ||
                Environment.OSVersion.Version.Major >= 6)
            {
                using (Process p = Process.GetCurrentProcess())
                {
                    bool retVal;
                    if (!IsWow64Process(p.Handle, out retVal))
                    {
                        return false;
                    }
                    return retVal;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
