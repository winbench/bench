using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Mastersign.CliTools
{
    public class MarkdownPropertyWriter : IPropertyWriter
    {
        private TextWriter writer;

        public MarkdownPropertyWriter(Stream stream)
        {
            writer = new StreamWriter(stream, new UTF8Encoding(false));
        }

        public void Dispose()
        {
            if (writer != null)
            {
                writer.Flush();
                writer.Dispose();
            }
            writer = null;
        }

        public void Write(string key, object value)
        {
            if (value == null) WriteNull(key);
            else if (value is bool) WriteValue(key, (bool)value);
            else if (value is string) WriteValue(key, (string)value);
            else if (value is string[]) WriteValue(key, (string[])value);
            else if (value is IDictionary<string, string>) WriteValue(key, (IDictionary<string, string>)value);
            else WriteUnknown(key);
        }


        private string EscapeValue(string value)
        {
            if (value == null)
                return "";
            if (Uri.IsWellFormedUriString(value, UriKind.Absolute))
                return "<" + value + ">";
            return "`" + value + "`";
        }

        private string EscapeValue(bool value)
        {
            return "`" + (value ? "true" : "false") + "`";
        }

        public void WriteValue(string key, IDictionary<string, string> value)
        {
            writer.WriteLine("* `{0}`:", key);
            foreach (var kvp in value)
            {
                writer.WriteLine("    + `{0}`: {1}", kvp.Key, EscapeValue(kvp.Value));
            }
        }

        public void WriteValue(string key, string[] value)
        {
            var sum = 0;
            var list = new List<string>(value);
            for (int i = 0; i < list.Count; i++)
            {
                sum += list[i].Length + 3;
                list[i] = "`" + list[i] + "`";
            }
            if (sum <= 100)
            {
                writer.WriteLine("* `{0}`: {1}", key, string.Join(", ", list.ToArray()));
            }
            else
            {
                writer.WriteLine("* `{0}`:", key);
                foreach (var item in value)
                {
                    writer.WriteLine("    + {0}", EscapeValue(item));
                }
            }
        }

        public void WriteValue(string key, string value)
        {
            writer.WriteLine("* `{0}`: {1}", key, EscapeValue(value));
        }

        public void WriteValue(string key, bool value)
        {
            writer.WriteLine("* `{0}`: {1}", key, EscapeValue(value));
        }

        public void WriteNull(string key)
        {
            writer.WriteLine("* `{0}`:", key);
        }

        public void WriteUnknown(string key)
        {
            writer.WriteLine("* `{0}`: _Unsupported Data Type_", key);
        }
    }
}
