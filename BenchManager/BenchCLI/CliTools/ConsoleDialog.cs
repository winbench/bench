using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.CliTools
{
    public class ConsoleDialog : ConsoleOperation
    {
        protected const char ESC = (char)27;
        protected const char ENTER = (char)13;

        private int lines;

        protected void Write(string text)
        {
            Console.Write(text);
        }

        protected void WriteLine(string text = null)
        {
            Console.WriteLine(text ?? string.Empty);
            lines++;
        }

        protected string ReadLine()
        {
            lines++;
            return Console.ReadLine();
        }

        protected void Write(string format, params object[] args)
            => Write(string.Format(format, args));

        protected void WriteLine(string format, params object[] args)
            => WriteLine(string.Format(format, args));

        protected void ClearLine(int row)
        {
            Console.SetCursorPosition(0, row);
            Console.Write(new string(' ', Console.BufferWidth));
        }

        public void Open()
        {
            lines = 0;
        }

        public void Close()
        {
            var bottom = Console.CursorTop;
            var top = Math.Max(0, bottom - lines);
            Console.SetCursorPosition(0, top);
            BackupState();
            for (int r = top; r <= bottom; r++) ClearLine(r);
            RestoreState();
        }

        protected char ReadExpectedChar(IList<char> expected)
        {
            char k;
            do
            {
                k = Console.ReadKey(true).KeyChar;
            } while (!expected.Contains(k));
            return k;
        }
    }
}
