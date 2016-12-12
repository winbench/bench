using System;
using System.Collections.Generic;
using System.Text;
using Mastersign.Docs;

namespace Mastersign.CliTools
{
    public class ArgumentParser
    {
        public ArgumentParserType ParserType { get; set; }

        public static string[] HelpIndicators = new[] { "/?", "-?", "-h", "--help" };

        public static string MainHelpIndicator = "-?";

        private readonly Dictionary<string, Argument> arguments = new Dictionary<string, Argument>();

        public string Name { get; private set; }

        public Document Description { get; private set; }

        public ArgumentParser(string name, IEnumerable<Argument> arguments)
        {
            Name = name;
            ParserType = ArgumentParserType.CaseSensitive;
            Description = new Document();
            foreach (var arg in arguments)
            {
                RegisterArgument(arg);
            }
        }

        public ArgumentParser(string name, params Argument[] arguments)
            : this(name, (IEnumerable<Argument>)arguments)
        {
        }

        public void RegisterArgument(Argument arg)
        {
            arguments.Add(arg.Name, arg);
        }

        public void RegisterArguments(params Argument[] arguments)
        {
            foreach (var arg in arguments)
            {
                RegisterArgument(arg);
            }
        }

        private T[] FilterArguments<T>(ArgumentType type) where T : Argument
        {
            var res = new List<T>();
            foreach (var a in arguments.Values)
            {
                if (a.Type == type)
                {
                    res.Add((T)a);
                }
            }
            res.Sort((a, b) => a.Name.CompareTo(b.Name));
            return res.ToArray();
        }

        public FlagArgument[] GetFlags()
            => FilterArguments<FlagArgument>(ArgumentType.Flag);

        public OptionArgument[] GetOptions()
            => FilterArguments<OptionArgument>(ArgumentType.Option);

        public PositionalArgument[] GetPositionals()
        {
            var list = new List<PositionalArgument>(FilterArguments<PositionalArgument>(ArgumentType.Positional));
            list.Sort((a1, a2) => a1.OrderIndex.CompareTo(a2.OrderIndex));
            return list.ToArray();
        }

        public CommandArgument[] GetCommands()
            => FilterArguments<CommandArgument>(ArgumentType.Command);

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

        private string[] MissingPositinalArguments(int foundArgumentsCount)
        {
            var positionalArgs = GetPositionals();
            var missingArgs = Math.Max(0, positionalArgs.Length - foundArgumentsCount);
            var list = new string[missingArgs];
            for (int i = 0; i < missingArgs; i++)
            {
                list[i] = positionalArgs[foundArgumentsCount + i].Name;
            }
            return list;
        }

