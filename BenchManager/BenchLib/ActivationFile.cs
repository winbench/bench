using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Mastersign.Bench
{
    public class ActivationFile : IEnumerable<string>
    {
        private static readonly Regex SpaceExp = new Regex(@"\s");

        private static readonly Regex DisabledExp = new Regex(@"^#\s*(?<id>\S+)(?<comment>\s+.+?)?\s*$");

        private readonly string FilePath;

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

        public void SignIn(string id)
        {
            EditFile(lines => Activator(lines, id));
        }

        public void SignOut(string id)
        {
            EditFile(lines => Deactivator(lines, id));
        }

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
