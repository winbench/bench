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
            w.BeginSyntaxListItem();
            WriteFullCommandChain(w, parser);
            w.EndSyntaxListItem();
            w.BeginSyntaxListItem();
            WriteFullCommandChain(w, parser);
            if (HasCommands(parser))
            {
                w.SyntaxElement(" ");
                w.Variable("command");
                w.SyntaxElement(" ...");
            }
            w.EndSyntaxListItem();
            w.EndSyntaxList();

            w.BeginSyntaxList("Help");
            w.BeginSyntaxListItem();
            WriteSlimCommandChain(w, parser);
            WriteFullHelpIndicator(w);
            w.EndSyntaxListItem();
            if (HasCommands(parser))
            {
                w.BeginSyntaxListItem();
                WriteSlimCommandChain(w, parser);
                w.SyntaxElement(" ");
                w.Variable("command");
                WriteFullHelpIndicator(w);
                w.EndSyntaxListItem();
            }
            w.EndSyntaxList();

            var flags = parser.GetFlags();
            if (flags.Length > 0)
            {
                w.BeginSyntaxList("Flags");
                foreach (FlagArgument flag in flags)
                {
                    w.BeginSyntaxListItem();
                    FormatFlag(w, flag);
                    w.EndSyntaxListItem();
                    w.BeginDetail();
                    if (!flag.Description.IsEmpty)
                    {
                        w.BeginLine();
                        flag.Description.WriteTo(w);
                        w.EndLine();
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
                    w.BeginSyntaxListItem();
                    FormatOption(w, option);
                    w.EndSyntaxListItem();
                    w.BeginDetail();
                    if (hasDefinitions)
                    {
                        if (!option.Description.IsEmpty)
                        {
                            w.BeginParagraph();
                            w.BeginLine();
                            option.Description.WriteTo(w); ;
                            w.EndLine();
                            w.EndParagraph();
                        }
                        w.BeginDefinitionList();
                        if (!option.PossibleValueInfo.IsEmpty)
                        {
                            w.BeginDefinition("Expected");
                            option.PossibleValueInfo.WriteTo(w);
                            w.EndDefinition();
                        }
                        if (!option.DefaultValueInfo.IsEmpty)
                        {
                            w.BeginDefinition("Default");
                            option.DefaultValueInfo.WriteTo(w);
                            w.EndDefinition();
                        }
                        w.EndDefinitionList();
                    }
                    else if (!option.Description.IsEmpty)
                    {
                        w.BeginLine();
                        option.Description.WriteTo(w);
                        w.EndLine();
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
                    w.BeginSyntaxListItem();
                    FormatCommand(w, cmd);
                    w.EndSyntaxListItem();
                    w.BeginDetail();
                    if (!cmd.Description.IsEmpty)
                    {
                        w.BeginParagraph();
                        w.BeginLine();
                        cmd.Description.WriteTo(w);
                        w.EndLine();
                        w.EndParagraph();
                    }
                    if (!cmd.SyntaxInfo.IsEmpty)
                    {
                        w.BeginDefinitionList();
                        w.BeginDefinition("Syntax");
                        cmd.SyntaxInfo.WriteTo(w);
                        w.EndDefinition();
                        w.EndDefinitionList();
                    }
                    w.EndDetail();
                }
                w.EndSyntaxList();
            }
        }
    }
}
