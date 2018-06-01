using Mastersign.Bench.Dashboard.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mastersign.Bench.Dashboard
{
    public partial class TaskInfoForm : Form
    {
        public TaskInfoForm()
        {
            InitializeComponent();
        }

        public void SetTaskInfo(TaskInfo info)
        {
            lblTimestamp.Text = info.Timestamp.ToString("yyyy-MM-dd HH:mm:ss");
            if (string.IsNullOrWhiteSpace(info.AppId))
            {
                lblContextLabel.Visible = false;
                lblContext.Visible = false;
            }
            else
            {
                lblContext.Text = info.AppId;
            }
            lblMessage.Text = info.Message;
            if (string.IsNullOrWhiteSpace(info.DetailedMessage))
            {
                lblDetailsLabel.Visible = false;
                lblDetails.Visible = false;
            }
            else
            {
                lblDetails.Text = info.DetailedMessage;
            }
            txtOutput.Text = info.ConsoleOutput;
            Icon = info is TaskError ? Resources.error : Resources.ok;
        }
    }
}
