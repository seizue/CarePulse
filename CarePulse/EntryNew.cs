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
    public partial class EntryNew : Form
    {
        public event EventHandler SaveChangesCompleted;

        public EntryNew()
        {
            InitializeComponent();
            GenerateUniqueId();

            // TextChanged event handler for txtboxSurveyScore
            txtboxSurveyScore.TextChanged += txtboxSurveyScore_TextChanged;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnNewTemplateSurvey_Click(object sender, EventArgs e)
        {
            NewTemplates newTemplates = new NewTemplates();
            newTemplates.ShowDialog();
        }


        private void btnAnsSurvey_Click(object sender, EventArgs e)
        {
            // Generate the ID if not already entered
            if (string.IsNullOrWhiteSpace(txtBoxIDNo.Text))
            {
                string generatedId = GenerateUniqueId();
                txtBoxIDNo.Text = generatedId;  
            }

            string id = txtBoxIDNo.Text.Trim();
            if (string.IsNullOrWhiteSpace(id))
            {
                MessageBox.Show("Please enter an ID number first.");
                return;
            }

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


        // Method to generate a unique respondent ID in the format X1D-25D
        private string GenerateUniqueId()
        {
            string id;
            bool idExists;

            do
            {
                id = GenerateRandomId();
                idExists = CheckIdExists(id);
            }
            while (idExists);  // Keep generating until a unique ID is found

            return id;
        }

        // Method to generate a random ID (X1D-25D format)
        private string GenerateRandomId()
        {
            Random random = new Random();
            StringBuilder id = new StringBuilder();

            // Generate random alphanumeric characters
            id.Append((char)random.Next('A', 'Z' + 1));  // First letter
            id.Append(random.Next(0, 10));                // First number
            id.Append((char)random.Next('A', 'Z' + 1));  // Second letter
            id.Append('-');
            id.Append(random.Next(10, 100));             // First two-digit number
            id.Append((char)random.Next('A', 'Z' + 1));  // Third letter

            return id.ToString();
        }

        // Method to check if the ID already exists in the saved data
        private bool CheckIdExists(string id)
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CarePulse", "AnsweredSurvey");
            Directory.CreateDirectory(path);

            // Get all the files in the directory (assuming the files are named by the ID)
            var existingIds = Directory.GetFiles(path)
                                        .Select(file => Path.GetFileNameWithoutExtension(file))
                                        .ToList();

            // Check if the generated ID already exists
            return existingIds.Contains(id);
        }

        private void LoadSurveyTemplates()
        {
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

        private void EntryNew_Load(object sender, EventArgs e)
        {
            string generatedId = GenerateUniqueId();
            txtBoxIDNo.Text = generatedId;

            LoadSurveyTemplates();
            PopulateMonthComboBox();
            PopulateYearComboBox();

            // Set initial values from date picker
            datePickerDateSurvey_ValueChanged(null, null);
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


        private void btnSaveChanges_Click(object sender, EventArgs e)
        {
            // Validate required fields
            if (string.IsNullOrWhiteSpace(txtBoxIDNo.Text) ||
                string.IsNullOrWhiteSpace(txtboxName.Text) ||
                string.IsNullOrWhiteSpace(txtboxSurveyScore.Text) ||
                comboBoxSelectSurveyTemplate.SelectedItem == null ||
                string.IsNullOrWhiteSpace(txtboxPatientFeedBack.Text) ||
                comboBoxMonthSurvey.SelectedItem == null ||
                comboBoxYearSurvey.SelectedItem == null)
            {
                MessageBox.Show("All fields are required. Please make sure everything is filled out.", "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Extract values
            string id = txtBoxIDNo.Text.Trim();
            string name = txtboxName.Text.Trim();
            string score = txtboxSurveyScore.Text.Trim() + "%"; 
            string feedback = txtboxPatientFeedBack.Text.Trim();
            string selectedTemplate = comboBoxSelectSurveyTemplate.SelectedItem.ToString();
            string month = comboBoxMonthSurvey.SelectedItem.ToString();
            string year = comboBoxYearSurvey.SelectedItem.ToString();
            DateTime date = datePickerDateSurvey.Value;

            // Load the temporary survey file
            string tempFolderPath = Path.Combine(Path.GetTempPath(), "CarePulse", "TemporarySurvey");
            string tempFilePath = Path.Combine(tempFolderPath, $"{id}.json");

            if (!File.Exists(tempFilePath))
            {
                MessageBox.Show("Temporary survey file not found. Please complete the survey before saving.", "Missing Survey", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string tempJson = File.ReadAllText(tempFilePath);
            var surveyAnswers = JsonConvert.DeserializeObject<Dictionary<string, object>>(tempJson);

            // Final object to save
            var finalData = new
            {
                RespondentID = id,
                Name = name,
                SurveyScore = score, // This now contains the % symbol
                PatientFeedback = feedback,
                SurveyTemplate = selectedTemplate,
                Date = date.ToString("yyyy-MM-dd"),
                Month = month,
                Year = year,
                Answers = surveyAnswers["Answers"]
            };

            // Save to: Survey_{ID}_{Month}_{Year}.json
            string finalFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CarePulse", "AnsweredSurvey", "FinalizedSurveys");
            Directory.CreateDirectory(finalFolder);
            string finalFileName = $"Survey_{id}_{month}_{year}.json";
            string finalPath = Path.Combine(finalFolder, finalFileName);

            File.WriteAllText(finalPath, JsonConvert.SerializeObject(finalData, Formatting.Indented));

            // Delete the temporary file after saving permanently
            File.Delete(tempFilePath);

            MessageBox.Show("Survey data saved successfully!", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);

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

