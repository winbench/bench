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
                w.Text(" | ");
                w.Keyword("--" + alias);
            }
            w.Text(" | ");
            w.Keyword("-" + a.Mnemonic);
        }

        private static void FormatOption(DocumentWriter w, OptionArgument a)
        {
            FormatFlag(w, a);
            w.Text(" ");
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
                w.Text(", ");
                w.Keyword(alias);
            }
            w.Text(", ");
            w.Keyword(new string(a.Mnemonic, 1));
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
                w.Text(" (").Variable("flag").Text(" | ")
                    .Variable("option").Text(")*");
            }
            else
            {
                if (hasFlags)
                {
                    w.Text(" ").Variable("flag").Text("*");
                }
                if (hasOptions)
                {
                    w.Text(" ").Variable("option").Text("*");
                }
            }
            foreach (var a in p.GetPositionals())
            {
                w.Text(" ").Append(FormatPositional, a);
            }
            if (p.AcceptsAdditionalArguments)
            {
                w.Text(" ...");
            }
        }

        public static void FullCommandChain(DocumentWriter w, CommandBase cmd)
        {
            var cmdChain = cmd.CommandChain();
            for (int i = 0; i < cmdChain.Length; i++)
            {
                if (i > 0) w.Text(" ");
                var c = cmdChain[i];
                w.Keyword(c.Name).Append(FlagsAndOptionsGeneric, c.ArgumentParser);
            }
        }

        public static void CommandSyntax(DocumentWriter w, CommandBase cmd)
        {
            var cmdChain = cmd.CommandChain();
            for (int i = 0; i < cmdChain.Length; i++)
            {
                if (i > 0) w.Text(" ");
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
                w.Text(" ").Variable("sub-command");
            }
        }

        public static void SlimCommandChain(DocumentWriter w, CommandBase cmd)
        {
            var cmdChain = cmd.CommandChain();
            for (int i = 0; i < cmdChain.Length; i++)
            {
                if (i > 0) w.Text(" ");
                var c = cmdChain[i];
                w.Keyword(c.Name);
            }
        }

        public static string CommandAnchor(CommandBase cmd)
        {
            return "cmd_" + cmd.CommandChain("-");
        }

        public static string CommandAnchor(CommandBase cmd, string subCmd)
        {
            return CommandAnchor(cmd) + "-" + subCmd;
        }

        private static Document allHelpIndicators;
        private static Document AllHelpIndicators
        {
            get
            {
                if (allHelpIndicators == null)
                {
                    var d = new Document();
                    var helpIndicators = ArgumentParser.HelpIndicators;
                    for (int i = 0; i < helpIndicators.Length; i++)
                    {
                        if (i > 0) d.Text(", ");
                        d.Keyword(helpIndicators[i]);
                    }
                    allHelpIndicators = d;
                }
                return allHelpIndicators;
            }
        }

        public static void WriteHelp(DocumentWriter w, CommandBase cmd,
            bool withHelpSection = true, bool withCommandLinks = false)
        {
            var parser = cmd.ArgumentParser;
            w.Append(parser.Description);
            WriteUsage(w, cmd, withHelp: !withHelpSection);
            if (withHelpSection) WriteHelpUsage(w, cmd);
            WriteFlags(w, cmd);
            WriteOptions(w, cmd);
            WritePositionals(w, cmd);
            WriteCommands(w, cmd, withCommandLinks);
            WriteAdditionalArguments(w, cmd);
        }

        private static void WriteUsage(DocumentWriter w, CommandBase cmd,
            bool withHelp = false)
        {
            w.Headline2(CommandAnchor(cmd) + "_usage", "Usage");

            w.Begin(BlockType.List);
            if (withHelp)
            {
                w.Begin(BlockType.ListItem)
                    .Append(SlimCommandChain, cmd).Text(" ")
                    .Keyword(ArgumentParser.MainHelpIndicator);
                w.End(BlockType.ListItem);
            }
            w.ListItem(FullCommandChain, cmd);
            if (HasCommands(cmd.ArgumentParser))
            {
                w.Begin(BlockType.ListItem)
                    .Append(FullCommandChain, cmd)
                    .Text(" ").Variable("command").Text(" ...");
                w.End(BlockType.ListItem);
            }
            w.End(BlockType.List);
        }

        private static void WriteHelpUsage(DocumentWriter w, CommandBase cmd)
        {
            w.Headline2(CommandAnchor(cmd) + "_help", "Help");
            w.Begin(BlockType.Paragraph)
                .Text("Showing the help can be triggered by one of the following flags: ")
                .Append(AllHelpIndicators)
                .Text(".");
            w.End(BlockType.Paragraph);
            w.Begin(BlockType.List);
            w.Begin(BlockType.ListItem)
                .Append(SlimCommandChain, cmd).Text(" ")
                .Keyword(ArgumentParser.MainHelpIndicator);
            w.End(BlockType.ListItem);
            if (HasCommands(cmd.ArgumentParser))
            {
                w.Begin(BlockType.ListItem)
                    .Append(SlimCommandChain, cmd)
                    .Text(" ").Variable("command").Text(" ")
                    .Keyword(ArgumentParser.MainHelpIndicator);
                w.End(BlockType.ListItem);
            }
            w.End(BlockType.List);
        }

        private static void WriteFlags(DocumentWriter w, CommandBase cmd)
        {
            var flags = cmd.ArgumentParser.GetFlags();
            if (flags.Length > 0)
            {
                w.Headline2(CommandAnchor(cmd) + "_flags", "Flags");
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

        private static void WriteOptions(DocumentWriter w, CommandBase cmd)
        {
            var options = cmd.ArgumentParser.GetOptions();
            if (options.Length > 0)
            {
                w.Headline2(CommandAnchor(cmd) + "_options", "Options");
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

        private static void WritePositionals(DocumentWriter w, CommandBase cmd)
        {
            var positionals = cmd.ArgumentParser.GetPositionals();
            if (positionals.Length > 0)
            {
                w.Headline2(CommandAnchor(cmd) + "_positionals", "Positional Arguments");
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

        private static void WriteCommands(DocumentWriter w, CommandBase cmd, bool withLinks = false)
        {
            var commands = cmd.ArgumentParser.GetCommands();
            if (commands.Length > 0)
            {
                w.Headline2(CommandAnchor(cmd) + "_commands", "Commands");
                w.Begin(BlockType.DefinitionList);
                foreach (var cmdArg in commands)
                {
                    w.Begin(BlockType.Definition);
                    w.Begin(BlockType.DefinitionTopic);
                    if (withLinks)
                    {
                        w.Begin(BlockType.Link);
                        w.LinkTarget("#" + CommandAnchor(cmd, cmdArg.Name));
                        w.Begin(BlockType.LinkContent);
                    }
                    w.Append(FormatCommand, cmdArg);
                    if (withLinks)
                    {
                        w.End(BlockType.LinkContent);
                        w.End(BlockType.Link);
                    }
                    w.End(BlockType.DefinitionTopic);
                    w.Begin(BlockType.DefinitionContent);
                    if (!cmdArg.Description.IsEmpty)
                    {
                        w.Paragraph(cmdArg.Description);
                    }
                    if (!cmdArg.SyntaxInfo.IsEmpty)
                    {
                        w.Begin(BlockType.PropertyList);
                        w.Property("Syntax", cmdArg.SyntaxInfo);
                        w.End(BlockType.PropertyList);
                    }
                    w.End(BlockType.DefinitionContent);
                    w.End(BlockType.Definition);
                }
                w.End(BlockType.DefinitionList);
            }
        }

        private static void WriteAdditionalArguments(DocumentWriter w, CommandBase cmd)
        {
            if (!cmd.ArgumentParser.AcceptsAdditionalArguments ||
                cmd.ArgumentParser.AdditionalArgumentsDescription.IsEmpty) return;
            w.Headline2(CommandAnchor(cmd) + "_additional_args", "Additional Arguments");
            w.Paragraph(cmd.ArgumentParser.AdditionalArgumentsDescription);
        }
    }
}
