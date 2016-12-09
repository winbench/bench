using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.CliTools
{
    public class ConsolePropertyWriter : IPropertyWriter
    {
        public void Dispose()
        {
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

        private string EscapeString(string value)
        {
            return value != null
                ? "\"" + value.Replace(@"\", @"\\").Replace("\"", "\\\"") + "\""
                : null;
        }

        public void WriteValue(string key, IDictionary<string, string> value)
        {
            var pairs = new List<string>();
            foreach (var kvp in (Dictionary<string, string>)value)
            {
                pairs.Add(string.Format("{0}: {1}",
                    EscapeString(kvp.Key), EscapeString(kvp.Value)));
            }
            Console.WriteLine(key + " = {" + string.Join(", ", pairs.ToArray()) + "}");
        }

        public void WriteValue(string key, string[] value)
        {
            var items = new List<string>();
            foreach (var item in value)
            {
                items.Add(EscapeString(item));
            }
            Console.WriteLine("{0} = [{1}]", key, string.Join(", ", items.ToArray()));
        }

        public void WriteValue(string key, string value)
        {
            Console.WriteLine("{0} = {1}", key, EscapeString(value.ToString()));
        }

        public void WriteValue(string key, bool value)
        {
            Console.WriteLine("{0} = {1}", key, value ? "True" : "False");
        }

        public void WriteNull(string key)
        {
            Console.WriteLine("{0} = Null", key);
        }

        public void WriteUnknown(string key)
        {
            Console.WriteLine("{0} = Unsupported Data Type", key);
        }
    }
}
