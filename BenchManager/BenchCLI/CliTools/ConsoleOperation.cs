using System;
using System.Collections.Generic;
using System.Text;

namespace Mastersign.CliTools
{
    public class ConsoleOperation
    {
        public static ConsoleColor DefaultForegroundColor = Console.ForegroundColor;
        public static ConsoleColor DefaultBackgroundColor = Console.BackgroundColor;

        private int backupRow;
        private int backupColumn;
        private ConsoleColor backupForegroundColor;
        private ConsoleColor backupBackgroundColor;

        public int StoredCursorTop => backupRow;
        public int StoredCursorLeft => backupColumn;

        protected void BackupState()
        {
            backupRow = Console.CursorTop;
            backupColumn = Console.CursorLeft;
            backupForegroundColor = Console.ForegroundColor;
            backupBackgroundColor = Console.BackgroundColor;
            Console.CursorVisible = false;
        }

        protected void RestoreState()
        {
            Console.ForegroundColor = backupForegroundColor;
            Console.BackgroundColor = backupBackgroundColor;
            Console.SetCursorPosition(backupColumn, backupRow);
            Console.CursorVisible = true;
        }
    }
}