        public ArgumentParsingResult Parse(string[] args)
        {
            var index = new ArgumentIndex(
                ParserType == ArgumentParserType.CaseSensitive,
                arguments.Values);
            IDictionary<string, bool> flagValues = new Dictionary<string, bool>();
            IDictionary<string, string> optionValues = new Dictionary<string, string>();
            IDictionary<string, string> positionalValues = new Dictionary<string, string>();
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
                    position++;
                    continue;
                }
                var a = index.LookUp(args[position], positionalValues.Count);
                if (a == null)
                {
                    if (GetCommands().Length > 0)
                    {
                        invalid = args[position];
                    }
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
                    case ArgumentType.Positional:
                        var pArg = (PositionalArgument)a;
                        if (pArg.ValuePredicate != null && !pArg.ValuePredicate(args[position]))
                        {
                            invalid = pArg.Name + ": " + args[position];
                            break;
                        }
                        positionalValues[a.Name] = args[position];
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
                return new ArgumentParsingResult(this, ArgumentParsingResultType.Help,
                    null, null, null, optionValues, flagValues, positionalValues);
            }
            if (invalid != null)
            {
                return new ArgumentParsingResult(this, ArgumentParsingResultType.InvalidArgument,
                    null, invalid, null, optionValues, flagValues, positionalValues);
            }
            var missingPositionalArguments = MissingPositinalArguments(positionalValues.Count);
            if (missingPositionalArguments.Length > 0)
            {
                return new ArgumentParsingResult(this, ArgumentParsingResultType.MissingArgument,
                    null, string.Join(", ", missingPositionalArguments),
                    null, optionValues, flagValues, positionalValues);
            }
            var rest = new string[args.Length - position];
            Array.Copy(args, position, rest, 0, rest.Length);
            if (command != null)
            {
                return new ArgumentParsingResult(this, ArgumentParsingResultType.Command,
                    command, null, rest, optionValues, flagValues, positionalValues);
            }
            return new ArgumentParsingResult(this, ArgumentParsingResultType.NoCommand,
                null, null, rest, optionValues, flagValues, positionalValues);
        }
    }

    internal class ArgumentIndex
    {
        private readonly Dictionary<string, Argument> flags = new Dictionary<string, Argument>();
        private readonly Dictionary<string, Argument> flagMnemonics = new Dictionary<string, Argument>();
        private readonly Dictionary<string, Argument> options = new Dictionary<string, Argument>();
        private readonly Dictionary<string, Argument> optionMnemonics = new Dictionary<string, Argument>();
        private readonly Dictionary<string, Argument> commands = new Dictionary<string, Argument>();
        private readonly Dictionary<string, Argument> commandMnemonics = new Dictionary<string, Argument>();
        private readonly List<Argument> positionals = new List<Argument>();

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
            var namedArg = arg as NamedArgument;
            switch (arg.Type)
            {
                case ArgumentType.Flag:
                    flags[PrepareArgument(arg.Name)] = arg;
                    foreach (var alias in namedArg.Aliases)
                    {
                        flags[PrepareArgument(alias)] = arg;
                    }
                    flagMnemonics[PrepareArgument(namedArg.Mnemonic)] = arg;
                    break;
                case ArgumentType.Option:
                    options[PrepareArgument(arg.Name)] = arg;
                    foreach (var alias in namedArg.Aliases)
                    {
                        options[PrepareArgument(alias)] = arg;
                    }
                    optionMnemonics[PrepareArgument(namedArg.Mnemonic)] = arg;
                    break;
                case ArgumentType.Command:
                    commands[PrepareArgument(arg.Name)] = arg;
                    foreach (var alias in namedArg.Aliases)
                    {
                        commands[PrepareArgument(alias)] = arg;
                    }
                    commandMnemonics[PrepareArgument(namedArg.Mnemonic)] = arg;
                    break;
                case ArgumentType.Positional:
                    positionals.Add(arg);
                    break;
                default:
                    throw new NotSupportedException();
            }
            positionals.Sort((a1, a2) =>
                ((PositionalArgument)a1).OrderIndex.CompareTo(((PositionalArgument)a2).OrderIndex));
        }

        public Argument LookUp(string v, int consumedPositionals)
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

            if (consumedPositionals < positionals.Count) return positionals[consumedPositionals];

            return null;
        }
    }

    public enum ArgumentParserType
    {
        CaseSensitive,
        CaseInsensitive
    }

    public enum ArgumentType
    {
        Flag,
        Option,
        Positional,
        Command
    }

    public abstract class Argument
    {
        public ArgumentType Type { get; private set; }

        public string Name { get; private set; }


        public Document Description { get; private set; }

        protected Argument(ArgumentType type, string name)
        {
            Type = type;
            Name = name;
            Description = new Document();
        }
    }

    public abstract class NamedArgument : Argument
    {
        public string Mnemonic { get; private set; }

        public string[] Aliases { get; private set; }

        protected NamedArgument(ArgumentType type, string name, string mnemonic,
            params string[] aliases)
            : base(type, name)
        {
            Mnemonic = mnemonic ?? name.Substring(0, 1);
            Aliases = aliases;
        }
    }

    public class FlagArgument : NamedArgument
    {
        public FlagArgument(string name, string mnemonic,
            params string[] aliases)
            : base(ArgumentType.Flag, name, mnemonic, aliases)
        {
        }
    }

    public delegate bool ArgumentValuePredicate(string value);

    public class OptionArgument : NamedArgument
    {
        public Document PossibleValueInfo { get; private set; }

        public Document DefaultValueInfo { get; private set; }

        public ArgumentValuePredicate ValuePredicate { get; private set; }

        public OptionArgument(string name, string mnemonic,
            ArgumentValuePredicate valuePredicate,
            params string[] aliases)
            : base(ArgumentType.Option, name, mnemonic, aliases)
        {
            PossibleValueInfo = new Document();
            DefaultValueInfo = new Document();
            ValuePredicate = valuePredicate;
        }
    }

    public class EnumOptionArgument<T> : OptionArgument
    {
        private static bool IsEnumMember(string v)
        {
            try
            {
                Enum.Parse(typeof(T), v, true);
                return true;
            }
            catch (ArgumentException)
            {
                return false;
            }
        }

        public T DefaultValue { get; private set; }

        public EnumOptionArgument(string name, string mnemonic,
            T defaultValue, params string[] aliases)
            : base(name, mnemonic, IsEnumMember)
        {
            DefaultValue = defaultValue;
            var enumNames = Enum.GetNames(typeof(T));
            for (int i = 0; i < enumNames.Length; i++)
            {
                if (i > 0) PossibleValueInfo.Text(" | ");
                PossibleValueInfo.Keyword(enumNames[i]);
            }
            DefaultValueInfo.Keyword(defaultValue.ToString());
        }
    }

    public class PositionalArgument : Argument
    {
        public Document PossibleValueInfo { get; private set; }

        public int OrderIndex { get; private set; }

        public ArgumentValuePredicate ValuePredicate { get; private set; }

        public PositionalArgument(string name,
            ArgumentValuePredicate valuePredicate, int position)
            : base(ArgumentType.Positional, name)
        {
            PossibleValueInfo = new Document();
            OrderIndex = position;
            ValuePredicate = valuePredicate;
        }
    }

    public class CommandArgument : NamedArgument
    {
        public Document SyntaxInfo { get; private set; }

        public CommandArgument(string name, string mnemonic,
            params string[] aliases)
            : base(ArgumentType.Command, name, mnemonic, aliases)
        {
            SyntaxInfo = new Document();
        }
    }

    public class ArgumentParsingResult
    {
        public ArgumentParser Parser { get; private set; }

        public ArgumentParsingResultType Type { get; private set; }

        public string Command { get; private set; }

        public string ErrorMessage { get; private set; }

        public string[] Rest { get; private set; }

        private readonly IDictionary<string, string> options;

        public IDictionary<string, string> OptionValues => options;

        private readonly IDictionary<string, bool> flags;

        public IDictionary<string, bool> Flags => flags;

        private readonly IDictionary<string, string> positionals;

        public IDictionary<string, string> PositionalValues => positionals;

        public bool IsCompletedInteractively { get; private set; }

        public ArgumentParsingResult(ArgumentParser parser,
            ArgumentParsingResultType type,
            string command, string errorMessage, string[] rest,
            IDictionary<string, string> options,
            IDictionary<string, bool> flags,
            IDictionary<string, string> positionals)
        {
            Parser = parser;
            Type = type;
            Command = command;
            ErrorMessage = errorMessage;
            Rest = rest;
            this.options = options ?? new Dictionary<string, string>();
            this.flags = flags ?? new Dictionary<string, bool>();
            this.positionals = positionals ?? new Dictionary<string, string>();
        }

        public ArgumentParsingResult DeriveHelp()
        {
            return new ArgumentParsingResult(Parser, ArgumentParsingResultType.Help,
                Command, null, Rest, options, flags, positionals);
        }

        public ArgumentParsingResult DeriveInteractivelyCompleted(
            string command)
        {
            return new ArgumentParsingResult(Parser,
                command != null
                ? ArgumentParsingResultType.Command
                : ArgumentParsingResultType.NoCommand,
                command, null, Rest, options, flags, positionals)
            { IsCompletedInteractively = true };
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

        public string GetPositionalValue(string name, string def = null)
        {
            string res;
            return positionals.TryGetValue(name, out res) ? res : def;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Result Type: " + Type);
            switch (Type)
            {
                case ArgumentParsingResultType.InvalidArgument:
                    sb.AppendLine("Invalid Argument: " + ErrorMessage);
                    break;
                case ArgumentParsingResultType.MissingArgument:
                    sb.AppendLine("Missing Argument(s): " + ErrorMessage);
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
                    if (positionals.Count > 0)
                    {
                        sb.AppendLine("Positionals:");
                        var positionalNames = new List<string>(positionals.Keys);
                        positionalNames.Sort();
                        foreach (var n in positionalNames)
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

    public enum ArgumentParsingResultType
    {
        InvalidArgument,
        MissingArgument,
        Help,
        Command,
        NoCommand
    }
}
