using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mastersign.Bench.Dashboard
{
    internal partial class ConfigInfoDialog : Form
    {
        public ConfigInfoDialog(BenchConfiguration config)
        {
            InitializeComponent();
            gridResolved.DoubleBuffered(true);
            LoadProperties(config);
        }

        private void LoadProperties(BenchConfiguration config)
        {
            var names = config.PropertyNames().ToArray();
            Array.Sort(names);

            gridResolved.Rows.Clear();
            foreach (var name in names)
            {
                AddRow(gridResolved, name, config.GetValue(name));
            }

            gridRaw.Rows.Clear();
            foreach (var name in names)
            {
                AddRow(gridRaw, name, config.GetRawValue(name));
            }
        }

        private void AddRow(DataGridView grid, string name, object value)
        {
            if (value is bool)
            {
                AddRow(grid, name, value.ToString());
            }
            else if (value is string)
            {
                AddRow(grid, name, (string)value);
            }
            else if (value is string[])
            {
                AddRow(grid, name, string.Join(", ",
                    ((string[])value).Select(v => $"`{v}`")));
            }
            else if (value is IDictionary<string, string>)
            {
                AddRow(grid, name, string.Join(", ",
                    ((IDictionary<string, string>)value).Select(kvp => $"`{kvp.Key}`=`{kvp.Value}`")));
            }
            else if (value == null)
            {
                AddRow(grid, name, null);
            }
            else
            {
                AddRow(grid, name, "UNKNOWN: " + value.ToString());
            }
        }

        private void AddRow(DataGridView grid, string name, string value)
        {
            grid.Rows.Add(name, value);
        }

        private void UpdateCellSelection(DataGridView grid, int columnIndex, int rowIndex)
        {
            var cell = grid[columnIndex, rowIndex];
            if (!cell.Selected)
            {
                cell.DataGridView.ClearSelection();
                cell.DataGridView.CurrentCell = cell;
                cell.Selected = true;
            }
        }

        private void ShowContextMenuForCell(DataGridView grid, int columnIndex, int rowIndex, Point location)
        {
            var cell = grid[columnIndex, rowIndex];
            tsmiCopyValue.Tag = cell.Value;
            if (string.IsNullOrWhiteSpace(cell.Value as string)) return;
            var cellRect = grid.GetCellDisplayRectangle(columnIndex, rowIndex, true);
            var p = cellRect.Location;
            p.Offset(location);
            ctxmProperties.Show(grid, p);
        }

        private void gridCellMouseDownHandler(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            var grid = (DataGridView)sender;
            UpdateCellSelection(grid, e.ColumnIndex, e.RowIndex);
            ShowContextMenuForCell(grid, e.ColumnIndex, e.RowIndex, e.Location);
        }

        private void tsmiCopyValueClickHandler(object sender, EventArgs e)
        {
            var value = ((ToolStripItem)sender).Tag as string;
            Clipboard.SetText(value);
        }

        private void gridKeyDownHandler(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.F10 && e.Shift) || e.KeyCode == Keys.Apps)
            {
                e.SuppressKeyPress = true;
                var grid = sender as DataGridView;
                var cell = grid?.CurrentCell;
                if (cell != null)
                {
                    var r = cell.DataGridView.GetCellDisplayRectangle(cell.ColumnIndex, cell.RowIndex, false);
                    ShowContextMenuForCell(grid, cell.ColumnIndex, cell.RowIndex, new Point(0, r.Height));
                }
            }
        }
    }
}
