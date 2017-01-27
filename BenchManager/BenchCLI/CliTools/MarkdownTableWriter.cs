using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Mastersign.CliTools
{
    class MarkdownTableWriter : ITableWriter
    {
        private string[] columns;
        private List<string[]> rows;

        private TextWriter writer;

        public MarkdownTableWriter(TextWriter writer)
        {
            if (writer == null) throw new ArgumentNullException();
            this.writer = writer;
        }

        public MarkdownTableWriter(Stream target)
            : this(new StreamWriter(target, new UTF8Encoding(false)))
        {
        }

        public void Initialize(params string[] columns)
        {
            this.columns = columns;
            this.rows = new List<string[]>();
        }

        public void Write(params object[] values)
        {
            if (writer == null) throw new ObjectDisposedException(nameof(ConsoleTableWriter));
            if (columns == null) throw new InvalidOperationException();
            if (values.Length != columns.Length) throw new ArgumentException("Incorrect number of values.");
            var row = new List<string>();
            for (int i = 0; i < values.Length; i++)
            {
                row.Add(Format(values[i]));
            }
            rows.Add(row.ToArray());
        }

        private string Format(object value)
        {
            if (value == null) return string.Empty;
            if (value is bool) return ((bool)value) ? "`true`" : "`false`";
            if (value is string) return (string)value;
            return "_UNSUPPORTED TYPE_";
        }

        private void WriteTable()
        {
            var c = columns.Length;
            var lengths = new int[c];
            for (int i = 0; i < c; i++) lengths[i] = columns[i].Length;
            foreach (var row in rows)
            {
                for (int i = 0; i < c; i++) lengths[i] = Math.Max(lengths[i], row[i].Length);
            }
            Write("| ");
            for (int i = 0; i < c; i++)
            {
                if (i > 0) Write(" | ");
                Write(columns[i].PadRight(lengths[i]));
            }
            Write(" |");
            NewLine();
            Write("|:");
            for (int i = 0; i < c; i++)
            {
                if (i > 0) Write("-|:");
                Write(new string('-', lengths[i]));
            }
            Write("-|");
            NewLine();
            foreach (var row in rows)
            {
                Write("| ");
                for (int i = 0; i < c; i++)
                {
                    if (i > 0) Write(" | ");
                    Write(row[i].PadRight(lengths[i]));
                }
                Write(" |");
                NewLine();
            }
        }

        private void Write(string value)
        {
            writer.Write(value);
        }

        private void NewLine()
        {
            writer.WriteLine();
        }

        public void Dispose()
        {
            if (writer == null) return;
            WriteTable();
            writer.Dispose();
            writer = null;
        }
    }
}
