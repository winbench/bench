using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Mastersign.Bench.Markdown
{
    /// <summary>
    /// This class parses Markdown files for configuration properties.
    /// </summary>
    public class MarkdownPropertyParser
    {
        #region Static Members

        private static readonly Regex MdListExp = new Regex("^[\\*\\+-]\\s+(?<key>[a-zA-Z][a-zA-Z0-9]*?)\\s*:\\s*(?<value>.*?)\\s*$");
        private static readonly string MdListFormat = "* {0}: {1}";

        private static readonly Regex TrueValueExp = new Regex("^true$", RegexOptions.IgnoreCase);
        private static readonly Regex ListValueExp = new Regex("^`.*?`(?:\\s*,\\s*`.*?`)+$");
        private static readonly char[] ListSeparator = { ',' };
        private static readonly string ListSeparatorStr = ", ";
        private static readonly Regex ListStartExp = new Regex("^[\\*\\+-]\\s+(?<key>[a-zA-Z][a-zA-Z0-9]*?)\\s*:\\s*$");
        private static readonly Regex ListElementExp = new Regex("^(?:\t|  +)[\\*\\+-]\\s+(?<value>.*?)\\s*$");
        private static readonly string ListElementFormat = "`{0}`";

        private static readonly Regex DefaultGroupDocsBeginCue = new Regex("^#{3}\\s+\\S");
        private static readonly Regex DefaultGroupDocsEndCue = new Regex("^#{1,3}\\s+\\S");
        private static readonly Regex DefaultGroupBeginCue = new Regex("^###\\s+(?<category>.+?)\\s*(?:\\{.*?\\})?\\s*(?:###)?$");
        private static readonly Regex DefaultGroupEndCue = new Regex("^\\s*$");

        private static readonly Regex DefaultCategoryCue = new Regex("^##\\s+(?<category>.+?)\\s*(?:\\{.*?\\})?\\s*(?:##)?$");

        private static bool IsListStart(string line, ref string key)
        {
            var m = ListStartExp.Match(line);
            if (m.Success)
            {
                key = m.Groups["key"].Value;
                return true;
            }
            return false;
        }

        private static bool IsListElement(string line, IList<string> elements)
        {
            var m = ListElementExp.Match(line);
            if (m.Success)
            {
                var value = m.Groups["value"].Value;
                elements.Add(string.Format(ListElementFormat, CleanListElement(value)));
                return true;
            }
            return false;
        }

        private static string CleanListElement(string value)
        {
            var tickQuotes =
                value.StartsWith("`") || value.EndsWith("`");
            var angleQuotes =
                value.StartsWith("<") && value.Contains(">") ||
                value.EndsWith(">") && value.Contains("<");
            if (tickQuotes)
            {
                value = value.Replace("`", "");
            }
            if (angleQuotes)
            {
                value = value.Replace("<", "").Replace(">", "");
            }
            return value;
        }

        private static string RemoveQuotes(string value)
        {
            value = value.Trim();
            if (IsQuoted(value))
            {
                return value.Substring(1, value.Length - 2);
            }
            else
            {
                return value;
            }
        }

        private static bool IsQuoted(string value)
        {
            return value.StartsWith("`") && value.EndsWith("`") ||
                   value.StartsWith("<") && value.EndsWith(">");
        }

        #endregion

        /// <summary>
        /// This regular expression is used to activate the property parsing.
        /// If this property is not <c>null</c>, the recognition of properties
        /// starts after a line matches this expression.
        /// </summary>
        public Regex ActivationCue { get; set; }

        /// <summary>
        /// This regular expression is used to deactivate the property parsing.
        /// If this property is not <c>null</c>, the recognition of properties
        /// stops after a line matches this expression.
        /// </summary>
        public Regex DeactivationCue { get; set; }

        /// <summary>
        /// This regular expression is used to detect the beginning of a group category
        /// in the Markdown file.
        /// If this property is not <c>null</c>, and a line matches this expression,
        /// the current category is changed for all further property groups in the file.
        /// </summary>
        /// <remarks>
        /// The regular expression needs a named capture group with name <c>category</c>.
        /// </remarks>
        public Regex CategoryCue { get; set; }

        /// <summary>
        /// This regular expression is used to detect the beginning of a property group.
        /// Properties, which are detected before this expression matches a line,
        /// are stored as ungrouped properties.
        /// Properties, which are detected after this expression matches a line,
        /// are stored as group properties.
        /// </summary>
        /// <remarks>
        /// The regular expression needs a named capture group with name <c>group</c>.
        /// </remarks>
        public Regex GroupBeginCue { get; set; }

        /// <summary>
        /// This regular expression is used to detect the beginning of the documentation
        /// of a group. All lines, which are not recognized as properties or another cue,
        /// are collected as the documentation for the group.
        /// </summary>
        public Regex GroupDocsBeginCue { get; set; }

        /// <summary>
        /// This regular expression is used to detect the end of a property group.
        /// Properties, which are recognized after this expression matches a line,
        /// are stored as ungrouped properties.
        /// </summary>
        public Regex GroupEndCue { get; set; }

        /// <summary>
        /// This regular expression is used to detect the end of the documentation
        /// of a group. When this cue is triggered, the collected documentation
        /// is attached to all groups, which where detected by <see cref="GroupBeginCue"/>
        /// since the last <see cref="GroupDocsBeginCue"/>.
        /// </summary>
        public Regex GroupDocsEndCue { get; set; }

        /// <summary>
        /// Gets or sets the property target, where recognized grouoped properties are stored in.
        /// </summary>
        public IGroupedPropertyTarget GroupPropertyTarget { get; set; }

        /// <summary>
        /// Gets or sets the target, where recognized ungrouped properties are stored in.
        /// </summary>
        public IPropertyTarget PropertyTarget { get; set; }

        /// <summary>
        /// Gets or sets a metadata object, which is going to be attached to every group,
        /// properties are recognized for.
        /// </summary>
        public object CurrentGroupMetadata { get; set; }

        private readonly List<string> collectedGroupDocs = new List<string>();
        private readonly List<string> docGroups = new List<string>();

        /// <summary>
        /// A switch to control, whether non-property lines are collected
        /// as the documentation for a group.
        /// </summary>
        public bool CollectGroupDocs { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="MarkdownPropertyParser"/>.
        /// </summary>
        public MarkdownPropertyParser()
        {
            ActivationCue = null;
            DeactivationCue = null;
            CategoryCue = DefaultCategoryCue;
            GroupBeginCue = DefaultGroupBeginCue;
            GroupEndCue = DefaultGroupEndCue;
            GroupDocsBeginCue = DefaultGroupDocsBeginCue;
            GroupDocsEndCue = DefaultGroupDocsEndCue;
        }

        private void OnPropertyValue(string name, object value)
        {
            if (CurrentGroup == null)
            {
                var target = PropertyTarget;
                if (target != null)
                {
                    target.SetValue(name, value);
                }
            }
            else
            {
                var groupTarget = GroupPropertyTarget;
                if (groupTarget != null)
                {
                    groupTarget.SetGroupValue(CurrentGroup, name, value);
                    if (CurrentGroupMetadata != null)
                    {
                        groupTarget.SetGroupMetadata(CurrentGroup, CurrentGroupMetadata);
                    }
                }
            }
        }

        private int LineNo;
        private string CodePreamble;
        private string CurrentCategory;
        private string CurrentGroup;
        private string ListKey;
        private List<string> ListItems = new List<string>();

        private MdContext Context;
        private bool collectingGroupDocsActive;

        /// <summary>
        /// Parses the data in the given stream as UTF8 encoded Markdown text
        /// and recognizes configuration properties.
        /// </summary>
        /// <param name="source">The source stream.</param>
        public void Parse(Stream source)
        {
            Parse(new StreamReader(source, Encoding.UTF8));
        }


        /// <summary>
        /// Parses the given text input and recognizes configuration properties.
        /// </summary>
        /// <param name="source">The text input.</param>
        public void Parse(TextReader source)
        {
            LineNo = 0;
            CodePreamble = null;
            CurrentGroup = null;
            Context = ActivationCue == null ? MdContext.Text : MdContext.Inactive;
            string line = null;
            while ((line = source.ReadLine()) != null)
            {
                ProcessLine(line);
                LineNo++;
            }
            ProcessLine("");
        }

        private void ProcessLine(string line)
        {
            switch (Context)
            {
                case MdContext.Inactive:
                    if (MdSyntax.IsYamlHeaderStart(LineNo, line))
                        Context = MdContext.YamlHeader;
                    else if (MdSyntax.IsHtmlCommentStart(line))
                        Context = MdContext.HtmlComment;
                    else if (MdSyntax.IsCodeBlockStart(line, ref CodePreamble))
                        Context = MdContext.CodeBlock;
                    else if (IsActivationCue(line))
                        Context = MdContext.Text;
                    break;
                case MdContext.YamlHeader:
                    if (MdSyntax.IsYamlHeaderEnd(LineNo, line))
                        Context = MdContext.Text;
                    break;
                case MdContext.HtmlComment:
                    if (MdSyntax.IsHtmlCommentEnd(line))
                        Context = MdContext.Text;
                    ProcessPossibleGroupDocsLine(line);
                    break;
                case MdContext.CodeBlock:
                    if (MdSyntax.IsCodeBlockEnd(line, ref CodePreamble))
                        Context = MdContext.Text;
                    ProcessPossibleGroupDocsLine(line);
                    break;
                case MdContext.Text:
                    if (MdSyntax.IsYamlHeaderStart(LineNo, line))
                        Context = MdContext.YamlHeader;
                    else if (MdSyntax.IsHtmlCommentStart(line))
                    {
                        Context = MdContext.HtmlComment;
                        ProcessPossibleGroupDocsLine(line);
                    }
                    else if (MdSyntax.IsCodeBlockStart(line, ref CodePreamble))
                    {
                        Context = MdContext.CodeBlock;
                        ProcessPossibleGroupDocsLine(line);
                    }
                    else if (IsDeactivationCue(line))
                        Context = MdContext.Inactive;
                    else if (IsListStart(line, ref ListKey))
                        Context = MdContext.List;
                    else
                    {
                        ProcessTextLine(line);
                    }
                    break;
                case MdContext.List:
                    if (!IsListElement(line, ListItems))
                    {
                        var listValue = string.Join(ListSeparatorStr, ListItems.ToArray());
                        ProcessTextLine(string.Format(MdListFormat, ListKey, listValue));
                        ListKey = null;
                        ListItems.Clear();
                        Context = MdContext.Text;
                        ProcessLine(line);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private bool IsActivationCue(string line)
        {
            return ActivationCue != null && ActivationCue.IsMatch(line);
        }

        private bool IsDeactivationCue(string line)
        {
            return DeactivationCue != null && DeactivationCue.IsMatch(line);
        }

        private void ProcessTextLine(string line)
        {
            if (CategoryCue != null)
            {
                var cm = CategoryCue.Match(line);
                if (cm.Success)
                {
                    CurrentCategory = cm.Groups["category"].Value;
                }
            }
            if (GroupEndCue != null)
            {
                var gm = GroupEndCue.Match(line);
                if (gm.Success)
                {
                    CurrentGroup = null;
                }
            }
            if (GroupBeginCue != null)
            {
                var gm = GroupBeginCue.Match(line);
                if (gm.Success)
                {
                    CurrentGroup = gm.Groups["group"].Value;
                    if (CurrentCategory != null)
                    {
                        GroupPropertyTarget.SetGroupCategory(CurrentGroup, CurrentCategory);
                    }
                    if (collectingGroupDocsActive)
                    {
                        docGroups.Add(CurrentGroup);
                    }
                }
            }
            var m = MdListExp.Match(line);
            if (m.Success)
            {
                var key = m.Groups["key"].Value;
                var value = m.Groups["value"].Value;
                if (ListValueExp.IsMatch(value))
                {
                    var values = value.Split(ListSeparator, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < values.Length; i++)
                    {
                        values[i] = RemoveQuotes(values[i]);
                    }
                    OnPropertyValue(key, values);
                }
                else
                {
                    value = RemoveQuotes(value);
                    OnPropertyValue(key, value);
                }
            }
            else
            {
                ProcessPossibleGroupDocsLine(line);
            }
        }

        private void CheckForGroupDocsBegin(string line)
        {
            if (CollectGroupDocs && GroupDocsBeginCue.IsMatch(line))
            {
                collectingGroupDocsActive = true;
            }
        }

        private void CheckForGroupDocsEnd(string line)
        {
            if (CollectGroupDocs && GroupDocsEndCue.IsMatch(line))
            {
                var docText = string.Join(Environment.NewLine, collectedGroupDocs.ToArray()).Trim();
                foreach (var g in docGroups)
                {
                    GroupPropertyTarget.SetGroupDocumentation(g, docText);
                }
                collectedGroupDocs.Clear();
                docGroups.Clear();
                collectingGroupDocsActive = false;
            }
        }

        private void ProcessPossibleGroupDocsLine(string line)
        {
            CheckForGroupDocsEnd(line);
            if (collectingGroupDocsActive)
            {
                collectedGroupDocs.Add(line);
            }
            CheckForGroupDocsBegin(line);
        }

    }
}
