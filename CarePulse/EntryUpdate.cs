using Newtonsoft.Json;
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

            Survey survey = new Survey(id);

            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                SurveyTemplate template = JsonConvert.DeserializeObject<SurveyTemplate>(json);

                if (template?.Questions != null)
                {
                    survey.SetSurveyQuestions(template.Questions);
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

            survey.ShowDialog();
        }
    }
}
