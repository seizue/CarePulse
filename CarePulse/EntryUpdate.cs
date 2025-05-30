﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq; // Add this directive to resolve JObject namespace issue
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

namespace CarePulse
{
    public partial class EntryUpdate : Form
    {
        public event EventHandler SaveChangesCompleted;

        public EntryUpdate(string respondentID, string patientName, string surveyScore, string date, string month, string year, string patientFeedback, string surveyTemplate, string answers)
        {
            InitializeComponent();

            // Populate comboboxes before setting their values
            PopulateMonthComboBox();
            PopulateYearComboBox();
            LoadSurveyTemplates();

            // Set the data to the corresponding text boxes
            txtBoxIDNo.Text = respondentID;
            txtboxName.Text = patientName;
            txtboxSurveyScore.Text = surveyScore;

            // Safely parse the date string to a DateTime object
            if (DateTime.TryParse(date, out DateTime parsedDate))
            {
                datePickerDateSurvey.Value = parsedDate;

                // Synchronize comboboxes with the parsed date
                comboBoxMonthSurvey.SelectedItem = parsedDate.ToString("MMMM");
                comboBoxYearSurvey.SelectedItem = parsedDate.Year.ToString();
            }
            else
            {
                // Handle invalid date format (optional)
                MessageBox.Show("Invalid date format. Setting to today's date.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                datePickerDateSurvey.Value = DateTime.Now;

                // Synchronize comboboxes with today's date
                comboBoxMonthSurvey.SelectedItem = DateTime.Now.ToString("MMMM");
                comboBoxYearSurvey.SelectedItem = DateTime.Now.Year.ToString();
            }

            txtboxPatientFeedBack.Text = patientFeedback;

            foreach (var item in comboBoxSelectSurveyTemplate.Items)
            {
                Console.WriteLine(item);
            }

            // Set the selected template if it exists in the combobox (case-insensitive match)
            var matchingTemplate = comboBoxSelectSurveyTemplate.Items
                .Cast<string>()
                .FirstOrDefault(item => string.Equals(item, surveyTemplate, StringComparison.OrdinalIgnoreCase));

            if (matchingTemplate != null)
            {
                comboBoxSelectSurveyTemplate.SelectedItem = matchingTemplate;

                // Force UI update to reflect the selected item
                comboBoxSelectSurveyTemplate.Refresh();
            }
            else
            {
                MessageBox.Show($"Survey template '{surveyTemplate}' is not available in the list.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            // TextChanged event handler for txtboxSurveyScore
            txtboxSurveyScore.TextChanged += txtboxSurveyScore_TextChanged;

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void datePickerDateSurvey_ValueChanged(object sender, EventArgs e)
        {

            DateTime selectedDate = datePickerDateSurvey.Value;

            // Set the selected month (e.g., "April")
            comboBoxMonthSurvey.SelectedItem = selectedDate.ToString("MMMM");

            // Set the selected year (e.g., "2025")
            comboBoxYearSurvey.SelectedItem = selectedDate.Year.ToString();
        }


        private void PopulateMonthComboBox()
        {
            comboBoxMonthSurvey.Items.Clear();
            for (int month = 1; month <= 12; month++)
            {
                comboBoxMonthSurvey.Items.Add(new DateTime(1, month, 1).ToString("MMMM"));
            }
        }

        private void PopulateYearComboBox()
        {
            comboBoxYearSurvey.Items.Clear();
            int currentYear = DateTime.Now.Year;
            for (int year = currentYear - 5; year <= currentYear + 5; year++)
            {
                comboBoxYearSurvey.Items.Add(year.ToString());
            }
        }

        private void LoadSurveyTemplates()
        {
            comboBoxSelectSurveyTemplate.Items.Clear(); // Ensure the combobox is cleared before populating
            string templatesPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CarePulse", "SurveyTemplate");

            if (Directory.Exists(templatesPath))
            {
                var files = Directory.GetFiles(templatesPath, "*.json");

                foreach (var file in files)
                {
                    comboBoxSelectSurveyTemplate.Items.Add(Path.GetFileNameWithoutExtension(file));
                }
            }
            else
            {
                Directory.CreateDirectory(templatesPath);
            }

            if (comboBoxSelectSurveyTemplate.Items.Count == 0)
            {
                MessageBox.Show("No survey templates found. Please add templates to the application data folder.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void EntryUpdate_Load(object sender, EventArgs e)
        {
            // Synchronize comboboxes with the datePickerDateSurvey value
            comboBoxMonthSurvey.SelectedItem = datePickerDateSurvey.Value.ToString("MMMM");
            comboBoxYearSurvey.SelectedItem = datePickerDateSurvey.Value.Year.ToString();
        }

        private void comboBoxSelectSurveyTemplate_SelectedIndexChanged(object sender, EventArgs e)
        {
            string id = txtBoxIDNo.Text.Trim();

            if (string.IsNullOrWhiteSpace(id))
            {
                MessageBox.Show("Please enter a valid respondent ID before selecting a template.");
                return;
            }

            string selectedTemplate = comboBoxSelectSurveyTemplate.SelectedItem.ToString();

            string templatesPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CarePulse", "SurveyTemplate");
            string templateFilePath = Path.Combine(templatesPath, selectedTemplate + ".json");

            if (!File.Exists(templateFilePath))
            {
                txtboxSurveyStatus.Text = "Template file not found.";
                return;
            }

            SurveyTemplate template = JsonConvert.DeserializeObject<SurveyTemplate>(File.ReadAllText(templateFilePath));
            if (template == null || template.Questions == null || template.Questions.Count == 0)
            {
                txtboxSurveyStatus.Text = "No questions found in the template.";
                return;
            }

            string answeredPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CarePulse", "AnsweredSurvey", id + ".json");

            Dictionary<string, object> answersDict = null;
            if (File.Exists(answeredPath))
            {
                var json = File.ReadAllText(answeredPath);
                var parsed = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                if (parsed != null && parsed.ContainsKey("Answers"))
                {
                    answersDict = JsonConvert.DeserializeObject<Dictionary<string, object>>(parsed["Answers"].ToString());
                }
            }

            int totalQuestions = template.Questions.Count;
            int answeredCount = 0;

            if (answersDict != null)
            {
                foreach (var question in template.Questions)
                {
                    if (answersDict.TryGetValue(question, out object answer) && answer != null && !string.IsNullOrWhiteSpace(answer.ToString()))
                    {
                        answeredCount++;
                    }
                }
            }

            int unansweredCount = totalQuestions - answeredCount;

            txtboxSurveyStatus.Text =
    $"Selected Template: {selectedTemplate}\r\n" +
    $"Total Questions: {totalQuestions}";
        }

        private void btnViewTemplates_Click(object sender, EventArgs e)
        {
            string id = txtBoxIDNo.Text.Trim();
            if (comboBoxSelectSurveyTemplate.SelectedItem == null)
            {
                MessageBox.Show("Please select a survey template before continuing.", "Template Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string selectedTemplate = comboBoxSelectSurveyTemplate.SelectedItem.ToString();

            string templatesPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CarePulse", "SurveyTemplate");
            string filePath = Path.Combine(templatesPath, selectedTemplate + ".json");

            SelectedSurvey selectedSurvey = new SelectedSurvey(id);

            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                SurveyTemplate template = JsonConvert.DeserializeObject<SurveyTemplate>(json);

                if (template?.Questions != null)
                {
                    selectedSurvey.SetSurveyQuestions(template.Questions);
                }
                else
                {
                    MessageBox.Show("This template has no questions defined.", "Template Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                MessageBox.Show("Template file not found.", "Missing Template", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            selectedSurvey.ShowDialog();
        }

        private void btnEditSurvey_Click(object sender, EventArgs e)
        {
            string id = txtBoxIDNo.Text.Trim();
            if (comboBoxSelectSurveyTemplate.SelectedItem == null)
            {
                MessageBox.Show("Please select a survey template before continuing.", "Template Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string selectedTemplate = comboBoxSelectSurveyTemplate.SelectedItem.ToString();
            string templatesPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CarePulse", "SurveyTemplate");
            string filePath = Path.Combine(templatesPath, selectedTemplate + ".json");
            List<string> questions = new List<string>();
            Dictionary<string, string> responses = new Dictionary<string, string>();

            // Load template questions
            if (File.Exists(filePath))
            {
                try
                {
                    string json = File.ReadAllText(filePath);
                    SurveyTemplate template = JsonConvert.DeserializeObject<SurveyTemplate>(json);
                    if (template?.Questions != null)
                    {
                        questions = template.Questions;
                    }
                    else
                    {
                        MessageBox.Show("This template has no questions defined.", "Template Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading template: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                MessageBox.Show("Template file not found.", "Missing Template", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Load existing responses from FinalizedSurveys folder
            string finalizedSurveysPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "CarePulse", "AnsweredSurvey", "FinalizedSurveys");
            string responseFilePath = Path.Combine(finalizedSurveysPath,
                $"Survey_{id}_{comboBoxMonthSurvey.SelectedItem}_{comboBoxYearSurvey.SelectedItem}.json");

            if (File.Exists(responseFilePath))
            {
                try
                {
                    string json = File.ReadAllText(responseFilePath);

                    // First try to deserialize as a direct dictionary (for simple formats)
                    try
                    {
                        var directResult = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                        if (directResult != null)
                        {
                            responses = directResult;
                        }
                    }
                    catch
                    {
                        // If direct deserialization fails, try the nested structure
                        try
                        {
                            // Try to deserialize as a full survey result object
                            var surveyResult = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

                            if (surveyResult != null && surveyResult.ContainsKey("Answers"))
                            {
                                // Get the Answers object and convert it to a string dictionary
                                string answersJson = JsonConvert.SerializeObject(surveyResult["Answers"]);
                                responses = JsonConvert.DeserializeObject<Dictionary<string, string>>(answersJson);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error parsing response structure: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                    // Debug - check what's in the responses dictionary
                    if (responses.Count == 0)
                    {
                        MessageBox.Show("No responses found in the file.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        // Debug - show the first response to check format
                        var firstKey = responses.Keys.First();
                        MessageBox.Show($"Found {responses.Count} responses. First key: {firstKey}, Value: {responses[firstKey]}",
                            "Debug Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading responses: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("No finalized survey responses found for this respondent.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            // Instantiate the Survey form with the loaded questions and responses
            Survey survey = new Survey(id, questions, responses);
            survey.ShowDialog();
        }

        private void btnSaveChanges_Click(object sender, EventArgs e)
        {
            // Check if at least one field has been edited
            if (string.IsNullOrWhiteSpace(txtBoxIDNo.Text) &&
                string.IsNullOrWhiteSpace(txtboxName.Text) &&
                string.IsNullOrWhiteSpace(txtboxSurveyScore.Text) &&
                comboBoxSelectSurveyTemplate.SelectedItem == null &&
                string.IsNullOrWhiteSpace(txtboxPatientFeedBack.Text) &&
                comboBoxMonthSurvey.SelectedItem == null &&
                comboBoxYearSurvey.SelectedItem == null)
            {
                MessageBox.Show("At least one field must be edited to save changes.", "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Extract values
            string id = txtBoxIDNo.Text.Trim();
            string name = txtboxName.Text.Trim();
            string score = txtboxSurveyScore.Text.Trim() + "%";
            string feedback = txtboxPatientFeedBack.Text.Trim();
            string selectedTemplate = comboBoxSelectSurveyTemplate.SelectedItem?.ToString();
            string month = comboBoxMonthSurvey.SelectedItem?.ToString();
            string year = comboBoxYearSurvey.SelectedItem?.ToString();
            DateTime date = datePickerDateSurvey.Value;

            // Define path to the existing file
            string finalFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                                             "CarePulse", "AnsweredSurvey", "FinalizedSurveys");
            Directory.CreateDirectory(finalFolder); // Ensure directory exists
            string finalFileName = $"Survey_{id}_{month}_{year}.json";
            string finalPath = Path.Combine(finalFolder, finalFileName);

            // Load existing data if file exists
            Dictionary<string, object> existingData = new Dictionary<string, object>();
            JObject answersObject = null;

            if (File.Exists(finalPath))
            {
                string existingJson = File.ReadAllText(finalPath);
                existingData = JsonConvert.DeserializeObject<Dictionary<string, object>>(existingJson);

                // Extract answers object if it exists
                if (existingData.ContainsKey("Answers") && existingData["Answers"] != null)
                {
                    answersObject = JObject.FromObject(existingData["Answers"]);
                }
            }

            // Check if survey has been edited
            string tempFolderPath = Path.Combine(Path.GetTempPath(), "CarePulse", "TemporarySurvey");
            string tempFilePath = Path.Combine(tempFolderPath, $"{id}.json");

            // If we have new answers from a temporary file, use those
            if (File.Exists(tempFilePath))
            {
                string tempJson = File.ReadAllText(tempFilePath);
                var tempData = JsonConvert.DeserializeObject<Dictionary<string, object>>(tempJson);

                if (tempData != null && tempData.ContainsKey("Answers") && tempData["Answers"] != null)
                {
                    answersObject = JObject.FromObject(tempData["Answers"]);
                }

                // Delete the temporary file after using it
                File.Delete(tempFilePath);
            }

            // Update the data with new values (only if they exist)
            var updatedData = new Dictionary<string, object>();

            // Always include the ID
            updatedData["RespondentID"] = id;

            // Update name if provided
            updatedData["Name"] = !string.IsNullOrWhiteSpace(name) ? name :
                (existingData.ContainsKey("Name") ? existingData["Name"] : "");

            // Update score if provided
            updatedData["SurveyScore"] = !string.IsNullOrWhiteSpace(score) ? score :
                (existingData.ContainsKey("SurveyScore") ? existingData["SurveyScore"] : "");

            // Update feedback if provided
            updatedData["PatientFeedback"] = !string.IsNullOrWhiteSpace(feedback) ? feedback :
                (existingData.ContainsKey("PatientFeedback") ? existingData["PatientFeedback"] : "");

            // Update template if selected
            updatedData["SurveyTemplate"] = selectedTemplate != null ? selectedTemplate :
                (existingData.ContainsKey("SurveyTemplate") ? existingData["SurveyTemplate"] : "");

            // Update date, month, year
            updatedData["Date"] = date.ToString("yyyy-MM-dd");
            updatedData["Month"] = month ?? (existingData.ContainsKey("Month") ? existingData["Month"] : "");
            updatedData["Year"] = year ?? (existingData.ContainsKey("Year") ? existingData["Year"] : "");

            // Include answers
            updatedData["Answers"] = answersObject;

            // Save the updated data
            File.WriteAllText(finalPath, JsonConvert.SerializeObject(updatedData, Formatting.Indented));

            MessageBox.Show("Survey data updated successfully!", "Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Trigger the SaveChangesCompleted event
            SaveChangesCompleted?.Invoke(this, EventArgs.Empty);

            this.Close();
        }

        private void txtboxSurveyScore_TextChanged(object sender, EventArgs e)
        {
            // Store current cursor position
            int cursorPosition = txtboxSurveyScore.SelectionStart;

            // Filter out non-numeric characters
            string filteredText = new string(txtboxSurveyScore.Text.Where(c => char.IsDigit(c)).ToArray());

            // Only update if the text has changed to avoid infinite loop
            if (txtboxSurveyScore.Text != filteredText)
            {
                txtboxSurveyScore.Text = filteredText;

                // Restore cursor position, but account for potential character removal
                txtboxSurveyScore.SelectionStart = Math.Min(cursorPosition, filteredText.Length);
            }
        }
    }
}
