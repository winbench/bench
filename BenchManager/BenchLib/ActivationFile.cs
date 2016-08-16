using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Mastersign.Bench
{
    /// <summary>
    /// Represents a text file with a list of app IDs.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The syntax of the text file follows the following rules:
    /// </para>
    /// <list type="bullet">
    ///     <item>Empty lines are ignored.</item>
    ///     <item>Lines with nothing but white space are ignored.</item>
    ///     <item>White space at the beginning and the end of lines is trimmed.</item>
    ///     <item>Lines starting with <c>#</c> are ignored.</item>
    ///     <item>The first word (contiguous non white space) in a line is considered to be an app ID.</item>
    ///     <item>Additional characters after the first word are ignored, and can be used to commment the entry.</item>
    /// </list>
    /// </remarks>
    /// <example>
    /// A text file represented by this class could look like this:
    /// <code>
    /// # --- Activated Apps --- #
    ///
    /// AppA
    /// AppB (this app has a comment)
    ///  AppC (this app ID is valid, despite the fact, that it is indended)
    ///
    /// # AppD (this app is not activated, because the line is commented out)
    /// AppE some arbitrary comment
    /// </code>
    /// </example>
    public class ActivationFile : IEnumerable<string>
    {
        private static readonly Regex SpaceExp = new Regex(@"\s");

        private static readonly Regex DisabledExp = new Regex(@"^#\s*(?<id>\S+)(?<comment>\s+.+?)?\s*$");

        private readonly string FilePath;

        /// <summary>
        /// Initializes a new instance of <see cref="ActivationFile"/>.
        /// </summary>
        /// <param name="path">An absolute path to the text file.</param>
        public ActivationFile(string path)
        {
            FilePath = path;
        }

        private static bool IsValidLine(string line)
        {
            if (string.IsNullOrEmpty(line)) return false;
            if (line.StartsWith("#")) return false;
            return true;
        }

        private static string CleanLine(string line)
        {
            var m = SpaceExp.Match(line);
            if (m.Success)
            {
                return line.Substring(0, m.Index);
            }
            else
            {
                return line;
            }
        }

        private delegate IEnumerable<string> LineProcessor(IEnumerable<string> lines);

        private void EditFile(LineProcessor proc)
        {
            var lines = new List<string>();
            if (File.Exists(FilePath))
            {
                lines.AddRange(File.ReadAllLines(FilePath));
            }
            var processed = new List<string>(proc(lines));
            File.WriteAllLines(FilePath, processed.ToArray());
        }

        private static IEnumerable<string> Activator(IEnumerable<string> lines, string id)
        {
            var found = false;
            foreach (var l in lines)
            {
                var m = DisabledExp.Match(l);
                if (m.Success && m.Groups["id"].Value == id)
                {
                    yield return id + m.Groups["comment"].Value;
                    found = true;
                }
                else
                {
                    yield return l;
                }
            }
            if (!found)
            {
                yield return id;
            }
        }

        private static IEnumerable<string> Deactivator(IEnumerable<string> lines, string id)
        {
            foreach (var l in lines)
            {
                var line = l.Trim();
                if (IsValidLine(line) && CleanLine(line) == id)
                {
                    yield return "# " + l;
                }
                else
                {
                    yield return l;
                }
            }
        }

        /// <summary>
        /// Makes shure, the given app ID is listed active.<br/>
        /// The text file is updated immediately.
        /// </summary>
        /// <remarks>
        /// If the given app ID is already listed, but commented out, the commenting <c>#</c> is removed.
        /// If the given app ID is not listed, it is added at the end of the file.
        /// </remarks>
        /// <param name="id">An app ID. Must be a string without whitespace.</param>
        public void SignIn(string id)
        {
            EditFile(lines => Activator(lines, id));
        }

        /// <summary>
        /// Makes shure, the given app ID is not listed active.<br/>
        /// The text file is updated immediately.
        /// </summary>
        /// <remarks>
        /// If the given app ID is not listed, or commented out, the text file is not changed.
        /// If the given app ID is listed and not commented out, its line is prepended with a <c># </c> to comment it out.
        /// </remarks>
        /// <param name="id">An app ID. Must be a string without whitespace.</param>
        public void SignOut(string id)
        {
            EditFile(lines => Deactivator(lines, id));
        }

        /// <summary>
        /// Returns all app IDs listed as active.
        /// </summary>
        /// <returns>An enumerator of strings.</returns>
        public IEnumerator<string> GetEnumerator()
        {
            if (!File.Exists(FilePath)) yield break;
            using (var r = File.OpenText(FilePath))
            {
                string line;
                while ((line = r.ReadLine()) != null)
                {
                    line = line.Trim();
                    if (IsValidLine(line))
                    {
                        yield return CleanLine(line);
                    }
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
