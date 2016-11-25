using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.Bench.Cli
{
    static class HelpFormatter
    {
        private static void FormatFlag(IDocumentWriter w, Argument a)
        {
            w.Keyword("--" + a.Name);
            foreach (var alias in a.Aliases)
            {
                w.SyntaxElement(" | ");
                w.Keyword("--" + alias);
            }
            w.SyntaxElement(" | ");
            w.Keyword("-" + a.Mnemonic);
        }

        private static void FormatOption(IDocumentWriter w, Argument a)
        {
            FormatFlag(w, a);
            w.SyntaxElement(" ");
            w.Variable("value");
        }

        private static void FormatCommand(IDocumentWriter w, Argument a)
        {
            w.Keyword(a.Name);
            foreach (var alias in a.Aliases)
            {
                w.SyntaxElement(", ");
                w.Keyword(alias);
            }
            w.SyntaxElement(", ");
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

        private static void WriteFlagsAndOptionsGeneric(IDocumentWriter w, ArgumentParser p)
        {
            var hasFlags = HasFlags(p);
            var hasOptions = HasOptions(p);
            if (hasFlags && hasOptions)
            {
                w.SyntaxElement(" (");
                w.Variable("flag");
                w.SyntaxElement(" | ");
                w.Variable("option");
                w.SyntaxElement(")*");
            }
            else
            {
                if (hasFlags)
                {
                    w.SyntaxElement(" ");
                    w.Variable("flag");
                    w.SyntaxElement("*");
                }
                if (hasOptions)
                {
                    w.SyntaxElement(" ");
                    w.Variable("option");
                    w.SyntaxElement("*");
                }
            }
        }

        private static void WriteFullCommandChain(IDocumentWriter w, ArgumentParser parser)
        {
            var parserChain = GetParserChain(parser);
            for (int i = 0; i < parserChain.Length; i++)
            {
                if (i > 0) w.SyntaxElement(" ");
                var p = parserChain[i];
                w.Keyword(p.Name);
                WriteFlagsAndOptionsGeneric(w, p);
            }
        }

        private static void WriteSlimCommandChain(IDocumentWriter w, ArgumentParser parser)
        {
            var parserChain = GetParserChain(parser);
            for (int i = 0; i < parserChain.Length; i++)
            {
                if (i > 0) w.SyntaxElement(" ");
                var p = parserChain[i];
                w.Keyword(p.Name);
            }
        }

        private static void WriteFullHelpIndicator(IDocumentWriter w)
        {
            w.SyntaxElement(" (");
            var helpIndicators = ArgumentParser.HelpIndicators;
            for (int i = 0; i < helpIndicators.Length; i++)
            {
                if (i > 0) w.SyntaxElement(" | ");
                w.Keyword(helpIndicators[i]);
            }
            w.SyntaxElement(")");
        }

        public static void WriteHelp(IDocumentWriter w, ArgumentParser parser)
        {
            w.BeginSyntaxList("Usage");
            w.BeginSyntax();
            WriteFullCommandChain(w, parser);
            w.EndSyntax();
            w.BeginSyntax();
            WriteFullCommandChain(w, parser);
            if (HasCommands(parser))
            {
                w.SyntaxElement(" ");
                w.Variable("command");
                w.SyntaxElement(" ...");
            }
            w.EndSyntax();
            w.EndSyntaxList();

            w.BeginSyntaxList("Help");
            w.BeginSyntax();
            WriteSlimCommandChain(w, parser);
            WriteFullHelpIndicator(w);
            w.EndSyntax();
            if (HasCommands(parser))
            {
                w.BeginSyntax();
                WriteSlimCommandChain(w, parser);
                w.SyntaxElement(" ");
                w.Variable("command");
                WriteFullHelpIndicator(w);
                w.EndSyntax();
            }
            w.EndSyntaxList();

            var flags = parser.GetFlags();
            if (flags.Length > 0)
            {
                w.BeginSyntaxList("Flags");
                foreach (FlagArgument flag in flags)
                {
                    w.BeginSyntax();
                    FormatFlag(w, flag);
                    w.EndSyntax();
                    w.BeginDetail();
                    if (flag.Description != null)
                    {
                        w.Line(flag.Description);
                    }
                    w.EndDetail();
                }
                w.EndSyntaxList();
            }
            var options = parser.GetOptions();
            if (options.Length > 0)
            {
                w.BeginSyntaxList("Options");
                foreach (OptionArgument option in options)
                {
                    var hasDefinitions = option.PossibleValueInfo != null || option.DefaultValueInfo != null;
                    w.BeginSyntax();
                    FormatOption(w, option);
                    w.EndSyntax();
                    w.BeginDetail();
                    if (hasDefinitions)
                    {
                        if (option.Description != null)
                        {
                            w.BeginParagraph();
                            w.Line(option.Description);
                            w.EndParagraph();
                        }
                        w.BeginDefinitionList();
                        if (option.PossibleValueInfo != null)
                        {
                            w.Definition("Expected", option.PossibleValueInfo);
                        }
                        if (option.DefaultValueInfo != null)
                        {
                            w.Definition("Default", option.DefaultValueInfo);
                        }
                        w.EndDefinitionList();
                    }
                    else if (option.Description != null)
                    {
                        w.Line(option.Description);
                    }
                    w.EndDetail();
                }
                w.EndSyntaxList();
            }
            var commands = parser.GetCommands();
            if (commands.Length > 0)
            {
                w.BeginSyntaxList("Commands");
                foreach (CommandArgument cmd in parser.GetCommands())
                {
                    w.BeginSyntax();
                    FormatCommand(w, cmd);
                    w.EndSyntax();
                    w.BeginDetail();
                    if (cmd.Description != null)
                    {
                        w.BeginParagraph();
                        w.Line(cmd.Description);
                        w.EndParagraph();
                    }
                    if (cmd.SyntaxInfo != null)
                    {
                        w.BeginDefinitionList();
                        w.Definition("Syntax", cmd.SyntaxInfo);
                        w.EndDefinitionList();
                    }
                    w.EndDetail();
                }
                w.EndSyntaxList();
            }
        }
    }
}
