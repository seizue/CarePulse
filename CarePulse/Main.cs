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

namespace CarePulse
{
    public partial class Main: Form
    { 
        private bool isStateChanging = false;
        private const int DEFAULT_WIDTH = 1050;
        private const int DEFAULT_HEIGHT = 671;

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

        public void InitializeControls()
        {
            // Add system event handlers for display and taskbar changes
            Microsoft.Win32.SystemEvents.DisplaySettingsChanged += SystemEvents_DisplaySettingsChanged;
            Microsoft.Win32.SystemEvents.UserPreferenceChanged += SystemEvents_UserPreferenceChanged;

            // Initialize form properties
            this.StartPosition = FormStartPosition.Manual;
            this.Size = new Size(DEFAULT_WIDTH, DEFAULT_HEIGHT);


            // Setup event handlers
            this.Load += Main_Load;
            this.Resize += Main_Resize;
            this.SizeChanged += Main_SizeChanged;
        }

        private void SystemEvents_DisplaySettingsChanged(object sender, EventArgs e)
        {
            BeginInvoke(new Action(UpdateFormPosition));
        }


        private void SystemEvents_UserPreferenceChanged(object sender, Microsoft.Win32.UserPreferenceChangedEventArgs e)
        {
            // Check if the change is related to window metrics (including taskbar)
            if (e.Category == Microsoft.Win32.UserPreferenceCategory.Window)
            {
                BeginInvoke(new Action(UpdateFormPosition));
            }
        }

        private void UpdateFormPosition()
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                // Store current state
                FormWindowState currentState = this.WindowState;

                // Temporarily restore to normal to force Windows to recalculate working area
                this.WindowState = FormWindowState.Normal;

                // Update the MaximizedBounds
                Screen screen = Screen.FromControl(this);
                this.MaximizedBounds = screen.WorkingArea;

                // Restore to maximized state
                this.WindowState = currentState;
            }
            else if (this.WindowState == FormWindowState.Normal)
            {
                PositionForm();
            }
        }

        private void PositionForm()
        {
            if (this.WindowState != FormWindowState.Normal)
                return;

            Screen screen = Screen.FromControl(this);
            Rectangle workingArea = screen.WorkingArea;

            // Calculate center position within the working area
            int left = workingArea.Left + (workingArea.Width - this.Width) / 2;
            int top = workingArea.Top + (workingArea.Height - this.Height) / 2;

            // Ensure the form stays within the working area bounds
            if (left < workingArea.Left) left = workingArea.Left;
            if (top < workingArea.Top) top = workingArea.Top;
            if (left + this.Width > workingArea.Right) left = workingArea.Right - this.Width;
            if (top + this.Height > workingArea.Bottom) top = workingArea.Bottom - this.Height;

            this.Location = new Point(left, top);
        }


        private void Main_Load(object sender, EventArgs e)
        {
            BeginInvoke(new Action(() =>
            {
                // Apply saved window state
                string savedWindowState = Properties.Settings.Default.MainFormWindowState;
                if (!string.IsNullOrEmpty(savedWindowState) &&
                    Enum.TryParse<FormWindowState>(savedWindowState, out FormWindowState state))
                {
                    isStateChanging = true;

                    // First ensure we're in the correct position for the normal state
                    if (state == FormWindowState.Normal)
                    {
                        PositionForm();
                    }
                    else if (state == FormWindowState.Maximized)
                    {
                        // Set MaximizedBounds before maximizing
                        Screen screen = Screen.FromControl(this);
                        this.MaximizedBounds = screen.WorkingArea;
                    }

                    // Then apply the window state
                    this.WindowState = state;
                    isStateChanging = false;
                }
                else
                {
                    PositionForm();
                }
            }));
        }

        private void Main_Resize(object sender, EventArgs e)
        {
            if (!isStateChanging)
            {
                if (this.WindowState == FormWindowState.Normal)
                {
                    PositionForm();
                }
            }
        }

        private void Main_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized && !isStateChanging)
            {
                // Update MaximizedBounds when the window is maximized
                Screen screen = Screen.FromControl(this);
                Rectangle workingArea = screen.WorkingArea;
                this.MaximizedBounds = workingArea;
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {

            base.OnFormClosing(e);

            try
            {
                // Unsubscribe from system events to prevent memory leaks
                Microsoft.Win32.SystemEvents.DisplaySettingsChanged -= SystemEvents_DisplaySettingsChanged;
                Microsoft.Win32.SystemEvents.UserPreferenceChanged -= SystemEvents_UserPreferenceChanged;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error during form closing: {ex.Message}");
            }
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

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            UpdatePanelIndicator(85);
        }

        private void btnMain_Click(object sender, EventArgs e)
        {
            UpdatePanelIndicator(135);
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            UpdatePanelIndicator(185);
            Settings settings = new Settings(this);
            settings.ShowDialog();
        }

        //Updates the text color of two buttons to indicate the active selection.
        private void UpdateButtonColors(Button activeButton, Button inactiveButton)
        {
            activeButton.ForeColor = Color.FromArgb(128, 64, 0);
            inactiveButton.ForeColor = Color.FromArgb(45, 53, 44);
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            UpdateButtonColors(btnHome, btnReports);
        }

        private void btnReports_Click(object sender, EventArgs e)
        { 
            UpdateButtonColors(btnReports, btnHome); 
        }

        private void btnGithub_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://github.com/seizue/CarePulse",
                UseShellExecute = true
            });
        }

    }
}
