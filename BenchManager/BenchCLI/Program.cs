﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Mastersign.Bench.Cli.Controller;

namespace Mastersign.Bench.Cli
{
    class Program
    {
        static int Main(string[] args)
        {
            var controller = new RootCommand();
            return controller.Process(args) ? 0 : -1;
        }

        public static string CliExecutable()
        {
            var assemblyName = Assembly.GetExecutingAssembly().GetName();
            return new Uri(assemblyName.CodeBase).LocalPath;
        }

        public static string Version()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString(3);
        }
    }
}
