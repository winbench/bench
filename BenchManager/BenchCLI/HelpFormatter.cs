using System;
using System.Collections.Generic;
using System.Text;
using Mastersign.Docs;

namespace Mastersign.Bench.Cli
{
    static class HelpFormatter
    {
        private static void FormatFlag(DocumentWriter w, Argument a)
        {
            w.Keyword("--" + a.Name);
            foreach (var alias in a.Aliases)
            {
                w.Syntactic(" | ");
                w.Keyword("--" + alias);
            }
            w.Syntactic(" | ");
            w.Keyword("-" + a.Mnemonic);
        }

        private static void FormatOption(DocumentWriter w, Argument a)
        {
            FormatFlag(w, a);
            w.Syntactic(" ");
            w.Variable("value");
        }

        private static void FormatCommand(DocumentWriter w, Argument a)
        {
            w.Keyword(a.Name);
            foreach (var alias in a.Aliases)
            {
                w.Syntactic(", ");
                w.Keyword(alias);
            }
            w.Syntactic(", ");
            w.Keyword(a.Mnemonic);
        }

        public static ArgumentParser[] GetParserChain(ArgumentParser parser)
        {
            var list = new List<ArgumentParser>();
            while (parser != null)
            {
                list.Add(parser);
                parser = parser.Parent;
            }
            list.Reverse();
            return list.ToArray();
        }

        public static string[] GetCommandChain(ArgumentParser parser)
        {
            var list = new List<string>();
            foreach (var p in GetParserChain(parser))
            {
                list.Add(p.Name);
            }
            return list.ToArray();
        }

        private static bool HasFlags(ArgumentParser p)
        {
            return p.GetFlags().Length > 0;
        }

        private static bool HasOptions(ArgumentParser p)
        {
            return p.GetOptions().Length > 0;
        }

        private static bool HasCommands(ArgumentParser p)
        {
            return p.GetCommands().Length > 0;
        }

        private static void FlagsAndOptionsGeneric(DocumentWriter w, ArgumentParser p)
        {
            var hasFlags = HasFlags(p);
            var hasOptions = HasOptions(p);
            if (hasFlags && hasOptions)
            {
                w.Syntactic(" (").Variable("flag").Syntactic(" | ")
                    .Variable("option").Syntactic(")*");
            }
            else
            {
                if (hasFlags)
                {
                    w.Syntactic(" ").Variable("flag").Syntactic("*");
                }
                if (hasOptions)
                {
                    w.Syntactic(" ").Variable("option").Syntactic("*");
                }
            }
        }

        private static void FullCommandChain(DocumentWriter w, ArgumentParser parser)
        {
            var parserChain = GetParserChain(parser);
            for (int i = 0; i < parserChain.Length; i++)
            {
                if (i > 0) w.Syntactic(" ");
                var p = parserChain[i];
                w.Keyword(p.Name).Append(FlagsAndOptionsGeneric, p);
            }
        }

        private static void SlimCommandChain(DocumentWriter w, ArgumentParser parser)
        {
            var parserChain = GetParserChain(parser);
            for (int i = 0; i < parserChain.Length; i++)
            {
                if (i > 0) w.Syntactic(" ");
                var p = parserChain[i];
                w.Keyword(p.Name);
            }
        }

        private static Document fullHelpIndicator;
        private static Document FullHelpIndicator
        {
            get
            {
                if (fullHelpIndicator == null)
                {
                    var d = new Document();
                    d.Syntactic(" (");
                    var helpIndicators = ArgumentParser.HelpIndicators;
                    for (int i = 0; i < helpIndicators.Length; i++)
                    {
                        if (i > 0) d.Syntactic(" | ");
                        d.Keyword(helpIndicators[i]);
                    }
                    d.Syntactic(")");
                    fullHelpIndicator = d;
                }
                return fullHelpIndicator;
            }
        }

        public static void WriteHelp(DocumentWriter w, ArgumentParser parser)
        {
            WriteUsage(w, parser);
            WriteHelpUsage(w, parser);
            WriteFlags(w, parser);
            WriteOptions(w, parser);
            WriteCommands(w, parser);
        }

