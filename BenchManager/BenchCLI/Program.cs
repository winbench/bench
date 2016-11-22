using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Bench.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            var parser = MainParser();
            var result = parser.Parse(args);
            Console.WriteLine(result);
        }

        static ArgumentParser MainParser()
        {
            return new ArgumentParser(
                new Argument(ArgumentType.Option, "verbosity", "v", "verb"),
                new Argument(ArgumentType.Option, "logfile", "l", "log"),
                new Argument(ArgumentType.Option, "root"),

                new Argument(ArgumentType.Command, "initialize"),
                new Argument(ArgumentType.Command, "setup"),
                new Argument(ArgumentType.Command, "update-env", "e"),
                new Argument(ArgumentType.Command, "reinstall"),
                new Argument(ArgumentType.Command, "renew", "n"),
                new Argument(ArgumentType.Command, "upgrade"),

                new Argument(ArgumentType.Command, "config"),
                new Argument(ArgumentType.Command, "downloads", "d", "dl"),
                new Argument(ArgumentType.Command, "app"),
                new Argument(ArgumentType.Command, "project"));
        }
    }
}
