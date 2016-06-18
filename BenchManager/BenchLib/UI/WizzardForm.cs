using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Mastersign.Bench.UI
{
    public partial class WizzardForm : Form
    {
        private WizzardTask task;
        private WizzardStepControl[] stepControls;
        private int currentStep;
        private bool finished;

        static WizzardForm()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
            }
            catch (Exception) { }
        }

        public static bool ShowWizzard(WizzardTask task)
        {
            var wizzardForm = new WizzardForm(task);
            Application.Run(wizzardForm);

            return !task.IsCanceled;
        }

        public WizzardForm(WizzardTask task)
        {
            InitializeComponent();
            picIcon.Image = new Icon(Icon, new Size(48, 48)).ToBitmap();
            currentStep = 0;
            this.task = task;
            task.Before();
            stepControls = task.StepControls;
            UpdateWizzard();
        }

        private void UpdateWizzard()
        {
            panelContent.Controls.Clear();
            var step = stepControls[currentStep];
            step.Dock = DockStyle.Fill;
            step.Visible = true;
            panelContent.Controls.Add(step);
            lblCurrentStep.Text = step.Description;
            UpdateButtons();
        }

        private void UpdateButtons()
        {
            btnBack.Enabled = currentStep > 0;
            btnNext.Enabled = currentStep < stepControls.Length - 1;
            btnFinish.Enabled = currentStep == stepControls.Length - 1;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            if (currentStep > 0)
            {
                currentStep--;
                UpdateWizzard();
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (currentStep < stepControls.Length - 1)
            {
                currentStep++;
                UpdateWizzard();
            }
        }

        private void btnFinish_Click(object sender, EventArgs e)
        {
            finished = true;
            task.After();
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            finished = false;
            Close();
        }

        private void InitializeWizzardForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (finished) return;
            task.Cancel();
        }
    }
}
