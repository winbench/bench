using System;
using System.Collections.Generic;
using System.Text;
using Mastersign.Docs;

namespace Mastersign.CliTools
{
    public static class HelpFormatter
    {
        private static void FormatFlag(DocumentWriter w, NamedArgument a)
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

        private static void FormatOption(DocumentWriter w, OptionArgument a)
        {
            FormatFlag(w, a);
            w.Syntactic(" ");
            w.Variable("value");
        }

        private static void FormatPositional(DocumentWriter w, PositionalArgument a)
        {
            w.Variable(a.Name);
        }

        private static void FormatCommand(DocumentWriter w, CommandArgument a)
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

        private static bool HasPositionals(ArgumentParser p)
        {
            return p.GetPositionals().Length > 0;
        }

        public static void FlagsAndOptionsGeneric(DocumentWriter w, ArgumentParser p)
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
            foreach (var a in p.GetPositionals())
            {
                w.Syntactic(" ").Append(FormatPositional, a);
            }
        }

        public static void FullCommandChain(DocumentWriter w, CommandBase cmd)
        {
            var cmdChain = cmd.CommandChain();
            for (int i = 0; i < cmdChain.Length; i++)
            {
                if (i > 0) w.Syntactic(" ");
                var c = cmdChain[i];
                w.Keyword(c.Name).Append(FlagsAndOptionsGeneric, c.ArgumentParser);
            }
        }

        public static void CommandSyntax(DocumentWriter w, CommandBase cmd)
        {
            var cmdChain = cmd.CommandChain();
            for (int i = 0; i < cmdChain.Length; i++)
            {
                if (i > 0) w.Syntactic(" ");
                var c = cmdChain[i];
                w.Keyword(c.Name);
            }
            if (cmdChain.Length > 0)
            {
                var p = cmdChain[cmdChain.Length - 1].ArgumentParser;
                w.Append(FlagsAndOptionsGeneric, p);
            }
            if (cmd.ArgumentParser.GetCommands().Length > 0)
            {
                w.Syntactic(" ").Variable("sub-command");
            }
        }

        public static void SlimCommandChain(DocumentWriter w, CommandBase cmd)
        {
            var cmdChain = cmd.CommandChain();
            for (int i = 0; i < cmdChain.Length; i++)
            {
                if (i > 0) w.Syntactic(" ");
                var c = cmdChain[i];
                w.Keyword(c.Name);
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

        public static void WriteHelp(DocumentWriter w, CommandBase cmd)
        {
            var parser = cmd.ArgumentParser;
            w.Append(parser.Description);
            WriteUsage(w, cmd);
            WriteHelpUsage(w, cmd);
            WriteFlags(w, parser);
            WriteOptions(w, parser);
            WritePositionals(w, parser);
            WriteCommands(w, parser);
        }

        private static void WriteUsage(DocumentWriter w, CommandBase cmd)
        {
            w.Headline2("Usage");

            w.Begin(BlockType.List);
            w.ListItem(FullCommandChain, cmd);
            if (HasCommands(cmd.ArgumentParser))
            {
                w.Begin(BlockType.ListItem);
                w.Append(FullCommandChain, cmd);
                w.Syntactic(" ").Variable("command").Syntactic(" ...");
                w.End(BlockType.ListItem);
            }
            w.End(BlockType.List);
        }

        private static void WriteHelpUsage(DocumentWriter w, CommandBase cmd)
        {
            w.Headline2("Help");

            w.Begin(BlockType.List);
            w.Begin(BlockType.ListItem);
            w.Append(SlimCommandChain, cmd);
            w.Append(FullHelpIndicator);
            w.End(BlockType.ListItem);
            if (HasCommands(cmd.ArgumentParser))
            {
                w.Begin(BlockType.ListItem);
                w.Append(SlimCommandChain, cmd)
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
                foreach (var flag in flags)
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
                foreach (var option in options)
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

        private static void WritePositionals(DocumentWriter w, ArgumentParser parser)
        {
            var positionals = parser.GetPositionals();
            if (positionals.Length > 0)
            {
                w.Headline2("Positional Arguments");
                w.Begin(BlockType.DefinitionList);
                foreach (var pArg in positionals)
                {
                    var hasDefinitions = pArg.PossibleValueInfo != null;
                    w.Begin(BlockType.Definition);
                    w.Begin(BlockType.DefinitionTopic)
                        .Text(pArg.OrderIndex.ToString().PadLeft(2) + ". ")
                        .Text(pArg.Name)
                        .End(BlockType.DefinitionTopic);
                    w.Begin(BlockType.DefinitionContent);
                    if (hasDefinitions)
                    {
                        if (!pArg.Description.IsEmpty)
                        {
                            w.Paragraph(pArg.Description);
                        }
                        w.Begin(BlockType.PropertyList);
                        if (!pArg.PossibleValueInfo.IsEmpty)
                        {
                            w.Property("Expected", pArg.PossibleValueInfo);
                        }
                        w.End(BlockType.PropertyList);
                    }
                    else if (!pArg.Description.IsEmpty)
                    {
                        w.Append(pArg.Description);
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
                foreach (var cmd in parser.GetCommands())
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
