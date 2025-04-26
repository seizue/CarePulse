using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace CarePulse
{
    public partial class Report : UserControl
    {
        private List<Dictionary<string, object>> allSurveyData = new List<Dictionary<string, object>>();
        private int currentPage = 1;
        private int recordsPerPage = 12;
        private int totalPages = 0;

        public Report()
        {
            InitializeComponent();

            // Populate comboBoxFilterMonth with month names
            comboBoxFilterMonth.Items.AddRange(new string[]
            {
                "January", "February", "March", "April", "May", "June",
                "July", "August", "September", "October", "November", "December"
            });

            // Set default selection to "All" or the first month
            comboBoxFilterMonth.SelectedIndex = -1;

            LoadJsonToDataGrid();
        }

        private void LoadJsonToDataGrid()
        {
            string finalizedSurveysPath = Path.Combine(
              Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
              "CarePulse", "AnsweredSurvey", "FinalizedSurveys");
            string postedFolderPath = Path.Combine(finalizedSurveysPath, "Posted");


            // Check if the directory exists
            if (!Directory.Exists(postedFolderPath))
            {
                Console.WriteLine("No directory found at: " + postedFolderPath);
                return;
            }

            // Get all JSON files in the directory
            var jsonFiles = Directory.GetFiles(postedFolderPath, "*.json");

            // If no JSON files are found, log to console and return
            if (jsonFiles.Length == 0)
            {
                Console.WriteLine("No JSON files found in: " + postedFolderPath);
                lblPageInfo.Text = "Page 0 of 0";
                return;
            }

            // Clear existing data list
            allSurveyData.Clear();

            // Load all data from JSON files
            foreach (var file in jsonFiles)
            {
                string jsonContent = File.ReadAllText(file);
                var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonContent);
                if (data != null)
                {
                    allSurveyData.Add(data);
                }
            }

            // Calculate total pages
            totalPages = (int)Math.Ceiling((double)allSurveyData.Count / recordsPerPage);

            // Ensure current page is valid
            if (currentPage > totalPages && totalPages > 0)
                currentPage = totalPages;
            else if (currentPage < 1)
                currentPage = 1;


            // Display the current page
            DisplayCurrentPage();
        }

        private void DisplayCurrentPage()
        {
            // Clear existing rows in the DataGridView
            datagridReport.Rows.Clear();

            // Calculate start and end indices for the current page
            int startIndex = (currentPage - 1) * recordsPerPage;
            int endIndex = Math.Min(startIndex + recordsPerPage, allSurveyData.Count);

            // Loop through data for the current page
            for (int i = startIndex; i < endIndex; i++)
            {
                var data = allSurveyData[i];

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

                // Calculate the number of answered questions
                int answeredCount = 0;
                if (data.ContainsKey("Answers") && data["Answers"] is Newtonsoft.Json.Linq.JObject answerCounts)
                {
                    foreach (var answer in answerCounts)
                    {
                        if (!string.IsNullOrWhiteSpace(answer.Value.ToString()))
                        {
                            answeredCount++;
                        }
                    }
                }

                string responseSummary = $"Answered: {answeredCount}";

                // Add a new row to the DataGridView
                int rowIndex = datagridReport.Rows.Add();
                DataGridViewRow row = datagridReport.Rows[rowIndex];
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

            // Update pagination controls
            UpdatePaginationControls();

            // Apply auto-sizing after rows are loaded
            ApplyAutoSizing();
        }

        private void UpdatePaginationControls()
        {
            // Update page info label
            lblPageInfo.Text = $"Page {currentPage} of {totalPages}";

            // Enable/disable pagination buttons based on current page
            btnPreviousPage.Enabled = (currentPage > 1);
            btnStartPage.Enabled = (currentPage > 1);
            btnNextPage.Enabled = (currentPage < totalPages);
            btnLastPage.Enabled = (currentPage < totalPages);
        }



        private void ApplyAutoSizing()
        {
            // Disable auto-sizing for rows
            datagridReport.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;

            // Enable auto-fill for columns
            datagridReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Set AutoGenerateColumns to false to prevent duplicate columns
            datagridReport.AutoGenerateColumns = false;

            // Enable text wrapping for columns that might contain long text
            datagridReport.Columns["cpSurveyQuestionsAnswers"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            datagridReport.Columns["cpPatientsFeedback"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            // Set height for all rows
            foreach (DataGridViewRow row in datagridReport.Rows)
            {
                row.Height = 34;
            }

            datagridReport.Columns["cpID"].FillWeight = 25;
            datagridReport.Columns["cpDatePeriod"].FillWeight = 35;
            datagridReport.Columns["cpName"].FillWeight = 70;
            datagridReport.Columns["cpResponseTotal"].FillWeight = 60;
            datagridReport.Columns["cpSurveyScore"].FillWeight = 25;
            datagridReport.Columns["cpSurveyQuestionsAnswers"].FillWeight = 30;
            datagridReport.Columns["cpMonth"].FillWeight = 15;
            datagridReport.Columns["cpYear"].FillWeight = 15;
            datagridReport.Columns["cpPatientsFeedback"].FillWeight = 60;
            datagridReport.Columns["cpSurveyTemplate"].FillWeight = 30;

        }

        private void btnStartPage_Click(object sender, EventArgs e)
        {
            if (currentPage != 1)
            {
                currentPage = 1;
                DisplayCurrentPage();
            }
        }

        private void btnPreviousPage_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                DisplayCurrentPage();
            }
        }

        private void btnNextPage_Click(object sender, EventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                DisplayCurrentPage();
            }
        }

        private void btnLastPage_Click(object sender, EventArgs e)
        {
            if (currentPage != totalPages)
            {
                currentPage = totalPages;
                DisplayCurrentPage();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            ApplyAutoSizing();

            string searchText = txtboxSearch.Text.Trim().ToLower();

            if (string.IsNullOrEmpty(searchText))
            {
                // If search text is empty, reload all data
                LoadJsonToDataGrid();
                return;
            }

            // Filter the data based on RespondentID or Name
            var filteredData = allSurveyData.Where(data =>
                (data.ContainsKey("RespondentID") && data["RespondentID"].ToString().ToLower().Contains(searchText)) ||
                (data.ContainsKey("Name") && data["Name"].ToString().ToLower().Contains(searchText))
            ).ToList();

            if (filteredData.Count == 0)
            {
                MessageBox.Show("No matching records found.", "Search Results", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }


            // Update the DataGridView with the filtered data
            DisplayFilteredData(filteredData);
        }


        private void DisplayFilteredData(List<Dictionary<string, object>> filteredData)
        {
            // Clear existing rows in the DataGridView
            datagridReport.Rows.Clear();

            // Loop through the filtered data
            foreach (var data in filteredData)
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
                int answeredCount = 0; // Initialize answeredCount to avoid unassigned variable issues
                if (data.ContainsKey("Answers") && data["Answers"] is Newtonsoft.Json.Linq.JObject answers)
                {
                    var formattedAnswers = answers.Properties()
                        .Select(p => $"{p.Name}: {p.Value?.ToString() ?? "No Response"}");
                    surveyQuestionsAnswers = string.Join("; ", formattedAnswers);

                    // Calculate the number of answered questions
                    answeredCount = answers.Properties().Count(p => !string.IsNullOrWhiteSpace(p.Value?.ToString()));
                }

                string responseSummary = $"Answered: {answeredCount}";

                // Add a new row to the DataGridView
                int rowIndex = datagridReport.Rows.Add();
                DataGridViewRow row = datagridReport.Rows[rowIndex];
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

                ApplyAutoSizing();

            }
        }


        private void btnClearSearchText_Click(object sender, EventArgs e)
        {
            // Clear search textbox
            txtboxSearch.Clear();

            // Reset the data grid view
            LoadJsonToDataGrid();

            // Refocus if needed
            txtboxSearch.Focus();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            // Ensure a row is selected
            if (datagridReport.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a row to view the data.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Get the selected row
            DataGridViewRow selectedRow = datagridReport.SelectedRows[0];

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
            string finalizedSurveysPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CarePulse", "AnsweredSurvey", "FinalizedSurveys", "Posted");
            string filePath = Path.Combine(finalizedSurveysPath, $"PostedData_{respondentID}_{patientName}_{month}_{year}.json");

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


        private void btnRollBack_Click(object sender, EventArgs e)
        {

        }


        private void btnFilter_Click(object sender, EventArgs e)
        {
            // Toggle the visibility of the panelFilter
            panelFilter.Visible = !panelFilter.Visible;
        }

    
        private void btnExportCSV_Click(object sender, EventArgs e)
        {
            // Prompt the user to choose between exporting all data or a specific row
            DialogResult result = MessageBox.Show(
                "Do you want to export all data? Click 'Yes' for all data or 'No' for a specific row.",
                "Export Options",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question);

            if (result == DialogResult.Cancel)
            {
                // User canceled the operation
                return;
            }

            bool exportAllData = (result == DialogResult.Yes);

            // If exporting a specific row, ensure a row is selected
            if (!exportAllData && datagridReport.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a row to export.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Prompt the user to select a location to save the CSV file
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "CSV files (*.csv)|*.csv";
                saveFileDialog.Title = "Export Data to CSV";
                saveFileDialog.FileName = "SurveyData.csv";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Open a StreamWriter to write the CSV file
                        using (StreamWriter writer = new StreamWriter(saveFileDialog.FileName))
                        {
                            // Write the header row
                            writer.WriteLine("RespondentID,Name,Date,SurveyScore,Month,Year,SurveyTemplate,PatientFeedback,SurveyQuestionsAnswers");

                            if (exportAllData)
                            {
                                // Write all data
                                foreach (var data in allSurveyData)
                                {
                                    WriteDataRow(writer, data);
                                }
                            }
                            else
                            {
                                // Write only the selected row
                                DataGridViewRow selectedRow = datagridReport.SelectedRows[0];
                                var data = GetDataFromRow(selectedRow);
                                WriteDataRow(writer, data);
                            }
                        }

                        MessageBox.Show("Data successfully exported to CSV.", "Export Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occurred while exporting data: {ex.Message}", "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }


        private void WriteDataRow(StreamWriter writer, Dictionary<string, object> data)
        {
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

            // Write the row to the CSV file
            writer.WriteLine($"{respondentID},{name},{date},{surveyScore},{month},{year},{surveyTemplate},{patientFeedback},{surveyQuestionsAnswers}");
        }

        private Dictionary<string, object> GetDataFromRow(DataGridViewRow row)
        {
            return new Dictionary<string, object>
            {
                { "RespondentID", row.Cells["cpID"].Value?.ToString() ?? string.Empty },
                { "Name", row.Cells["cpName"].Value?.ToString() ?? string.Empty },
                { "Date", row.Cells["cpDatePeriod"].Value?.ToString() ?? string.Empty },
                { "SurveyScore", row.Cells["cpSurveyScore"].Value?.ToString() ?? string.Empty },
                { "Month", row.Cells["cpMonth"].Value?.ToString() ?? string.Empty },
                { "Year", row.Cells["cpYear"].Value?.ToString() ?? string.Empty },
                { "SurveyTemplate", row.Cells["cpSurveyTemplate"].Value?.ToString() ?? string.Empty },
                { "PatientFeedback", row.Cells["cpPatientsFeedback"].Value?.ToString() ?? string.Empty },
                { "Answers", row.Cells["cpSurveyQuestionsAnswers"].Value?.ToString() ?? string.Empty }
            };
        }

        private void comboBoxFilterMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Get the selected month
            string selectedMonth = comboBoxFilterMonth.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(selectedMonth))
            {
                // If no month is selected, reload all data
                LoadJsonToDataGrid();
                return;
            }

            // Map month names to their corresponding numbers
            Dictionary<string, string> monthMap = new Dictionary<string, string>
            {
                { "January", "01" }, { "February", "02" }, { "March", "03" },
                { "April", "04" }, { "May", "05" }, { "June", "06" },
                { "July", "07" }, { "August", "08" }, { "September", "09" },
                { "October", "10" }, { "November", "11" }, { "December", "12" }
            };

            string selectedMonthNumber = monthMap[selectedMonth];

            // Filter the data based on the selected month
            var filteredData = allSurveyData.Where(data =>
            {
                if (data.ContainsKey("Date") && DateTime.TryParse(data["Date"].ToString(), out DateTime recordDate))
                {
                    return recordDate.ToString("MM") == selectedMonthNumber;
                }
                return false;
            }).ToList();

            // Update the DataGridView with the filtered data
            DisplayFilteredData(filteredData);
        }

        private void btnFilterData_Click(object sender, EventArgs e)
        {
            // Ensure both date pickers have valid dates
            if (datePickerStart.Value > datePickerEnd.Value)
            {
                MessageBox.Show("Start date cannot be later than end date.", "Invalid Date Range", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Parse the selected date range
            DateTime startDate = datePickerStart.Value.Date;
            DateTime endDate = datePickerEnd.Value.Date;

            // Filter the data based on the Date field
            var filteredData = allSurveyData.Where(data =>
            {
                if (data.ContainsKey("Date") && DateTime.TryParse(data["Date"].ToString(), out DateTime recordDate))
                {
                    return recordDate.Date >= startDate && recordDate.Date <= endDate;
                }
                return false;
            }).ToList();


            // Update the DataGridView with the filtered data
            DisplayFilteredData(filteredData);

            panelFilter.Visible = false;
        }

        private void btnViewAll_Click(object sender, EventArgs e)
        {
            // Clear search textbox
            txtboxSearch.Clear();

            // Reset the data grid view
            LoadJsonToDataGrid();

            panelFilter.Visible = false;
        }
    }
}
