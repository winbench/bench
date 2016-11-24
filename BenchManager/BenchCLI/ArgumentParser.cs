using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Bench.Cli
{
    class ArgumentParser
    {
        public ArgumentParserType ParserType { get; set; }

        public string[] HelpIndicators = new[] { "/?", "-?", "-h", "--help" };

        private readonly List<Argument> arguments = new List<Argument>();

        public ArgumentParser(IEnumerable<Argument> arguments)
        {
            ParserType = ArgumentParserType.CaseSensitive;
            foreach (var arg in arguments)
            {
                RegisterArgument(arg);
            }
        }

        public ArgumentParser(params Argument[] arguments)
            : this((IEnumerable<Argument>)arguments)
        {
        }

        public void RegisterArgument(Argument arg)
        {
            arguments.Add(arg);
        }

        private Argument[] FilterArguments(ArgumentType type)
        {
            var res = new List<Argument>();
            foreach (var a in arguments)
            {
                if (a.Type == type)
                {
                    res.Add(a);
                }
            }
            res.Sort((a, b) => a.Name.CompareTo(b.Name));
            return res.ToArray();
        }

        public Argument[] GetFlags()
        {
            return FilterArguments(ArgumentType.Flag);
        }

        public Argument[] GetOptions()
        {
            return FilterArguments(ArgumentType.Option);
        }

        public Argument[] GetCommands()
        {
            return FilterArguments(ArgumentType.Command);
        }

        private bool IsHelpIndicator(string v)
        {
            foreach (var i in HelpIndicators)
            {
                if (i.Equals(v, StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        public ArgumentParsingResult Parse(string[] args)
        {
            var index = new ArgumentIndex(ParserType == ArgumentParserType.CaseSensitive, arguments);
            IDictionary<string, bool> flagValues = new Dictionary<string, bool>();
            IDictionary<string, string> optionValues = new Dictionary<string, string>();
            string command = null;
            var position = 0;
            var help = false;
            string invalid = null;

            while (position < args.Length && command == null)
            {
                var arg = args[position];
                if (IsHelpIndicator(arg))
                {
                    help = true;
                    break;
                }
                var a = index.LookUp(args[position]);
                if (a == null)
                {
                    invalid = args[position];
                    break;
                }
                switch (a.Type)
                {
                    case ArgumentType.Flag:
                        flagValues[a.Name] = true;
                        break;
                    case ArgumentType.Option:
                        position++;
                        if (args.Length <= position)
                        {
                            invalid = args[position - 1] + " ???";
                            break;
                        }
                        var opt = (OptionArgument)a;
                        if (opt.ValuePredicate != null && !opt.ValuePredicate(args[position]))
                        {
                            invalid = args[position - 1] + " " + args[position];
                            break;
                        }
                        optionValues[a.Name] = args[position];
                        break;
                    case ArgumentType.Command:
                        command = a.Name;
                        break;
                    default:
                        throw new NotSupportedException();
                }
                if (invalid != null) break;
                position++;
            }
            if (help)
            {
                return new ArgumentParsingResult(ArgumentParsingResultType.Help, 
                    null, null, null, null, null);
            }
            if (invalid != null)
            {
                return new ArgumentParsingResult(ArgumentParsingResultType.InvalidArgument,
                    null, invalid, null, null, null);
            }
            var rest = new string[args.Length - position];
            Array.Copy(args, position, rest, 0, rest.Length);
            if (command != null)
            {
                return new ArgumentParsingResult(ArgumentParsingResultType.Command, 
                    command, null, rest, optionValues, flagValues);
            }
            return new ArgumentParsingResult(ArgumentParsingResultType.NoCommand, 
                null, null, rest, optionValues, flagValues);
        }
    }

    class ArgumentIndex
    {
        private readonly Dictionary<string, Argument> flags = new Dictionary<string, Argument>();
        private readonly Dictionary<string, Argument> flagMnemonics = new Dictionary<string, Argument>();
        private readonly Dictionary<string, Argument> options = new Dictionary<string, Argument>();
        private readonly Dictionary<string, Argument> optionMnemonics = new Dictionary<string, Argument>();
        private readonly Dictionary<string, Argument> commands = new Dictionary<string, Argument>();
        private readonly Dictionary<string, Argument> commandMnemonics = new Dictionary<string, Argument>();

        private readonly bool CaseSensitive;

        public ArgumentIndex(bool caseSensitive, IEnumerable<Argument> args)
        {
            CaseSensitive = caseSensitive;
            foreach (var arg in args)
            {
                AddArgument(arg);
            }
        }

        private string PrepareArgument(string arg)
        {
            if (arg == null) throw new ArgumentNullException();
            return CaseSensitive ? arg : arg.ToLowerInvariant();
        }

        private void AddArgument(Argument arg)
        {
            switch (arg.Type)
            {
                case ArgumentType.Flag:
                    flags[PrepareArgument(arg.Name)] = arg;
                    foreach (var alias in arg.Aliases)
                    {
                        flags[PrepareArgument(alias)] = arg;
                    }
                    flagMnemonics[PrepareArgument(arg.Mnemonic)] = arg;
                    break;
                case ArgumentType.Option:
                    options[PrepareArgument(arg.Name)] = arg;
                    foreach (var alias in arg.Aliases)
                    {
                        options[PrepareArgument(alias)] = arg;
                    }
                    optionMnemonics[PrepareArgument(arg.Mnemonic)] = arg;
                    break;
                case ArgumentType.Command:
                    commands[PrepareArgument(arg.Name)] = arg;
                    foreach (var alias in arg.Aliases)
                    {
                        commands[PrepareArgument(alias)] = arg;
                    }
                    commandMnemonics[PrepareArgument(arg.Mnemonic)] = arg;
                    break;
                default:
                    throw new NotSupportedException();
            }
        }

        public Argument LookUp(string v)
        {
            Argument a;
            v = PrepareArgument(v);
            if (v.StartsWith("--"))
            {
                var name = v.Substring(2);
                if (flags.TryGetValue(name, out a)) return a;
                if (options.TryGetValue(name, out a)) return a;
                return null;
            }
            if (v.StartsWith("-"))
            {
                var mnemonic = v.Substring(1);
                if (flagMnemonics.TryGetValue(mnemonic, out a)) return a;
                if (optionMnemonics.TryGetValue(mnemonic, out a)) return a;
                return null;
            }
            if (commands.TryGetValue(v, out a)) return a;
            if (commandMnemonics.TryGetValue(v, out a)) return a;
            return null;
        }
    }

    enum ArgumentParserType
    {
        CaseSensitive,
        CaseInsensitive
    }

    enum ArgumentType
    {
        Flag,
        Option,
        Command
    }

    abstract class Argument
    {
        public ArgumentType Type { get; private set; }

        public string Name { get; private set; }

        public string Mnemonic { get; private set; }

        public string[] Aliases { get; private set; }

        public string Description { get; private set; }

        protected Argument(ArgumentType type, string name, string mnemonic,
            string description, params string[] aliases)
        {
            Type = type;
            Name = name;
            Mnemonic = mnemonic ?? name.Substring(0, 1);
            Aliases = aliases;
            Description = description;
        }
    }

    class FlagArgument : Argument
    {
        public FlagArgument(string name, string mnemonic,
            string description, params string[] aliases)
            : base(ArgumentType.Flag, name, mnemonic, description, aliases)
        {
        }
    }

    delegate bool OptionValuePredicate(string value);

    class OptionArgument : Argument
    {
        public string PossibleValueInfo { get; private set; }

        public string DefaultValueInfo { get; private set; }

        public OptionValuePredicate ValuePredicate { get; private set; }

        public OptionArgument(string name, string mnemonic,
            string description, string possibleValueInfo, string defaultValueInfo,
            OptionValuePredicate valuePredicate,
            params string[] aliases)
            : base(ArgumentType.Option, name, mnemonic, description, aliases)
        {
            PossibleValueInfo = possibleValueInfo;
            DefaultValueInfo = defaultValueInfo;
            ValuePredicate = valuePredicate;
        }
    }

    class CommandArgument : Argument
    {
        public string SyntaxInfo { get; private set; }

        public CommandArgument(string name, string mnemonic,
            string description, string syntaxInfo = null,
            params string[] aliases)
            : base(ArgumentType.Command, name, mnemonic, description, aliases)
        {
            SyntaxInfo = syntaxInfo;
        }
    }

    class ArgumentParsingResult
    {
        public ArgumentParsingResultType Type { get; private set; }

        public string Command { get; private set; }

        public string InvalidArgument { get; private set; }

        public string[] Rest { get; private set; }

        private readonly IDictionary<string, string> options;

        private readonly IDictionary<string, bool> flags;

        public ArgumentParsingResult(ArgumentParsingResultType type,
            string command, string invalidArgument, string[] rest,
            IDictionary<string, string> options, IDictionary<string, bool> flags)
        {
            Type = type;
            Command = command;
            InvalidArgument = invalidArgument;
            Rest = rest;
            this.options = options ?? new Dictionary<string, string>();
            this.flags = flags ?? new Dictionary<string, bool>();
        }

        public string GetOptionValue(string name, string def = null)
        {
            string res;
            return options.TryGetValue(name, out res) ? res : def;
        }

        public bool GetFlag(string name)
        {
            return flags.ContainsKey(name);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Result Type: " + Type);
            switch (Type)
            {
                case ArgumentParsingResultType.InvalidArgument:
                    sb.AppendLine("Invalid Argument: " + InvalidArgument);
                    break;
                case ArgumentParsingResultType.Command:
                case ArgumentParsingResultType.NoCommand:
                    if (flags.Count > 0)
                    {
                        var flagNames = new List<string>(flags.Keys);
                        sb.AppendLine("Flags: " + string.Join(", ", flagNames.ToArray()));
                    }
                    if (options.Count > 0)
                    {
                        sb.AppendLine("Options:");
                        var optionNames = new List<string>(options.Keys);
                        optionNames.Sort();
                        foreach (var n in optionNames)
                        {
                            sb.AppendLine("  * " + n + " = " + options[n]);
                        }
                    }
                    if (Rest != null && Rest.Length > 0)
                    {
                        sb.AppendLine("Rest: " + string.Join(" ", Rest));
                    }
                    break;
                default:
                    break;
            }
            return sb.ToString();
        }
    }

    enum ArgumentParsingResultType
    {
        InvalidArgument,
        Help,
        Command,
        NoCommand
    }
}
