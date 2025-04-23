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
using System.IO;
using Newtonsoft.Json;

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
           
            LoadJsonToDataGrid();
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
                row.Height = 35; 
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

        private void btnSurveyTemplate_Click(object sender, EventArgs e)
        {         
            NewTemplates newTemplates = new NewTemplates();
            newTemplates.ShowDialog();
        }


        private void btnSettings_Click(object sender, EventArgs e)
        {                   
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

        private void btnNew_Click(object sender, EventArgs e)
        {
            EntryNew entryNew = new EntryNew();

            // Subscribe to the SaveChangesCompleted event
            entryNew.SaveChangesCompleted += (s, ea) =>
            {
                // Refresh the data grid when the event is triggered
                LoadJsonToDataGrid();
            };

            entryNew.ShowDialog();
        }


        private void LoadJsonToDataGrid()
        {
            string finalizedSurveysPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CarePulse", "AnsweredSurvey", "FinalizedSurveys");

            // Check if the directory exists
            if (!Directory.Exists(finalizedSurveysPath))
            {
                Console.WriteLine("No directory found at: " + finalizedSurveysPath);
                return;
            }

            // Get all JSON files in the directory
            var jsonFiles = Directory.GetFiles(finalizedSurveysPath, "*.json");

            // If no JSON files are found, log to console and return
            if (jsonFiles.Length == 0)
            {
                Console.WriteLine("No JSON files found in: " + finalizedSurveysPath);
                return;
            }

            // Clear existing rows in the DataGridView
            datagridCPHome.Rows.Clear();

            // Loop through each JSON file and load its data
            foreach (var file in jsonFiles)
            {
                string jsonContent = File.ReadAllText(file);

                // Deserialize JSON into a dictionary
                var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonContent);

                if (data != null)
                {
                    // Extract required fields
                    string respondentID = data.ContainsKey("RespondentID") ? data["RespondentID"].ToString() : string.Empty;
                    string name = data.ContainsKey("Name") ? data["Name"].ToString() : string.Empty;
                    string date = data.ContainsKey("Date") ? data["Date"].ToString() : string.Empty;
                    string surveyScore = data.ContainsKey("SurveyScore") ? data["SurveyScore"].ToString() : string.Empty;
                    string month = data.ContainsKey("Month") ? data["Month"].ToString() : string.Empty;
                    string year = data.ContainsKey("Year") ? data["Year"].ToString() : string.Empty;
                    string surveyTemplate = data.ContainsKey("SurveyTemplate") ? data["SurveyTemplate"].ToString() : string.Empty;
                    string patientFeedback = data.ContainsKey("PatientFeedback") ? data["PatientFeedback"].ToString() : string.Empty;

                    // Extract answers and format them as a string
                    string surveyQuestionsAnswers = string.Empty;
                    if (data.ContainsKey("Answers") && data["Answers"] is Newtonsoft.Json.Linq.JObject answers)
                    {
                        var formattedAnswers = answers.Properties()
                            .Select(p => $"{p.Name}: {p.Value?.ToString() ?? "No Response"}");
                        surveyQuestionsAnswers = string.Join("; ", formattedAnswers);
                    }

                    // Calculate the number of answered and unanswered questions
                    int answeredCount = 0, unansweredCount = 0;
                    if (data.ContainsKey("Answers") && data["Answers"] is Newtonsoft.Json.Linq.JObject answerCounts)
                    {
                        foreach (var answer in answerCounts)
                        {
                            if (!string.IsNullOrWhiteSpace(answer.Value.ToString()))
                            {
                                answeredCount++;
                            }
                            else
                            {
                                unansweredCount++;
                            }
                        }
                    }

                    string responseSummary = $"Answered: {answeredCount}, Unanswered: {unansweredCount}";

                    // Add a new row to the DataGridView
                    int rowIndex = datagridCPHome.Rows.Add();
                    DataGridViewRow row = datagridCPHome.Rows[rowIndex];
                    row.Cells["cpID"].Value = respondentID;
                    row.Cells["cpName"].Value = name;
                    row.Cells["cpDatePeriod"].Value = date;
                    row.Cells["cpResponseTotal"].Value = responseSummary;
                    row.Cells["cpSurveyScore"].Value = surveyScore;
                    row.Cells["cpSurveyQuestionsAnswers"].Value = surveyQuestionsAnswers;
                    row.Cells["cpMonth"].Value = month;
                    row.Cells["cpYear"].Value = year;
                    row.Cells["cpPatientsFeedback"].Value = patientFeedback;
                    row.Cells["cpSurveyTemplate"].Value = surveyTemplate;
                }
            }

            // Set the row height after rows are added
            SetDataGridViewRowHeight();
        }




        private void btnUpdate_Click(object sender, EventArgs e)
        {
            // Ensure a row is selected
            if (datagridCPHome.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a row to view the data.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Get the selected row
            DataGridViewRow selectedRow = datagridCPHome.SelectedRows[0];

            // Retrieve data from the selected row
            string respondentID = selectedRow.Cells["cpID"].Value?.ToString() ?? string.Empty;
            string patientName = selectedRow.Cells["cpName"].Value?.ToString() ?? string.Empty;
            string surveyScore = selectedRow.Cells["cpSurveyScore"].Value?.ToString() ?? string.Empty;
            string date = selectedRow.Cells["cpDatePeriod"].Value?.ToString() ?? string.Empty;
            string surveyTemplate = selectedRow.Cells["cpSurveyTemplate"].Value?.ToString() ?? string.Empty;
            string month = selectedRow.Cells["cpMonth"].Value?.ToString() ?? string.Empty;
            string year = selectedRow.Cells["cpYear"].Value?.ToString() ?? string.Empty;
            string patientFeedback = selectedRow.Cells["cpPatientsFeedback"].Value?.ToString() ?? string.Empty;

            // Retrieve the JSON file for the selected respondent
            string finalizedSurveysPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CarePulse", "AnsweredSurvey", "FinalizedSurveys");
            string filePath = Path.Combine(finalizedSurveysPath, $"Survey_{respondentID}_{month}_{year}.json");

            string answers = string.Empty;

            if (File.Exists(filePath))
            {
                string jsonContent = File.ReadAllText(filePath);
                var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonContent);

                if (data != null && data.ContainsKey("Answers"))
                {
                    answers = data["Answers"].ToString();
                }
            }
            else
            {
                MessageBox.Show("Answers file not found for the selected respondent.", "File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Debugging: Log the surveyTemplate value
            Console.WriteLine($"Survey Template: {surveyTemplate}");

            // Open the EntryUpdate form and pass the data
            EntryUpdate entryUpdate = new EntryUpdate(respondentID, patientName, surveyScore, date, month, year, patientFeedback, surveyTemplate, answers);

            // Subscribe to the SaveChangesCompleted event
            entryUpdate.SaveChangesCompleted += (s, ea) =>
            {
                // Refresh the data grid when the event is triggered
                LoadJsonToDataGrid();
            };

            entryUpdate.ShowDialog();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

        }

        private void btnView_Click(object sender, EventArgs e)
        {
            // Ensure a row is selected
            if (datagridCPHome.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a row to view the data.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Get the selected row
            DataGridViewRow selectedRow = datagridCPHome.SelectedRows[0];

            // Retrieve data from the selected row
            string respondentID = selectedRow.Cells["cpID"].Value?.ToString() ?? string.Empty;
            string patientName = selectedRow.Cells["cpName"].Value?.ToString() ?? string.Empty;
            string surveyScore = selectedRow.Cells["cpSurveyScore"].Value?.ToString() ?? string.Empty;
            string date = selectedRow.Cells["cpDatePeriod"].Value?.ToString() ?? string.Empty;
            string surveyTemplate = selectedRow.Cells["cpSurveyTemplate"].Value?.ToString() ?? string.Empty;
            string month = selectedRow.Cells["cpMonth"].Value?.ToString() ?? string.Empty;
            string year = selectedRow.Cells["cpYear"].Value?.ToString() ?? string.Empty;
            string patientFeedback = selectedRow.Cells["cpPatientsFeedback"].Value?.ToString() ?? string.Empty;

            // Retrieve the JSON file for the selected respondent
            string finalizedSurveysPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CarePulse", "AnsweredSurvey", "FinalizedSurveys");
            string filePath = Path.Combine(finalizedSurveysPath, $"Survey_{respondentID}_{month}_{year}.json");

            string answers = string.Empty;

            if (File.Exists(filePath))
            {
                string jsonContent = File.ReadAllText(filePath);
                var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonContent);

                if (data != null && data.ContainsKey("Answers"))
                {
                    answers = data["Answers"].ToString();
                }
            }
            else
            {
                MessageBox.Show("Answers file not found for the selected respondent.", "File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Open the ViewData form and pass the data
            ViewData viewData = new ViewData(respondentID, patientName, surveyScore, date, month, year, patientFeedback, surveyTemplate, answers);
            viewData.ShowDialog();
        }


    }
}
