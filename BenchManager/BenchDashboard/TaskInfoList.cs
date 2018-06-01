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

namespace Mastersign.Bench.Dashboard
{
    public partial class TaskInfoList : UserControl
    {
        private BindingList<TaskInfoWrapper> infos = new BindingList<TaskInfoWrapper>();

        public TaskInfoList()
        {
            InitializeComponent();
            dataGrid.AutoGenerateColumns = false;
            dataGrid.DataSource = infos;
        }

        public void AddTaskInfo(TaskInfo info)
        {
            infos.Insert(0, new TaskInfoWrapper(info));
            dataGrid.Rows[0].Selected = true;
        }

        public void Clear()
        {
            infos.Clear();
        }

        class TaskInfoWrapper
        {
            public TaskInfoWrapper(TaskInfo info)
            {
                TaskInfo = info;
            }

            public TaskInfo TaskInfo { get; }

            public Bitmap Icon
            {
                get
                {
                    if (TaskInfo is TaskError) return Resources.error_outline_16;
                    return Resources.ok_outline_16;
                }
            }

            public string Timestamp => TaskInfo.Timestamp.ToString("HH:mm:ss");

            public string Context => TaskInfo.AppId;

            public string Message => TaskInfo.Message;
        }

        private void dataGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var infoWrapper = dataGrid.Rows[e.RowIndex].DataBoundItem as TaskInfoWrapper;
            if (infoWrapper != null)
            {
                ShowTaskInfoForm(infoWrapper.TaskInfo);
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
