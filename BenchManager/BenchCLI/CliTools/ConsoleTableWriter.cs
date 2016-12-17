using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.CliTools
{
    class ConsoleTableWriter : ITableWriter
    {
        private string[] columns;
        private List<string[]> rows;
        private bool isDisposed;

        public void Initialize(params string[] columns)
        {
            this.columns = columns;
            this.rows = new List<string[]>();
        }

        public void Write(params object[] values)
        {
            if (isDisposed) throw new ObjectDisposedException(nameof(ConsoleTableWriter));
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
            if (value is bool) return ((bool)value) ? "TRUE" : "FALSE";
            if (value is string) return (string)value;
            return "UNSUPPORTED TYPE";
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
            for (int i = 0; i < c; i++)
            {
                if (i > 0) Write(" | ");
                Write(columns[i].PadRight(lengths[i]));
            }
            NewLine();
            for (int i = 0; i < c; i++)
            {
                if (i > 0) Write("-|-");
                Write(new string('-', lengths[i]));
            }
            NewLine();
            foreach (var row in rows)
            {
                for (int i = 0; i < c; i++)
                {
                    if (i > 0) Write(" | ");
                    Write(row[i].PadRight(lengths[i]));
                }
                NewLine();
            }
        }

        private int lineLength = 0;
        private void Write(string value)
        {
            var w = Console.WindowWidth;
            if (lineLength >= w) return;
            if (lineLength + value.Length < w)
            {
                Console.Write(value);
                lineLength += value.Length;
            }
			else
            {
                Console.Write(value.Substring(0, w - lineLength));
                lineLength = w;
            }
        }

        private void NewLine()
        {
            Console.WriteLine();
            lineLength = 0;
        }

        public void Dispose()
        {
            if (isDisposed) return;
            WriteTable();
            isDisposed = true;
            columns = null;
            rows = null;
        }
    }
}