        private static void WriteUsage(DocumentWriter w, ArgumentParser parser)
        {
            w.Headline2("Usage");

            w.Begin(BlockType.List);
            w.ListItem(FullCommandChain, parser);
            w.Begin(BlockType.ListItem);
            w.Append(FullCommandChain, parser);
            if (HasCommands(parser))
            {
                w.Syntactic(" ").Variable("command").Syntactic(" ...");
            }
            w.End(BlockType.ListItem);
            w.End(BlockType.List);
        }

        private static void WriteHelpUsage(DocumentWriter w, ArgumentParser parser)
        {
            w.Headline2("Help");

            w.Begin(BlockType.List);
            w.Begin(BlockType.ListItem);
            w.Append(SlimCommandChain, parser);
            w.Append(FullHelpIndicator);
            w.End(BlockType.ListItem);
            if (HasCommands(parser))
            {
                w.Begin(BlockType.ListItem);
                w.Append(SlimCommandChain, parser)
                    .Syntactic(" ").Variable("command")
                    .Append(FullHelpIndicator);
                w.End(BlockType.ListItem);
            }
            w.End(BlockType.List);
        }

        private static void WriteFlags(DocumentWriter w, ArgumentParser parser)
        {
            var flags = parser.GetFlags();
            if (flags.Length > 0)
            {
                w.Headline2("Flags");
                w.Begin(BlockType.DefinitionList);
                foreach (FlagArgument flag in flags)
                {
                    w.Begin(BlockType.Definition);
                    w.DefinitionTopic(FormatFlag, flag);
                    w.DefinitionContent(flag.Description);
                    w.End(BlockType.Definition);
                }
                w.End(BlockType.DefinitionList);
            }
        }

        private static void WriteOptions(DocumentWriter w, ArgumentParser parser)
        {
            var options = parser.GetOptions();
            if (options.Length > 0)
            {
                w.Headline2("Options");
                w.Begin(BlockType.DefinitionList);
                foreach (OptionArgument option in options)
                {
                    var hasDefinitions = option.PossibleValueInfo != null || option.DefaultValueInfo != null;
                    w.Begin(BlockType.Definition);
                    w.DefinitionTopic(FormatOption, option);
                    w.Begin(BlockType.DefinitionContent);
                    if (hasDefinitions)
                    {
                        if (!option.Description.IsEmpty)
                        {
                            w.Paragraph(option.Description);
                        }
                        w.Begin(BlockType.PropertyList);
                        if (!option.PossibleValueInfo.IsEmpty)
                        {
                            w.Property("Expected", option.PossibleValueInfo);
                        }
                        if (!option.DefaultValueInfo.IsEmpty)
                        {
                            w.Property("Default", option.DefaultValueInfo);
                        }
                        w.End(BlockType.PropertyList);
                    }
                    else if (!option.Description.IsEmpty)
                    {
                        w.Append(option.Description);
                    }
                    w.End(BlockType.DefinitionContent);
                    w.End(BlockType.Definition);
                }
                w.End(BlockType.DefinitionList);
            }
        }

        private static void WriteCommands(DocumentWriter w, ArgumentParser parser)
        {
            var commands = parser.GetCommands();
            if (commands.Length > 0)
            {
                w.Headline2("Commands");
                w.Begin(BlockType.DefinitionList);
                foreach (CommandArgument cmd in parser.GetCommands())
                {
                    w.Begin(BlockType.Definition);
                    w.DefinitionTopic(FormatCommand, cmd);
                    w.Begin(BlockType.DefinitionContent);
                    if (!cmd.Description.IsEmpty)
                    {
                        w.Paragraph(cmd.Description);
                    }
                    if (!cmd.SyntaxInfo.IsEmpty)
                    {
                        w.Begin(BlockType.PropertyList);
                        w.Property("Syntax", cmd.SyntaxInfo);
                        w.End(BlockType.PropertyList);
                    }
                    w.End(BlockType.DefinitionContent);
                    w.End(BlockType.Definition);
                }
                w.End(BlockType.DefinitionList);
            }
        }
    }
}
