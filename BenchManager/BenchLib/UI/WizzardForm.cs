using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Mastersign.Bench.UI
{
    /// <summary>
    /// A form hosting a number of wizzard controls for an assisted configuration process.
    /// </summary>
    internal partial class WizzardForm : Form
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
            this.task = task;
            task.Before();
            stepControls = task.StepControls;
            currentStep = -1;
            MoveForward();
        }

        private bool CanMoveForward
        {
            get
            {
                for (int i = currentStep + 1; i < stepControls.Length; i++)
                {
                    if (task.IsStepVisible(stepControls[i])) return true;
                }
                return false;
            }
        }

        private bool CanMoveBackward
        {
            get
            {
                for (int i = currentStep - 1; i >= 0; i--)
                {
                    if (task.IsStepVisible(stepControls[i])) return true;
                }
                return false;
            }
        }

        private void MoveForward()
        {
            while (currentStep < stepControls.Length - 1)
            {
                currentStep++;
                if (task.IsStepVisible(stepControls[currentStep])) break;
            }
            UpdateWizzard();
        }

        private void MoveBackward()
        {
            while (currentStep > 0)
            {
                currentStep--;
                if (task.IsStepVisible(stepControls[currentStep])) break;
            }
            UpdateWizzard();
        }

        private void UpdateWizzard()
        {
            panelContent.Controls.Clear();
            if (stepControls.Length > 0)
            {
                var step = stepControls[currentStep];
                step.Dock = DockStyle.Fill;
                step.Visible = true;
                panelContent.Controls.Add(step);
                lblCurrentStep.Text = step.Description;
            }
            UpdateButtons();
        }

        private void UpdateButtons()
        {
            btnBack.Enabled = CanMoveBackward;
            btnNext.Enabled = CanMoveForward;
            btnFinish.Enabled = !CanMoveForward;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            MoveBackward();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            MoveForward();
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
