using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace MarkdownSharp.Extensions
{
    class GitHubCodeBlockExtension : IExtensionInterface
    {
        public string Transform(string text)
        {
            return DoGithubCodeBlocks(text);
        }

        private static Regex _newlinesLeadingTrailing = new Regex(@"^\n+|\n+\z", RegexOptions.Compiled);

        private static Regex _githubCodeBlock = new Regex(@"(?<!\\)(`{3,}) *(\S+)? *\n([\s\S]+?)\s*\1 *(?:\n+|$)", RegexOptions.Compiled);

        private string DoGithubCodeBlocks(string text)
        {
            return _githubCodeBlock.Replace(text, new MatchEvaluator(GithubCodeEvaluator));
        }

        private string GithubCodeEvaluator(Match match)
        {
            string codeBlock = match.Groups[3].Value;
            string typeBlock = match.Groups[2].Value;

            //removed Outdent on the codeblock
            codeBlock = Markdown.EncodeCode(codeBlock);
            codeBlock = _newlinesLeadingTrailing.Replace(codeBlock, "");

            return string.Concat("\n\n<pre><code class=\"language-", typeBlock, "\">", codeBlock,
                "\n</code></pre>\n\n");
        }
    }
}
