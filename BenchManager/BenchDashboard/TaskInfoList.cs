using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mastersign.Bench.Dashboard.Properties;
using System.Collections.Concurrent;

namespace Mastersign.Bench.Dashboard
{
    public partial class TaskInfoList : UserControl
    {
        private ConcurrentQueue<TaskInfoWrapper> infoQueue = new ConcurrentQueue<TaskInfoWrapper>();
        private Timer timer;

        public TaskInfoList()
        {
            InitializeComponent();
            dataGrid.AutoGenerateColumns = false;
            InitializeTimer();
            Disposed += DisposedHandler;
        }

        private void InitializeTimer()
        {
            timer = new Timer { Interval = 250 };
            timer.Tick += Timer_Tick;
            timer.Enabled = true;
        }

        private void DisposedHandler(object sender, EventArgs e)
        {
            timer.Enabled = false;
            timer.Tick -= Timer_Tick;
            timer.Dispose();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            var added = false;
            while (infoQueue.TryDequeue(out var wrapper))
            {
                dataGrid.Rows.Insert(0, wrapper.Icon, wrapper.Timestamp, wrapper.Context, wrapper.Message);
                dataGrid.Rows[0].Tag = wrapper.TaskInfo;
                added = true;
            }
            if (added) dataGrid.Rows[0].Selected = true;
        }

        public void AddTaskInfo(TaskInfo info) => infoQueue.Enqueue(new TaskInfoWrapper(info));

        public void Clear() => dataGrid.Rows.Clear();

        class TaskInfoWrapper
        {
            public TaskInfoWrapper(TaskInfo info)
            {
                TaskInfo = info;
            }

            public TaskInfo TaskInfo { get; }

            public Bitmap Icon => TaskInfo is TaskError ? Resources.error_outline_16 : Resources.ok_outline_16;

            public string Timestamp => TaskInfo.Timestamp.ToString("HH:mm:ss");

            public string Context => TaskInfo.AppId;

            public string Message => TaskInfo.Message;
        }

        private void CellDoubleClickHandler(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGrid.Rows[e.RowIndex].Tag is TaskInfo info)
            {
                ShowTaskInfoForm(info);
            }
        }

        private void ShowTaskInfoForm(TaskInfo info)
        {
            var infoForm = new TaskInfoForm();
            infoForm.SetTaskInfo(info);
            infoForm.ShowDialog(ParentForm);
        }
    }
}
