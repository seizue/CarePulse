using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarePulse
{
    public partial class Main: Form
    {
        public Main()
        {
            InitializeComponent();
            SetDataGridViewRowHeight();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnMaximize_Click(object sender, EventArgs e)
        {

            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                // Maximize the window without covering the taskbar
                this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
                this.WindowState = FormWindowState.Maximized;
            }
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void SetDataGridViewRowHeight()
        {
            foreach (DataGridViewRow row in datagridCPHome.Rows)
            {
                row.Height = 28; 
            }
        }

        // Moves the panel indicator to a specified vertical position.
        private void UpdatePanelIndicator(int yOffset) => panelIndicator.Location = new Point(1, yOffset);

        private void btnDashboard_Click(object sender, EventArgs e) => UpdatePanelIndicator(85);
        private void btnMain_Click(object sender, EventArgs e) => UpdatePanelIndicator(135);
        private void btnSettings_Click(object sender, EventArgs e) => UpdatePanelIndicator(185);

        //Updates the text color of two buttons to indicate the active selection.
        private void UpdateButtonColors(Button activeButton, Button inactiveButton)
        {
            activeButton.ForeColor = Color.FromArgb(128, 64, 0);
            inactiveButton.ForeColor = Color.FromArgb(45, 53, 44);
        }

        private void btnHome_Click(object sender, EventArgs e) => UpdateButtonColors(btnHome, btnReports);
        private void btnReports_Click(object sender, EventArgs e) => UpdateButtonColors(btnReports, btnHome);
        private void btnGithub_Click(object sender, EventArgs e)
        {

        }

    }
}
