using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Mastersign.Bench.Dashboard
{
    // http://stackoverflow.com/questions/3427696
    public class ImmediateToolStrip : ToolStrip
    {
        const uint WM_LBUTTONDOWN = 0x201;
        const uint WM_LBUTTONUP = 0x202;

        static private bool down = false;

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_LBUTTONUP && !down)
            {
                m.Msg = (int)WM_LBUTTONDOWN; base.WndProc(ref m);
                m.Msg = (int)WM_LBUTTONUP;
            }

            if (m.Msg == WM_LBUTTONDOWN) down = true;
            if (m.Msg == WM_LBUTTONUP) down = false;

            base.WndProc(ref m);
        }
    }
}
