using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Mastersign.CliTools
{
    class ConsoleTableWriter : ITableWriter
    {
        private string[] columns;
        private Alignment[] alignment;
        private List<string[]> rows;
        private bool isDisposed;

        public void Initialize(params string[] columns)
        {
            this.columns = columns ?? throw new ArgumentNullException(nameof(columns));
            this.alignment = new Alignment[columns.Length];
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
                var a = GetAlignmentFromValue(values[i]);
                if (a != Alignment.Unknown) alignment[i] = a;
            }
            rows.Add(row.ToArray());
        }

        private string Format(object value)
        {
            if (value == null) return string.Empty;
            if (value is bool) return ((bool)value) ? "TRUE" : "FALSE";
            if (value is int) return ((int)value).ToString(CultureInfo.InvariantCulture);
            if (value is float) return ((float)value).ToString("0.00", CultureInfo.InvariantCulture);
            if (value is string) return (string)value;
            return "UNSUPPORTED TYPE";
        }

        enum Alignment { Unknown = 0, Left = 1, Center = 2, Right = 3 }

        private Alignment GetAlignmentFromValue(object value)
        {
            if (value is bool) return Alignment.Center;
            if (value is int) return Alignment.Right;
            if (value is float) return Alignment.Right;
            if (value is string) return Alignment.Left;
            return Alignment.Unknown;
        }

        private static string Align(string value, int l, Alignment a)
        {
            switch (a)
            {
                case Alignment.Center:
                    var d = l - value.Length;
                    if (d <= 0) return value;
                    var left = (int)Math.Floor(d * 0.5);
                    var right = (int)Math.Ceiling(d * 0.5);
                    return new string(' ', left) + value + new string(' ', right);
                case Alignment.Right:
                    return value.PadLeft(l);
                default:
                    return value.PadRight(l);
            }
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
                Write(Align(columns[i], lengths[i], alignment[i]));
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
                    Write(Align(row[i], lengths[i], alignment[i]));
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
                Console.Write(value.Substring(0, w - lineLength - 1));
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
